using Xamarin.Forms;

namespace FormsApp.Page
{
    class StructPage:NavigationPage
    {
        public StructPage()
        {
            Title = "语音倒计时";
            this.BarTextColor = Color.White;
            this.BarBackgroundColor = Consts.ThemeColor;
            MainPage mainPage=new MainPage();
            PushAsync(mainPage);
        }
    }
}
