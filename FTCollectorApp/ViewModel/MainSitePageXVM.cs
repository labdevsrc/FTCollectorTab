/// Current active 


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

    public partial class MainSitePageXVM : ObservableObject
    {
        static string BUILDING_SITE = "Building";
        static string CABINET_SITE = "Cabinet";
        static string PULLBOX_SITE = "PullBox";
        static string STRUCTURE_SITE = "Structure";

        [ObservableProperty]
        bool isBuildingSelected = false;

        [ObservableProperty]
        bool isCabinetSelected = false;

        [ObservableProperty]
        bool isPulboxSelected = false;

        [ObservableProperty]
        bool isStructureSelected = false;

        [ObservableProperty]
        bool isRoadwaySelected = false;

        //[ObservableProperty]
        //[AlsoNotifyChangeFor(nameof(MinorSiteList))]
        string selectedMajorType = string.Empty;
        public string SelectedMajorType
        {
            get => selectedMajorType;
            set
            {
                SetProperty(ref selectedMajorType, value);

                if (SelectedMajorType.Equals(BUILDING_SITE))
                {
                    IsBuildingSelected = true;
                    IsCabinetSelected = false;
                    IsPulboxSelected = false;
                    IsStructureSelected = false;
                }
                else if (SelectedMajorType.Equals(CABINET_SITE))
                    IsCabinetSelected = true;

                else if (SelectedMajorType.Equals(PULLBOX_SITE))
                    IsPulboxSelected = true;
                else if (SelectedMajorType.Equals(STRUCTURE_SITE))
                    IsStructureSelected = true;

                OnPropertyChanged(nameof(MinorSiteList));

            }
        }

        [ObservableProperty]
        string selectedMinorType = string.Empty;

        // Tag number confirmation - start

        [ObservableProperty]
        string reEnterStatus;
        [ObservableProperty]
        bool isTagNumberMatch = false;

        string tagNumber = string.Empty;
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
        public ICommand RecordGPSCommand { get; set; }


        private void ExecuteCheckTagNumberCommand()
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


        // Tag number confirmation - end


        [ObservableProperty]
        Color bckgdBtnColorB = Color.FromHex("#1976D2");

        [ObservableProperty]
        Color bckgdBtnColorC = Color.FromHex("#1976D2");

        [ObservableProperty]
        Color bckgdBtnColorPb = Color.FromHex("#1976D2");

        [ObservableProperty]
        Color bckgdBtnColorS = Color.FromHex("#1976D2");


        ObservableCollection<CodeSiteType> CodeSiteTypeList;
        ReadGPSTimer timer;

        public MainSitePageXVM()
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

            CheckTagNumberCommand = new Command(() => ExecuteCheckTagNumberCommand());
            RecordGPSCommand = new Command(

                execute: async () =>
                {
                    string result = String.Empty;
                    Console.WriteLine();
                    
                    string codekey = CodeSiteTypeList.Where(a => (a.MajorType == SelectedMajorType) && (a.MinorType == SelectedMinorType)).Select(a => a.CodeKey).First();
                    Session.site_type_key = codekey;
                    Session.site_major = SelectedMajorType;
                    Session.site_minor = SelectedMinorType;


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

                        var OkAnswer = await Application.Current.MainPage.DisplayAlert("DONE", result.Equals("CREATE_DONE") ? "Create Site Success" : "Update Site Success", "Goto " + SelectedMajorType, "Create Again");
                        if (OkAnswer)
                        {
                            // stop timer gps
                            if(timer != null)
                                timer.Stop();
                            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(true);
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Unknown Response", result, "TRY AGAIN");
                    }

                },
                canExecute: () => {

                    if (TagNumber.Length > 2)
                    {
                        Console.WriteLine();
                        return IsTagNumberMatch;
                    }
                    else
                    {
                        Console.WriteLine();
                        return false;
                    }
                }
            );

            /*SiteClickedCmd = new Command<string>(
                execute: (string arg) => {
                    SelectedMajorType = arg;
                    bckgdBtnColorB = arg.Equals(BUILDING_SITE) ? Color.Orange : Color.FromHex("#1976D2");
                    bckgdBtnColorC = arg.Equals(CABINET_SITE) ? Color.Orange : Color.FromHex("#1976D2");
                    bckgdBtnColorPb = arg.Equals(PULLBOX_SITE) ? Color.Orange : Color.FromHex("#1976D2");
                    bckgdBtnColorS = arg.Equals(STRUCTURE_SITE) ? Color.Orange : Color.FromHex("#1976D2");
                    Console.WriteLine();
                }
            );*/



            if (timer == null)
            {
                timer = new ReadGPSTimer(TimeSpan.FromSeconds(5), OnGPSTimerStart);
                timer.Start();
            }

            Console.WriteLine();


            // if we want to pass function with Dictionary
            // var dico = new Dictionary<int, Delegate>();
            // dico[1] = new Func<int, int, int>(Func1);

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







        ///// Obsolete - will be deleted
        ///
        ObservableCollection<SiteColsType> BuildingColsTypes =
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

    }
}
