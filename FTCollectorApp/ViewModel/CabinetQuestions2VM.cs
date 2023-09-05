
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Services;
using FTCollectorApp.View;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.View.SitesPage.Fiber;
using FTCollectorApp.View.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using FTCollectorApp.View.SitesPage.Popup;

namespace FTCollectorApp.ViewModel
{
    [QueryProperty(nameof(SiteType), "site_type")]
    [QueryProperty(nameof(TagNumber), "tag_number")]
    [QueryProperty(nameof(DBUpdateIndex), "siteindex")]

    public partial class CabinetQuestions2VM : ObservableObject
    {
        public ObservableCollection<CabinetType> CabinetTypeList { 
            get {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CabinetType>();
                    var table = conn.Table<CabinetType>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<CabinetType>(table);
                }
            } 
        
        }
        [ObservableProperty] CabinetType selectedCabinetType;

        [ObservableProperty] string tagNumber;
        [ObservableProperty] string siteType;
        [ObservableProperty] string dBUpdateIndex;
        [ObservableProperty] bool isHasKey = false;
        [ObservableProperty] bool isHasGroundRod = false;
        [ObservableProperty] string filterCount = "0";
        [ObservableProperty] FilterType selectedFilterType;
        [ObservableProperty] FilterSize selectedFilterSize;
        public ObservableCollection<FilterType> FilterTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<FilterType>();
                    var table = conn.Table<FilterType>().ToList();
                    foreach (var col in table)
                    {
                        col.FilterTypeDesc = HttpUtility.HtmlDecode(col.FilterTypeDesc); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<FilterType>(table);
                }
            }
        }

        public ObservableCollection<FilterSize> FilterSizeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<FilterSize>();
                    var table = conn.Table<FilterSize>().ToList();
                    foreach (var col in table)
                    {
                        col.data = HttpUtility.HtmlDecode(col.data); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<FilterSize>(table);
                }
            }
        }
        ///--end

        bool isKeyTypeDisplay = false;
        public bool IsKeyTypeDisplay
        {
            get => isKeyTypeDisplay;
            set
            {
                if (IsHasKey)
                    SetProperty(ref isKeyTypeDisplay, true);
                else
                    SetProperty(ref isKeyTypeDisplay, false);
            }
        }


        [ObservableProperty] string dotDistrict;

        //public BuildingSitePageViewModel(string siteType, string tagNumber)
        public CabinetQuestions2VM()
        {
            Console.WriteLine();

            SendResultCommand = new Command(resultPage => ExecuteGetResultCommand(ResultPage));
            Console.WriteLine();
            //SiteType =  siteType;
            //TagNumber = Sess tagNumber;
            //OwnerName = Session.OwnerName;
            Session.current_page = "cabinet";
            DotDistrict = Session.DOTdistrict;


        }

        ////////////
        ///
        public ICommand SendResultCommand { get; }

        public ICommand ShowDuctPageCommand { get; set; }

        public ICommand FiberBtnCommand { get; set; }
        public ICommand ToggleEntriesCommand { get; set; }
        
        public ICommand ShowRackPageCommand { get; }

        //private async Task ExecuteNavigateToDuctPageCommand()
        //{
        //    await Application.Current.MainPage.Navigation.PushAsync(new DuctPage());
        //}
        private async Task ExecuteNavigateToRackPageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RacksPage());
        }

        public ICommand ShowActiveDevicePageCommand { get; }
        private async Task ExecuteNavigateToActiveDevicePageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ActiveDevicePage());
        }

        private string _resultPage;
        public string ResultPage
        {
            get => _resultPage;
            set
            {
                _resultPage = value;
                OnPropertyChanged(nameof(ResultPage));
            }
        }

        private void ExecuteGetResultCommand(string result)
        {
            ResultPage = result;
            Console.WriteLine();
        }



        ///--end


        // Question 2
        
        [ObservableProperty] string streetAddress;
        [ObservableProperty] string postalCode;
        [ObservableProperty] BuildingType selectedBuilding;
        [ObservableProperty] string manufactured;
        [ObservableProperty] string installedAt;

        [ObservableProperty] string selectedElectSiteKey;
        [ObservableProperty] string selectedKeyCode;
        [ObservableProperty] string selectedDistrict;
        [ObservableProperty] string selectedRackCount;

        [ObservableProperty] string commsProvider;
        [ObservableProperty] string serialNumber;
        [ObservableProperty] bool isHasPowerDisconnect = false;
        [ObservableProperty] bool is3rdComms = false;

        [ObservableProperty] bool isHaveSunShield = false;
        [ObservableProperty] string uDSOwner;
        [ObservableProperty] string distanceEOTL;


        List<KeyValuePair<string, string>> keyvaluepair()
        {
            /*url: 'ajaxSavecabinet.php',      
            data: {"time":getCurtime(),"owner2": owner2,"tag": tag2,"site2": site2,"sitname2": sitname2,
            "mfd2": mfd2,"mfr2": mfr2,"mod2": mod2,"pic2": pic2,"rs2": rs2,"height2": height2,
            "depth2": depth2,"width2": width2,"CLEAR_ZONE_IND2": CLEAR_ZONE_IND2,"longitude2": longitude2,
            "lattitude2": lattitude2,"intersect2": intersect2,"material2": material2,"mounting2": mounting2,
            "offilter2": offilter2,"fltrsize2": fltrsize2,"sunshield2": sunshield2,"installed2": installed2
            ,"comment2": comment2,"etc2": etc2,"fosc2": fosc2,"vault2": vault2,"trlane2": trlane2,
            "bucket2": bucket2,"type2":type2,"serialno":serialno,"ground":ground,"key":key,"ktype":ktype,
            "traveldir":traveldir,"roadway":roadway,"accuracy":accuracy,"altitude":altitude,"loct":loct,"ctype":ctype,
            "laneclosure":laneclosure,"dotdis":dotdis,"powr":powr,"elecsite":elecsite,"comm":comm,"commprovider":commprovider,"sitaddr":sitaddr,"udsowner":udsowner,"orientationid":orientationid,"otag":otag},*/

            Console.WriteLine();
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  // 2
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 2
                new KeyValuePair<string, string>("accuracy", Session.accuracy), //3
                new KeyValuePair<string, string>("altitude", Session.altitude),  //4
                new KeyValuePair<string, string>("oid", Session.ownerkey), //1
                //new KeyValuePair<string, string>("owner", Session.ownerkey), //5
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("jobnum",Session.jobnum), //  7 

                new KeyValuePair<string, string>("tag",Session.tag_number), //8
                //new KeyValuePair<string, string>("site2", SiteName),  /// site_id
                //new KeyValuePair<string, string>("owner_tag_number", OwnerTagNumber),  /// site_id
                new KeyValuePair<string, string>("type2", Session.site_type_key),  /// code_site_type.key
                //new KeyValuePair<string, string>("sitname2", SiteName),


                //new KeyValuePair<string, string>("manufactured_date", Manufactured),
                //new KeyValuePair<string, string>("manufacturer", SelectedManuf?.ManufKey is null ? "" : SelectedManuf.ManufKey),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("manufacturer_key", ""),  // manufacturer , for Cabinet, pull box

                //new KeyValuePair<string, string>("mod2", SelectedModelDetail?.ModelKey is null ? "" : SelectedModelDetail.ModelKey), /// model name, Building : x,  Cabinet/Pull Box : o
                new KeyValuePair<string, string>("pic2", ""),
                new KeyValuePair<string, string>("otag", ""),
                //new KeyValuePair<string, string>("roadway", SelectedRoadway?.RoadwayKey is null ? "" : SelectedRoadway.RoadwayKey),
                new KeyValuePair<string, string>("pid", ""),
                //new KeyValuePair<string, string>("loct", LocationName),

                //new KeyValuePair<string, string>("orientation", SelectedOrientation?.CompasKey is null ? "" : SelectedOrientation.CompasKey),
                //new KeyValuePair<string, string>("laneclosure", IsLaneClosure ? "1":"0"),
                new KeyValuePair<string, string>("dotdis",  Session.DOTdistrict),
                new KeyValuePair<string, string>("powr", IsHasPowerDisconnect ? "1":"0"),
                new KeyValuePair<string, string>("elecsite", ""),
                new KeyValuePair<string, string>("comm", Is3rdComms ? "1":"0"),
                new KeyValuePair<string, string>("commprovider", CommsProvider),
                new KeyValuePair<string, string>("sitaddr", StreetAddress), // site_street_addres
                new KeyValuePair<string, string>("staddr", StreetAddress), // site_street_addres
                new KeyValuePair<string, string>("pscode",PostalCode),
                new KeyValuePair<string, string>("udsowner", UDSOwner),
                new KeyValuePair<string, string>("btype", ""),
                new KeyValuePair<string, string>("cabinet_type",SelectedCabinetType?.CabinetTypeKey is null ? "":SelectedCabinetType?.CabinetTypeKey),
                new KeyValuePair<string, string>("rs2", "L"),

                //new KeyValuePair<string, string>("height2", Height),
                //new KeyValuePair<string, string>("depth2", Depth),
                //new KeyValuePair<string, string>("width2", Width),
                //new KeyValuePair<string, string>("CLEAR_ZONE_IND2", IsSiteClearZone ? "1":"0"),

                //new KeyValuePair<string, string>("intersect2", SelectedIntersection?.IntersectionKey is null ? "": SelectedIntersection.IntersectionKey),
                //new KeyValuePair<string, string>("material2", SelectedMatCode?.MaterialKey is null ? "":SelectedMatCode.MaterialKey),
                //new KeyValuePair<string, string>("mounting2", SelectedMounting?.MountingKey is null ? "":SelectedMounting.MountingKey),
                new KeyValuePair<string, string>("filter_count", FilterCount ),
                new KeyValuePair<string, string>("filter_type", SelectedFilterType?.FilterTypeKey is null ? "": SelectedFilterType.FilterTypeKey ),//FilterTypeSelected),
                new KeyValuePair<string, string>("filter_size", SelectedFilterSize?.FtSizeKey  is null ? "": SelectedFilterSize.FtSizeKey  ),//FilterSizeKeySelected),
                new KeyValuePair<string, string>("sunshield2", IsHaveSunShield ? "1":"0"),
                new KeyValuePair<string, string>("installed2", InstalledAt),
                //new KeyValuePair<string, string>("comment2", Notes), // Notes, pr description

                new KeyValuePair<string, string>("gravel_bottom","0" ),
                new KeyValuePair<string, string>("lid_pieces", ""),
                new KeyValuePair<string, string>("has_apron", "0"),
                new KeyValuePair<string, string>("haskey", IsHasKey ? "1":"0"),

                new KeyValuePair<string, string>("etc2", ""),
                new KeyValuePair<string, string>("fosc2", ""),
                new KeyValuePair<string, string>("vault2", ""),
                new KeyValuePair<string, string>("trlane2", DistanceEOTL),
                //new KeyValuePair<string, string>("bucket2", IsBucketTruck ? "1":"0"),
                new KeyValuePair<string, string>("serialno", SerialNumber),
                new KeyValuePair<string, string>("key", ""),
                new KeyValuePair<string, string>("ktype", ""), //SelectedKeyType),
                new KeyValuePair<string, string>("ground", IsHasGroundRod ? "1":"0"),
                //new KeyValuePair<string, string>("traveldir", SelectedTravelDirection?.CompasKey is null ? "": SelectedTravelDirection.CompasKey),
                //new KeyValuePair<string, string>("traveldir2", SelectedTravelDirection2?.CompasKey is null ? "": SelectedTravelDirection2.CompasKey),
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("owner_county", Session.countycode),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),

                new KeyValuePair<string, string>("gps_offset_latitude", ""),
                new KeyValuePair<string, string>("gps_offset_longitude", ""),
                new KeyValuePair<string, string>("LATITUDE", Session.lattitude2),
                new KeyValuePair<string, string>("LONGITUDE", Session.longitude2),


                new KeyValuePair<string, string>("plansheet","0"),
                new KeyValuePair<string, string>("psitem","0"),
                new KeyValuePair<string, string>("stage", Session.stage),
            };


            return keyValues;

        }


        [ICommand]
        async void SaveContinue()
        {
            Console.WriteLine();
            try
            {
                var KVPair = keyvaluepair();
                //var result = await CloudDBService.PostSaveBuilding(KVPair);
                var result = await CloudDBService.UpdateSiteQuestionPage2(KVPair);
                if (result.Equals("OK"))
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Uploading Data Done", "OK");
                    Session.CabinetPage2CreateCnt = 1;
                }

                else
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", result, "RETRY");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e.ToString());
                await Application.Current.MainPage.DisplayAlert("Warning", "Data Insert Problem", "BACK");
            }

        }

        [ICommand]
        async void CompleteSite()
        {
            //await Application.Current.MainPage.Navigation.PushAsync(new CompleteSitePage());

            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new CompleteSitePopUp());
        }

        [ICommand]
        async void Capture()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
        }



    }
}
