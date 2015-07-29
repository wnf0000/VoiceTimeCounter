using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Speech.Tts;
using FormsApp.Service;
using Java.Util;
using TextToSpeech = FormsApp.Droid.TextToSpeech;

[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeech))]
namespace FormsApp.Droid
{
    /// <summary>
    /// Text to speech implementation Android
    /// </summary>
    public class TextToSpeech : Java.Lang.Object, ITextToSpeech, Android.Speech.Tts.TextToSpeech.IOnInitListener
    {
        Android.Speech.Tts.TextToSpeech _tts;
        //string text;
        Locale _language;
        //float pitch, speakRate;
        bool queue = false;
        //bool initialized;
        private string _engine;

        public ITextToSpeech Init(string engine = null)
        {
            if (string.IsNullOrEmpty(engine))
            {
                _tts = new Android.Speech.Tts.TextToSpeech(Application.Context, this);
                _engine = _tts.DefaultEngine;
            }
            else
            {
                _tts = new Android.Speech.Tts.TextToSpeech(Application.Context, this, engine);
                _engine = engine;
            }
            HasInit = true;
            return this;
        }
        //void SetOptions(string engine, bool queue, Locale locale, float pitch,
        //    float speakRate, float volume)
        //{
        //    //Locale.Canada;
        //    this.language = locale;
        //    this.pitch = pitch;
        //    this.speakRate = speakRate;
        //    this.queue = queue;
        //    this.engine = string.IsNullOrEmpty(engine) ? textToSpeech.Engines[1].Name : engine;
        //    if (textToSpeech == null)
        //    {
        //        textToSpeech = new Android.Speech.Tts.TextToSpeech(Application.Context, this);
        //        //initialized = true;
        //    }
        //    textToSpeech.SetLanguage(this.language);
        //    textToSpeech.SetPitch(this.pitch);
        //    textToSpeech.SetSpeechRate(this.speakRate);
        //    textToSpeech.SetEngineByPackageName(this.engine);

        //}
        #region IOnInitListener implementation
        /// <summary>
        /// OnInit of TTS
        /// </summary>
        /// <param name="status"></param>
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                if (Inited != null) Inited(this, EventArgs.Empty);
                //if (_tts.Engines.Count == 0 && NoEngine != null) NoEngine(this, EventArgs.Empty);
            }
            else
            {
                if (InitedError != null) InitedError(this, EventArgs.Empty);
            }
        }
        #endregion

        ///// <summary>
        ///// Speak back text
        ///// </summary>
        ///// <param name="text">Text to speak</param>
        ///// <param name="queue">If you want to chain together speak command or cancel current</param>
        ///// <param name="language">Locale of voice</param>
        ///// <param name="pitch">Pitch of voice</param>
        ///// <param name="speakRate">Speak Rate of voice (All) (0.0 - 2.0f)</param>
        ///// <param name="volume">Volume of voice (iOS/WP) (0.0-1.0)</param>
        //private void Speak(string text, string engine = null, bool queue = false, Locale language = null, float? pitch = null, float? speakRate = null, float? volume = null)
        //{
        //    if (!string.IsNullOrEmpty(engine))
        //    {
        //        this.engine = engine;
        //        textToSpeech.SetEngineByPackageName(this.engine);
        //    }

        //    if (language != null)
        //    {
        //        this.language = language;
        //        textToSpeech.SetLanguage(this.language);
        //    }

        //    if (pitch.HasValue)
        //    {
        //        this.pitch = pitch.Value;
        //        textToSpeech.SetPitch(this.pitch);
        //    }

        //    if (speakRate.HasValue)
        //    {
        //        this.speakRate = speakRate.Value;
        //        textToSpeech.SetSpeechRate(this.speakRate);
        //    }

        //    SpeakText(text);
        //}



        private void SpeakText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            if (!queue && _tts.IsSpeaking)
                _tts.Stop();

            _tts.Speak(text, queue ? QueueMode.Add : QueueMode.Flush, null);
        }

        ///// <summary>
        ///// Get all installed and valide lanaguages
        ///// </summary>
        ///// <returns>List of CrossLocales</returns>
        //public IEnumerable<CrossLocale> GetInstalledLanguages()
        //{
        //    if (textToSpeech != null && initialized)
        //    {
        //        int version = (int)global::Android.OS.Build.VERSION.SdkInt;
        //        bool isLollipop = version >= 21;
        //        if (isLollipop)
        //        {
        //            try
        //            {
        //                //in a different method as it can crash on older target/compile for some reason
        //                return GetInstalledLanguagesLollipop();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Something went horribly wrong, defaulting to old implementation to get languages: " + ex);
        //            }
        //        }

        //        var languages = new List<CrossLocale>();
        //        var allLocales = Locale.GetAvailableLocales();
        //        foreach (var locale in allLocales)
        //        {

        //            try
        //            {
        //                var result = textToSpeech.IsLanguageAvailable(locale);

        //                if (result == LanguageAvailableResult.CountryAvailable)
        //                {
        //                    languages.Add(new CrossLocale { Country = locale.Country, Language = locale.Language, DisplayName = locale.DisplayName });
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error checking language; " + locale + " " + ex);
        //            }
        //        }

        //        return languages.GroupBy(c => c.ToString())
        //              .Select(g => g.First());
        //    }
        //    else
        //    {
        //        return Locale.GetAvailableLocales()
        //          .Where(a => !string.IsNullOrWhiteSpace(a.Language) && !string.IsNullOrWhiteSpace(a.Country))
        //          .Select(a => new CrossLocale { Country = a.Country, Language = a.Language, DisplayName = a.DisplayName })
        //          .GroupBy(c => c.ToString())
        //          .Select(g => g.First());
        //    }
        //}

        ///// <summary>
        ///// In a different method as it can crash on older target/compile for some reason
        ///// </summary>
        ///// <returns></returns>
        //private IEnumerable<CrossLocale> GetInstalledLanguagesLollipop()
        //{
        //    return textToSpeech.AvailableLanguages
        //      .Select(a => new CrossLocale { Country = a.Country, Language = a.Language, DisplayName = a.DisplayName });

        //}


        public ActionResult SetLanguage(Language lang)
        {

            if (lang == Language.zh_CN)
            {
                _language = Locale.China;
            }
            else if (lang == Language.en_US)
            {
                _language = Locale.Us;
            }
            else
            {
                _language = Locale.Default;
            }
            var res = _tts.SetLanguage(_language);
            if (res == LanguageAvailableResult.NotSupported || res == LanguageAvailableResult.MissingData)
            {
                return new ActionResult()
                {
                    Result = false,
                    Message = "数据丢失或不支持"
                };
            }
            else
            {
                return new ActionResult()
                {
                    Result = true
                };
            }
        }

        public bool HasInit { get;private set; }

        public ITextToSpeech New()
        {

            return new TextToSpeech();
        }

        public ActionResult SetEngine(string eng)
        {

            this._engine = eng;
            var res = _tts.SetEngineByPackageName(this._engine);
            if (res == OperationResult.Success)
            {
                return new ActionResult()
                {
                    Result = true
                };
            }
            else
            {
                return new ActionResult()
                {
                    Result = false
                };
            }
        }


        

        public void Speak(string text)
        {
            SpeakText(text);
        }

        public string CurrentEngine { get { return this._engine; } }

        //public event EventHandler NoEngine;


        public List<Engine> GetEngines()
        {
            return _tts.Engines.Select(w => new Engine()
            {
                Name = w.Name,
                Label = w.Label,

            }).ToList();
        }

        //bool IsSupportChinese(Locale locale)
        //{
        //    var r = textToSpeech.IsLanguageAvailable(Locale.China);
        //    return r != LanguageAvailableResult.MissingData && r != LanguageAvailableResult.NotSupported;
        //}
        new public void Dispose()
        {
            if (_tts != null)
            {
                _tts.Stop();
                _tts.Shutdown();
                _tts.Dispose();
                _tts = null;
            }
            base.Dispose(true);
        }

        public event EventHandler Inited;
        public event EventHandler InitedError;

        //void IDisposable.Dispose()
        //{
        //    if (_textToSpeech != null)
        //    {
        //        _textToSpeech.Stop();
        //        _textToSpeech.Shutdown();
        //        _textToSpeech.Dispose();
        //        _textToSpeech = null;
        //    }
        //}
    }


}