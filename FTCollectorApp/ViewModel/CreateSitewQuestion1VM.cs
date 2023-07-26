using System.Collections.ObjectModel;
using System.Linq;

using System.Text;
using System.Web;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

using FTCollectorApp.Services;
using FTCollectorApp.Model;
using FTCollectorApp.View.Utils;
using SQLite;
using Xamarin.Forms;
using FTCollectorApp.Model.Reference;
using System.ComponentModel;
using FTCollectorApp.View.SitesPage.Popup;
using FTCollectorApp.View.SitesPage;

namespace FTCollectorApp.ViewModel
{
    public partial class CreateSitewQuestion1VM : ObservableObject
    {
        /* Create Site, Enter PC Tag - start */
        static string BUILDING_SITE = "Building";
        static string CABINET_SITE = "Cabinet";
        static string PULLBOX_SITE = "PullBox";
        static string STRUCTURE_SITE = "Structure";


        [ObservableProperty] bool isBuildingSelected = false;

        [ObservableProperty] bool isCabinetSelected = false;

        [ObservableProperty] bool isPulboxSelected = false;

        [ObservableProperty] bool isStructureSelected = false;

        [ObservableProperty] bool isRoadwaySelected = false;
        
        ObservableCollection<CodeSiteType> CodeSiteTypeList;
        ReadGPSTimer timer;


        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(MinorSiteList))] string selectedMajorType = string.Empty;

        [ObservableProperty]  string selectedMinorType = string.Empty;

        // Tag number confirmation - start

        [ObservableProperty] string reEnterStatus;
        [ObservableProperty] bool isTagNumberMatch = false;

        string tagNumber = string.Empty;
        public string TagNumber
        {
            get => tagNumber;
            set
            {
                Console.WriteLine();
                SetProperty(ref tagNumber, value);
                Session.tag_number = value;
                (ResetSiteCommand as Command).ChangeCanExecute(); // 
                if (!string.IsNullOrEmpty(ReEnterTagNumber))
                    CheckTagNumberCommand?.Execute(null); // when reentertag changed, do checktagnumber 
            }
        }

        string reEnterTagNumber;
        public string ReEnterTagNumber
        {
            get => reEnterTagNumber;
            set
            {
                SetProperty(ref reEnterTagNumber, value);
                if (!string.IsNullOrEmpty(TagNumber))
                    CheckTagNumberCommand?.Execute(null); // when reentertag changed, do checktagnumber 
            }
        }


        ObservableCollection<CodeSiteType> BufferMajorSite = new ObservableCollection<CodeSiteType>();
        public ObservableCollection<string> MinorSiteList
        {
            get
            {
                Console.WriteLine();
                var table = CodeSiteTypeList.Where(a => a.MajorType == SelectedMajorType).OrderBy(d => d.MinorType).ToList();
                BufferMajorSite = new ObservableCollection<CodeSiteType>(table);
                var table2 = BufferMajorSite.Select(c => c.MinorType);
                return new ObservableCollection<string>(table2);
            }
        }


        async void AddShellTabPage(string title) {
            ShellSection shell_section = new ShellSection
            {
                Title = title
            };

            switch (title)

            {
                case "DUCT":
                    shell_section.Icon = "duct.png";
                    shell_section.Items.Add(new ShellContent()
                    {
                        Content = new DuctPage(),

                    });
                    break;
                case "RACK":
                    shell_section.Icon = "rack2.png";
                    shell_section.Items.Add(new ShellContent()
                    {
                        Content = new RacksPage(),
                    });
                    break;
                case "SLOT_BLADE":
                    shell_section.Icon = "fa-link.png";
                    shell_section.Items.Add(new ShellContent()
                    {
                        Content = new SlotBladePage(),
                    });

                    break;
                case "ACTIVE_DEVICE":
                    shell_section.Icon = "fa-link.png";
                    shell_section.Items.Add(new ShellContent()
                    {
                        Content = new ActiveDevicePage(),
                    });
                case "PORT":
                    shell_section.Icon = "fa-link.png";
                    shell_section.Items.Add(new ShellContent()
                    {
                        Content = new PortPage(),

                    });
                    break;
                default:
                    await Application.Current.MainPage.DisplayAlert("Unknown", "Unknown Page","OK");
                    break;

            }


            AppShell.mytabbar.Items.Add(shell_section);

            Console.WriteLine();

        }


        public ICommand CheckTagNumberCommand { get; set; }
        public ICommand RecordGPSCommand { get; set; }

        public ICommand ResetSiteCommand { get; set; }

        public ICommand ShowFiberPageCommand { get; set; }
        public ICommand ShowActiveDevicePageCommand { get; set; }

        public ICommand ShowRackPageCommand { get; set; }
        public ICommand ShowDuctPageCommand { get; set; }

        public ICommand CaptureCommand { get; set; }

        /* Create Site, Enter PC Tag - end */

        public CreateSitewQuestion1VM()
        {
            Console.WriteLine();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<CodeSiteType>();
                var table = conn.Table<CodeSiteType>().ToList();
                try
                {
                    foreach (var col in table)
                    {
                        col.MinorType = HttpUtility.HtmlDecode(col.MinorType); // should use for escape char "
                        Console.WriteLine();

                    }
                    CodeSiteTypeList = new ObservableCollection<CodeSiteType>(table);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception " + e.ToString());
                }
                Console.WriteLine();
            }

            Console.WriteLine();

            CheckTagNumberCommand = new Command(
                execute:async() =>
                {
                    if (TagNumber.Equals(ReEnterTagNumber))
                    {
                        ReEnterStatus = "MATCH!";

                        IsTagNumberMatch = true;

                    }
                    else
                    {
                        ReEnterStatus = "No Match";
                        IsTagNumberMatch = false;
                    }
                    (RecordGPSCommand as Command).ChangeCanExecute();
                }
            );
            ResetSiteCommand = new Command(
                execute: async () =>
                {
                    TagNumber = string.Empty;
                    ReEnterTagNumber = string.Empty;
                    (RecordGPSCommand as Command).ChangeCanExecute();
                    IsDisplayQuestionList = false;
                },
                canExecute: () =>
                {
                    return TagNumber.Length > 1;
                }

            );

            CaptureCommand = new Command(
                execute : async() =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
                }
            );

            ShowDuctPageCommand = new Command(
                execute: async () => {
                    AddShellTabPage("DUCT");
                    //await Application.Current.MainPage.Navigation.PushAsync(new DuctPage());
                    //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new DuctPopup());
                },

                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.SiteCreateCnt > 1;
                }
              );

            ShowPortPageCommand = new Command(
                execute: async () => {
                    AddShellTabPage("PORT");
                },

                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.DuctSaveCount >= 1;
                }
              );

            ShowActiveDevicePageCommand = new Command(
                execute: async () => {
                    AddShellTabPage("SLOT_BLADE");
                },

                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.DuctSaveCount >= 1;
                }
              );

            ShowFiberPageCommand = new Command(
                execute: async () => {
                    AddShellTabPage("FIBER");
                },

                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.DuctSaveCount >= 1;
                }
              );


            ShowActiveDevicePageCommand = new Command(
                execute: async () => {
                    AddShellTabPage("ACTIVE_DEVICE");
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.RackCount >= 1;
                }
              );






            ShowRackPageCommand = new Command(
                execute: async () => {
                    AddShellTabPage("RACK");
                    //await Application.Current.MainPage.Navigation.PushAsync(new DuctPage());
                    //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new DuctPopup());
                },

                canExecute: () =>
                {
                    Console.WriteLine();
                    return Session.SiteCreateCnt > 1;
                }

                );

            RecordGPSCommand = new Command(

                execute: async () =>
                {
                    string result = String.Empty;
                    Console.WriteLine();

                    /* move to Minor Site Type */
                    string codekey = CodeSiteTypeList.Where(a => (a.MajorType == SelectedMajorType) && (a.MinorType == SelectedMinorType)).Select(a => a.CodeKey).First();
                    Session.site_type_key = codekey;
                    Session.site_major = SelectedMajorType;
                    Session.site_minor = SelectedMinorType;


                    if (IsDisplayQuestionList) return;


                    result = await CloudDBService.PostCreateSiteAsync(TagNumber, codekey);
                    if (result.Equals("DUPLICATED"))
                    {
                        Session.Result = "CreateSiteOK";

                        var OkAnswer = await Application.Current.MainPage.DisplayAlert("Please Confirm", "Update existed Tag Number ? ", "OK", "Cancel");
                        if (OkAnswer)
                        {
                            result = await CloudDBService.UpdateSite(TagNumber, codekey);
                            Console.WriteLine(result);
                            Session.Result = "CreateSiteOK";
                        }
                        else
                            return;

                    }

                    if (result.Equals("CREATE_DONE") || result.Equals("UPDATE_DONE"))
                    {
                        // get answer from popup 

                        if (result.Equals("CREATE_DONE"))
                        {
                            Session.SiteCreateCnt = 0;
                            Session.DuctSaveCount = 0;
                            Session.RackCount = 0;
                            Session.ActiveDeviceCount = 0;
                        }

                        var OkAnswer = await Application.Current.MainPage.DisplayAlert("DONE", result.Equals("CREATE_DONE") ? "Create Site Success" : "Update Site Success", "Enter " + SelectedMajorType, "Create Again");
                        if (OkAnswer)
                        {
                            IsDisplayQuestionList = true;
                            // stop timer gps
                            if (timer != null)
                                timer.Stop();
                            //await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(true);
                        }

                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Unknown Response", result, "TRY AGAIN");
                    }

                },
                canExecute: () => {

                    if (IsTagNumberMatch && SelectedMinorType?.Length > 2)
                    {
                        Console.WriteLine();
                        return true ;
                    }
                    else 
                    {
                        Console.WriteLine();
                        return false;
                    }
                }
            );


            if (timer == null)
            {
                timer = new ReadGPSTimer(TimeSpan.FromSeconds(5), OnGPSTimerStart);
                timer.Start();
            }

            Console.WriteLine();
        }

        // GPS Location Service - start
        [ObservableProperty]
        string accuracy;
        async void OnGPSTimerStart()
        {
            try
            {
                await LocationService.GetLocation();
                Accuracy = $"{LocationService.Coords.Accuracy}";
                Session.accuracy = String.Format("{0:0.######}", LocationService.Coords.Accuracy);
                //Session.longitude2 = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                //Session.lattitude2 = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                Session.live_longitude = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                Session.live_lattitude = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                Session.altitude = String.Format("{0:0.######}", LocationService.Coords.Altitude);
                //{ String.Format("{0:0.#######}", _location.Latitude.ToString())}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Accuracy = "No GPS";
            }

        }
        // GPS Location Service - end


        // Questions #1
        [ObservableProperty] bool isDisplayQuestionList = false;
        [ObservableProperty] bool isLaneClosure = false;
        [ObservableProperty] bool isSiteClearZone = false;
        [ObservableProperty] bool isBucketTruck = false;
        [ObservableProperty] bool isSearchingRoadway = false;
        [ObservableProperty] bool isSearchingMajorRoadway = false;
        [ObservableProperty] bool isSearchingMinorRoadway = false;
        [ObservableProperty] bool isintersectionVisible = false;

        /// START - Property, bindable object for Roadway, intersection

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

            }
        }


        Roadway selectedMajorRoadway;
        public Roadway SelectedMajorRoadway
        {
            get => selectedMajorRoadway;
            set
            {
                SetProperty(ref (selectedMajorRoadway), value);
                SearchMajorRoadway = value.RoadwayName;
            }
        }

        Roadway selectedMinorRoadway;
        public Roadway SelectedMinorRoadway
        {
            get => selectedMinorRoadway;
            set
            {
                SetProperty(ref (selectedMinorRoadway), value);
                SearchMinorRoadway = value.RoadwayName;
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
                OnPropertyChanged(nameof(RoadwayList));
                OnPropertyChanged(nameof(IntersectionList));
                tempSearch = value;
                //IsSearchingMajorRoadway = false;
                //IsSearchingMinorRoadway = false;
                Console.WriteLine();
            }
        }

        string searchMajorRoadway;
        public string SearchMajorRoadway
        {
            get => searchMajorRoadway;
            set
            {
                IsSearchingMajorRoadway = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchMajorRoadway), value);
                //OnPropertyChanged(nameof(IntersectionList));
                tempSearch = value;
                OnPropertyChanged(nameof(RoadwayList));
                //IsSearchingRoadway = false;
                //IsSearchingMinorRoadway = false;
                Console.WriteLine();
            }
        }

        string searchMinorRoadway;
        public string SearchMinorRoadway
        {
            get => searchMinorRoadway;
            set
            {
                IsSearchingMinorRoadway = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchMinorRoadway), value);
                tempSearch = value;
                OnPropertyChanged(nameof(RoadwayList));
                //IsSearchingRoadway = false;
                //IsSearchingMajorRoadway = false;
                Console.WriteLine();
            }
        }

        string tempSearch = string.Empty;
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

                    if (tempSearch != null)
                    {
                        Console.WriteLine();
                        table = conn.Table<Roadway>().Where(i => i.RoadwayName.ToLower().Contains(tempSearch.ToLower())).
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

        /// MaterialCodeList,  MountingTypeList
        /// </summary>

        [ObservableProperty] MaterialCode selectedMatCode;
        [ObservableProperty] Mounting selectedMounting;
        [ObservableProperty] string siteName;
        [ObservableProperty] string ownerTagNumber;
        [ObservableProperty] string locationName;



        [ObservableProperty] string height;
        [ObservableProperty] string depth;
        [ObservableProperty] string width;
        [ObservableProperty] string notes;
        [ObservableProperty] CompassDirection selectedTravelDirection;
        [ObservableProperty] CompassDirection selectedOrientation;
        [ObservableProperty] CompassDirection selectedTravelDirection2;
        [ObservableProperty] string distanceEOTL;


        // Question 2
        /*
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
        [ObservableProperty] FilterType selectedFilterType;
        [ObservableProperty] FilterSize selectedFilterSize;
        [ObservableProperty] bool isHaveSunShield = false;
        [ObservableProperty] bool isHasGroundRod = false;
        [ObservableProperty] string uDSOwner;*/

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


                //new KeyValuePair<string, string>("manufactured_date", Manufactured),
                new KeyValuePair<string, string>("manufacturer", SelectedManuf?.ManufKey is null ? "" : SelectedManuf.ManufKey),  // manufacturer , for Cabinet, pull box
                new KeyValuePair<string, string>("manufacturer_key", ""),  // manufacturer , for Cabinet, pull box

                new KeyValuePair<string, string>("mod2", SelectedModelDetail?.ModelKey is null ? "" : SelectedModelDetail.ModelKey), /// model name, Building : x,  Cabinet/Pull Box : o
                new KeyValuePair<string, string>("pic2", ""),
                new KeyValuePair<string, string>("otag", ""),
                new KeyValuePair<string, string>("roadway", SelectedRoadway?.RoadwayKey is null ? "" : SelectedRoadway.RoadwayKey),
                new KeyValuePair<string, string>("pid", ""),
                new KeyValuePair<string, string>("loct", LocationName),
                //new KeyValuePair<string, string>("staddr", StreetAddress), // site_address
                //new KeyValuePair<string, string>("pscode", PostalCode),

                //new KeyValuePair<string, string>("btype", SelectedBuilding?.BuildingTypeKey is null ? "":SelectedBuilding.BuildingTypeKey),
                new KeyValuePair<string, string>("orientation", SelectedOrientation?.CompasKey is null ? "" : SelectedOrientation.CompasKey),

                new KeyValuePair<string, string>("laneclosure", IsLaneClosure ? "1":"0"),
                //new KeyValuePair<string, string>("dotdis",  SelectedDistrict is null ? "" : SelectedDistrict),
                //new KeyValuePair<string, string>("powr", IsHasPowerDisconnect ? "1":"0"),
                //new KeyValuePair<string, string>("elecsite", SelectedElectSiteKey),
                //new KeyValuePair<string, string>("comm", Is3rdComms ? "1":"0"),
                //new KeyValuePair<string, string>("commprovider", CommsProvider),
                //new KeyValuePair<string, string>("sitaddr", StreetAddress), // site_street_addres
                //new KeyValuePair<string, string>("udsowner", UDSOwner),

                new KeyValuePair<string, string>("rs2", "L"),

                //new KeyValuePair<string, string>("height2", Height),
                //new KeyValuePair<string, string>("depth2", Depth),
                //new KeyValuePair<string, string>("width2", Width),
                new KeyValuePair<string, string>("CLEAR_ZONE_IND2", IsSiteClearZone ? "1":"0"),

                new KeyValuePair<string, string>("intersect2", SelectedIntersection?.IntersectionKey is null ? "": SelectedIntersection.IntersectionKey),
                new KeyValuePair<string, string>("material2", SelectedMatCode?.MaterialKey is null ? "":SelectedMatCode.MaterialKey),
                new KeyValuePair<string, string>("mounting2", SelectedMounting?.MountingKey is null ? "":SelectedMounting.MountingKey),
                //new KeyValuePair<string, string>("offilter2", SelectedFilterType?.FilterTypeKey is null ? "": SelectedFilterType.FilterTypeKey ),//FilterTypeSelected),
                //new KeyValuePair<string, string>("fltrsize2", SelectedFilterSize?.FtSizeKey  is null ? "": SelectedFilterSize.FtSizeKey  ),//FilterSizeKeySelected),
                //new KeyValuePair<string, string>("sunshield2", IsHaveSunShield ? "1":"0"),
                //new KeyValuePair<string, string>("installed2", InstalledAt),
                new KeyValuePair<string, string>("comment2", Notes), // Notes, pr description

                new KeyValuePair<string, string>("gravel_bottom","" ),
                new KeyValuePair<string, string>("lid_pieces", ""),
                new KeyValuePair<string, string>("has_apron", ""),
                //new KeyValuePair<string, string>("rack_count", SelectedRackCount is null ? "" : SelectedRackCount),

                new KeyValuePair<string, string>("etc2", ""),
                new KeyValuePair<string, string>("fosc2", ""),
                new KeyValuePair<string, string>("vault2", ""),
                new KeyValuePair<string, string>("trlane2", DistanceEOTL),
                new KeyValuePair<string, string>("bucket2", IsBucketTruck ? "1":"0"),
                //new KeyValuePair<string, string>("serialno", SerialNumber),
                new KeyValuePair<string, string>("key", ""),
                new KeyValuePair<string, string>("ktype", ""), //SelectedKeyType),
                //new KeyValuePair<string, string>("ground", IsHasGroundRod ? "1":"0"),
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
            };


            return keyValues;

        }


        [ICommand]
        async void SaveContinue()
        {
            Console.WriteLine();
            try
            {

                // create new tab  BuildingQuestions2 page
                Console.WriteLine();

                var answer = await Application.Current.MainPage.DisplayAlert("Info", "Would you like Continue to Site Page#2 ?", "SAVE","PAGE #2");
                if (answer)
                {
                    var KVPair = keyvaluepair();
                    var result = await CloudDBService.InsertSiteQuestions1(KVPair);
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
                else
                {

                    ShellSection shell_section = new ShellSection
                    {
                        Title = "SITE#2",
                        Icon = "building.png"
                    };

                    shell_section.Items.Add(new ShellContent()
                    {
                        Content = new  BuildingQuestions2(),

                    });
                    AppShell.mytabbar.Items.Add(shell_section);
                    await Shell.Current.GoToAsync($"BuildingQuestions2?tag_number={TagNumber}&&site_type={Session.site_type_key}");  
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e.ToString());
                await Application.Current.MainPage.DisplayAlert("Warning", "Data Insert Problem", "BACK");
            }

        }

        [ICommand] void ManufacturerItemSelected()
        {
            IsSearching = false;
        }

        [ICommand] void RoadwayItemSelected()
        {
            IsSearchingRoadway = false;
        }
        [ICommand] void MajorRoadwayItemSelected()
        {
            IsSearchingMajorRoadway = false;
        }
        [ICommand] void MinorRoadwayItemSelected()
        {
            IsSearchingMinorRoadway = false;
        }

    }


}
