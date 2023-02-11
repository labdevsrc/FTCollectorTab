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

namespace FTCollectorApp.ViewModel
{

    public class SiteColsType
    {
        public string ColName { get; set; }
        public int ColType { get; set; }
    }


    public partial class CommonsiteVM : ObservableObject
    {
        static string BUILDING_SITE = "Building";
        static string CABINET_SITE = "Cabinet";
        static string PULLBOX_SITE = "PullBox";
        static string STRUCTURE_SITE = "Structure";


        List<SiteColsType> userAddSiteDetailInfo = new List<SiteColsType>();

        public ObservableCollection<SiteColsType> UserSelectList
        {
            get
            {
                Console.WriteLine();
                return new ObservableCollection<SiteColsType>(userAddSiteDetailInfo);
            }
        }


        //[ObservableProperty]
        SiteColsType selectedColToAdd;
        public SiteColsType SelectedColToAdd
        {
            get => selectedColToAdd;
            set
            {
                if (value == null)
                    return;
                SetProperty(ref selectedColToAdd, value);
                userAddSiteDetailInfo.Add(value);
                OnPropertyChanged(nameof(UserSelectList));   
                Console.WriteLine();
  
            }
        }

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(MinorSiteList))]
        string selectedMajorType;

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
                IsTagNumberMatch = true;
            }
            else
            {
                ReEnterStatus = "No Match";
                IsTagNumberMatch = false;
            }
        }


        public ObservableCollection<SiteColsType> SiteColNamesList
        {
            get
            {
                Console.WriteLine();
                return new ObservableCollection<SiteColsType>(siteColsTypes);
            }
        }

        // Tag number confirmation - end


        ObservableCollection<CodeSiteType> CodeSiteTypeList;
        ObservableCollection<CodeSiteType> BufferMajorSite = new ObservableCollection<CodeSiteType>();

        ObservableCollection<SiteColsType> siteColsTypes =
            new ObservableCollection<SiteColsType>()
            {
                // Entry string
                new SiteColsType{ ColName= "Owner Name", ColType = 1},
                new SiteColsType{ ColName= "Owner Tag Number", ColType = 2},
                new SiteColsType{ ColName= "Property Id", ColType = 3},
                new SiteColsType{ ColName= "StreetAddress", ColType = 4},
                new SiteColsType{ ColName= "PostalCode", ColType = 5},
                new SiteColsType{ ColName= "Dot District", ColType = 6},

                // Boolean type
                new SiteColsType{ ColName= "Lane Closure", ColType = 11},
                new SiteColsType{ ColName= "Has Power disconnect", ColType = 12},

            };


        Dictionary<string, int> DSitecoltypes = new Dictionary<string, int>()
        {
            { "Owner Name",1 },
                { "Owner Tag Number", 2 },
                { "Property Id", 3 },
                { "StreetAddress",4 },
                { "PostalCode", 5 },
            { "Lane Closure",6 },
            { "Dot District",7 },
            { "Has Power disconnect",8 },
            { "Electric Site Key" ,9 },
            { "Enter UDS owner",10 },
            { "Building Classification",11 },
            { "Choose a roadway" ,12 },
            { "Choose an intersection" ,13 },
            { "Direction of Travel",14 },
            { "Material",  15 },
            { "Mounting", 16 },
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
                    Console.WriteLine();
                }
                );
            CheckTagNumberCommand = new Command(() => ExecuteCheckTagNumberCommand());
            BldBtnClickedCmd = new Command(
                execute : () => { SelectedMajorType = BUILDING_SITE;
                    bckgdBtnColorB = Color.FromHex("#1976D2");
                    bckgdBtnColorC = Color.LightBlue;
                    bckgdBtnColorPb = Color.FromHex("#1976D2");
                    bckgdBtnColorS = Color.FromHex("#1976D2");
                }
                );
            CabBtnClickedCmd = new Command(
                execute: () => { SelectedMajorType = CABINET_SITE;
                    bckgdBtnColorB = Color.FromHex("#1976D2");
                    bckgdBtnColorC = Color.LightGreen;
                    bckgdBtnColorPb = Color.FromHex("#1976D2");
                    bckgdBtnColorS = Color.FromHex("#1976D2");

                }
            );
            PullBoxBtnClickedCmd = new Command(
                execute: () => { SelectedMajorType = PULLBOX_SITE;
                    bckgdBtnColorB = Color.LightBlue;
                    bckgdBtnColorC = Color.LightBlue;
                    bckgdBtnColorPb = Color.LightGreen;
                    bckgdBtnColorS = Color.LightBlue;
                }
            );
            StructBtnClickedCmd = new Command(
                execute: () => {
                    SelectedMajorType = STRUCTURE_SITE ;
                    bckgdBtnColorB = Color.LightBlue;
                    bckgdBtnColorC = Color.LightBlue;
                    bckgdBtnColorPb = Color.LightBlue;
                    bckgdBtnColorS = Color.LightGreen;
                }
            );


            if (timer == null)
            {
                timer = new ReadGPSTimer(TimeSpan.FromSeconds(5), OnGPSTimerStart);
                timer.Start();
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


        public ICommand BldBtnClickedCmd { get; set; }

        public ICommand CabBtnClickedCmd { get; set; }
        public ICommand StructBtnClickedCmd { get; set; }
        public ICommand PullBoxBtnClickedCmd { get; set; }
        public ICommand RemoveDetailInfoCmd { get; set; }

    }
}
