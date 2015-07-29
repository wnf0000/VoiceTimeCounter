//namespace FormsApp.Model
//{
//    /// <summary>
//    /// 终极阶段
//    /// </summary>
//    public class FilnalStage : Stage
//    {
//        public FilnalStage()
//        {
//        }
//        public FilnalStage(int stagePoint)
//        {
//            StagePoint = stagePoint;
//        }
//        /// <summary>
//        /// 语音播报间隔（秒）
//        /// </summary>
//        new public int VoiceInterval { get { return 1; } }
//        /// <summary>
//        /// 附件播报文字(如：加油！)
//        /// </summary>
//        new public string ExtralText { get { return string.Empty; } }

//        public override string ToString()
//        {
//            return string.Format("距结束还剩{0}秒时 每{1}秒播报一次", StagePoint, VoiceInterval);
//        }
//    }
//}