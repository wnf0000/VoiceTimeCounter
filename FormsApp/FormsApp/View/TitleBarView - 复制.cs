using System.Collections.Generic;
using Xamarin.Forms;

namespace FormsApp.View
{
    public class TitleBarView : StackLayout
    {
        private Label textLabel;
        private LineView lineView;
        public static readonly BindableProperty TextProperty =
              BindableProperty.Create<TitleBarView, string>(p => p.Text, default(string));
        public static readonly BindableProperty TextColorProperty =
             BindableProperty.Create<TitleBarView, Color>(p => p.TextColor, Color.FromHex("#666"));
        public static readonly BindableProperty UnderLineColorProperty =
             BindableProperty.Create<TitleBarView, Color>(p => p.UnderLineColor, Color.FromHex("#666"));
        public static readonly BindableProperty UnderLineHeightProperty =
             BindableProperty.Create<TitleBarView, int>(p => p.UnderLineHeight, 1);
        public static readonly BindableProperty FontSizeProperty =
             BindableProperty.Create<TitleBarView, double>(p => p.FontSize, 16);
        public static readonly BindableProperty FontAttributesProperty =
             BindableProperty.Create<TitleBarView, FontAttributes>(p => p.FontAttributes, FontAttributes.Bold);
        public static readonly BindableProperty FontFamilyProperty =
             BindableProperty.Create<TitleBarView, string>(p => p.FontFamily, null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        public Color UnderLineColor
        {
            get { return (Color)GetValue(UnderLineColorProperty); }
            set { SetValue(UnderLineColorProperty, value); }
        }
        public int UnderLineHeight
        {
            get { return (int)GetValue(UnderLineHeightProperty); }
            set { SetValue(UnderLineHeightProperty, value); }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        new protected IList<Xamarin.Forms.View> Children {  get { return base.Children; } }
        public TitleBarView()
        {
            //Spacing = 0;
            //Padding=new Thickness(10,0);
            Orientation = StackOrientation.Vertical;
            textLabel = new Label();
            textLabel.BindingContext = this;
            lineView = new LineView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            lineView.BindingContext = this;

            textLabel.SetBinding(Label.TextProperty,"Text");
            textLabel.SetBinding(Label.TextColorProperty, "TextColor");
            textLabel.SetBinding(Label.FontAttributesProperty, "FontAttributes");

            textLabel.SetBinding(Label.FontFamilyProperty, "FontFamily");

            textLabel.SetBinding(Label.FontSizeProperty, "FontSize");

            lineView.SetBinding(LineView.BackgroundColorProperty, "UnderLineColor");

            lineView.SetBinding(LineView.HeightRequestProperty, "UnderLineHeight");

            Children.Add(textLabel);
            Children.Add(lineView);

        }
    }
}
