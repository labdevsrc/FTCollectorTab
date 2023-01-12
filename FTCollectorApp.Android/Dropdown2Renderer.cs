using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FTCollectorApp.Droid;
using FTCollectorApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Dropdown2), typeof(Dropdown2Renderer))]
namespace FTCollectorApp.Droid
{
    public class Dropdown2Renderer : PickerRenderer
    {
        public Dropdown2Renderer(Context context) : base(context)
        {

        }

        //protected override void OnElementChanged(ElementChangedEventArgs<Dropdown> e)
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            /*if (Control == null)
            {
                spinner = new AppCompatSpinner(Context);
                SetNativeControl(spinner);
            }

            if (e.OldElement != null)
            {
                Control.ItemSelected -= OnItemSelected;
            }*/

            if (e.OldElement == null)
            {
                //Control.SetBackgroundResource(Resource.Layout.rounded_shape);
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(5f);
                gradientDrawable.SetStroke(3, Android.Graphics.Color.Black);
                //gradientDrawable.SetColor(Android.Graphics.Color.LightGray);
                Control.SetBackground(gradientDrawable);

                Control.SetPadding(50, Control.PaddingTop, Control.PaddingRight,
                    Control.PaddingBottom);
            }
        }
    }
}