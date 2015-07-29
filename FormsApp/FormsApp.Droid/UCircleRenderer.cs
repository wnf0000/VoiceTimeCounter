using System.ComponentModel;
using Android.Graphics;
using FormsApp.Droid;
using FormsApp.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(UCircle), typeof(UCircleRenderer))]
namespace FormsApp.Droid
{

    public class UCircleRenderer : ViewRenderer<UCircle, global::Android.Views.View>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<UCircle> e)
        {
            Element.Invalidate();
            base.OnElementChanged(e);

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == UCircle.TextProperty.PropertyName ||
                e.PropertyName == UCircle.FontProperty.PropertyName ||
                e.PropertyName == UCircle.RadiusProperty.PropertyName ||
                e.PropertyName == UCircle.BorderColorProperty.PropertyName ||
                e.PropertyName == UCircle.FontProperty.PropertyName ||
                e.PropertyName == UCircle.FillColorProperty.PropertyName
                )
                Invalidate();

        }

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            var virtualElm = (UCircle)Element;
            var radius = Context.Dip2Px(virtualElm.Radius);
            return new SizeRequest(new Size(radius * 2, radius * 2));
        }
        protected override void OnDraw(Canvas canvas)
        {

            var virtualElm = (UCircle)Element;
            if (virtualElm.Radius == default(int)) return;

            var paint = new Paint();

            var borderWidth = Context.Dip2Px(virtualElm.BorderWidth);
            paint.StrokeWidth = borderWidth;
            paint.SetStyle(Paint.Style.Stroke);
            paint.AntiAlias = true;
            var radius = Context.Dip2Px(virtualElm.Radius);


            if (virtualElm.BorderWidth == 0)
            {
                var fillColor = virtualElm.FillColor.ToAndroid();
                paint.SetStyle(Paint.Style.Fill);
                paint.StrokeWidth = 1;
                paint.SetARGB(fillColor.A, fillColor.R, fillColor.G, fillColor.B);

                canvas.DrawCircle(radius, radius, radius, paint);
                //canvas.DrawOval(new RectF(0, 0, radius*2, radius*2), paint);
            }
            else
            {
                var fillColor = virtualElm.FillColor.ToAndroid();
                var bdColor = virtualElm.BorderColor.ToAndroid();
                //paint.SetARGB(bgColor.A, bgColor.R, bgColor.G, bgColor.B);
                //paint.StrokeWidth = 2;
                //canvas.DrawCircle(virtualElm.Radius, virtualElm.Radius, virtualElm.Radius - 2, paint);
                //绘制内圆  
                paint.SetStyle(Paint.Style.Fill);
                paint.SetARGB(fillColor.A, fillColor.R, fillColor.G, fillColor.B);
                paint.StrokeWidth = 1;
                canvas.DrawCircle(radius, radius, radius - borderWidth, paint);
                //绘制圆环  
                paint.SetStyle(Paint.Style.Stroke);
                paint.SetARGB(bdColor.A, bdColor.R, bdColor.G, bdColor.B);
                paint.StrokeWidth = borderWidth;
                canvas.DrawCircle(radius, radius, radius - borderWidth, paint);

                ////绘制内圆  
                //paint.SetARGB(bgColor.A, bgColor.R, bgColor.G, bgColor.B);
                //paint.StrokeWidth = 1;
                //canvas.DrawCircle(virtualElm.Radius, virtualElm.Radius, virtualElm.Radius, paint);
            }
            var txtColor = virtualElm.TextColor.ToAndroid();
            paint.SetStyle(Paint.Style.Fill);
            paint.SetARGB(txtColor.A, txtColor.R, txtColor.G, txtColor.B);
            paint.SetTypeface(virtualElm.Font.ToTypeface());

            paint.TextSize = Context.Sp2Px((float)virtualElm.Font.FontSize);

            float tX = (radius * 2 - GetFontlength(paint, virtualElm.Text)) / 2;
            float tY = (radius * 2 - GetFontHeight(paint)) / 2 + GetFontLeading(paint);
            canvas.DrawText(virtualElm.Text, tX, tY, paint);

            base.OnDraw(canvas);
            paint.Dispose();
            canvas.Dispose();
        }
        float GetFontlength(Paint paint, string str)
        {
            return paint.MeasureText(str);
        }
        float GetFontHeight(Paint paint)
        {
            Paint.FontMetrics fm = paint.GetFontMetrics();
            return fm.Descent - fm.Ascent;
        }
        float GetFontLeading(Paint paint)
        {
            Paint.FontMetrics fm = paint.GetFontMetrics();
            return fm.Leading - fm.Ascent;
        }

    }
}