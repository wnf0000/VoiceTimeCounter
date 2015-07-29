using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace FormsApp.Model
{
    public class TimeSet : ObservableObject
    {
        //List<Stage> _Stage = null;
        [AutoIncrement]
        [PrimaryKey]
        public int Id { set; get; }

        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged();
                }
            }
            get { return _Name; }
        }

        private int _Duration;
        /// <summary>
        /// 时长（秒）
        /// </summary>
        public int Duration
        {
            set
            {
                if (_Duration != value)
                {
                    _Duration = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("FormatedDuration");
                }
            }
            get { return _Duration; }
        }

        private string _VoiceEngine;
        /// <summary>
        /// 语音引擎
        /// </summary>
        public string VoiceEngine
        {
            set
            {
                if (_VoiceEngine != value)
                {
                    _VoiceEngine = value;
                    NotifyPropertyChanged();
                }
            }
            get { return _VoiceEngine; }
        }

        private int _SecendsToReady;
        /// <summary>
        /// 准备时间
        /// </summary>
        public int SecendsToReady
        {
            set
            {
                if (_SecendsToReady != value)
                {
                    _SecendsToReady = value;
                    NotifyPropertyChanged();
                }
            }
            get { return _SecendsToReady; }
        }

        //private string _StagesDataString;
        //[Ignore]
        public string StagesDataString
        {
            set;
            get;
        }
        /// <summary>
        /// 将Stages()的数据序列化并复制给StagesDataString
        /// </summary>
        public void ListToStagesDataString()
        {
            StagesDataString = ObservableStages.Serialize();
        }
        //public Stage[] Stages
        //{
        //    set
        //    {
        //        _list.Clear();
        //        _list.AddRange(value);
        //    }
        //    get { return _list.ToArray(); }
        //}

        //public TimeSet()
        //{
        //    Stages = new Stage[0];
        //}
        private bool stageLoad = false;
        //public List<Stage> Stages()
        //{
        //    if (!stageLoad)
        //    {
        //        //_Stage = StagesDataString.Deserialize<List<Stage>>();
        //        _ObservableStages = StagesDataString.Deserialize<ObservableCollection<Stage>>();
        //        stageLoad = true;
        //    }
        //    if (_ObservableStages == null)
        //    {
        //        _ObservableStages = new ObservableCollection<Stage>();
        //    }
        //    return _ObservableStages;
        //}
        public void AddStage(Stage stage)
        {
            //Stages().Add(stage);
            if (stageLoad)
                _ObservableStages.Add(stage);
            else
            {
                ObservableStages.Add(stage);
            }
            _ObservableStages.Sort(Comparer<Stage>.Create((x, y) => y.StagePoint - x.StagePoint));
        }

        public void RemoveStage(Stage stage)
        {
            //Stages().Remove(stage);
            ObservableStages.Remove(stage);
        }

        public void RemoveStage(Func<Stage, bool> exp)
        {
            var m = ObservableStages.FirstOrDefault(exp);
            if (m != null) ObservableStages.Remove(m);
        }

        private ObservableCollection<Stage> _ObservableStages;
        /// <summary>
        /// 增加或删除请通过AddStage|RemoveStage来进行，不要直接在此属性上操作
        /// </summary>
        [Ignore]
        public ObservableCollection<Stage> ObservableStages
        {
            //get { return _ObservableStages ?? (_ObservableStages = new ObservableCollection<Stage>(Stages())); }
            get
            {
                if (!stageLoad)
                {
                    //_Stage = StagesDataString.Deserialize<List<Stage>>();
                    _ObservableStages = StagesDataString.Deserialize<ObservableCollection<Stage>>();
                    if (_ObservableStages != null)
                        //_ObservableStages.CollectionChanged += delegate
                        //{
                        //    NotifyPropertyChanged(() => HasFinalStage);
                        //};
                        _ObservableStages.Sort(Comparer<Stage>.Create((x, y) => y.StagePoint - x.StagePoint));

                }
                if (_ObservableStages == null)
                {
                    _ObservableStages = new ObservableCollection<Stage>();
                    //_ObservableStages.CollectionChanged += delegate
                    //{
                    //    NotifyPropertyChanged(() => HasFinalStage);
                    //};
                }
                stageLoad = true;
                return _ObservableStages;
            }
        }
        [Ignore]
        public string FormatedDuration
        {
            get
            {
                string str = "时长：";

                var ts = TimeSpan.FromSeconds(Duration);
                str += ts.ToString();
                //if (ts.TotalHours > 0)
                //    str += string.Format("{0}小时", ts.TotalHours);
                //if (ts.TotalMinutes > 0)
                //    str += string.Format("{0}分钟", ts.TotalMinutes);
                //if (ts.TotalSeconds > 0)
                //    str += string.Format("{0}秒钟", ts.TotalSeconds);
                return str;
            }
        }
        [Ignore]
        public bool HasFinalStage
        {
            get
            {
                var v = ObservableStages.Count(w => w.VoiceInterval == 1) > 0;
                return v;
            }
        }

        public Stage GetFinalStage()
        {
            return ObservableStages.FirstOrDefault(w => w.IsFinalStage);
        }
        [Ignore]
        public Color Color
        {
            get
            {
                int i = Id % 5;
                switch (i)
                {
                    default:
                    case 0:
                        return Xamarin.Forms.Color.FromHex("#AA66CC");
                    case 1:
                        return Xamarin.Forms.Color.FromHex("#99CC00");
                    case 2:
                        return Xamarin.Forms.Color.FromHex("#33B5E5");
                    case 3:
                        return Xamarin.Forms.Color.FromHex("#FF4444");
                    case 4:
                        return Xamarin.Forms.Color.FromHex("#FFBB33");

                }
            }
        }
        [Ignore]
        public bool IsEnglish
        {
            get { return string.IsNullOrEmpty(VoiceEngine)|| VoiceEngine.Contains("google") || VoiceEngine.Contains("pico"); }
        }
    }
}
