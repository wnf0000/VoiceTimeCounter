using System;
using Android.App;
using Android.Speech.Tts;
using Java.Util;

namespace VoiceTimeCounter.Droid
{
    /// <summary>
    /// Text to speech implementation Android
    /// </summary>
    public class TextToSpeech : Java.Lang.Object,  Android.Speech.Tts.TextToSpeech.IOnInitListener
    {
        Android.Speech.Tts.TextToSpeech textToSpeech;
        //string text;
        Locale language;
        float pitch, speakRate;
        bool queue;
        bool initialized;
        private string engine;
        /// <summary>
        /// Default constructor
        /// </summary>
        public TextToSpeech()
        {
            textToSpeech = new Android.Speech.Tts.TextToSpeech(Application.Context, this);
        }

        ///// <summary>
        ///// Initialize TTS
        ///// </summary>
        //public void Init(string engine = null, bool queue = false, Locale crossLocale = null, float? pitch = null, float? speakRate = null, float? volume = null)
        //{
        //    //Console.WriteLine("Current version: " + (int)global::Android.OS.Build.VERSION.SdkInt);
        //    //Android.Util.Log.Info("CrossTTS", "Current version: " + (int)global::Android.OS.Build.VERSION.SdkInt);
        //    this.language = crossLocale;
        //    this.pitch = pitch == null ? 1.0f : pitch.Value;
        //    this.speakRate = speakRate == null ? 1.0f : speakRate.Value;
        //    this.queue = queue;
            
            
        //    this.engine = string.IsNullOrEmpty(engine) ? textToSpeech.Engines[1].Name : engine;
        //    SetOptions(this.engine, this.queue, this.language, this.pitch, this.speakRate, volume.GetValueOrDefault(1));
        //}

        void SetOptions(string engine , bool queue , Locale locale , float pitch,
            float speakRate, float volume)
        {
            this.language = locale;
            this.pitch = pitch;
            this.speakRate = speakRate;
            this.queue = queue;
            this.engine = string.IsNullOrEmpty(engine) ? textToSpeech.Engines[1].Name : engine;
            if (textToSpeech == null)
            {
                textToSpeech = new Android.Speech.Tts.TextToSpeech(Application.Context, this);
                initialized = true;
            }
            textToSpeech.SetLanguage(this.language);
            textToSpeech.SetPitch(this.pitch);
            textToSpeech.SetSpeechRate(this.speakRate);
            textToSpeech.SetEngineByPackageName(this.engine);
            
        }
        #region IOnInitListener implementation
        /// <summary>
        /// OnInit of TTS
        /// </summary>
        /// <param name="status"></param>
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                SetOptions(textToSpeech.Engines[1].Name,false,null,1.0f,1.0f,1.0f);
                SetDefaultLanguageNonLollipop();
                initialized = true;
                //Speak();
            }
        }
        #endregion

        /// <summary>
        /// Speak back text
        /// </summary>
        /// <param name="text">Text to speak</param>
        /// <param name="queue">If you want to chain together speak command or cancel current</param>
        /// <param name="language">Locale of voice</param>
        /// <param name="pitch">Pitch of voice</param>
        /// <param name="speakRate">Speak Rate of voice (All) (0.0 - 2.0f)</param>
        /// <param name="volume">Volume of voice (iOS/WP) (0.0-1.0)</param>
        public void Speak(string text,string engine=null, bool queue = false, Locale language = null, float? pitch = null, float? speakRate = null, float? volume = null)
        {
            if (!string.IsNullOrEmpty(engine))
            {
                this.engine = engine;
                textToSpeech.SetEngineByPackageName(this.engine);
            }
       
            if (language != null)
            {
                this.language = language;
                textToSpeech.SetLanguage(this.language);
            }
           
            if (pitch.HasValue)
            {
                this.pitch = pitch.Value;
                textToSpeech.SetPitch(this.pitch);
            }
            
            if (speakRate.HasValue)
            {
                this.speakRate = speakRate.Value;
                textToSpeech.SetSpeechRate(this.speakRate);
            }
            
            SpeakText(text);
        }


        private void SetDefaultLanguage()
        {


            SetDefaultLanguageNonLollipop();
            /*int version = (int)global::Android.OS.Build.VERSION.SdkInt;
            bool isLollipop = version >= 21;
            if (isLollipop)
            {
              //in a different method as it can crash on older target/compile for some reason
              SetDefaultLanguageLollipop();
            }
            else
            {
              SetDefaultLanguageNonLollipop();
            }*/
        }

        private void SetDefaultLanguageNonLollipop()
        {
            //disable warning because we are checking ahead of time.
#pragma warning disable 0618
            if (textToSpeech.DefaultLanguage == null && textToSpeech.Language != null)
            {
                textToSpeech.SetLanguage(textToSpeech.Language);
                language = textToSpeech.Language;
            }
            else if (textToSpeech.DefaultLanguage != null)
            {
                textToSpeech.SetLanguage(textToSpeech.DefaultLanguage);
                language = textToSpeech.DefaultLanguage;
            }
#pragma warning restore 0618
        }

        /// <summary>
        /// In a different method as it can crash on older target/compile for some reason   
        /// </summary>
        private void SetDefaultLanguageLollipop()
        {
            /*if (textToSpeech.DefaultVoice != null)
            {
              textToSpeech.SetVoice(textToSpeech.DefaultVoice);
              if (textToSpeech.DefaultVoice.Locale != null)
                textToSpeech.SetLanguage(textToSpeech.DefaultVoice.Locale);
            }
            else
              SetDefaultLanguageNonLollipop();*/



        }

        private void SpeakText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            if (!queue && textToSpeech.IsSpeaking)
                textToSpeech.Stop();

            //if (
            //    language!=null && !string.IsNullOrWhiteSpace(language.Language))
            //{
            //    var result = textToSpeech.IsLanguageAvailable(language);
            //    if (result == LanguageAvailableResult.CountryAvailable)
            //    {
            //        textToSpeech.SetLanguage(language);
            //    }
            //    else
            //    {
            //        //Console.WriteLine("Locale: " + locale + " was not valid, setting to default.");
            //        SetDefaultLanguage();
            //    }
            //}
            //else
            //{
            //    SetDefaultLanguage();
            //}
            //var a=textToSpeech.AreDefaultsEnforced();
            
            textToSpeech.Speak(text, queue ? QueueMode.Add : QueueMode.Flush, null);
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

        void IDisposable.Dispose()
        {
            if (textToSpeech != null)
            {
                textToSpeech.Stop();
                textToSpeech.Dispose();
                textToSpeech = null;
            }
        }
    }
}