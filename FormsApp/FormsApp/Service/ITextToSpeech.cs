using System;
using System.Collections.Generic;

namespace FormsApp.Service
{


    public interface ITextToSpeech:IDisposable
    {
        ITextToSpeech Init(string engine = null);
        void Speak(string text);
        ActionResult SetEngine(string engine);
        List<Engine> GetEngines();
        string CurrentEngine { get; }
        ActionResult SetLanguage(Language language);
        bool HasInit { get; }
        ITextToSpeech New();
        //void Dispose();
        event EventHandler Inited;
        event EventHandler InitedError;
        //event EventHandler NoEngine;
    }

   public class ActionResult
    {
       public bool Result { set; get; }
       public string Message { set; get; }
    }
    public class Language
    {
        public string Name { set; get; }
        public string Code { set; get; }
        public static Language zh_CN = new Language() { Name = "简体中文", Code = "zh_CN" };
        public static Language en_US = new Language() { Name = "英语(美国)", Code = "en_US" };
        public static Language Default = new Language() { Name = "默认", Code = "Default" };
    }
    public class Engine
    {
        public string Name { set; get; }
        public string Label { set; get; }
        //public bool ChineseSupported { set; get; }
        public override string ToString()
        {
            return Label;
        }
    }

}
