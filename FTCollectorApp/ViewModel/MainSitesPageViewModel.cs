
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
    [QueryProperty(nameof(SiteType), nameof(TagNumber))]

    public partial class MainSitesPageViewModel : ObservableObject
    {

        static string BUILDING_SITE = "Building";
        static string CABINET_SITE = "Cabinet";
        static string PULLBOX_SITE = "PullBox";

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsSiteCabinet))]
        [AlsoNotifyChangeFor(nameof(IsSitePullBox))]
        string titleSite = "";

        // NIPC for IsSiteCabinet
        [ObservableProperty]
        bool isSiteBuilding = true;

        [ObservableProperty]
        bool isSiteStructure = false;


        // NIPC for IsSiteCabinet
        bool isSiteCabinet = false;
        public bool IsSiteCabinet
        {
            get => isSiteCabinet;
            set
            {
                Console.WriteLine();
                value = false;
                if (Session.site_major.Equals(CABINET_SITE))
                {
                    IsSiteBuilding = false;
                    IsSitePullBox = false;
                    IsSiteStructure = false;
                    value = true;
                }
                SetProperty(ref isSiteCabinet, value);
            }
        }

        // NIPC for IsSitePullBox
        bool isSitePullBox = false;
        public bool IsSitePullBox
        {
            get => isSitePullBox;
            set
            {
                value = false;
                if (Session.site_major.Equals(PULLBOX_SITE))
                {
                    IsSiteBuilding = false;
                    IsSiteCabinet = false;
                    IsSiteStructure = false;
                    value = true;
                }
                SetProperty(ref isSitePullBox, value);
            }
        }

        public void CheckSiteType()
        {
            TitleSite = Session.site_major;
            Console.WriteLine();
        }

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
        bool isLaneClosure = false;

        [ObservableProperty]
        bool isHasPowerDisconnect = false;

        [ObservableProperty]
        bool is3rdComms = false;

        // switch widget - start
        [ObservableProperty]
        bool isHaveSunShield = false;

        [ObservableProperty]
        bool isHasGroundRod = false;

        [ObservableProperty]
        bool isHasKey = false;

        [ObservableProperty]
        bool isSiteClearZone = false;

        [ObservableProperty]
        bool isBucketTruck = false;

        [ObservableProperty]
        string selectedRackCount;

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

        [ObservableProperty]
        string isInClearZone = "No";

        [ObservableProperty]
        string dotDistrict;

        [ObservableProperty]
        string electricSiteKey;

        [ObservableProperty]
        string keyCode;


        /* Replace DotDistrict Picker popup 
         * 
         */
        /*public ObservableCollection<string> DotDistrict
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
        }*/

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



        [ObservableProperty]
        string siteCategory = Session.site_major;



    //public BuildingSitePageViewModel(string siteType, string tagNumber)
    public MainSitesPageViewModel()
        {
            Console.WriteLine();

            SendResultCommand = new Command(resultPage => ExecuteGetResultCommand(ResultPage));
            Console.WriteLine();
            //SiteType = siteType;
            //TagNumber = tagNumber;
            OwnerName = Session.OwnerName;
            Session.current_page = "building";
            Session.site_major = string.Empty;
            Session.site_minor = string.Empty;

            CheckJobVerified();

            ShowDuctPageCommand = new Command(
                execute: async () => {

                    //await Application.Current.MainPage.Navigation.PushAsync(new DuctPage());
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new DuctPopup());
                },

                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.SiteCreateCnt >= 1;
                }
              );


            FiberBtnCommand = new Command(
                execute: async () => {

                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new FiberPopup());
                    //await Application.Current.MainPage.Navigation.PushAsync(new FiberMainMenu());

                },

                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.DuctSaveCount >= 1;
                }
              );



            ShowRackPageCommand = new Command(
                execute: async () => {
                    await Application.Current.MainPage.Navigation.PushAsync(new RacksPage());
                    //RefreshCanExecutes();
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.DuctSaveCount >= 1;
                }
              );

            ShowActiveDevicePageCommand = new Command(
                execute: async () => {
                    await Application.Current.MainPage.Navigation.PushAsync(new ActiveDevicePage());
                    //RefreshCanExecutes();
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.RackCount >= 1;
                }
              );

            DisplayCreateSitePopup();

            // dump Create Site selection 
            MessagingCenter.Subscribe<CreateSitePopupVM>(this, "CheckSite", (sender) =>
            {
                TitleSite = Session.site_major;
                SiteType = Session.site_minor;
                TagNumber = Session.tag_number;

                Console.WriteLine(); // Do something whenever the "Hi" message is received
            });

            // dump DuctSaveCnt selection 
            MessagingCenter.Subscribe<CreateSitePopupVM>(this, "DuctSaveCnt", (sender) =>
            {
                //DuctSaveCount = 
                (FiberBtnCommand as Command).ChangeCanExecute();

                Console.WriteLine(); // Do something whenever the "Hi" message is received
            });
        }


        // VERY IMPORTANT !!
        // Must check Job verified or not
        // if not verified Block user input
        // 
        async void CheckJobVerified()
        {
            if (!Session.IsVerified)
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Uploading Data Done", "OK");
            }
        }



        async void DisplayCreateSitePopup()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new CreateSitePopup());
        }

        void RefreshCanExecutes()
        {
            (ShowDuctPageCommand as Command).ChangeCanExecute();
            (ShowRackPageCommand as Command).ChangeCanExecute();
            (ShowActiveDevicePageCommand as Command).ChangeCanExecute();
        }

        ////////////
        ///
        public ICommand SendResultCommand { get; }

        public ICommand ShowDuctPageCommand { get; set; }

        public ICommand FiberBtnCommand { get; set; }

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

        /// <summary>
        /// MaterialCodeList,  MountingTypeList
        /// </summary>

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
                    Console.WriteLine();
                    return new ObservableCollection<MaterialCode>(table);
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
                    Console.WriteLine();
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



        /// start
        [ObservableProperty]
        FilterType selectedFilterType;

        [ObservableProperty]
        FilterSize selectedFilterSize;
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



        [ObservableProperty]
        string uDSOwner;

        [ObservableProperty]
        BuildingType selectedBuilding;

        [ObservableProperty]
        CompassDirection selectedTravelDirection;

        [ObservableProperty]
        CompassDirection selectedOrientation;

        [ObservableProperty]
        CompassDirection selectedTravelDirection2;

        //[ObservableProperty]
        //string selectedElectSiteKey;

        //[ObservableProperty]
        //string selectedKeyCode;

        //[ObservableProperty]
        //string selectedDistrict;

        [ObservableProperty]
        string manufactured;


        [ObservableProperty]
        string installedAt;

        [ObservableProperty]
        string height;
        [ObservableProperty]
        string depth;
        [ObservableProperty]
        string width;


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


                new KeyValuePair<string, string>("manufacturer", ""),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("manufacturer_key", ""),  // manufacturer , for Cabinet, pull box

                new KeyValuePair<string, string>("manufactured_date", Manufactured),
                new KeyValuePair<string, string>("mod2", ""), /// model name, Building : x,  Cabinet/Pull Box : o
                new KeyValuePair<string, string>("pic2", ""),
                new KeyValuePair<string, string>("otag", ""),
                new KeyValuePair<string, string>("roadway", SelectedRoadway?.RoadwayName is null ? "" : SelectedRoadway.RoadwayName),
                new KeyValuePair<string, string>("pid", ""),
                new KeyValuePair<string, string>("loct", ""),
                new KeyValuePair<string, string>("staddr", StreetAddress), // site_address
                new KeyValuePair<string, string>("pscode", PostalCode),

                new KeyValuePair<string, string>("btype", SelectedBuilding?.BuildingTypeKey is null ? "":SelectedBuilding.BuildingTypeKey),
                new KeyValuePair<string, string>("orientation", SelectedOrientation?.CompasKey is null ? "" : SelectedOrientation.CompasKey),

                new KeyValuePair<string, string>("laneclosure", IsLaneClosure ? "1":"0"),
                new KeyValuePair<string, string>("dotdis", DotDistrict), //  SelectedDistrict is null ? "" : SelectedDistrict),
                new KeyValuePair<string, string>("powr", IsHasPowerDisconnect ? "1":"0"),
                new KeyValuePair<string, string>("elecsite", ElectricSiteKey), // SelectedElectSiteKey),
                new KeyValuePair<string, string>("comm", Is3rdComms ? "1":"0"),
                new KeyValuePair<string, string>("commprovider", CommsProvider),
                new KeyValuePair<string, string>("sitaddr", StreetAddress), // site_street_addres
                new KeyValuePair<string, string>("udsowner", UDSOwner),

                new KeyValuePair<string, string>("rs2", "L"),

                new KeyValuePair<string, string>("height2", Height),
                new KeyValuePair<string, string>("depth2", Depth),
                new KeyValuePair<string, string>("width2", Width),
                new KeyValuePair<string, string>("CLEAR_ZONE_IND2", IsSiteClearZone ? "1":"0"),

                new KeyValuePair<string, string>("intersect2", SelectedIntersection?.IntersectionKey is null ? "": SelectedIntersection.IntersectionKey),
                new KeyValuePair<string, string>("material2", SelectedMatCode?.MaterialKey is null ? "":SelectedMatCode.MaterialKey),
                new KeyValuePair<string, string>("mounting2", SelectedMounting?.MountingKey is null ? "":SelectedMounting.MountingKey),
                new KeyValuePair<string, string>("offilter2", SelectedFilterType?.FilterTypeKey is null ? "": SelectedFilterType.FilterTypeKey ),//FilterTypeSelected),
                new KeyValuePair<string, string>("fltrsize2", SelectedFilterSize?.FtSizeKey  is null ? "": SelectedFilterSize.FtSizeKey  ),//FilterSizeKeySelected),
                new KeyValuePair<string, string>("sunshield2", IsHaveSunShield ? "1":"0"),
                new KeyValuePair<string, string>("installed2", InstalledAt),
                new KeyValuePair<string, string>("comment2", Notes), // Notes, pr description

                new KeyValuePair<string, string>("gravel_bottom","" ),
                new KeyValuePair<string, string>("lid_pieces", ""),
                new KeyValuePair<string, string>("has_apron", ""),
                new KeyValuePair<string, string>("rack_count", SelectedRackCount is null ? "" : SelectedRackCount),

                new KeyValuePair<string, string>("etc2", ""),
                new KeyValuePair<string, string>("fosc2", ""),
                new KeyValuePair<string, string>("vault2", ""),
                new KeyValuePair<string, string>("trlane2", ""),
                new KeyValuePair<string, string>("bucket2", IsBucketTruck ? "1":"0"),
                new KeyValuePair<string, string>("serialno", SerialNumber),
                new KeyValuePair<string, string>("key", ""),
                new KeyValuePair<string, string>("ktype", ""), //SelectedKeyType),
                new KeyValuePair<string, string>("ground", IsHasGroundRod ? "1":"0"),
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
                new KeyValuePair<string, string>("stage", Session.stage),

                // cabinet page values - start
                new KeyValuePair<string, string>("manufacturer", SelectedManuf?.ManufName is null ? "" : SelectedManuf.ManufName ),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("manufacturer_key", SelectedManuf?.ManufKey is null ? "" : SelectedManuf.ManufKey ),  // manufacturer , for Cabinet, pull box
                // cabinet page values - end
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
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e.ToString());
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



        //Properties, Bindable object for manufacturer dropdown list - start
        [ObservableProperty]
        bool isManufSearching = false;

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
                IsManufSearching = string.IsNullOrEmpty(value) ? false : true;

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

        [ICommand]
        public async void CreteSite()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new CreateSitePopup(), true);
        }

        //Properties, Bindable object for manufacturer dropdown list - end


        /// start exclude_site
        /// 
        [ObservableProperty]
        bool isSiteNameVisible = false;

        [ObservableProperty]
        bool isproperty_idVisible = false;

        [ObservableProperty]
        bool isintersectionVisible = false;

        [ObservableProperty]
        bool isroadwayVisible = false;

        [ObservableProperty]
        bool isdirection_of_travelVisible = false;

        [ObservableProperty]
        bool isorientationVisible = false;

        [ObservableProperty]
        bool issite_street_addressVisible = false;

        [ObservableProperty]
        bool isdescriptionVisible = false;

        [ObservableProperty]
        bool isdepthVisible = false;

        [ObservableProperty]
        bool isheightVisible = false;

        [ObservableProperty]
        bool isweightVisible = false;

        [ObservableProperty]
        bool iswidthVisible = false;

        [ObservableProperty]
        bool isdiameterVisible = false;

        [ObservableProperty]
        bool ismountingVisible = false;


        [ObservableProperty]
        bool islane_closure_requiredVisible = false;


        [ObservableProperty]
        bool isHasPowerDisConnectedVisible = false;


        [ObservableProperty]
        bool ishas_commsVisible = false;

        [ObservableProperty]
        bool iscomms_providerVisible = false;

        [ObservableProperty]
        bool isUDS_ownerVisible = false;

        [ObservableProperty]
        bool isUDS_nameVisible = false;

        [ObservableProperty]
        bool isFilterSizeVisible = false;

        [ObservableProperty]
        bool isFilterTypeVisible = false;



        [ObservableProperty]
        bool isapron_widthVisible = false;

        [ObservableProperty]
        bool ishas_apronVisible = false;

        [ObservableProperty]
        bool isapron_heightVisible = false;

        [ObservableProperty]
        bool isgravel_bottomVisible = false;

        [ObservableProperty]
        bool isHasGroundRodVisible = false;

        [ObservableProperty]
        bool isGroundResistanceVisible = false;


        [ObservableProperty]
        bool isSiteStreetAddrVisible = false;

        [ObservableProperty]
        bool isserial_numberVisible = false;


        [ObservableProperty]
        bool isHasKeyVisible;

        [ObservableProperty]
        bool iskey_typeVisible;

        [ObservableProperty]
        bool isfilter_countVisible;

        [ObservableProperty]
        bool isinstalledVisible;

        [ObservableProperty]
        bool isdistance_to_eotlVisible;

        [ObservableProperty]
        bool isbucket_truckVisible;

        [ObservableProperty]
        bool isclear_zone_indVisible;

        [ObservableProperty]
        bool ishas_power_disconnectVisible;

        [ObservableProperty]
        bool isuds_tag_keyVisible;

        [ObservableProperty]
        bool iselectric_site_keyVisible;



        [ICommand]
        void ToggleTest()
        {
            IsSiteNameVisible = !IsSiteNameVisible;
            Isproperty_idVisible = !Isproperty_idVisible;
            IsintersectionVisible = !IsintersectionVisible;
            IsroadwayVisible = !IsroadwayVisible;
            Isdirection_of_travelVisible = !Isdirection_of_travelVisible;
            IsorientationVisible = !IsorientationVisible;
            Issite_street_addressVisible = !Issite_street_addressVisible;
            IsdescriptionVisible = !IsdescriptionVisible;
            IsheightVisible = !IsheightVisible;
            IsweightVisible = !IsweightVisible;
            IswidthVisible = !IswidthVisible;
            IsdepthVisible = !IsdepthVisible;
            IsweightVisible = !IsweightVisible;
            IsdiameterVisible = !IsdiameterVisible;

            IsmountingVisible = !IsmountingVisible;
            Ishas_apronVisible = !Ishas_apronVisible;
            Isapron_widthVisible = !Isapron_widthVisible;
            Isapron_heightVisible = !Isapron_heightVisible;
            Isgravel_bottomVisible = !Isgravel_bottomVisible;

            IsHasGroundRodVisible = !IsHasGroundRodVisible;
            IsGroundResistanceVisible = !IsGroundResistanceVisible;
            IsHasKeyVisible = !IsHasKeyVisible;
            Iskey_typeVisible = !Iskey_typeVisible;
            Isserial_numberVisible = !Isserial_numberVisible;
            Isfilter_countVisible = !Isfilter_countVisible;

            IsHaveSunShield = !IsHaveSunShield;

            IsUDS_ownerVisible = !IsUDS_ownerVisible;
            IsUDS_nameVisible = !IsUDS_nameVisible;


            IsinstalledVisible = !IsinstalledVisible; // vars.installed.Equals("1") ? true : false;
            Isdistance_to_eotlVisible = !Isdistance_to_eotlVisible;// vars.distance_to_eotl.Equals("1") ? true : false;
            Isbucket_truckVisible = !Isbucket_truckVisible; // vars.bucket_truck.Equals("1") ? true : false;
            Islane_closure_requiredVisible = !Islane_closure_requiredVisible; // vars.lane_closure_required.Equals("1") ? true : false;
            Isclear_zone_indVisible = !Isclear_zone_indVisible; // vars.clear_zone_ind.Equals("1") ? true : false;
            Ishas_power_disconnectVisible = !Ishas_power_disconnectVisible; //vars.has_power_disconnect.Equals("1") ? true : false;
            Ishas_commsVisible = !Ishas_commsVisible; // vars.has_comms.Equals("1") ? true : false;
            Iscomms_providerVisible = !Iscomms_providerVisible; // vars.comms_provider.Equals("1") ? true : false;

            Isuds_tag_keyVisible = !Isuds_tag_keyVisible; // vars.uds_tag_key.Equals("1") ? true : false;
            Iselectric_site_keyVisible = !Iselectric_site_keyVisible; // vars.electric_site_key.Equals("1") ? true : false;
        }

        void PopulateVisibleVars()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<ExcludeSite>();
                var vars = conn.Table<ExcludeSite>().First();

                IsSiteNameVisible = vars.SiteName.Equals("1") ? true : false;
                Isproperty_idVisible = vars.property_id.Equals("1") ? true : false;
                IsintersectionVisible = vars.intersection.Equals("1") ? true : false;
                IsroadwayVisible = vars.roadway.Equals("1") ? true : false;
                Isdirection_of_travelVisible = vars.direction_of_travel.Equals("1") ? true : false;
                IsorientationVisible = vars.orientation.Equals("1") ? true : false;
                Issite_street_addressVisible = vars.site_street_address.Equals("1") ? true : false;
                IsdescriptionVisible = vars.description.Equals("1") ? true : false;

                IsdepthVisible = vars.depth.Equals("1") ? true : false;
                IsheightVisible = vars.height.Equals("1") ? true : false;
                IswidthVisible = vars.width.Equals("1") ? true : false;
                IsweightVisible = vars.weight.Equals("1") ? true : false;
                IsdiameterVisible = vars.diameter.Equals("1") ? true : false;

                IsmountingVisible = vars.mounting.Equals("1") ? true : false;
                


                Ishas_apronVisible = vars.has_apron.Equals("1") ? true : false;
                Isapron_widthVisible = vars.apron_width.Equals("1") ? true : false;
                Isapron_heightVisible = vars.apron_height.Equals("1") ? true : false;
                Isgravel_bottomVisible = vars.gravel_bottom.Equals("1") ? true : false;
                IsHasGroundRodVisible = vars.has_ground_rod.Equals("1") ? true : false;

                IsGroundResistanceVisible = vars.ground_resistance.Equals("1") ? true : false;
                IsHasKeyVisible = vars.has_key.Equals("1") ? true : false;
                Iskey_typeVisible = vars.key_type.Equals("1") ? true : false;

                Isserial_numberVisible = vars.serial_number.Equals("1") ? true : false;
                Isfilter_countVisible = vars.filter_count.Equals("1") ? true : false;

                IsHaveSunShield = vars.has_ground_rod.Equals("1") ? true : false;

                IsinstalledVisible = vars.installed.Equals("1") ? true : false;
                Isdistance_to_eotlVisible = vars.distance_to_eotl.Equals("1") ? true : false;
                Isbucket_truckVisible = vars.bucket_truck.Equals("1") ? true : false;
                Islane_closure_requiredVisible = vars.lane_closure_required.Equals("1") ? true : false;
                Isclear_zone_indVisible = vars.clear_zone_ind.Equals("1") ? true : false;
                Ishas_power_disconnectVisible = vars.has_power_disconnect.Equals("1") ? true : false;
                Ishas_commsVisible = vars.has_comms.Equals("1") ? true : false;
                Iscomms_providerVisible = vars.comms_provider.Equals("1") ? true : false;


                IsUDS_ownerVisible = vars.UDS_owner.Equals("1") ? true : false;
                IsUDS_nameVisible = vars.UDS_name.Equals("1") ? true : false;

                Isuds_tag_keyVisible = vars.uds_tag_key.Equals("1") ? true : false;
                Iselectric_site_keyVisible = vars.electric_site_key.Equals("1") ? true : false;

            }
        }





        /// end
    }
}

