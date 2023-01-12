using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.View.TraceFiberPages;
using FTCollectorApp.View.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class LocatePointViewModel : ObservableObject
    {

        string gPSAccuracy;
        public string GPSAccuracy
        {
            get => LocationService.Coords.Accuracy.ToString();
            set
            {
                SetProperty(ref (gPSAccuracy), value);
            }
            
        }


        [ObservableProperty]
        string commentText;

        [ObservableProperty]
        CodeLocatePoint selectedSiteType;

        Location location;


        string curLocPoint;
        public string CurLocPoint
        {
            get {
                Console.WriteLine(LocPointNumber);
                return LocPointNumber.ToString();
            } 
            set
            {
                Console.WriteLine();
                SetProperty(ref curLocPoint, value);
            }
        }


        public ICommand RecordCommand { get; set; }
        public ICommand OpenGPSOffsetPopupCommand { get; set; }

        public ICommand FinishCommand { get; set; }
        public ICommand CaptureCommand { get; set; }

        GpsPoint maxGPSpoint;
        int LocPointNumber = 0;
        ReadGPSTimer RdGpstimer;

        public LocatePointViewModel()
        {


            maxGPSpoint = new GpsPoint
            {
                MaxId = "1"
            };

            CaptureCommand = new Command(() => ExecuteCaptureCommand());
            FinishCommand = new Command(() => ExecuteFinishCommand());
            OpenGPSOffsetPopupCommand = new Command(() => ExecuteOpenGPSOffsetPopupCommand());
            RecordCommand = new Command(() => ExecuteRecordCommand());

            // get max value in gps_point table
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<GpsPoint>();
                maxGPSpoint = conn.Table<GpsPoint>().First(); // table gps_point only contain single row
                if (int.Parse(maxGPSpoint.MaxId) < int.Parse(Session.GpsPointMaxIdx))
                    maxGPSpoint.MaxId = Session.GpsPointMaxIdx;
                // compare with from gps_point MAX(id)


                LocPointNumber = int.Parse(Session.GpsPointMaxIdx) + 1;

                Console.WriteLine(maxGPSpoint.MaxId);
            }

            Session.LocpointnumberStart = LocPointNumber.ToString(); // preserve for End Trace page


            Session.lattitude_offset = string.Empty;
            Session.longitude_offset = string.Empty;
            Session.gps_offset_bearing = string.Empty;
            Session.gps_offset_distance = string.Empty;

            if (RdGpstimer == null)
            {
                RdGpstimer = new ReadGPSTimer(TimeSpan.FromSeconds(5), OnGPSTimerStart);
                RdGpstimer.Start();
            }

            Session.current_page = "duct";

        }

        async void OnGPSTimerStart()
        {
            try
            {
                await LocationService.GetLocation();

                Session.accuracy = String.Format("{0:0.######}", LocationService.Coords.Accuracy);

                Session.live_longitude = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                Session.live_lattitude = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                Session.altitude = String.Format("{0:0.######}", LocationService.Coords.Altitude);

                OnPropertyChanged(nameof(GPSAccuracy));

            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

        }

        List<KeyValuePair<string, string>> keyvaluepairLocate()
        {
            //Session.GpsPointMaxIdx = (int.Parse(maxGPSpoint?.MaxId) + 1).ToString();

            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("jobnum", Session.jobnum),
                new KeyValuePair<string, string>("ownerkey", Session.ownerkey),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD),

                new KeyValuePair<string, string>("locate_point_number", LocPointNumber.ToString()),
                new KeyValuePair<string, string>("tag_from", Session.FromDuct?.HosTagNumber is null ? "0" :Session.FromDuct.HosTagNumber ),
                new KeyValuePair<string, string>("tag_from_key", Session.FromDuct?.HostSiteKey is null ? "0" :Session.FromDuct.HostSiteKey ),
                new KeyValuePair<string, string>("duct_from", Session.FromDuct?.ConduitKey is null ? "0" :Session.FromDuct.ConduitKey ),
                new KeyValuePair<string, string>("duct_from_key", Session.FromDuct?.ConduitKey is null ? "0" :Session.FromDuct.ConduitKey ),
                new KeyValuePair<string, string>("cable_key", Session.Cable1?.AFRKey is null ? "0" : Session.Cable1.AFRKey),
                new KeyValuePair<string, string>("cable_type", Session.Cable1?.CableType is null ? "0" : Session.Cable1.CableType),

                new KeyValuePair<string, string>("lattitude", Session.lattitude2),
                new KeyValuePair<string, string>("longitude", Session.longitude2),
                new KeyValuePair<string, string>("altitude", Session.altitude),
                new KeyValuePair<string, string>("accuracy", Session.accuracy),

                new KeyValuePair<string, string>("gps_offset_latitude", Session.lattitude_offset),
                new KeyValuePair<string, string>("gps_offset_longitude", Session.longitude_offset),

                new KeyValuePair<string, string>("gps_offset_bearing", Session.gps_offset_bearing),
                new KeyValuePair<string, string>("gps_offset_distance", Session.gps_offset_distance),

                new KeyValuePair<string, string>("comment", CommentText),

                new KeyValuePair<string, string>("site_type", SelectedSiteType?.IdLocatePoint is null ? "0" :SelectedSiteType.IdLocatePoint),


            };

            return keyValues;

        }
        public ObservableCollection<CodeLocatePoint> LocatePointType
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CodeLocatePoint>();
                    Console.WriteLine();
                    var table = conn.Table<CodeLocatePoint>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<CodeLocatePoint>(table);
                }

            }
        }

        private async void ExecuteRecordCommand()
        {
            Console.WriteLine();
            if (string.IsNullOrEmpty(Session.gps_offset_bearing) && string.IsNullOrEmpty(Session.gps_offset_distance))
            {
                Console.WriteLine();
                Session.lattitude2 = Session.live_lattitude;
                Session.longitude2 = Session.live_longitude;

                Session.lattitude_offset = string.Empty;
                Session.longitude_offset = string.Empty;
            }
            else // offset gps record 
            {
                Console.WriteLine();
                Session.lattitude_offset = Session.live_lattitude;
                Session.longitude_offset = Session.live_longitude;
            }

            var KVPair = keyvaluepairLocate(); // update existed chassis
            var result = await CloudDBService.Insert_gps_point(KVPair);

            if (result.Equals("OK"))
            {

                Session.LocpointnumberEnd = LocPointNumber.ToString();
                Session.GpsPointMaxIdx = LocPointNumber.ToString();

                LocPointNumber++;
                OnPropertyChanged(nameof(CurLocPoint)); // update Point number count
                await Application.Current.MainPage.DisplayAlert("DONE", "Gps point inserted", "OK");

            }

        }

        private async void ExecuteOpenGPSOffsetPopupCommand()
        {
            // OffsetGPSPopUp output 
            // Computed new coord 
            // - Session.lattitude2
            // - Session.longitude2
            // bearing, distance value
            // - Session.gps_offset_bearing
            // - Session.gps_offset_distance


            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new OffsetGPSPopUp());

        }

        public async void ExecuteFinishCommand()
        {
            RdGpstimer.Stop();
            //await Application.Current.MainPage.Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PushAsync(new EndTracePage());

        }
        public async void ExecuteCaptureCommand()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
        }
    }
}
