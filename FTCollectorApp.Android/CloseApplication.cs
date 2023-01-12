using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FTCollectorApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace FTCollectorApp.Droid
{
    public class CloseApplication : ICloseApps
    {
        public void closeApplication()
        {
            var activity = (Activity)Android.App.Application.Context;
            activity.FinishAffinity();
            //CrossCurrentActivity.Current.Activity.FinishAndRemoveTask();
        }
    }
}