using Android.Content;

namespace FormsApp.Droid
{
    public static class Extention
    {
        public static int Px2Dip(this Context context, float pxValue) { 
            float scale = context.Resources.DisplayMetrics.Density; 
            return (int) (pxValue / scale + 0.5f); 
        }

        public static int Dip2Px(this Context context, float dipValue)
        { 
             float scale = context.Resources.DisplayMetrics.Density; 
            return (int) (dipValue * scale + 0.5f); 
        }


        public static int Px2Sp(this Context context, float pxValue)
        { 
            float fontScale = context.Resources.DisplayMetrics.ScaledDensity; 
            return (int) (pxValue / fontScale + 0.5f); 
        }


        public static int Sp2Px(this Context context, float spValue)
        { 
            float fontScale = context.Resources.DisplayMetrics.ScaledDensity; 
            return (int) (spValue * fontScale + 0.5f); 
        } 
    }
}
