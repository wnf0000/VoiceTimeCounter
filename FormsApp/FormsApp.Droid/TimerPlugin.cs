using System;
using FormsApp.Service;

[assembly: Xamarin.Forms.Dependency(typeof(FormsApp.Droid.Timer))]
namespace FormsApp.Droid
{
    class Timer : System.Timers.Timer, ITimer
    {
        public void OnTick(Action<DateTime> action)
        {
            this.Elapsed += (s, e) => action(e.SignalTime);
        }

        public ITimer New()
        {
            return new Timer();
        }
    }
}