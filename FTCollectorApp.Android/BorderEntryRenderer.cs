using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FTCollectorApp.Droid.UI.Renderers;
using FTCollectorApp.Utils;

[assembly: ExportRenderer(typeof(BorderEntry), typeof(BorderEntryRenderer))]
namespace FTCollectorApp.Droid.UI.Renderers
{

    public class BorderEntryRenderer : EntryRenderer
    {
        public BorderEntryRenderer(Context context) : base(context)
        {
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                //Control.SetBackgroundResource(Resource.Layout.rounded_shape);
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(5f);
                gradientDrawable.SetStroke(2, Android.Graphics.Color.Black);
                //gradientDrawable.SetColor(Android.Graphics.Color.LightGray);
                Control.SetBackground(gradientDrawable);

                //Control.SetPadding(10, Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);
                Control.SetPadding(10, 2, Control.PaddingRight,2);
            }
        }
    }
}