using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Model.AWS;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.View;
using FTCollectorApp.View.SyncPages;
using FTCollectorApp.View.TraceFiberPages;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class DuctTraceViewModel : ObservableObject
    {
        string[] colorFiberHex = { "#0000FF", "#FFA500", "#008000", "#A52A2A", "#708090", "#FFFFFF", "#FF0000", "#00000", "#FFFF00", "#963D7F", "#FF00FF", "#00FFFF" };
        string[] colorFiber = { "Blue", "Orange", "Green", "Brown", "Slate", "White", "Red", "Black", "Yellow", "Violet", "Rose", "Aqua" };

        /// UOM autocomplete - start              
        UnitOfMeasure selectedUOM;
        public UnitOfMeasure SelectedUOM
        {
            get => selectedUOM;
            set
            {
                SetProperty(ref (selectedUOM), value);
                SearchUOM = value.UOMUnit;
                //OnPropertyChanged(nameof(DuctConduitDatas));
                OnPropertyChanged(nameof(SearchUOM));
            }
        }

        [ObservableProperty]
        bool isSearchingUOM = false;

        string searchUOM;
        public string SearchUOM
        {
            get => searchUOM;
            set
            {
                IsSearchingUOM = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchUOM), value);
                OnPropertyChanged(nameof(UnitOfMeasures));
                Console.WriteLine();
            }
        }
        /// UOM autocomplete - end  


        /// Install methode autocomplete - start              
        DuctInstallType selectedDuctInstall;
        public DuctInstallType SelectedDuctInstall
        {
            get => selectedDuctInstall;
            set
            {
                SetProperty(ref (selectedDuctInstall), value);
                SearchInstallType = value.DuctInstallDesc;
                //OnPropertyChanged(nameof(DuctConduitDatas));
                OnPropertyChanged(nameof(SearchInstallType));
            }
        }
        [ObservableProperty]
        bool isSearchInstallType = false;

        string searchInstallType;
        public string SearchInstallType
        {
            get => searchInstallType;
            set
            {
                IsSearchInstallType = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchInstallType), value);
                OnPropertyChanged(nameof(DuctInstallList));
                Console.WriteLine();
            }
        }
        ///  Install methode autocomplete  - end  


        /// AutoComplete Cable1 - start              
        AFiberCable selectedCable1;
        public AFiberCable SelectedCable1
        {
            get => selectedCable1;
            set
            {
                SetProperty(ref (selectedCable1), value);
                SearchCable1 = value.CableIdDesc;
                //OnPropertyChanged(nameof(DuctConduitDatas));
                OnPropertyChanged(nameof(SearchCable1));

                // For locate point page, gps_point table
                Session.Cable1 = new AFiberCable
                {
                    AFRKey = value.AFRKey,
                    CableType = value.CableType
                };
            }
        }
        [ObservableProperty]
        bool isSearchingCable1 = false;

        string searchCable1;
        public string SearchCable1
        {
            get => searchCable1;
            set
            {
                IsSearchingCable1 = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchCable1), value);
                OnPropertyChanged(nameof(aFiberCableList1));
                Console.WriteLine();
            }
        }
        /// AutoComplete Cable1 - end    

        /// AutoComplete Cable2 - start              
        AFiberCable selectedCable2;
        public AFiberCable SelectedCable2
        {
            get => selectedCable2;
            set
            {
                SetProperty(ref (selectedCable2), value);
                SearchCable2 = value.CableIdDesc;
                //OnPropertyChanged(nameof(DuctConduitDatas));
                OnPropertyChanged(nameof(SearchCable2));

                // For locate point page, gps_point table
                Session.Cable1 = new AFiberCable
                {
                    AFRKey = value.AFRKey,
                    CableType = value.CableType
                };
            }
        }
        [ObservableProperty]
        bool isSearchingCable2 = false;

        string searchCable2;
        public string SearchCable2
        {
            get => searchCable2;
            set
            {
                IsSearchingCable2 = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchCable2), value);
                OnPropertyChanged(nameof(aFiberCableList2));
                Console.WriteLine();
            }
        }
        /// AutoComplete Cable2 - end 

        /// AutoComplete Cable3 - start              
        AFiberCable selectedCable3;
        public AFiberCable SelectedCable3
        {
            get => selectedCable3;
            set
            {
                SetProperty(ref (selectedCable3), value);
                SearchCable3 = value.CableIdDesc;
                //OnPropertyChanged(nameof(DuctConduitDatas));
                OnPropertyChanged(nameof(SearchCable3));

                // For locate point page, gps_point table
                Session.Cable1 = new AFiberCable
                {
                    AFRKey = value.AFRKey,
                    CableType = value.CableType
                };
            }
        }
        [ObservableProperty]
        bool isSearchingCable3 = false;

        string searchCable3;
        public string SearchCable3
        {
            get => searchCable3;
            set
            {
                IsSearchingCable3 = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchCable3), value);
                OnPropertyChanged(nameof(aFiberCableList3));
                Console.WriteLine();
            }
        }
        /// AutoComplete Cable3 - end 

        /// AutoComplete Cable4 - start              
        AFiberCable selectedCable4;
        public AFiberCable SelectedCable4
        {
            get => selectedCable4;
            set
            {
                SetProperty(ref (selectedCable4), value);
                SearchCable4 = value.CableIdDesc;
                //OnPropertyChanged(nameof(DuctConduitDatas));
                OnPropertyChanged(nameof(SearchCable4));

                // For locate point page, gps_point table
                Session.Cable1 = new AFiberCable
                {
                    AFRKey = value.AFRKey,
                    CableType = value.CableType
                };
            }
        }
        [ObservableProperty]
        bool isSearchingCable4 = false;

        string searchCable4;
        public string SearchCable4
        {
            get => searchCable4;
            set
            {
                IsSearchingCable4 = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchCable4), value);
                OnPropertyChanged(nameof(aFiberCableList4));
                Console.WriteLine();
            }
        }
        /// AutoComplete Cable4 - end 



        ConduitsGroup selectedTagNum;
        public ConduitsGroup SelectedTagNum {
            get => selectedTagNum;
            set
            {
                SetProperty(ref (selectedTagNum), value);
                SearchTag = value.HosTagNumber;
                Console.WriteLine("SelectedTagNum");
                OnPropertyChanged(nameof(DuctConduitDatas));
                OnPropertyChanged(nameof(SearchTag));


                // populate FromDuct , it wiill be use in Locate point (gps_point table)and in End Trace page (a_fiber_cable)
                Session.FromDuct = new ConduitsGroup {
                    ConduitKey = value.ConduitKey,
                    Direction = value.Direction,
                    DirCnt = value.DirCnt,
                    HosTagNumber = value.HosTagNumber,
                    HostSiteKey = value.HostSiteKey,
                    HostType = value.HostType,
                    HostTypeKey = value.HostTypeKey
                };

            }
        }


        [ObservableProperty]
        bool isSearching = false;

        string searchTag;
        public string SearchTag
        {
            get
            {
                Console.WriteLine();
                return searchTag;
            }
            set
            {
                IsSearching = string.IsNullOrEmpty(value) ? false : true;

                SetProperty(ref (searchTag), value);
                OnPropertyChanged(nameof(SiteInListView));
                Console.WriteLine();
            }
        }

        ConduitsGroup selectedDuct;
        public ConduitsGroup SelectedDuct
        {
            get=> selectedDuct;

            set
            {

                if (value?.DuctColor != null)
                {
                    if (int.Parse(value.DuctColor) > 0)
                    {
                        value.ColorName = colorFiber[int.Parse(value.DuctColor) - 1];
                        value.ColorHex = colorFiberHex[int.Parse(value.DuctColor) - 1];
                    }
                }
                Console.WriteLine();
                
                Session.ToDuct = value;
                SetProperty(ref selectedDuct, value);

            }
        }


        [ObservableProperty]
        string sheathMark1;

        [ObservableProperty]
        string sheathMark2;

        [ObservableProperty]
        string sheathMark3;

        [ObservableProperty]
        string sheathMark4;



        ObservableCollection<ConduitsGroup> ConduitsGroupListTable;
        public ObservableCollection<ColorCode> ColorHextList;


        public ICommand SaveAndContinueCommand { get; set; }
        //public ICommand SaveLocallyAndContinueCommand { get; set; }
        //public ICommand RemoveCable1Command { get; set; }

        //public ICommand RemoveCable2Command { get; set; }
        //public ICommand RemoveCable3Command { get; set; }
        //public ICommand RemoveCable4Command { get; set; }
        public DuctTraceViewModel()
        {
            SaveAndContinueCommand = new Command(ExecuteSaveAndContinueCommand);
            //SaveLocallyAndContinueCommand = new Command(ExecuteSaveLocally);
            //RemoveCable1Command = new Command(ExecuteRemoveCable1Command);
            //RemoveCable2Command = new Command(ExecuteRemoveCable2Command);
            //RemoveCable3Command = new Command(ExecuteRemoveCable3Command);
            //RemoveCable4Command = new Command(ExecuteRemoveCable4Command);


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<ConduitsGroup>();
                var table1 = conn.Table<ConduitsGroup>().Where(a => a.OwnerKey == Session.ownerkey).ToList();
                ConduitsGroupListTable = new ObservableCollection<ConduitsGroup>(table1);

                conn.CreateTable<ColorCode>();
                var table2 = conn.Table<ColorCode>().ToList();
                ColorHextList = new ObservableCollection<ColorCode>(table2);


            }
            Session.current_page = "duct";

            
        }

        [ICommand]
        void SaveLocally()
        {
            Insert2SQLite();
        }

        [ICommand]
        async void DisplayTask()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SyncPage());
        }

        [ICommand]
        void RemoveCable1()
        //private void ExecuteRemoveCable1Command()
        {
            SearchCable1 = "";
            //SelectedCable1 = null;
            Console.WriteLine();
            SheathMark1 = "";
        }

        [ICommand]
        void RemoveCable2()
        //private void ExecuteRemoveCable2Command()
        {
            SearchCable2 = "";
            //SelectedCable2 = null;
            Console.WriteLine();
            SheathMark2 = "";
        }

        [ICommand]
        void RemoveCable3()
        //private void ExecuteRemoveCable3Command()
        {
            SearchCable3 = "";
            Console.WriteLine();
            //SelectedCable3 = null;
            SheathMark3 = "";
        }

        [ICommand]
        void RemoveCable4()
        //private void ExecuteRemoveCable4Command()
        {
            SearchCable4 = "";
            //SelectedCable4 = null;
            Console.WriteLine();
            SheathMark4 = "";
        }

        public ObservableCollection<AFiberCable> aFiberCableList1
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<AFiberCable>();
                    // City of Port St Lucie for Demo purpose
                    var AFRAll = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey && a.JobNumber == Session.jobnum).ToList();
                    foreach (var col in AFRAll)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }

                    if (SearchCable1 != null)
                    {
                        var table = AFRAll.Where(i => i.CableIdDesc.ToLower().Contains(SearchCable1.ToLower())).
                            GroupBy(b => b.CableIdDesc).Select(g => g.First()).ToList();

                        Console.WriteLine();
                        return new ObservableCollection<AFiberCable>(table);
                    }
                    return new ObservableCollection<AFiberCable>(AFRAll);
                }
            }
        }

        public ObservableCollection<AFiberCable> aFiberCableList2
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<AFiberCable>();
                    // City of Port St Lucie for Demo purpose
                    var AFRAll = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey && a.JobNumber == Session.jobnum).ToList();
                    foreach (var col in AFRAll)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }

                    if (SearchCable2 != null)
                    {
                        var table = AFRAll.Where(i => i.CableIdDesc.ToLower().Contains(SearchCable2.ToLower())).
                            GroupBy(b => b.CableIdDesc).Select(g => g.First()).ToList();

                        Console.WriteLine();
                        return new ObservableCollection<AFiberCable>(table);
                    }
                    return new ObservableCollection<AFiberCable>(AFRAll);
                }
            }
        }

        public ObservableCollection<AFiberCable> aFiberCableList3
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<AFiberCable>();
                    // City of Port St Lucie for Demo purpose
                    var AFRAll = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey && a.JobNumber == Session.jobnum).ToList();
                    foreach (var col in AFRAll)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }

                    if (SearchCable3 != null)
                    {
                        var table = AFRAll.Where(i => i.CableIdDesc.ToLower().Contains(SearchCable3.ToLower())).
                            GroupBy(b => b.CableIdDesc).Select(g => g.First()).ToList();

                        Console.WriteLine();
                        return new ObservableCollection<AFiberCable>(table);
                    }
                    return new ObservableCollection<AFiberCable>(AFRAll);
                }
            }
        }


        public ObservableCollection<AFiberCable> aFiberCableList4
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<AFiberCable>();
                    // City of Port St Lucie for Demo purpose
                    var AFRAll = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey && a.JobNumber == Session.jobnum).ToList();
                    foreach (var col in AFRAll)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }

                    if (SearchCable4 != null)
                    {
                        var table = AFRAll.Where(i => i.CableIdDesc.ToLower().Contains(SearchCable4.ToLower())).
                            GroupBy(b => b.CableIdDesc).Select(g => g.First()).ToList();

                        Console.WriteLine();
                        return new ObservableCollection<AFiberCable>(table);
                    }
                    return new ObservableCollection<AFiberCable>(AFRAll);
                }
            }
        }



        public ObservableCollection<DuctInstallType> DuctInstallList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctInstallType>();
                    var table = conn.Table<DuctInstallType>().ToList();
                    if (SearchInstallType != null)
                    {
                        table = conn.Table<DuctInstallType>().Where(i => i.DuctInstallDesc.ToLower().Contains(SearchInstallType.ToLower())).
                            GroupBy(b => b.DuctInstallDesc).Select(g => g.First()).ToList();

                    }
                    return new ObservableCollection<DuctInstallType>(table);
                }
            }
        }
        public ObservableCollection<UnitOfMeasure> UnitOfMeasures
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<UnitOfMeasure>();
                    var table = conn.Table<UnitOfMeasure>().ToList();
                    if (SearchUOM != null)
                    {
                        table = conn.Table<UnitOfMeasure>().Where(i => i.UOMUnit.ToLower().Contains(SearchUOM.ToLower())).
                            GroupBy(b => b.UOMUnit).Select(g => g.First()).ToList();

                    }
                    return new ObservableCollection<UnitOfMeasure>(table);
                }
            }
        }

        public ObservableCollection<ConduitsGroup> BeginningSiteList
        {
            get
            {
                var table = ConduitsGroupListTable.GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                Console.WriteLine("BeginningSite");
                return new ObservableCollection<ConduitsGroup>(table);

            }
        }


        public ObservableCollection<ConduitsGroup> DuctConduitDatas
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    var table = ConduitsGroupListTable.ToList();
                    if (SelectedTagNum?.HosTagNumber != null)
                    {
                        table = ConduitsGroupListTable.Where(b => b.HosTagNumber == SelectedTagNum.HosTagNumber).ToList();
                        Console.WriteLine();
                    }
                    foreach (var col in table)
                    {
                        col.DuctSize = HttpUtility.HtmlDecode(col.DuctSize);
                        col.WhichDucts = col.Direction + " " + col.DirCnt;
                    }
                    Console.WriteLine("DuctConduitDatas ");
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }

        public ObservableCollection<Site> Sites
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Site>();
                    var tableSite = conn.Table<Site>().OrderBy(g => g.TagNumber).ToList();
                    return new ObservableCollection<Site>(tableSite);
                }
            }
        }

        public ObservableCollection<ConduitsGroup> SiteInListView
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {

                    var table = ConduitsGroupListTable.GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                    if (SearchTag != null)
                    {
                        Console.WriteLine();
                        table = ConduitsGroupListTable.Where(i => i.HosTagNumber.ToLower().Contains(SearchTag.ToLower())).
                            GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                    }
                    Console.WriteLine();
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }


        List<KeyValuePair<string, string>> mainKVpair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("ownerkey", Session.ownerkey), // 
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                new KeyValuePair<string, string>("job", Session.jobnum), // 
                new KeyValuePair<string, string>("job_key", Session.jobkey), // 

                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("from_site", SelectedTagNum?.HosTagNumber is null ?"0" : SelectedTagNum.HosTagNumber  ),
                new KeyValuePair<string, string>("from_site_key", SelectedTagNum?.HostSiteKey is null ?"0" : SelectedTagNum.HostSiteKey  ),
                new KeyValuePair<string, string>("from_site_duct", SelectedDuct?.ConduitKey is null ?"0" : SelectedDuct.ConduitKey  ),  // 2
                new KeyValuePair<string, string>("from_site_duct_key", SelectedDuct?.ConduitKey is null ?"0" : SelectedDuct.ConduitKey  ),  // 2
                new KeyValuePair<string, string>("install_method", SelectedDuctInstall?.DuctInstallKey is null ? "0":SelectedDuctInstall.DuctInstallKey),  // 7
                new KeyValuePair<string, string>("uom", selectedUOM?.UOMKey is null ? "0":selectedUOM.UOMKey),  // 7
                new KeyValuePair<string, string>("stage", Session.stage),  // 7


                new KeyValuePair<string, string>("from_site_duct_direction", SelectedDuct?.Direction is null ? "":SelectedDuct.Direction),
                new KeyValuePair<string, string>("from_site_duct_direction_count", SelectedDuct?.DirCnt is null ? "":SelectedDuct.DirCnt),


            };
            return keyValues;

        }

        List<KeyValuePair<string, string>> allKVPair()
        {

            var allKVpair = mainKVpair();

            var keyValues = new List<KeyValuePair<string, string>>{

                new KeyValuePair<string, string>("sheath_mark1", SheathMark1),
                new KeyValuePair<string, string>("sheath_mark2", SheathMark2),
                new KeyValuePair<string, string>("sheath_mark3", SheathMark3),
                new KeyValuePair<string, string>("sheath_mark4", SheathMark4),
                new KeyValuePair<string, string>("cable_id1", SelectedCable1?.CableIdDesc is null ? "":SelectedCable1.CableIdDesc),
                new KeyValuePair<string, string>("cable_id2", SelectedCable2?.CableIdDesc is null ? "":SelectedCable2.CableIdDesc),
                new KeyValuePair<string, string>("cable_id3", SelectedCable3?.CableIdDesc is null ? "":SelectedCable3.CableIdDesc),
                new KeyValuePair<string, string>("cable_id4", SelectedCable4?.CableIdDesc is null ? "":SelectedCable4.CableIdDesc),

                new KeyValuePair<string, string>("cable_type1", SelectedCable1?.CableType is null ? "":SelectedCable1.CableType),
                new KeyValuePair<string, string>("cable_type2", SelectedCable2?.CableType is null ? "":SelectedCable2.CableType),
                new KeyValuePair<string, string>("cable_type3", SelectedCable3?.CableType is null ? "":SelectedCable3.CableType),
                new KeyValuePair<string, string>("cable_type4", SelectedCable4?.CableType is null ? "":SelectedCable4.CableType),

                new KeyValuePair<string, string>("cable_id1_key", SelectedCable1?.AFRKey is null ? "":SelectedCable1.AFRKey),
                new KeyValuePair<string, string>("cable_id2_key", SelectedCable2?.AFRKey is null ? "":SelectedCable2.AFRKey),
                new KeyValuePair<string, string>("cable_id3_key", SelectedCable3?.AFRKey is null ? "":SelectedCable3.AFRKey),
                new KeyValuePair<string, string>("cable_id4_key", SelectedCable4?.AFRKey is null ? "":SelectedCable4.AFRKey),
            };

            allKVpair.AddRange(keyValues);

            return allKVpair;
        }



        void Insert2SQLite()
        {
            var a_fiber_segment_table = new List<a_fiber_segment>();
            int maxId = 1;
            string listTaskID = string.Empty;

            // put to local SQLite
            Console.WriteLine();

            if (Application.Current.Properties.ContainsKey(Constants.TaskCount))
                maxId = (int) Application.Current.Properties[Constants.TaskCount];



            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                try
                {
                    // create a_fiber_segment if not exist yet
                    conn.CreateTable<a_fiber_segment>();
                    var table = conn.Table<a_fiber_segment>().Select(a => a.id).ToList();
                    var currTask = new UnSyncTaskList
                    {
                        StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        TargetTable = "a_fiber_segment",
                        ajaxTarget = Constants.ajaxSaveDuctTrace,
                        taskName = "INSERT_AFIBERSEGMENT",
                        Status = "UNSYNC",
                        rowCount = conn.Table<a_fiber_segment>().Count().ToString()
                    };


                    foreach (var col in table)
                    {                        
                        if (col > maxId)
                            maxId = col;
                    }
                    Console.WriteLine("maxId :" + maxId);

                    //var a_fiber_segmentKV = mainKVpair();

                    var A_Fiber_Segment = new a_fiber_segment
                    {
                        owner_key = Session.ownerkey,
                        OWNER_CD = Session.ownerCD,
                        job = Session.jobnum,
                        job_key = Session.jobkey,
                        from_site = SelectedTagNum?.HosTagNumber is null ? "0" : SelectedTagNum.HosTagNumber,
                        from_site_key = SelectedTagNum?.HostSiteKey is null ? "0" : SelectedTagNum.HostSiteKey,
                        from_site_duct = SelectedDuct?.ConduitKey is null ? "0" : SelectedDuct.ConduitKey,
                        from_site_duct_key = SelectedDuct?.ConduitKey is null ? "0" : SelectedDuct.ConduitKey,
                        install_method = SelectedDuctInstall?.DuctInstallKey is null ? "0" : SelectedDuctInstall.DuctInstallKey,
                        uom = selectedUOM?.UOMKey is null ? "0" : selectedUOM.UOMKey,
                        stage = Session.stage,
                        created_on = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        created_by = Session.uid,
                        SyncStatus = "NOTSYNC"
                    };



                    // Insert 1 row with Cable1
                    if (SelectedCable1 != null)
                    {

                        A_Fiber_Segment.AWSid2 = maxId++;
                        A_Fiber_Segment.sheath_out = SheathMark1;
                        A_Fiber_Segment.cable_id = SelectedCable1?.CableIdDesc is null ? "" : SelectedCable1.CableIdDesc;
                        A_Fiber_Segment.cable_id_key = SelectedCable1?.AFRKey is null ? "" : SelectedCable1.AFRKey;
                        A_Fiber_Segment.cable_type = SelectedCable1?.CableType is null ? "" : SelectedCable1.CableType;

                        conn.Insert(A_Fiber_Segment);
                        listTaskID = maxId.ToString();
                        //currTask.TaskIdList.Add(maxId);

                        Application.Current.Properties[Constants.TaskCount] = maxId;

                    }
                    Console.WriteLine();

                    // Insert 1 row with Cable2
                    if (SelectedCable2 != null)
                    {

                        A_Fiber_Segment.AWSid2 = maxId++;
                        A_Fiber_Segment.sheath_out = SheathMark2;
                        A_Fiber_Segment.cable_id = SelectedCable2?.CableIdDesc is null ? "" : SelectedCable2.CableIdDesc;
                        A_Fiber_Segment.cable_id_key = SelectedCable2?.AFRKey is null ? "" : SelectedCable2.AFRKey;
                        A_Fiber_Segment.cable_type = SelectedCable2?.CableType is null ? "" : SelectedCable2.CableType;

                        conn.Insert(A_Fiber_Segment);

                        if(string.IsNullOrEmpty(listTaskID))
                            listTaskID = maxId.ToString();
                        else
                            listTaskID = listTaskID + "," + maxId.ToString();
                        //currTask.TaskIdList.Add(maxId);
                        Application.Current.Properties[Constants.TaskCount] = maxId;
                    }
                    Console.WriteLine();
                    // Insert 1 row with Cable3
                    if (SelectedCable3 != null)
                    {

                        A_Fiber_Segment.AWSid2 = maxId++;
                        A_Fiber_Segment.sheath_out = SheathMark3;
                        A_Fiber_Segment.cable_id = SelectedCable3?.CableIdDesc is null ? "" : SelectedCable3.CableIdDesc;
                        A_Fiber_Segment.cable_id_key = SelectedCable3?.AFRKey is null ? "" : SelectedCable3.AFRKey;
                        A_Fiber_Segment.cable_type = SelectedCable3?.CableType is null ? "" : SelectedCable3.CableType;

                        conn.Insert(A_Fiber_Segment);
                        //currTask.TaskIdList.Add(maxId);
                        if (string.IsNullOrEmpty(listTaskID))
                            listTaskID = maxId.ToString();
                        else
                            listTaskID = listTaskID + "," + maxId.ToString();
                        Application.Current.Properties[Constants.TaskCount] = maxId;
                    }

                    // Insert 1 row with Cable3
                    if (SelectedCable4 != null)
                    {

                        A_Fiber_Segment.AWSid2 = maxId++;
                        A_Fiber_Segment.sheath_out = SheathMark4;
                        A_Fiber_Segment.cable_id = SelectedCable4?.CableIdDesc is null ? "" : SelectedCable4.CableIdDesc;
                        A_Fiber_Segment.cable_id_key = SelectedCable4?.AFRKey is null ? "" : SelectedCable4.AFRKey;
                        A_Fiber_Segment.cable_type = SelectedCable4?.CableType is null ? "" : SelectedCable4.CableType;

                        conn.Insert(A_Fiber_Segment);

                        //currTask.TaskIdList.Add(maxId);
                        if (string.IsNullOrEmpty(listTaskID))
                            listTaskID = maxId.ToString();
                        else
                            listTaskID = listTaskID + "," + maxId.ToString();
                        Application.Current.Properties[Constants.TaskCount] = maxId;
                    }


                    currTask.TaskIdList = listTaskID; // text concatenate
                    currTask.TableID = maxId.ToString();

                    a_fiber_segment_table = conn.Table<a_fiber_segment>().ToList();

                    Console.WriteLine();

                    // update pending tasklist 
                    conn.CreateTable<UnSyncTaskList>();
                    conn.Insert(currTask);

                    // Add to session
                    Session.TaskPendingList?.Add(currTask);

                    foreach (var list in a_fiber_segment_table)
                    {
                        Console.WriteLine(list?.ToString());
                    }
                    
                }catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private async void ExecuteSaveAndContinueCommand()
        {

            if (string.IsNullOrEmpty(SelectedCable1?.CableIdDesc) 
                && string.IsNullOrEmpty(SelectedCable2?.CableIdDesc)
                && string.IsNullOrEmpty(SelectedCable3?.CableIdDesc)
                && string.IsNullOrEmpty(SelectedCable4?.CableIdDesc)
                )
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Cable 1 or 2 or 3 or 4 shouldn't be empty","OK");
                return;
            }

            //cek tag number or beginning site
            if (string.IsNullOrEmpty(SelectedTagNum.HosTagNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Tag number shouldn't be empty", "OK");
                return;
            }

            //cek which duct is empty or not
            if (string.IsNullOrEmpty(SelectedDuct.WhichDucts))
            {
                await Application.Current.MainPage.DisplayAlert("Warning", "Which Duct shouldn't be empty", "OK");
                return;
            }

            // put to key map before convert to json
            var KVPair = allKVPair();


            try
            {

                // JSON convert and send to AWS 
                var result = await CloudDBService.PostDuctTrace(KVPair);

                Console.WriteLine(result);

                if (result.Length > 30)
                {

                        var contentResponse = JsonConvert.DeserializeObject<ResponseKeyList>(result);
                        Console.WriteLine();
                        if (SelectedCable1 != null)
                        {
                            SelectedCable1.FiberSegmentIdx = contentResponse?.key1 is null ? "0" : contentResponse.key1;
                            Session.Cable1 = SelectedCable1;
                            Console.WriteLine(SelectedCable1.FiberSegmentIdx);
                        }
                        if (SelectedCable2 != null)
                        {
                            SelectedCable2.FiberSegmentIdx = contentResponse?.key2 is null ? "0" : contentResponse.key2;
                        Session.Cable2 = SelectedCable2;
                        Console.WriteLine(SelectedCable2.FiberSegmentIdx);
                        }
                        if (SelectedCable3 != null)
                        {
                            SelectedCable3.FiberSegmentIdx = contentResponse?.key3 is null ? "0" : contentResponse.key3;
                        Session.Cable3= SelectedCable3;
                        Console.WriteLine(SelectedCable3.FiberSegmentIdx);
                        }
                        if (SelectedCable4 != null)
                        {
                            SelectedCable4.FiberSegmentIdx = contentResponse?.key4 is null ? "0" : contentResponse.key4;
                        Session.Cable4 = SelectedCable4;
                        Console.WriteLine(SelectedCable4.FiberSegmentIdx);
                        }

                        Session.GpsPointMaxIdx = contentResponse?.locatepointkey is null ? "0" : contentResponse.locatepointkey;

                    Insert2SQLite();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }



            await Application.Current.MainPage.Navigation.PushAsync(new LocatePointPage());
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
