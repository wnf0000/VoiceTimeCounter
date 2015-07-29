
using Android.App;
using Android.Content.PM;
using Android.OS;
using FormsApp.Service;

namespace FormsApp.Droid
{
    [Activity(Label = "语音倒计时", Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        /*Theme="@android:style/Theme.Holo.Light",*/ MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            this.SetTheme(Android.Resource.Style.ThemeHoloLight);
            base.OnCreate(bundle);
            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            DbHelper.SetDbFolder(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
            DbHelper.Init();
            
            LoadApplication(new App());
            
        }
    }
}

