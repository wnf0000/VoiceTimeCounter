
using Android.Runtime;

namespace FormsApp.Droid
{
    [Preserve]
   public class LinkerInclude
   {
       [Preserve]
        public void A()
        {
            Serv serv=new Serv();
            serv.GetAppVersion();
            Toast toast=new Toast();
            toast.Show("hello");
        }
    }
}