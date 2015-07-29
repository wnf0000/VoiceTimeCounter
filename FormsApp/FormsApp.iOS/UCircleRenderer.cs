using System;
using System.ComponentModel;
using FormsApp.iOS;
using FormsApp.View;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(UCircle), typeof(UCircleRenderer))]
namespace FormsApp.iOS
{
    public class UCircleRenderer : ViewRenderer<UCircle, UILabel>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<UCircle> e)
        {
            if (e.OldElement != null || Element == null)
                return;
            if (Control == null)
            {
                var view = new UILabel();
                SetNativeControl(view);
            }
            try
            {
                //double min = Math.Min(Element.Width, Element.Height);
                Control.Layer.CornerRadius = Element.Radius;
                Control.Layer.MasksToBounds = false;
                Control.Layer.BorderColor = Element.BorderColor.ToCGColor();
                Control.Layer.BorderWidth = Element.BorderWidth;
                Control.Layer.BackgroundColor = Element.FillColor.ToCGColor();
                Control.Text = Element.Text;
                Control.TextColor = Element.TextColor.ToUIColor();
                Control.TextAlignment = UITextAlignment.Center;
                Control.Font = Element.Font.ToUIFont();
                Control.ClipsToBounds = true;
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
            var textView = NativeView as UITextView;
            if (e.PropertyName == UCircle.TextProperty.PropertyName)
            {
                textView.Text = Element.Text;
            }
            else if (e.PropertyName == UCircle.FontProperty.PropertyName)
            {
                textView.Font = Element.Font.ToUIFont();
            }
            else if (e.PropertyName == UCircle.RadiusProperty.PropertyName)
            {
                textView.Layer.CornerRadius = Element.Radius;
            }
            else if (e.PropertyName == UCircle.BorderColorProperty.PropertyName)
            {
                textView.Layer.BorderColor=Element.BorderColor.ToCGColor();
            }
            else if (e.PropertyName == UCircle.FillColorProperty.PropertyName)
            {
                textView.BackgroundColor = Element.FillColor.ToUIColor();
                textView.Layer.BackgroundColor = Element.FillColor.ToCGColor();
                
            }
            else if (e.PropertyName == UCircle.BorderWidthProperty.PropertyName)
            {
                textView.Layer.BorderWidth = Element.BorderWidth;

            }
            base.OnElementPropertyChanged(sender, e);
            //if (e.PropertyName == UCircle.TextProperty.PropertyName ||
            //    e.PropertyName == UCircle.FontProperty.PropertyName ||
            //    e.PropertyName == UCircle.RadiusProperty.PropertyName ||
            //    e.PropertyName == UCircle.BorderColorProperty.PropertyName ||
            //    e.PropertyName == UCircle.FillColorProperty.PropertyName
            //    )
            //{ }
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            //return base.GetDesiredSize(widthConstraint, heightConstraint);
            var virtualElm = (UCircle)Element;
            //var radius = Context.Dip2Px(virtualElm.Radius);
            var radius = virtualElm.Radius;
            return new SizeRequest(new Size(radius * 2, radius * 2));
        }
    }
}