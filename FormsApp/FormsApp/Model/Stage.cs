
namespace FormsApp.Model
{

    public class Stage : ObservableObject
    {
        public Stage()
        {
            _VoiceInterval = 2;
        }

        public Stage(int stagePoint, int voiceInterval, string extralText)
        {
            StagePoint = stagePoint;
            VoiceInterval = voiceInterval;
            ExtralText = extralText;
        }
        //[AutoIncrement]
        //[PrimaryKey]
        //public int Id { set; get; }

        private int _StagePoint;
        /// <summary>
        /// 进入阶段的时间点（距离结束的秒数）
        /// </summary>
        public int StagePoint
        {
            set
            {
                if (_StagePoint != value)
                {
                    _StagePoint = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("Desc");
                }
            }
            get { return _StagePoint; }
        }

        private int _VoiceInterval;
        /// <summary>
        /// 语音播报间隔（秒）
        /// </summary>
        public int VoiceInterval
        {
            set
            {
                if (_VoiceInterval != value)
                {
                    _VoiceInterval = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("Desc");
                }
            }
            get { return _VoiceInterval; }
        }

        private string _ExtralText;
        /// <summary>
        /// 附件播报文字(如：加油！)
        /// </summary>
        public string ExtralText
        {
            set
            {
                if (_ExtralText != value)
                {
                    _ExtralText = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("Desc");
                }
            }
            get { return _ExtralText; }
        }

        public string Desc { get { return ToString(); } }

        public bool IsFinalStage
        {
            get { return VoiceInterval == 1; }
        }

        public override string ToString()
        {
            return string.Format("距结束还剩{0}秒时 每{1}秒播报一次",StagePoint,VoiceInterval);
        }
    }
}
