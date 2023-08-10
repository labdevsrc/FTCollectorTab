using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTCollectorApp.Model;
using FTCollectorApp.Utils;
using FTCollectorApp.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OffsetGPSPopUp
    {
        public OffsetGPSPopUp()
        {
            InitializeComponent();
            BindingContext = new OffsetGPSPopUpViewModel();
        }

        /*protected override void OnAppearing()
        {
            base.OnAppearing();
            btnClose.Clicked += (s, e) => PopupNavigation.Instance.PopAsync(true);
         }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            // live coords is offset_coords
            Session.lattitude_offset = Session.live_lattitude;
            Session.longitude_offset = Session.live_longitude;

            // put bearing and distance as AWS table params
            Session.gps_offset_bearing = entryBearing.Text;
            Session.gps_offset_distance = entryDistance.Text;

            Position pos1 = new Position();
            pos1.Latitude = double.Parse(Session.lattitude_offset);
            pos1.Longitude = double.Parse(Session.longitude_offset);



            // compute actual coords based on bearing and distance 
            
            Haversine hv = new Haversine();
            Position newPos = new Position();
            newPos = hv.NewCoordsCalc(pos1, double.Parse(entryBearing.Text), int.Parse(entryDistance.Text),
                DistanceType.Kilometers);

            // Session.lattitude, Session.longitude : computed coords with Haversine formula
            Session.lattitude2 = newPos.Latitude.ToString();
            Session.longitude2 = newPos.Longitude.ToString();

            await PopupNavigation.Instance.PopAsync(true);
        }*/
    }
}