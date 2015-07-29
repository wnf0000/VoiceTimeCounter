using FormsApp.iOS;
using FormsApp.Service;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(Serv))]
namespace FormsApp.iOS
{
    class Serv : IServ
    {
        public int GetAppVersion()
        {
            var version = int.Parse(NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString());
            return version;
        }

        public string GetAppVersionName()
        {
            var version = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
            return version;
        }
    }
}