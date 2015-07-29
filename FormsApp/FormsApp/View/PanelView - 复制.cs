using Xamarin.Forms;

namespace FormsApp.View
{
    [ContentProperty("Content")]
    public class PanelView : TitleBarView
    {
        public static readonly BindableProperty ContentProperty =
                BindableProperty.Create<PanelView, Xamarin.Forms.View>(p => p.Content, null);
        public Xamarin.Forms.View Content
        {
            get { return (Xamarin.Forms.View)GetValue(ContentProperty); }
            set
            {
                SetValue(ContentProperty, value);
                if (Children.Count == 3)
                {
                    Children[3] = Content;
                }
                else
                {
                    Children.Add(Content);
                }
            }
        }
        public PanelView()
        {

        }
       
    }
}
