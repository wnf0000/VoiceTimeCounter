using System.Collections.Generic;
using System.Timers;
using Android.App;
using Android.Widget;
using Android.OS;

namespace VoiceTimeCounter.Droid
{
    [Activity(Label = "语音定时",Theme="@android:style/Theme.Holo.Light", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private TextToSpeech tts;
        private Timer timer;
        private int seconds = 0;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            tts= new TextToSpeech();

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            timer=new Timer(1000);
            timer.Elapsed += timer_Elapsed;
            button.Click += delegate
            {
                seconds = 60;
                timer.Start();
                tts.Speak("开始");
                //tts.Speak("go！");
            };
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            string text = string.Format("{0}", seconds);
            tts.Speak(text);
            seconds--;
            if(seconds<=0)
                timer.Stop();
        }


    }
}

