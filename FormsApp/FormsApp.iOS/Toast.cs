using FormsApp.iOS;
using FormsApp.Service;

[assembly: Xamarin.Forms.Dependency(typeof(Toast))]
namespace FormsApp.iOS
{
    class Toast : IToast
    {
        public void Show(string text, ToastLenght lenght = ToastLenght.Short)
        {
            //if (lenght == ToastLenght.Short)
            //{
            //    Android.Widget.Toast.MakeText(Android.App.Application.Context, text, ToastLength.Short).Show();
            //}
            //else
            //{
            //    Android.Widget.Toast.MakeText(Android.App.Application.Context, text, ToastLength.Long).Show();
            //}
        }
    }
}