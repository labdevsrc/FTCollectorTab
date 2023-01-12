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
using Xamarin.Essentials;

namespace FTCollectorApp.Droid
{
    public class SettingsServiceAndroid : ISettingsService
    {
        public void OpenSettings()
        {
            var intent = new Android.Content.Intent(Android.Provider.Settings.ActionLocationSourceSettings);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}