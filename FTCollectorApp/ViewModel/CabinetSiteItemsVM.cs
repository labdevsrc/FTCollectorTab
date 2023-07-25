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

namespace FTCollectorApp.ViewModel
{
    [QueryProperty(nameof(SiteType), nameof(TagNumber))]
    public partial class CabinetSiteItemsVM: ObservableObject
    {
        [ObservableProperty]
        string siteType;

        [ObservableProperty]
        string tagNumber;

        [ObservableProperty]
        string ownerName;

        [ObservableProperty]
        string ownerTagNumber;

        [ObservableProperty]
        string propertyId;

        [ObservableProperty]
        string siteName;


        [ObservableProperty]
        string streetAddress;

        [ObservableProperty]
        string postalCode;
        [ObservableProperty]
        string electSiteKeyCnt;

        [ObservableProperty]
        string commsProvider;

        [ObservableProperty]
        string serialNumber;

        [ObservableProperty]
        string notes;

        [ObservableProperty]
        string isLaneClosure = "No";

        [ObservableProperty]
        string isHasPowerDisconnect = "No";

        [ObservableProperty]
        string is3rdComms = "No";

        [ObservableProperty]
        string isSiteClearZone = "No";

        [ObservableProperty]
        string isHaveSunShield = "No";

        [ObservableProperty]
        string isBucketTruck = "No";

        [ObservableProperty]
        string isHasGround = "No";

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsKeyTypeDisplay))]
        string isHasKey = "No";


        [ObservableProperty]
        string selectedRackCount;

        bool isKeyTypeDisplay = false;
        public bool IsKeyTypeDisplay{
            get =>isKeyTypeDisplay;
            set{
                if(IsHasKey.Equals("Yes"))
                    SetProperty(ref isKeyTypeDisplay, true);
                else
                    SetProperty(ref isKeyTypeDisplay, false);
            }
        }

        [ObservableProperty]
        string isInClearZone = "No";
        public ObservableCollection<string> DotDistrict
        {
            get
            {

                List<string> iterable1to100 = new List<string>();
                for (int i = 0; i < 100; i++)
                {
                    iterable1to100.Add(i.ToString());
                }
                Console.WriteLine();
                return new ObservableCollection<string>(iterable1to100);

            }
        }

        public ObservableCollection<string> YesNo
        {
            get
            {

                var yesno = new List<string>();
                yesno.Add("Yes");
                yesno.Add("No");
                    Console.WriteLine();
                return new ObservableCollection<string>(yesno);
            }
        }

        public ObservableCollection<string> RackCount
        {
            get
            {
                List<string> iterable1to10 = new List<string>();
                for (int i = 1; i < 20; i++)
                {
                    iterable1to10.Add(i.ToString());
                }
                Console.WriteLine();
                return new ObservableCollection<string>(iterable1to10);
            }
        }


        //public CabinetSitePageViewModel(string siteType, string tagNumber)
        public CabinetSiteItemsVM()
        {
            Console.WriteLine();
            //ShowDuctPageCommand = new Command(async () => ExecuteNavigateToDuctPageCommand());
            //ShowRackPageCommand = new Command(async () => ExecuteNavigateToRackPageCommand());
            //ShowActiveDevicePageCommand = new Command(async () => ExecuteNavigateToActiveDevicePageCommand());
            SendResultCommand = new Command(resultPage => ExecuteGetResultCommand(ResultPage));
            Console.WriteLine();
            //SiteType = siteType;
            //TagNumber = tagNumber;
            OwnerName = Session.OwnerName;
            Session.current_page = "cabinet";


            ShowDuctPageCommand = new Command(
                execute: async () => {
                    await Application.Current.MainPage.Navigation.PushAsync(new DuctPage());
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return true; // Session.SiteCreateCnt > 1;
                }
              );

            FiberBtnCommand = new Command(
                execute: async () => {

                    await Application.Current.MainPage.Navigation.PushAsync(new FiberMainMenu());
                },

                canExecute: () =>
                {
                    Console.WriteLine();
                    return true; // Session.DuctSaveCount >= 1;
                }
              );

            ShowRackPageCommand = new Command(
                execute: async () => {
                    await Application.Current.MainPage.Navigation.PushAsync(new RacksPage());
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return true; // Session.DuctSaveCount >= 1;
                }
              );

            ShowActiveDevicePageCommand = new Command(
                execute: async () => {
                    await Application.Current.MainPage.Navigation.PushAsync(new ActiveDevicePage());
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return true; // Session.RackCount >= 1;
                }
              );
        }

        ////////////
        ///
        public ICommand SendResultCommand { get; set; }
        public ICommand ShowDuctPageCommand { get; set; }
        public ICommand FiberBtnCommand { get; set; }

        private async Task ExecuteNavigateToDuctPageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new DuctPage());
        }

        public ICommand ShowRackPageCommand { get; set; }
        private async Task ExecuteNavigateToRackPageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RacksPage());
        }

        public ICommand ShowActiveDevicePageCommand { get; set; }
        private async Task ExecuteNavigateToActiveDevicePageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ActiveDevicePage());
        }


        //public string ResultPage;
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


        public ObservableCollection<BuildingType> BuildingTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<BuildingType>();
                    var bdClassiTable = conn.Table<BuildingType>().ToList();
                    return new ObservableCollection<BuildingType>(bdClassiTable);
                }
            }
        }
        public ObservableCollection<Mounting> MountingTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Mounting>();
                    var mountingTable = conn.Table<Mounting>().OrderBy(a => a.MountingType).ToList();
                    return new ObservableCollection<Mounting>(mountingTable);
                }
            }
        }




        /// START - Property, bindable object for Roadway, intersection

        [ObservableProperty]
        bool isSearchingRoadway = false;

        [ObservableProperty]
        InterSectionRoad selectedIntersection;


        Roadway selectedRoadway;
        public Roadway SelectedRoadway
        {
            get => selectedRoadway;
            set
            {
                SetProperty(ref (selectedRoadway), value);
                SearchRoadway = value.RoadwayName;
                OnPropertyChanged(nameof(IntersectionList));
                OnPropertyChanged(nameof(SearchRoadway));
            }
        }

        string searchRoadway;
        public string SearchRoadway
        {
            get => searchRoadway;
            set
            {
                IsSearchingRoadway = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchRoadway), value);
                OnPropertyChanged(nameof(IntersectionList));
                Console.WriteLine();
            }
        }

        public ObservableCollection<Roadway> RoadwayList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<Roadway>();
                    var rwTable = conn.Table<Roadway>().ToList();
                    var table = rwTable.Where(a => a.RoadOwnerKey == Session.ownerkey).ToList();
                    if (SearchRoadway != null)
                    {
                        Console.WriteLine();
                        table = conn.Table<Roadway>().Where(i => i.RoadwayName.ToLower().Contains(SearchRoadway.ToLower())).
                            GroupBy(b => b.RoadwayName).Select(g => g.First()).ToList();
                    }
                    Console.WriteLine();
                    return new ObservableCollection<Roadway>(table);
                }
            }
        }

        public ObservableCollection<InterSectionRoad> IntersectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<InterSectionRoad>();
                    var data = conn.Table<InterSectionRoad>().ToList();
                    if (SelectedRoadway != null)
                    {
                        data = conn.Table<InterSectionRoad>().Where(b => b.major_roadway == SelectedRoadway.RoadwayKey || b.minor_roadway == SelectedRoadway.RoadwayKey).ToList();
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    //var data = conn.Table<InterSectionRoad>().Where(a => a.OWNER_CD == Session.ownerCD).GroupBy(b => b.IntersectionName).Select(g => g.First()).ToList();
                    return new ObservableCollection<InterSectionRoad>(data);
                }
            }
        }
        /// Building Site Page, Structure Site Page, 
        /// 

        public ObservableCollection<CompassDirection> TravelDirectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<CompassDirection>();
                    var data = conn.Table<CompassDirection>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<CompassDirection>(data);
                }
            }
        }

        [ObservableProperty]
        MaterialCode selectedMatCode;

        [ObservableProperty]
        Mounting selectedMounting;

        public ObservableCollection<MaterialCode> MaterialCodeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<MaterialCode>();
                    var table = conn.Table<MaterialCode>().ToList();
                    foreach (var col in table)
                    {
                        col.CodeDescription = HttpUtility.HtmlDecode(col.CodeDescription); // should use for escape char "
                    }
                    return new ObservableCollection<MaterialCode>(table);
                }
            }
        }


        public ObservableCollection<Orientation> OrientationList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Orientation>();
                    var table = conn.Table<Orientation>().ToList();
                    return new ObservableCollection<Orientation>(table);
                }
            }
        }

        //Properties, Bindable object for manufacturer dropdown list - start
        [ObservableProperty]
        bool isSearching = false;

        [ObservableProperty]
        ModelDetail selectedModelDetail;


        Manufacturer selectedManuf;
        public Manufacturer SelectedManuf
        {
            get => selectedManuf;
            set
            {
                SetProperty(ref (selectedManuf), value);
                SearchManufacturer = value.ManufName;
                OnPropertyChanged(nameof(ModelDetailList));
                OnPropertyChanged(nameof(SearchManufacturer));
            }
        }

        string searchManufacturer;
        public string SearchManufacturer
        {
            get => searchManufacturer;
            set
            {
                IsSearching = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchManufacturer), value);
                OnPropertyChanged(nameof(ManufacturerList));
                Console.WriteLine();
            }
        }

        public ObservableCollection<Manufacturer> ManufacturerList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Manufacturer>();
                    var table = conn.Table<Manufacturer>().GroupBy(b => b.ManufName).Select(g => g.First()).ToList();
                    foreach (var col in table)
                    {
                        col.ManufName = HttpUtility.HtmlDecode(col.ManufName); // should use for escape char "
                        col.ManufName = col.ManufName.Trim();
                    }
                    if (SearchManufacturer != null)
                    {
                        Console.WriteLine();
                        table = conn.Table<Manufacturer>().Where(i => i.ManufName.ToLower().Contains(SearchManufacturer.ToLower())).
                            GroupBy(b => b.ManufName).Select(g => g.First()).ToList();
                    }
                    return new ObservableCollection<Manufacturer>(table);
                }
            }
        }

        public ObservableCollection<ModelDetail> ModelDetailList
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ModelDetail>();
                    var table = conn.Table<ModelDetail>().ToList();
                    if (SelectedManuf != null)
                        table = conn.Table<ModelDetail>().Where(a => a.ManufKey == SelectedManuf.ManufKey).ToList();

                    Console.WriteLine();
                    foreach (var col in table)
                    {
                        col.ModelNumber = HttpUtility.HtmlDecode(col.ModelNumber); // should use for escape char 
                        col.ModelDescription = HttpUtility.HtmlDecode(col.ModelDescription); // should use for escape char 
                        if (col.ModelCode1 == "")
                            col.ModelCode1 = col.ModelCode2;
                        if (col.ModelCode2 == "")
                            col.ModelCode2 = col.ModelCode1;
                    }
                    Console.WriteLine();
                    return new ObservableCollection<ModelDetail>(table);
                }
            }
        }

        //Properties, Bindable object for manufacturer dropdown list - end


        [ObservableProperty]
        string uDSOwner;

        [ObservableProperty]
        BuildingType selectedBuilding;

        [ObservableProperty]
        CompassDirection selectedTravelDirection;

        [ObservableProperty]
        CompassDirection selectedTravelDirection2;


        [ObservableProperty]
        CompassDirection selectedOrientation;


        [ObservableProperty]
        string selectedElectSiteKey;

        [ObservableProperty]
        string selectedKeyCode;

        [ObservableProperty]
        string selectedDistrict;

        [ObservableProperty]
        string manufactured;


        [ObservableProperty]
        string installedAt;

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

                new KeyValuePair<string, string>("tag",TagNumber), //8
                new KeyValuePair<string, string>("site2", SiteName),  /// site_id
                new KeyValuePair<string, string>("owner_tag_number", OwnerTagNumber),  /// site_id
                new KeyValuePair<string, string>("type2", Session.site_type_key),  /// code_site_type.key
                new KeyValuePair<string, string>("sitname2", SiteName),


                new KeyValuePair<string, string>("manufacturer", SelectedManuf?.ManufName is null ? "" : SelectedManuf.ManufName ),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("manufacturer_key", SelectedManuf?.ManufKey is null ? "" : SelectedManuf.ManufKey ),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("manufactured_date", Manufactured),
                new KeyValuePair<string, string>("mod2", ""), /// model name, Building : x,  Cabinet/Pull Box : o
                new KeyValuePair<string, string>("pic2", ""),
                new KeyValuePair<string, string>("otag", ""),
                new KeyValuePair<string, string>("roadway", SelectedRoadway?.RoadwayName is null ? "" : SelectedRoadway.RoadwayName),
                new KeyValuePair<string, string>("pid", ""),
                new KeyValuePair<string, string>("loct", ""),
                new KeyValuePair<string, string>("staddr", StreetAddress), // site_address
                new KeyValuePair<string, string>("pscode", PostalCode), // break point 1

                new KeyValuePair<string, string>("btype", SelectedBuilding?.BuildingTypeKey is null ? "":SelectedBuilding.BuildingTypeKey),
                new KeyValuePair<string, string>("orientation", SelectedOrientation?.CompasKey is null ? "" : SelectedOrientation.CompasKey),

                new KeyValuePair<string, string>("laneclosure", IsLaneClosure.Equals("Yes") ? "1":"0"),
                new KeyValuePair<string, string>("dotdis",  SelectedDistrict is null ? "" : SelectedDistrict),
                new KeyValuePair<string, string>("powr", IsHasPowerDisconnect.Equals("Yes") ? "1":"0"),
                new KeyValuePair<string, string>("elecsite", SelectedElectSiteKey is null ? "" : SelectedElectSiteKey),
                new KeyValuePair<string, string>("comm", Is3rdComms.Equals("Yes") ? "1":"0"),
                new KeyValuePair<string, string>("commprovider", CommsProvider),
                new KeyValuePair<string, string>("sitaddr", StreetAddress), // site_street_addres
                new KeyValuePair<string, string>("udsowner", UDSOwner),

                new KeyValuePair<string, string>("rs2", "L"),

                new KeyValuePair<string, string>("CLEAR_ZONE_IND2", IsSiteClearZone.Equals("Yes") ? "1":"0"),

                new KeyValuePair<string, string>("intersect2", SelectedIntersection?.IntersectionKey is null ? "": SelectedIntersection.IntersectionKey),
                new KeyValuePair<string, string>("material2", SelectedMatCode?.MaterialKey is null ? "":SelectedMatCode.MaterialKey),
                new KeyValuePair<string, string>("mounting2", SelectedMounting?.MountingKey is null ? "":SelectedMounting.MountingKey),
                new KeyValuePair<string, string>("offilter2", ""),//FilterTypeSelected),
                new KeyValuePair<string, string>("fltrsize2", ""),//FilterSizeKeySelected),
                new KeyValuePair<string, string>("sunshield2", IsHaveSunShield.Equals("Yes") ? "1":"0"),
                new KeyValuePair<string, string>("installed2", InstalledAt),
                new KeyValuePair<string, string>("comment2", Notes), // Notes, pr description


                new KeyValuePair<string, string>("gravel_bottom","" ),
                new KeyValuePair<string, string>("lid_pieces", ""),
                new KeyValuePair<string, string>("has_apron", ""),

                new KeyValuePair<string, string>("etc2", ""),
                new KeyValuePair<string, string>("fosc2", ""),
                new KeyValuePair<string, string>("vault2", ""),
                new KeyValuePair<string, string>("trlane2", ""),
                new KeyValuePair<string, string>("bucket2", IsBucketTruck.Equals("Yes") ? "1":"0"),
                new KeyValuePair<string, string>("serialno", SerialNumber),
                new KeyValuePair<string, string>("key", ""),
                new KeyValuePair<string, string>("ktype", ""), //SelectedKeyType),
                new KeyValuePair<string, string>("ground", IsHasGround.Equals("Yes") ? "1":"0"),
                new KeyValuePair<string, string>("traveldir", SelectedTravelDirection?.CompasKey is null ? "": SelectedTravelDirection.CompasKey),
                new KeyValuePair<string, string>("traveldir2", SelectedTravelDirection2?.CompasKey is null ? "": SelectedTravelDirection2.CompasKey),

                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("owner_county", Session.countycode),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),

                new KeyValuePair<string, string>("gps_offset_latitude", ""),
                new KeyValuePair<string, string>("gps_offset_longitude", ""),
                new KeyValuePair<string, string>("LATITUDE", Session.lattitude2),
                new KeyValuePair<string, string>("LONGITUDE", Session.longitude2),


                new KeyValuePair<string, string>("plansheet","0"),
                new KeyValuePair<string, string>("psitem","0"),
                new KeyValuePair<string, string>("stage", Session.stage)
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
                var result = await CloudDBService.PostSaveBuilding(KVPair);
                Console.WriteLine();
                if (result.Equals("OK"))
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Uploading Data Done", "OK");
                    Session.SiteCreateCnt++;
                    (ShowDuctPageCommand as Command).ChangeCanExecute();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Warning", result, "RETRY");

                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception : "+ e.ToString());
                await Application.Current.MainPage.DisplayAlert("Warning", "Data Insert Problem", "BACK");
            }


        }

        [ICommand]
        async void CompleteSite()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CompleteSitePage());
        }

        [ICommand]
        async void Capture()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
        }

        [ICommand]
        async void ReturnToMain()
        {
            if (Session.stage.Equals("A"))
                await Application.Current.MainPage.Navigation.PushAsync(new AsBuiltDocMenu());
            if (Session.stage.Equals("I"))
                await Application.Current.MainPage.Navigation.PushAsync(new MainMenuInstall());
        }
    }
}
