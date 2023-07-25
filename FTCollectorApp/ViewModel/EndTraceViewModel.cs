using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SQLite;
using System.Web;
using FTCollectorApp.Model;
using Xamarin.Forms;
using System.Windows.Input;
using FTCollectorApp.Services;
using Newtonsoft.Json;
using FTCollectorApp.View;
using FTCollectorApp.Model.AWS;

namespace FTCollectorApp.ViewModel
{
    public partial class EndTraceViewModel: ObservableObject
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(DuctConduitDatas))]
        ConduitsGroup selectedTagNum;

        string[] colorFiberHex = { "#0000FF", "#FFA500", "#008000", "#A52A2A", "#708090", "#FFFFFF", "#FF0000","#00000", "#FFFF00", "#963D7F", "#FF00FF", "#00FFFF" };
        string[] colorFiber = { "Blue", "Orange", "Green", "Brown", "Slate", "White", "Red", "Black", "Yellow", "Violet", "Rose", "Aqua" };

        [ObservableProperty]
        bool isEntriesDiplayed = true;

        string sheathIn;
        public string SheathIn
        {
            get => sheathIn;
            set
            {

                SetProperty(ref sheathIn, value);
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    
                    conn.CreateTable<a_fiber_segment>();
                    conn.Update(new a_fiber_segment
                    {
                        id = SelectedFromSelCable.id,
                        AWSid2 = SelectedFromSelCable.AWSid2,
                        from_site = SelectedFromSelCable.from_site,
                        sheath_in = value
                    });
                    OnPropertyChanged(nameof(SelectedCableListView));
                    OnPropertyChanged(nameof(SelectedCableList));
                }
            }
        }
        // Auto Complete for Beginning Tag - Start
        // flag for Hide or show listview 
        [ObservableProperty]
        bool isSearching1 = false;

        [ObservableProperty]
        a_fiber_segment selectedFromSelCable;

        a_fiber_segment selectedBeginSite;
        public a_fiber_segment SelectedBeginSite
        {
            get=>selectedBeginSite;
            
            set 
            {
                Console.WriteLine();
                SetProperty(ref (selectedBeginSite), value);
                SearchFromSite = value.from_site;
                OnPropertyChanged(nameof(SearchFromSite));
            }

        }


        // search bar object
        string searchFromSite;
        public string SearchFromSite
        {
            get => searchFromSite;
            set
            {
                IsSearching1 = string.IsNullOrEmpty(value) ? false:true;
                IsEntriesDiplayed = true;
                SetProperty(ref (searchFromSite), value);

                OnPropertyChanged(nameof(SelectedCableListView));
                Console.WriteLine(  );
            }
        }

        // Auto Complete for Beginning Tag - End


        // Auto Complete for End Site Tag - Start
        [ObservableProperty]
        bool isSearching2 = false;
        // selected tag num in listview
        ConduitsGroup selectedSiteIn;
        public ConduitsGroup SelectedSiteIn
        {
            get
            {
                Console.WriteLine();

                return selectedSiteIn;
            }
            set
            {
                Console.WriteLine();
                SetProperty(ref (selectedSiteIn), value);
                SearchTag = value.HosTagNumber;
                OnPropertyChanged(nameof(DuctConduitDatas));
                OnPropertyChanged(nameof(SearchTag));
            }

        }
        // search bar object
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
                IsSearching2 = string.IsNullOrEmpty(value) ? false : true;
                IsEntriesDiplayed = true;
                SetProperty(ref (searchTag), value);

                OnPropertyChanged(nameof(EndSiteListView));
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
        // end of site - end
        // Auto Complete for End Site Tag - End


        [ObservableProperty]
        ConduitsGroup toDuct;

        ObservableCollection<ConduitsGroup> ConduitsGroupListTable;
        ObservableCollection<ColorCode> ColorHextList;

        ObservableCollection<a_fiber_segment> SQLite_a_fiber_segment;

        //public ICommand CompleteFiberCommand { get; set; }
        //public ICommand SuspendCommand { get; set; }
        public ICommand BrokenTraceWireCommand { get; set; }
        public ICommand DeleteTraceCommand { get; set; }
        public ICommand CreateNewCommand { get; set; }

        


        public EndTraceViewModel()
        {



            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {


                conn.CreateTable<ConduitsGroup>();
                var table1 = conn.Table<ConduitsGroup>().Where(b => b.OwnerKey  == Session.ownerkey).ToList();
                ConduitsGroupListTable = new ObservableCollection<ConduitsGroup>(table1);

                conn.CreateTable<ColorCode>();
                var table2 = conn.Table<ColorCode>().ToList();
                ColorHextList = new ObservableCollection<ColorCode>(table2);



            }

            //CompleteFiberCommand = new Command(ExecuteCompleteFiberCommand);
            //SuspendCommand = new Command(ExecuteSuspendCommand);
            BrokenTraceWireCommand = new Command(ExecuteBrokenTraceWireCommand);
            DeleteTraceCommand = new Command(ExecuteDeleteTraceCommand);
            CreateNewCommand = new Command(ExecuteCreateNewCommand);

            Session.current_page = "duct";
        }

        void ExecuteCreateNewCommand()
        {
            IsEntriesDiplayed = false;
            IsSearching2 = false;

            OnPropertyChanged(nameof(SelectedSiteIn));
            Console.WriteLine();
        }


        [ObservableProperty]
        bool isViewing = false;
        [ICommand]
        async void DisplayTraceCables()
        {
            IsViewing = !IsViewing;
        }
        ///list of SQLite storage

        public ObservableCollection<a_fiber_segment> SelectedCableList
        {
            get{
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    //var table = SQLite_a_fiber_segment.Where(a=> a.from_site == BeginningSite).To
                    conn.CreateTable<a_fiber_segment>();
                    var table = conn.Table<a_fiber_segment>().Where(b => b.owner_key == Session.ownerkey).ToList();
                    //var table4 = table3.Where(a => a.from_site == SearchBeginSite);

                    if (SearchFromSite != null)
                    {

                        table = conn.Table<a_fiber_segment>().Where(b => b.owner_key == Session.ownerkey).Where(i => i.from_site.ToLower().Contains(SearchFromSite.ToLower())).
                            GroupBy(b => b.from_site).Select(g => g.First()).ToList();
                    }
                    return new ObservableCollection<a_fiber_segment>(table);
                }
            }
       }

        public ObservableCollection<a_fiber_segment> SelectedCableListView
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    //var table = SQLite_a_fiber_segment.Where(a=> a.from_site == BeginningSite).To
                    conn.CreateTable<a_fiber_segment>();
                    var table = conn.Table<a_fiber_segment>().Where(b => b.owner_key == Session.ownerkey).ToList();
                    //var table4 = table3.Where(a => a.from_site == SearchBeginSite);

                    if (SearchFromSite != null)
                    {

                        table = conn.Table<a_fiber_segment>().Where(b => b.owner_key == Session.ownerkey).Where(i => i.from_site.ToLower().Contains(SearchFromSite.ToLower())).
                            GroupBy(b => b.from_site).Select(g => g.First()).ToList();
                    }
                    return new ObservableCollection<a_fiber_segment>(table);
                }
            }
        }

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            //Session.GpsPointMaxIdx = (int.Parse(maxGPSpoint?.MaxId) + 1).ToString();
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("jobnum", Session.jobnum),
                new KeyValuePair<string, string>("ownerkey", Session.ownerkey),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD),

                //new KeyValuePair<string, string>("locate_point_number", LocPointNumber.ToString()),
                new KeyValuePair<string, string>("locpoint_numstart", Session.LocpointnumberStart is null ? "0" : Session.LocpointnumberStart ),
                new KeyValuePair<string, string>("locpoint_numend", Session.LocpointnumberEnd is null ? "0" : Session.LocpointnumberEnd ),
                new KeyValuePair<string, string>("tag_to", Session.ToDuct?.HosTagNumber is null ? "0" :Session.ToDuct.HosTagNumber ),
                new KeyValuePair<string, string>("tag_to_key", Session.ToDuct?.HostSiteKey is null ? "0" :Session.ToDuct.HostSiteKey ),
                new KeyValuePair<string, string>("duct_to", Session.ToDuct?.ConduitKey is null ? "0" :Session.ToDuct.ConduitKey ),

                new KeyValuePair<string, string>("from_site", Session.FromDuct?.HosTagNumber is null ? "0" :Session.FromDuct.HosTagNumber ),
                new KeyValuePair<string, string>("from_site_key", Session.FromDuct?.HostSiteKey is null ? "0" :Session.FromDuct.HostSiteKey ),

            };
            return keyValues;
        }

        [ICommand]
        async void Suspend()
        {
            Application.Current.Properties[Constants.SavedFromDuctTagNumber] = Session.FromDuct?.HosTagNumber;
            Application.Current.Properties[Constants.SavedFromDuctTagNumberKey] = Session.FromDuct?.ConduitKey;
            //Application.Current.Properties[Constants.SavedToDuctTagNumber] = Session.ToDuct?.HosTagNumber;
            //Application.Current.Properties[Constants.SavedToDuctTagNumberKey] = Session.ToDuct?.ConduitKey;
            await Application.Current.MainPage.Navigation.PushAsync(new AsBuiltDocMenu());
            // ToDo
        }


        async void ExecuteBrokenTraceWireCommand()
        { 
            // ToDo
        }


        async void ExecuteDeleteTraceCommand()
        {
            // ToDo
            var KVPair = keyvaluepair();
            try
            {
                // JSON convert and send to AWS 
                var result = await CloudDBService.PostDuctTrace(KVPair);

                Console.WriteLine(result);

                if (result.Length > 30)
                {

                    var contentResponse = JsonConvert.DeserializeObject<ResponseKeyList>(result);
                    Console.WriteLine();

                    Session.GpsPointMaxIdx = contentResponse?.locatepointkey is null ? "0" : contentResponse.locatepointkey;



                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        [ICommand]
        async void CompleteFiber()
        {
            // put to key map before convert to json
            var KVPair = keyvaluepair();


            try
            {
                // JSON convert and send to AWS 
                var result = await CloudDBService.PostEndDuctTrace(KVPair);

                Console.WriteLine(result);

                if (result.Length > 30)
                {

                    var contentResponse = JsonConvert.DeserializeObject<ResponseKeyList>(result);
                    Console.WriteLine();

                    Session.GpsPointMaxIdx = contentResponse?.locatepointkey is null ? "0" : contentResponse.locatepointkey;

                    Application.Current.Properties[Constants.SavedFromDuctTagNumber] = "";
                    Application.Current.Properties[Constants.SavedFromDuctTagNumberKey] = "";
                    Application.Current.Properties[Constants.SavedToDuctTagNumber] = "";
                    Application.Current.Properties[Constants.SavedToDuctTagNumberKey] = "";

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public ObservableCollection<ConduitsGroup> DuctConduitDatas
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    var table = ConduitsGroupListTable.ToList();
                    if (SelectedSiteIn?.HosTagNumber != null)
                        table = ConduitsGroupListTable.Where(b => b.HosTagNumber == SelectedSiteIn.HosTagNumber).ToList();

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

        public ObservableCollection<ConduitsGroup> EndSiteListView
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    // create dummy list 
                    List<ConduitsGroup> temp = new List<ConduitsGroup>();
                    temp.Add(new ConduitsGroup
                    {
                        HosTagNumber = "New"
                    });

                    var table = ConduitsGroupListTable.GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                    if (SearchTag != null)
                    {
                        Console.WriteLine();
                        table = ConduitsGroupListTable.Where(i => i.HosTagNumber.ToLower().Contains(SearchTag.ToLower())).
                            GroupBy(b => b.HosTagNumber).Select(g => g.First()).ToList();
                    }
                    temp.AddRange(table);
                    Console.WriteLine();
                    return new ObservableCollection<ConduitsGroup>(temp);
                }
            }
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
