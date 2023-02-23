//// Obsolete ui
///  PCS 



using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Text;
using System.Web;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FTCollectorApp.Service;
using FTCollectorApp.Model;
using FTCollectorApp.View.Utils;
using SQLite;
using Xamarin.Forms;
using FTCollectorApp.Model.Reference;
using System.ComponentModel;



namespace FTCollectorApp.ViewModel
{




    public partial class CommonsiteVM : ObservableObject
    {
        static string BUILDING_SITE = "Building";
        static string CABINET_SITE = "Cabinet";
        static string PULLBOX_SITE = "PullBox";
        static string STRUCTURE_SITE = "Structure";

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(BufSiteColNamesList))]
        bool isBuildingSelected = false;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(BufSiteColNamesList))]
        bool isCabinetSelected = false;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(BufSiteColNamesList))]
        bool isPulboxSelected = false;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(BufSiteColNamesList))]
        bool isStructureSelected = false;


        public ObservableCollection<SiteColsType> BufSiteColNamesList
        {
            get
            {
                Console.WriteLine();
                if (SelectedMajorType.Equals(BUILDING_SITE))
                    return new ObservableCollection<SiteColsType>(BuildingColsTypes);
                else if (SelectedMajorType.Equals(CABINET_SITE))
                    return new ObservableCollection<SiteColsType>(CabinetColsTypes);
                else if (SelectedMajorType.Equals(BUILDING_SITE))
                    return new ObservableCollection<SiteColsType>(PullBoxColsTypes);
                else if (SelectedMajorType.Equals(STRUCTURE_SITE))
                    return new ObservableCollection<SiteColsType>(StructureColsTypes);
                else
                {
                    Console.WriteLine();
                    return new ObservableCollection<SiteColsType>();
                }
            }
        }


        ObservableCollection<SiteColsType> userAddSiteDetailInfo = new ObservableCollection<SiteColsType>();

        public ObservableCollection<SiteColsType> UserSelectedList
        {
            get
            {
                Console.WriteLine();

                return new ObservableCollection<SiteColsType>(userAddSiteDetailInfo);
            }
        }

        [ObservableProperty]
        bool isRoadwaySelected = false;

        //[ObservableProperty]
        SiteColsType selectedColToAdd;
        public SiteColsType SelectedColToAdd
        {
            get => selectedColToAdd;
            set
            {
                Console.WriteLine();
                // cek existence item
                /*if (userAddSiteDetailInfo.Count > 0) {
                    if (userAddSiteDetailInfo.Contains(value))
                    {
                        Console.WriteLine();
                        Application.Current.MainPage.DisplayAlert("Warning","This item already Existed.","CLOSE");
                        return;
                    }
                }*/

                SetProperty(ref selectedColToAdd, value);
                userAddSiteDetailInfo.Add(value);
                OnPropertyChanged(nameof(UserSelectedList));

                Console.WriteLine();


            }
        }




        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(MinorSiteList))]
        [AlsoNotifyChangeFor(nameof(BufSiteColNamesList))]
        string selectedMajorType = string.Empty;

        [ObservableProperty]
        string selectedMinorType;

        // Tag number confirmation - start

        [ObservableProperty]
        string reEnterStatus;
        [ObservableProperty]
        bool isTagNumberMatch = false;

        string tagNumber;
        public string TagNumber
        {
            get => tagNumber;
            set
            {
                Console.WriteLine();
                SetProperty(ref tagNumber, value);
                Session.tag_number = value;
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

        public ICommand CheckTagNumberCommand { get; set; }



        private void ExecuteCheckTagNumberCommand()
        {
            if (TagNumber.Equals(ReEnterTagNumber))
            {
                ReEnterStatus = "MATCH!";
                IsBuildingSelected = SelectedMajorType.Equals(BUILDING_SITE);
                IsCabinetSelected = SelectedMajorType.Equals(CABINET_SITE);
                IsPulboxSelected = SelectedMajorType.Equals(PULLBOX_SITE);
                IsStructureSelected = SelectedMajorType.Equals(STRUCTURE_SITE);
                IsTagNumberMatch = true;
            }
            else
            {
                ReEnterStatus = "No Match";
                IsTagNumberMatch = false;
            }
        }


        // Tag number confirmation - end


        ObservableCollection<CodeSiteType> CodeSiteTypeList;
        ObservableCollection<CodeSiteType> BufferMajorSite = new ObservableCollection<CodeSiteType>();
        ObservableCollection<BuildingSiteDetailInfo> buildingSiteDetailInfo =
        new ObservableCollection<BuildingSiteDetailInfo>()
        {
            new BuildingSiteDetailInfo{ ColName= "Building Specification", ColType = 1},
        };

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

        ObservableCollection <SiteColsType> BuildingColsTypes =
            new ObservableCollection<SiteColsType>()
            {
                // Entry string
                new SiteColsType{ ColName= "Owner Name", ColType = 1, IsEntry = true},
                new SiteColsType{ ColName= "Owner Tag Number", ColType = 2, IsEntry = true},
                new SiteColsType{ ColName= "Property Id", ColType = 3,IsEntry = true, IsDropDown=false, IsSwitch=false},
                new SiteColsType{ ColName= "PostalCode", ColType = 4,IsEntry = true},
                new SiteColsType{ ColName= "Enter Comms provider", ColType = 5, IsEntry = true, IsDropDown=false, IsSwitch=false},
                new SiteColsType{ ColName= "Enter UDS owner", ColType = 5, IsEntry = true, IsDropDown=false, IsSwitch=false},
                new SiteColsType{ ColName= "Enter serial number", ColType = 5, IsEntry = true, IsDropDown=false, IsSwitch=false},

                // Switch True False
                new SiteColsType{ ColName= "Lane Closure required ? ", ColType = 11,IsSwitch=true},
                new SiteColsType{ ColName= "Has Power disconnect", ColType = 12, IsSwitch=true },
                new SiteColsType{ ColName= "3rd party comms? ", ColType = 13,IsSwitch=true},
                new SiteColsType{ ColName= "Does it have a sun shield?", ColType = 14, IsSwitch=true },
                new SiteColsType{ ColName= "Has Ground Rod? ", ColType = 15,IsSwitch=true},
                new SiteColsType{ ColName= "Has Key?", ColType = 16, IsSwitch=true },
                new SiteColsType{ ColName= "Is Site in a clear zone? ", ColType = 17,IsSwitch=true},
                new SiteColsType{ ColName= "Bucket Truck ?", ColType = 18, IsSwitch=true },

                // Drop down
                new SiteColsType{ ColName= "Enter electric site key ", ColType = 20, IsDropDown=true},
                new SiteColsType{ ColName= "Enter the dot disctrict ", ColType = 21, IsDropDown=true},
                new SiteColsType{ ColName= "Height - Depth - Width", ColType = 22, IsDropDown=true},
                new SiteColsType{ ColName= "Material", ColType = 25, IsDropDown=true},
                new SiteColsType{ ColName= "Mounting", ColType = 26, IsDropDown=true},
                new SiteColsType{ ColName= "Filter Type", ColType = 27, IsDropDown=true},
                new SiteColsType{ ColName= "Filter size", ColType = 28, IsDropDown=true},
                new SiteColsType{ ColName= "# Of Racks ", ColType = 29, IsDropDown=true},
            };

        ObservableCollection<SiteColsType> CabinetColsTypes =
        new ObservableCollection<SiteColsType>()
        {
            // Entry string
            new SiteColsType{ ColName= "Owner Name", ColType = 1, IsEntry = true},
            new SiteColsType{ ColName= "Owner Tag Number", ColType = 2, IsEntry = true},
            new SiteColsType{ ColName= "Property Id", ColType = 3,IsEntry = true, IsDropDown=false, IsSwitch=false},
            new SiteColsType{ ColName= "StreetAddress", ColType = 4, IsEntry = true, IsDropDown=false, IsSwitch=false},
            new SiteColsType{ ColName= "PostalCode", ColType = 5,IsEntry = true},

            // Boolean type
            new SiteColsType{ ColName= "Lane Closure", ColType = 11,IsSwitch=true},
            new SiteColsType{ ColName= "Has Power disconnect", ColType = 12, IsSwitch=true },

            // Dropdown
            new SiteColsType{ ColName= "Dot District", ColType = 20, IsDropDown=true},
            new SiteColsType{ ColName= "Roadway", ColType = 21, IsDropDown=true},
            new SiteColsType{ ColName= "Intersection", ColType = 22, IsDropDown=true},

        };


        ObservableCollection<SiteColsType> PullBoxColsTypes = new ObservableCollection<SiteColsType>()
        {
            // Entry string
            new SiteColsType{ ColName= "Owner Name", ColType = 1, IsEntry = true},
            new SiteColsType{ ColName= "Owner Tag Number", ColType = 2, IsEntry = true},
            new SiteColsType{ ColName= "Property Id", ColType = 3,IsEntry = true, IsDropDown=false, IsSwitch=false},
            new SiteColsType{ ColName= "StreetAddress", ColType = 4, IsEntry = true, IsDropDown=false, IsSwitch=false},
            new SiteColsType{ ColName= "PostalCode", ColType = 5,IsEntry = true},

            // Boolean type
            new SiteColsType{ ColName= "Lane Closure", ColType = 11,IsSwitch=true},
            new SiteColsType{ ColName= "Has Power disconnect", ColType = 12, IsSwitch=true },

            // Dropdown
            new SiteColsType{ ColName= "Dot District", ColType = 20, IsDropDown=true},
            new SiteColsType{ ColName= "Roadway", ColType = 21, IsDropDown=true},
            new SiteColsType{ ColName= "Intersection", ColType = 22, IsDropDown=true},

        };

        ObservableCollection<SiteColsType> StructureColsTypes =
        new ObservableCollection<SiteColsType>()
        {
            // Entry string
            new SiteColsType{ ColName= "Enter Notes & comment", ColType = 1, IsEntry = true},

            // Boolean type
            new SiteColsType{ ColName= "Is Site in a clear zone?", ColType = 11,IsSwitch=true},
            new SiteColsType{ ColName= "Lane closure required ", ColType = 12, IsSwitch=true },
            new SiteColsType{ ColName= "Bucket Truck ", ColType = 13,IsSwitch=true},
            new SiteColsType{ ColName= "Is this a splice vault?", ColType = 14, IsSwitch=true },
            new SiteColsType{ ColName= "Gravel Bottom", ColType = 14,IsSwitch=true},
            new SiteColsType{ ColName= "Has Ground Rod", ColType = 15, IsSwitch=true },
            new SiteColsType{ ColName= "Has Apron ", ColType = 16, IsSwitch=true },
            new SiteColsType{ ColName= "Has Key? ", ColType = 17, IsSwitch=true },

            // Dropdown
            new SiteColsType{ ColName= "Enter distance to travel lane", ColType = 20, IsDropDown=true},
            new SiteColsType{ ColName= "Enter lid pieces", ColType = 21, IsDropDown=true},
            new SiteColsType{ ColName= "Height", ColType = 22, IsDropDown=true},
            new SiteColsType{ ColName= "Depth", ColType = 23, IsDropDown=true},
            new SiteColsType{ ColName= "Width", ColType = 24, IsDropDown=true},
        };



        [ObservableProperty]
        Color bckgdBtnColorB = Color.FromHex("#1976D2");

        [ObservableProperty]
        Color bckgdBtnColorC = Color.FromHex("#1976D2");

        [ObservableProperty]
        Color bckgdBtnColorPb = Color.FromHex("#1976D2");

        [ObservableProperty]
        Color bckgdBtnColorS = Color.FromHex("#1976D2");



        ReadGPSTimer timer;

        public CommonsiteVM()
        {
        Console.WriteLine();
        RemoveDetailInfoCmd = new Command(
            execute: () =>
            {
                userAddSiteDetailInfo.Remove(SelectedColToAdd);
                OnPropertyChanged(nameof(UserSelectedList));

                Console.WriteLine();
            }
        );
        CheckTagNumberCommand = new Command(() => ExecuteCheckTagNumberCommand());
        SiteClickedCmd = new Command<string>(
            execute : (string arg) => { SelectedMajorType = arg;
                bckgdBtnColorB = arg.Equals(BUILDING_SITE) ? Color.Orange : Color.FromHex("#1976D2");
                bckgdBtnColorC = arg.Equals(CABINET_SITE) ? Color.Orange : Color.FromHex("#1976D2");
                bckgdBtnColorPb = arg.Equals(PULLBOX_SITE) ? Color.Orange : Color.FromHex("#1976D2");
                bckgdBtnColorS = arg.Equals(STRUCTURE_SITE) ? Color.Orange : Color.FromHex("#1976D2");
                Console.WriteLine();
            }
        );

        

        if (timer == null)
        {
        //timer = new ReadGPSTimer(TimeSpan.FromSeconds(5), OnGPSTimerStart);
        //timer.Start();
        }


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

        // if we want to pass function with Dictionary
        // var dico = new Dictionary<int, Delegate>();
        // dico[1] = new Func<int, int, int>(Func1);

        }


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


        public ICommand SiteClickedCmd { get; set; }
        public ICommand RemoveDetailInfoCmd { get; set; }




        /// Roadway - intersection Properties - start

        [ObservableProperty]
        bool isSearchingRoadway = false;

        //[ObservableProperty]
        InterSectionRoad selectedIntersection;
        public InterSectionRoad SelectedIntersection
        {
            get => selectedIntersection;
            set
            {
                SetProperty(ref selectedIntersection, value);
                IsSearchingRoadway = false;
            }
        }


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
        /// Roadway - intersection - end
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
    }
}
