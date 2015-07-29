using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormsApp.Model;
using Newtonsoft.Json.Schema;
using Xamarin.Forms;

namespace FormsApp.Service
{
    public class Performer
    {
        private int _Position;
        public int Position { get { return _Position; } }
        private int currentSecond;
        private int mxaStatePoint;
        private bool controlStop = false;
        private bool controlPause = false;
        private List<Stage> stages;
        TimeSet timeSet { set; get; }
        public event EventHandler<SpeakEventArgs> Speak;
        public event EventHandler HeartBeat;
        public event EventHandler ToReady;

        private ITimer Timer;
        public Performer(TimeSet timeSet)
        {
            this.timeSet = timeSet;
            _Position = currentSecond = this.timeSet.Duration;
            stages = this.timeSet.ObservableStages.OrderByDescending(w => w.StagePoint).ToList();
            mxaStatePoint = stages.Max(w => w.StagePoint);
            Timer = DependencyService.Get<ITimer>();
            Timer.Interval = 1000;
            Timer.OnTick(t =>
            {
                if (HeartBeat != null) HeartBeat(null,null);
            });
        }

        private int secendsToReady = 0;
        public void Start()
        {
            Timer.Start();
            controlStop = false;
            controlPause = false;
            if (timeSet.SecendsToReady > 0)
            {
                secendsToReady = timeSet.SecendsToReady;
                Heart_Beat();
                To_Ready();
            }
            else
            {
                Heart_Beat();
                var start = stages.LastOrDefault(w => w.StagePoint> currentSecond);
                if (start == null)
                {
                    RunSpeak(null, null);
                }
                else
                {
                    Stage next = null;
                    if (stages.Count > 1)
                        next = stages.ElementAt(1);
                    RunSpeak(start, next);
                }
                //var start = stages.First();
                //Stage nextStage = null;
                //if (stages.Count > 1)
                //    nextStage = stages.ElementAt(1);
                //RunSpeak(start, nextStage);
            }
        }

        public void Stop()
        {
            Timer.Stop();
            controlStop = true;
            _Position = currentSecond = this.timeSet.Duration;
        }

        public void Pause()
        {
            controlPause = true;
        }
        void Heart_Beat()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (controlStop || controlPause) return false;
                _Position--;
                if(HeartBeat!=null) HeartBeat.Invoke(null,null);
                return !(controlStop || controlPause);
            });
        }

        private void To_Ready()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (controlStop || controlPause) return false;
                if (ToReady != null) ToReady.Invoke(null, null);
                secendsToReady--;
                var stop = (controlStop || controlPause) || secendsToReady == 0;
                if (stop)
                {
                    var start = stages.LastOrDefault(w => w.StagePoint > currentSecond);
                    if (start == null)
                    {
                        RunSpeak(null, null);
                    }
                    else
                    {
                        Stage next = null;
                        if (stages.Count > 1)
                            next = stages.ElementAt(1);
                        RunSpeak(start, next);
                    }
                }
                return !stop;
            });
        }

        async void RunSpeak(Stage startStage, Stage nextStage)
        {
            if (startStage == null)
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (controlStop || controlPause) return false;
                    //空转
                    if (HeartBeat != null) HeartBeat.Invoke(null, null);
                    currentSecond--;
                    if (currentSecond == mxaStatePoint)
                    {
                        var start = stages.FirstOrDefault(w => w.StagePoint == currentSecond);
                        Stage next = null;
                        if (stages.Count > 1)
                            next = stages.ElementAt(1);
                        RunSpeak(start, next);
                    }
                    var stop = (controlStop || controlPause) || currentSecond == mxaStatePoint;
                    return !stop;
                });
            }
            else
            {
                Device.StartTimer(TimeSpan.FromSeconds(startStage.VoiceInterval), () =>
                {
                    if (controlStop || controlPause) return false;

                    if (Speak != null)
                    {
                        var text = currentSecond.ToString();
                        var engin = timeSet.VoiceEngine;
                        Speak.Invoke(startStage, new SpeakEventArgs(text, engin, currentSecond));
                    }
                    currentSecond -= startStage.VoiceInterval;

                    if (nextStage != null && currentSecond <= nextStage.StagePoint)
                    {
                        var index = stages.IndexOf(nextStage);
                        if (index != -1 && index+1 < stages.Count)
                        {
                            var nextnextStage = stages.ElementAt(index + 1);
                            RunSpeak(nextStage, nextnextStage);
                        }
                    }
                    if (currentSecond < 0)
                    {
                        currentSecond = this.timeSet.Duration;
                    }
                    var stop = (controlStop || controlPause) || currentSecond < 0 || (nextStage != null && currentSecond <= nextStage.StagePoint);
                    return !stop;
                }); 
            }
            
            
            //return control;
        }
        public class SpeakEventArgs:EventArgs
        {
            public string Text { set; get; }
            public string Engine { set; get; }
            public int Seconds { set; get; }

            public SpeakEventArgs(string text,string engine,int seconds)
            {
                this.Text = text;
                this.Engine = engine;
                this.Seconds = seconds;
            }
        }
    }
}
