using FTCollectorApp.Service;
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
    public partial class MapPage : ContentPage
    {

        public MapPage()
        {
            InitializeComponent();            
        }

        /*private async void GetLocation()
        {
            var status = await CheckAndRequestLocationPermission();
            if (status == PermissionStatus.Granted)
            {
                var location = await Geolocation.GetLocationAsync();
                locationsMap.IsShowingUser = true;
                Coords = $"{location.Latitude},{location.Longitude}";
                Accuracy = $"{location.Accuracy}";
                txtLocation.Text = Coords;
                txtAccuracy.Text = Accuracy;
            }
        }

        private async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // permission 
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status;


        }*/

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LocateService.GetLocation();
            txtLocation.Text = $"{LocateService.Coords.Latitude}, {LocateService.Coords.Longitude}";
            txtAccuracy.Text = $"{LocateService.Coords.Accuracy}";

        }

        private async void btnClick_Clicked(object sender, EventArgs e)
        {
            await LocateService.GetLocation();
            txtLocation.Text = $"{LocateService.Coords.Latitude}, {LocateService.Coords.Longitude}";
            txtAccuracy.Text = $"{LocateService.Coords.Accuracy}";
        }
    }
}