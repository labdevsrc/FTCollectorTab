using FTCollectorApp.Model;
using FTCollectorApp.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GpsDevicePopUpView 
    {

        Location _location;

        private const string DeviceCheckedKey = "device_checked";
        private const string ExternalCheckedKey = "external_checked";
        private const string ManualCheckedKey = "manual_checked";
        private const string ManualLattitudeKey = "manual_lattitude";
        private const string ManualLongitudeKey = "manual_longitude";

        // Turn IsBusy to bind with XAML component Activity

        public GpsDevicePopUpView()
        {
            InitializeComponent();
            Session.manual_latti = "0";
            Session.manual_longi = "0";
            Session.lattitude2 = "0";
            Session.longitude2 = "0";
            BindingContext = this;
            this.CloseWhenBackgroundIsClicked = false;
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;

            if (Application.Current.Properties.ContainsKey(DeviceCheckedKey))
            {
                deviceChecked.IsChecked = (bool) Application.Current.Properties[DeviceCheckedKey];
            }
            if (Application.Current.Properties.ContainsKey(ExternalCheckedKey))
            {
                externalChecked.IsChecked = (bool) Application.Current.Properties[ExternalCheckedKey];
            }
            if (Application.Current.Properties.ContainsKey(ManualCheckedKey))
            {
                btnNoGPS.IsChecked = (bool) Application.Current.Properties[ManualCheckedKey];
            }

            if (Application.Current.Properties.ContainsKey(ManualLattitudeKey))
            {
                entryLat.Text = Application.Current.Properties[ManualLattitudeKey].ToString();
            }
            if (Application.Current.Properties.ContainsKey(ManualLongitudeKey))
            {
                entryLon.Text = Application.Current.Properties[ManualLongitudeKey].ToString();
            }

            await LocationService.GetLocation();
            if (LocationService.Coords != null)
            {
                _location = LocationService.Coords;
                txtAccuracy.Text = $"Accuracy is {String.Format("{0:0.###} m", _location.Accuracy.ToString())}";
                txtCoords.Text = $"Current Point is {String.Format("{0:0.#######}", _location.Latitude.ToString())} ,{String.Format("{0:0.#######}", _location.Longitude.ToString())} ";
                Session.gps_sts = "1";
                Session.lattitude2 = _location.Latitude.ToString();
                Session.longitude2 = _location.Longitude.ToString();
                Session.altitude = _location.Altitude.ToString();
                Session.accuracy = _location.Accuracy.ToString();
            }
            else
            {
                txtAccuracy.Text = "Location Service disabled";
                Session.gps_sts = "0";
                Session.accuracy = "10000";

            }

            Console.WriteLine($"GpsPopupView [OnAppearing]");

            IsBusy = false;

        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {

            // reset all first
            Application.Current.Properties[DeviceCheckedKey] = false;
            Application.Current.Properties[ExternalCheckedKey] = false;
            Application.Current.Properties[ManualCheckedKey] = false;
            Application.Current.Properties[ManualLattitudeKey] = 0;

            Application.Current.Properties[ManualLongitudeKey] = 0;

            if (deviceChecked.IsChecked)
                Application.Current.Properties[DeviceCheckedKey] = deviceChecked.IsChecked;
            else if (externalChecked.IsChecked)
                Application.Current.Properties[ExternalCheckedKey] = externalChecked.IsChecked;
            else if (btnNoGPS.IsChecked)
            {
                if (entryLat.Text.Length < 2 || entryLon.Text.Length < 2)
                {
                    DisplayAlert("warning", "Coords must be 8 digit", "OK");
                    return;
                }
                //IsValidCoords(entryLat.Text, entryLon.Text); cek if valid GPS coords
                Application.Current.Properties[ManualCheckedKey] = btnNoGPS.IsChecked;
                Application.Current.Properties[ManualLattitudeKey] = double.Parse(entryLat.Text);
                Application.Current.Properties[ManualLongitudeKey] = double.Parse(entryLon.Text);
            }
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async void DeviceChecked(object sender, CheckedChangedEventArgs e)
        {
            _location = LocationService.Coords;
            if (_location == null)
            {
                await LocationService.GetLocation();

                Console.WriteLine($"[DeviceChecked] Retry GPS");
            }

            if (_location != null)
            {
                var accuracy = String.Format("{0:0.###} m", _location.Accuracy.ToString());
                var lattitude = String.Format("{0:0.#######}", _location.Latitude.ToString());
                var longitude = String.Format("{0:0.#######}", _location.Longitude.ToString());
                var altitude = String.Format("{0:0.#}", _location.Altitude.ToString());
                txtAccuracy.Text = $"Accuracy is {accuracy}";
                txtCoords.Text = $"Current Point is {lattitude} ,{longitude} ";
                Session.gps_sts = "1";
                Session.lattitude2 = lattitude;
                Session.longitude2 = longitude;
                Session.altitude = altitude;
                Session.accuracy = accuracy;
                Console.WriteLine($"[DeviceChecked] Coords {lattitude}, {longitude}");
            }
            else
            {
                txtAccuracy.Text = "Location Service disabled";
            }


        }

        private async void ExternalChecked(object sender, CheckedChangedEventArgs e)
        {
            try
            {
                _location = LocationService.Coords;
                if (_location == null)
                {
                    await LocationService.GetLocation();
                }

                if (_location != null)
                {
                    var accuracy = String.Format("{0:0.###} m", _location.Accuracy.ToString());
                    var lattitude = String.Format("{0:0.#######}", _location.Latitude.ToString());
                    var longitude = String.Format("{0:0.#######}", _location.Longitude.ToString());
                    var altitude = String.Format("{0:0.#}", _location.Altitude.ToString());
                    txtAccuracy.Text = $"Accuracy is {accuracy}";
                    txtCoords.Text = $"Current Point is {lattitude} ,{longitude} ";
                    Session.gps_sts = "1";
                    Session.lattitude2 = lattitude;
                    Session.longitude2 = longitude;
                    Session.altitude = altitude;
                    Session.accuracy = accuracy;
                    Console.WriteLine($"[ExternalChecked] Coords {lattitude}, {longitude}");
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine($"[ExternalChecked] Exception {exp.ToString()}");
            }
        }

        private void NoGPSChecked(object sender, CheckedChangedEventArgs e)
        {

            Session.gps_sts = "0";
            //if (string.IsNullOrEmpty(entryLat.Text) || string.IsNullOrEmpty(entryLon.Text))
            //    return;

            Session.manual_latti = String.Format("{0:0.#######}", entryLat.Text);
            Session.manual_longi = String.Format("{0:0.#######}", entryLon.Text);

        }
    }
}