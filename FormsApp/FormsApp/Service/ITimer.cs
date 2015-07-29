using System;

namespace FormsApp.Service
{
  public  interface ITimer:IDisposable
  {
      //event EventHandler Tick;
      bool Enabled { set; get; }
      double Interval { set; get; }
      bool AutoReset { set; get; }
      void Start();
      void Stop();
      void OnTick(Action<DateTime> action);
      ITimer New();
  }
}
