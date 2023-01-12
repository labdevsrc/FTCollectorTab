using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using FTCollectorApp.Model;
using FTCollectorApp.Utils;

namespace FTCollectorApp.ViewModel
{
    public partial class OffsetGPSPopUpViewModel : ObservableObject
    {
        public OffsetGPSPopUpViewModel()
        {

        }
        [ObservableProperty]
        string bearing = "0";

        [ObservableProperty]
        string distance = "0";


        [ICommand]
        async void Save()
        {

            // live coords is offset_coordss
            Session.lattitude_offset = Session.live_lattitude;
            Session.longitude_offset = Session.live_longitude;

            // put bearing and distance as AWS table params
            Session.gps_offset_bearing = Bearing;
            Session.gps_offset_distance = Distance;

            Position pos1 = new Position();
            pos1.Latitude = double.Parse(Session.lattitude_offset);
            pos1.Longitude = double.Parse(Session.longitude_offset);



            // compute actual coords based on bearing and distance 

            Haversine hv = new Haversine();
            Position newPos = new Position();
            newPos = hv.NewCoordsCalc(pos1, double.Parse(Bearing), int.Parse(Distance),
                DistanceType.Kilometers);

            // Session.lattitude, Session.longitude : computed coords with Haversine formula
            Session.lattitude2 = newPos.Latitude.ToString();
            Session.longitude2 = newPos.Longitude.ToString();

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(true);
        }

        [ICommand]
        async void Close()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(true);
        }
    }
}
