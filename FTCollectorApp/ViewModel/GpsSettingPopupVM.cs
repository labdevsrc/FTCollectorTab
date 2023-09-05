using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;
using FTCollectorApp.Services;
using FTCollectorApp.Model;
using FTCollectorApp.View.Utils;

namespace FTCollectorApp.ViewModel
{
    public partial class GpsSettingPopupVM : ObservableObject
    {
        Location _location;

        private const string DeviceCheckedKey = "device_checked";
        private const string ExternalCheckedKey = "external_checked";
        private const string ManualCheckedKey = "manual_checked";
        private const string ManualLattitudeKey = "manual_lattitude";
        private const string ManualLongitudeKey = "manual_longitude";


        [ObservableProperty] bool isPhoneGPS;
        [ObservableProperty] bool isExternalGNSS;
        [ObservableProperty] bool isManualInput;
        [ObservableProperty] bool isBusy = false;
        [ObservableProperty] string entryLatitude;
        [ObservableProperty] string entryLongitude;
        [ObservableProperty] string accuracy;
        [ObservableProperty] string coords;

        ReadGPSTimer gpstimer;
        public GpsSettingPopupVM()
        {

            if (Application.Current.Properties.ContainsKey(DeviceCheckedKey))
            {
                IsPhoneGPS = (bool)Application.Current.Properties[DeviceCheckedKey];
            }
            if (Application.Current.Properties.ContainsKey(ExternalCheckedKey))
            {
                IsExternalGNSS = (bool)Application.Current.Properties[ExternalCheckedKey];
            }
            if (Application.Current.Properties.ContainsKey(ManualCheckedKey))
            {
                IsManualInput = (bool)Application.Current.Properties[ManualCheckedKey];
            }

            if (Application.Current.Properties.ContainsKey(ManualLattitudeKey))
            {
                EntryLatitude = Application.Current.Properties[ManualLattitudeKey].ToString();
            }
            if (Application.Current.Properties.ContainsKey(ManualLongitudeKey))
            {
                EntryLongitude = Application.Current.Properties[ManualLongitudeKey].ToString();
            }

            Coords = "Reading GPS Coords..";

            gpstimer = new ReadGPSTimer(TimeSpan.FromSeconds(5), OnGPSTimerStart);
            gpstimer.Start();
            //IsBusy = false;


        }


        [ICommand]
        async void SaveGPSSetting()
        {
            // reset all first
            Application.Current.Properties[DeviceCheckedKey] = false;
            Application.Current.Properties[ExternalCheckedKey] = false;
            Application.Current.Properties[ManualCheckedKey] = false;
            Application.Current.Properties[ManualLattitudeKey] = 0;

            Application.Current.Properties[ManualLongitudeKey] = 0;

            if (IsPhoneGPS)
                Application.Current.Properties[DeviceCheckedKey] = IsPhoneGPS;
            else if (IsExternalGNSS)
                Application.Current.Properties[ExternalCheckedKey] = IsExternalGNSS;
            else if (IsManualInput)
            {
                if (EntryLatitude.Length < 2 || EntryLongitude.Length < 2)
                {
                    Application.Current.MainPage.DisplayAlert("warning", "Coords must be 8 digit", "OK");
                    return;
                }
                //IsValidCoords(entryLat.Text, entryLon.Text); cek if valid GPS coords
                Application.Current.Properties[ManualCheckedKey] = IsManualInput;
                Application.Current.Properties[ManualLattitudeKey] = double.Parse(EntryLatitude);
                Application.Current.Properties[ManualLongitudeKey] = double.Parse(EntryLongitude);
            }

            gpstimer.Stop();

            await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
        }

        async void OnGPSTimerStart()
        {
            Console.WriteLine("10 sec timer ");
            try
            {
                IsBusy = true;
                await LocationService.GetLocation();

                Console.WriteLine($"OnGPSTimerStart Latitude : {LocationService.Coords.Latitude}, Longitude : {LocationService.Coords.Longitude} ");

                if (LocationService.Coords != null)
                {
                    _location = LocationService.Coords;
                    Accuracy = $"Accuracy is {String.Format("{0:0.###} m", _location.Accuracy.ToString())}";
                    Coords = $"Current Point is {String.Format("{0:0.#######}", _location.Latitude.ToString())} ,{String.Format("{0:0.#######}", _location.Longitude.ToString())} ";
                    Session.gps_sts = "1";
                    Session.lattitude2 = _location.Latitude.ToString();
                    Session.longitude2 = _location.Longitude.ToString();
                    Session.altitude = _location.Altitude.ToString();
                    Session.accuracy = _location.Accuracy.ToString();
                }
                else
                {
                    Accuracy = "Location Service disabled";
                    Session.gps_sts = "0";
                    Session.accuracy = "10000";

                }

                IsBusy = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception e " + e.ToString());
            }
        }
    }
}
