using Xamarin.Forms;

namespace FormsApp.View
{
    [ContentProperty("Text")]
    public class UCircle : Xamarin.Forms.View
    {
        public static readonly BindableProperty RadiusProperty =
             BindableProperty.Create<UCircle, int>(p => p.Radius, default(int));

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create<UCircle, Color>(p => p.BorderColor, default(Color));

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create<UCircle, Color>(p => p.TextColor, Color.Black);

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create<UCircle, int>(p => p.BorderWidth, default(int));

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create<UCircle, string>(p => p.Text, string.Empty);

        public static readonly BindableProperty FontProperty =
            BindableProperty.Create<UCircle, Font>(p => p.Font, Font.SystemFontOfSize(12));

        public static readonly BindableProperty FillColorProperty =
            BindableProperty.Create<UCircle, Color>(p => p.FillColor, Color.Transparent);
        
        
        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value);  }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value);  }
        }
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value);  }
        }
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value);  }
        }
        public Font Font
        {
            get { return (Font)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }
        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }
        //new public Color BackgroundColor
        //{
        //    get { return (Color)GetValue(BackgroundColorProperty); }
        //    set { SetValue(BackgroundColorProperty, value); }
        //}
        public void Invalidate()
        {
            BackgroundColor = Color.Transparent;
        }
    }
}
