using Android.App;
using Android.Content.PM;
using FormsApp.Droid;
using System;
using FormsApp.Service;

[assembly: Xamarin.Forms.Dependency(typeof(Serv))]
namespace FormsApp.Droid
{
    public class Serv : IServ
    {
        public int GetAppVersion()
        {
            PackageInfo packInfo = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0);
            int version = packInfo.VersionCode;
            return version;
        }

        public string GetAppVersionName()
        {
            PackageInfo packInfo = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0);
            String version = packInfo.VersionName;
            return version;
        }
    }
}