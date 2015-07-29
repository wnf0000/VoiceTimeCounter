using FormsApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace FormsApp.Service
{
    public class Performer : IDisposable
    {
        //private int _Position;
        //public int Position { get { return _Position; } }
        private int currentSecond;
        //private int mxaStagePoint;
        //private int secendsToReady = 0;
        private bool controlStop = false;
        private bool controlPause = false;
        private List<Stage> stages;
        TimeSet TimeSet { set; get; }
        public event EventHandler<SpeakEventArgs> Speak;
        public event EventHandler Tick;
        public event EventHandler<PrepareTickEventArgs> PrepareTick;
        public event EventHandler<SecondChangedEvenArgs> CurrentSecondChanged;
        public event EventHandler<StageChangedEventArgs> StageChanged;
        private ITimer _heartBeatTimer;
        private ITimer _readyCountDownTimer;
        private ITimer _stageTimer;

        private Stage _current, _next;



        private int _readyCountDownCount = 0;


        private int _stageInterval = 1;
        private int _stageCountDown = 0;
        public Performer(TimeSet timeSet)
        {
            this.TimeSet = timeSet;
            currentSecond = this.TimeSet.Duration;
            stages = this.TimeSet.ObservableStages.OrderByDescending(w => w.StagePoint).ToList();
            //mxaStagePoint = stages.Max(w => w.StagePoint);

            _readyCountDownCount = timeSet.SecendsToReady;
            var tm = DependencyService.Get<ITimer>();
            _heartBeatTimer = tm.New();
            _readyCountDownTimer = tm.New();
            _stageTimer = tm.New();

            //var b = _heartBeatTimer.Equals(_readyCountDownTimer);

            _heartBeatTimer.Interval = 1000;
            _heartBeatTimer.OnTick(HeartBeats);

            _readyCountDownTimer.Interval = 1000;
            _readyCountDownTimer.OnTick((t) =>
            {
                ReadyCountDown(t, () =>
                {
                    //stageTimer.Enabled = true;
                    //stageTimer.Start();
                    StartCountDown();
                    _heartBeatTimer.Enabled = true;
                    _heartBeatTimer.Start();
                });
            });

            _stageTimer.Interval = 1000;
            _stageTimer.OnTick((t) =>
            {
                StageCountDown(t);
            });

        }

        void HeartBeats(DateTime time)
        {
            if (controlStop || controlPause)
            {
                _heartBeatTimer.Stop();
                return;
            }
            if (Tick != null) Tick(null, null);
        }
        object locker = new object();
        void StageCountDown(DateTime time)
        {
            lock (locker)
            {
                var second = currentSecond;
                if (controlStop || controlPause)
                {
                    _stageTimer.Stop();
                    return;
                }
                if (_current != null)
                {
                    var interval = _current.VoiceInterval;
                    if (second == _current.StagePoint)
                    {
                        if (Speak != null)
                        {
                            var text = second.ToString();
                            var engin = TimeSet.VoiceEngine;
                            Speak.Invoke(_current, new SpeakEventArgs(text, engin, second, interval));
                        }
                        _stageCountDown = 0;
                    }
                    else
                    {
                        _stageCountDown++;
                        if (_stageCountDown == _stageInterval)
                        {
                            if (Speak != null)
                            {
                                var text = second.ToString();
                                var engin = TimeSet.VoiceEngine;
                                Speak.Invoke(_current, new SpeakEventArgs(text, engin, second, interval));
                            }
                            _stageCountDown = 0;
                        }
                    }

                    //else
                    //{
                    //    stageCountDown++;
                    //}

                }
                if (CurrentSecondChanged != null)
                    CurrentSecondChanged(this, new SecondChangedEvenArgs(second));
                currentSecond--;
                if (currentSecond < 0)
                {
                    controlStop = true;
                    _stageTimer.Stop();
                    currentSecond = TimeSet.Duration;
                    return;
                }
                if (_next != null)
                {
                    if (currentSecond == _next.StagePoint)
                    {

                        _current = _next;
                        if (StageChanged != null)
                        {
                            StageChanged(TimeSet, new StageChangedEventArgs(_current));
                        }
                        _stageInterval = _current.VoiceInterval;
                        _stageCountDown = 0;
                        var index = stages.IndexOf(_current);
                        if (index != -1 && index + 1 < stages.Count)
                        {
                            _next = stages.ElementAt(index + 1);
                        }
                        else
                        {
                            _next = null;
                        }
                    }
                }
                //_Position--;

            }


        }
        void ReadyCountDown(DateTime time, Action onOver)
        {
            if (controlStop || controlPause)
            {
                _readyCountDownTimer.Stop();
                _readyCountDownCount = TimeSet.SecendsToReady;
                return;
            }

            //准备时间倒计时
            if (TimeSet.SecendsToReady > 0)
            {
                if (PrepareTick != null) PrepareTick(TimeSet, new PrepareTickEventArgs(_readyCountDownCount, TimeSet.SecendsToReady));
                _readyCountDownCount--;
                if (_readyCountDownCount <= 0)
                {
                    if (onOver != null) onOver();
                    _readyCountDownTimer.Stop();
                    _readyCountDownCount = TimeSet.SecendsToReady;
                }
            }
            else
            {
                _readyCountDownTimer.Stop();
            }
        }
        public void Start()
        {
            controlStop = false;
            controlPause = false;
            if (TimeSet.SecendsToReady > 0)
            {
                _readyCountDownTimer.Enabled = true;
                _readyCountDownTimer.Start();
            }
            else
            {
                StartCountDown();
            }
        }

        void StartCountDown()
        {
            _current = stages.LastOrDefault(w => w.StagePoint >= currentSecond);
            if (StageChanged != null) StageChanged(TimeSet, new StageChangedEventArgs(_current));
            if (_current != null)
            {
                _stageInterval = _current.VoiceInterval;
                var index = stages.IndexOf(_current);
                if (index != -1 && index + 1 < stages.Count)
                {
                    _next = stages.ElementAt(index + 1);
                }
            }
            else
            {
                _next = stages[0];
            }
            _stageTimer.Enabled = true;
            _stageTimer.Start();
            _heartBeatTimer.Enabled = true;
            _heartBeatTimer.Start();
        }
        public void Stop()
        {
            controlStop = true;
            _heartBeatTimer.Stop();
            _readyCountDownTimer.Stop();
            _stageTimer.Stop();
            currentSecond = this.TimeSet.Duration;
        }

        public void Pause()
        {
            controlPause = true;
            _heartBeatTimer.Stop();
            _readyCountDownTimer.Stop();
            _stageTimer.Stop();
        }

        public class SpeakEventArgs : EventArgs
        {
            public string Text { set; get; }
            public string Engine { set; get; }
            public int Seconds { set; get; }
            public int VoiceInterval { set; get; }
            public SpeakEventArgs(string text, string engine, int seconds,int interval)
            {
                this.Text = text;
                this.Engine = engine;
                this.Seconds = seconds;
                this.VoiceInterval = interval;
            }
        }
        public class SecondChangedEvenArgs : EventArgs
        {
            public int Current { set; get; }

            public SecondChangedEvenArgs(int i)
            {
                Current = i;
            }
        }
        public class PrepareTickEventArgs
        {
            public PrepareTickEventArgs(int currentSecond, int totalSeconds)
            {
                CurrentSecond = currentSecond;
                TotalSeconds = totalSeconds;
            }
            public int TotalSeconds { set; get; }
            public int CurrentSecond { set; get; }
            public bool IsAtStartPoint { get { return TotalSeconds == CurrentSecond; } }
            public bool IsAtEndPoint { get { return CurrentSecond == 0; } }
        }
        public class StageChangedEventArgs
        {
            public StageChangedEventArgs(Stage currentStage)
            {
                CurrentStage = currentStage;
            }
            public Stage CurrentStage { set; get; }
        }

        public void Dispose()
        {
            _heartBeatTimer.Dispose();
            _readyCountDownTimer.Dispose();
            _stageTimer.Dispose();
            _heartBeatTimer = null;
            _readyCountDownTimer = null;
            _stageTimer = null;
            _current = null;
            _next = null;
        }
    }
}
