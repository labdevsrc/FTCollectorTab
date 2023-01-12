using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using System.Web;
using FTCollectorApp.Model;
using FTCollectorApp.Service;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Rg.Plugins.Popup.Services;
using FTCollectorApp.View;

namespace FTCollectorApp.ViewModel
{
    public partial class RacksViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;
        [ObservableProperty]
        RackNumber selectedRackKey;

        public RacksViewModel()
        {

            SaveandBackCommand = new Command(async _ => await ExecuteSaveandBackCommand());
            SaveCommand = new Command(async _ => await ExecuteSaveCommand());
            RefreshRackKeyListCommand = new Command(
                execute: async () =>
                {

                    Console.WriteLine();
                    IsBusy = true;
                    var contentConduit = await CloudDBService.GetRackNumber(); // async download from AWS table
                    if (contentConduit.ToString().Length > 20)
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                        {
                            conn.CreateTable<RackNumber>();
                            conn.DeleteAll<RackNumber>();
                            conn.InsertAll(contentConduit);
                        }

                        Console.WriteLine();
                        OnPropertyChanged(nameof(RackKeyList)); // update Racks dropdown list
                    }
                    IsBusy = false;
                    Console.WriteLine();
                });
            Session.current_page = "racks";
        }

        void InsertRackNumber()
        {

        }


        [ObservableProperty]
        string selectedRackNumber = "1";

        [ObservableProperty]
        bool isVertical = false;

        [ObservableProperty]
        bool isBack = false;


        [ObservableProperty]
        RackType selectedRackType;

        [ObservableProperty]
        string _XPos;

        [ObservableProperty]
        string _YPos;


        public ObservableCollection<RackType> RackTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<RackType>();
                    var table = conn.Table<RackType>().ToList();
                    return new ObservableCollection<RackType>(table);
                }
            }
        }


        public ObservableCollection<RackNumber> RackKeyList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<RackNumber>();

                    var table = conn.Table<RackNumber>().Where(a => (a.SiteId == Session.tag_number) && (a.OWNER_CD == Session.ownerCD)).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<RackNumber>(table);
                }
            }
        }


        // Mr woody : Manufacturer and model not necessary on Rack Page
        /*[ObservableProperty]
        [AlsoNotifyChangeFor(nameof(ModelDetailList))]
        Manufacturer selectedManufacturer;

        [ObservableProperty]
        ModelDetail selectedModelDetail;

        public ObservableCollection<ModelDetail> ModelDetailList
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ModelDetail>();
                    var table = conn.Table<ModelDetail>().ToList();
                    if (SelectedManufacturer?.ManufKey != null)
                        table = conn.Table<ModelDetail>().Where(a => a.ManufKey == SelectedManufacturer.ManufKey).ToList(); 

                    foreach (var col in table)
                    {
                        col.ModelNumber = HttpUtility.HtmlDecode(col.ModelNumber); // should use for escape char 
                        if (col.ModelCode1 == "") // sometimes this model entri is null
                            col.ModelCode1 = col.ModelCode2;
                        if (col.ModelCode2 == "")
                            col.ModelCode2 = col.ModelCode1;
                    }
                    return new ObservableCollection<ModelDetail>(table);
                    //Console.WriteLine();
                    //return _modelDetailList;
                }
            }
        }

        public ObservableCollection<Manufacturer> ManufacturerList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Manufacturer>();
                    var table = conn.Table<Manufacturer>().ToList();
                    foreach (var col in table)
                    {
                        col.ManufName = HttpUtility.HtmlDecode(col.ManufName); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<Manufacturer>(table);
                }
            }
        }*/

        const string AWS_SYNCED = "aws_synced";
        const string AWS_NOTSYNCED = "aws_notsynced";

        List<KeyValuePair<string, string>> keyvaluepair()
        {

            var keyValues = new List<KeyValuePair<string, string>>{

                // Session params
                //new KeyValuePair<string, string>("key", MAX_key),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD),
                new KeyValuePair<string, string>("jobnum",Session.jobnum),
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),
                new KeyValuePair<string, string>("tag",Session.tag_number),
                new KeyValuePair<string, string>("sitekey",Session.site_key),
                new KeyValuePair<string, string>("front_back", IsBack ? "B" : "F" ),
                new KeyValuePair<string, string>("type", SelectedRackType?.RackTypeKey==null ?"0":SelectedRackType.RackTypeKey),
                new KeyValuePair<string, string>("racknumber", SelectedRackNumber ??= "0"),
                new KeyValuePair<string, string>("orientation", IsVertical ?"V":"H"),

                new KeyValuePair<string, string>("xpos", XPos ??= ""),
                new KeyValuePair<string, string>("ypos", YPos ??= ""),
                /*new KeyValuePair<string, string>("manufacturer_key", SelectedManufacturer?.ManufKey==null ?"0": SelectedManufacturer.ManufKey),
                new KeyValuePair<string, string>("manufacturer", SelectedManufacturer?.ManufName == null ?"0": SelectedManufacturer.ManufName),
                new KeyValuePair<string, string>("model_key", SelectedModelDetail?.ModelKey==null ?"0":SelectedModelDetail.ModelKey ),
                new KeyValuePair<string, string>("model", SelectedModelDetail?.ModelDescription==null ?"0":SelectedModelDetail.ModelDescription ),

                new KeyValuePair<string, string>("height", SelectedModelDetail.height ??= "0"),
                new KeyValuePair<string, string>("width", SelectedModelDetail.width ??= "0"),
                new KeyValuePair<string, string>("depth", SelectedModelDetail.depth ??= "0"),*/

            };

            Console.WriteLine();

            return keyValues;

        }

        // This is for SQLite
        List<KeyValuePair<string, string>> keyvaluepair(string MAX_key, string tag)
        {

            var keyValues = new List<KeyValuePair<string, string>>{

                // Session params
                new KeyValuePair<string, string>("key", MAX_key),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD),
                new KeyValuePair<string, string>("jobnum",Session.jobnum),
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("jobkey", Session.jobkey),
                new KeyValuePair<string, string>("tag",Session.tag_number),
                new KeyValuePair<string, string>("sitekey",Session.site_key),
                new KeyValuePair<string, string>("front_back",IsBack? "B" : "F"),
                new KeyValuePair<string, string>("type", SelectedRackType?.RackTypeKey==null ?"0":SelectedRackType.RackTypeKey),
                new KeyValuePair<string, string>("racknumber", SelectedRackNumber ??= "0"),
                new KeyValuePair<string, string>("orientation", IsVertical ?"V":"H"),

                new KeyValuePair<string, string>("xpos", XPos ??= ""),
                new KeyValuePair<string, string>("ypos", YPos ??= ""),
                /*new KeyValuePair<string, string>("manufacturer_key", SelectedManufacturer?.ManufKey==null ?"0": SelectedManufacturer.ManufKey),
                new KeyValuePair<string, string>("manufacturer", SelectedManufacturer?.ManufName == null ?"0": SelectedManufacturer.ManufName), 
                new KeyValuePair<string, string>("model_key", SelectedModelDetail?.ModelKey==null ?"0":SelectedModelDetail.ModelKey ),
                new KeyValuePair<string, string>("model", SelectedModelDetail?.ModelDescription==null ?"0":SelectedModelDetail.ModelDescription ),

                new KeyValuePair<string, string>("height", SelectedModelDetail.height ??= "0"),
                new KeyValuePair<string, string>("width", SelectedModelDetail.width ??= "0"),
                new KeyValuePair<string, string>("depth", SelectedModelDetail.depth ??= "0"),
                new KeyValuePair<string, string>("SYNC_STS", tag ),*/

            };

            Console.WriteLine();

            return keyValues;

        }

        public ICommand SaveandBackCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RefreshRackKeyListCommand { get; set; }
        private async Task ExecuteSaveandBackCommand()
        {
            IsBusy = true;
            await GetRackRailShelfDatas(); // Update  RackRailShelfs property with rack_rail_shelf AWS table
            IsBusy = false;

            await Application.Current.MainPage.Navigation.PopAsync();
        }


        async Task GetRackRailShelfDatas()
        {
            var rackDatas = await CloudDBService.GetRackNumber();  // get from rack_rail_shelf table
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<RackNumber>();
                conn.DeleteAll<RackNumber>(); // becarefull, this will erase all local data
                conn.InsertAll(rackDatas);
            }

        }

        /// <summary>
        /// This function for update SQLite offline
        /// </summary>
        /// <param name="Synced"></param>
        private void InsertToSQLite(bool Synced)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<RackNumber>();
                var tableRack = conn.Table<RackNumber>().ToList();


                try
                {
                    foreach (var col in tableRack)
                    {
                        if (col.RackNumKey != null)
                            col.temp = int.Parse(col.RackNumKey);
                        else
                        {
                            col.RackNumKey = "0";
                            col.temp = 0;
                        }
                    }

                    var maxRackKey = tableRack.Max(x => x.temp);
                    maxRackKey += 1; // increment
                    Console.WriteLine();

                    // create new instance 
                    RackNumber rn = new RackNumber();
                    rn.RackNumKey = maxRackKey.ToString();
                    rn.Racknumber = SelectedRackNumber ??= "0";
                    rn.SiteId = Session.tag_number;
                    rn.SyncStatus = Synced ? RackNumber.AWS_SYNCED : RackNumber.AWS_NOTSYNCED;

                    // Insert to RackNumber Table
                    conn.Insert(rn);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        private async Task ExecuteSaveCommand()
        {
            if (SelectedRackNumber == null)
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Rack Number  must be non-zero", "Warning"));
                return;
            }
            
            /*if (SelectedModelDetail == null)
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Model is empty.\n Please Select One", "Warning"));
                return;
            }*/
            try
            {

                var KVPair = keyvaluepair();

                var result = await CloudDBService.PostSaveRacks(KVPair); 
                if (result.Trim().Equals("1"))
                {
                    Console.WriteLine();

                    var num = int.Parse(SelectedRackNumber) + 1;
                    SelectedRackNumber = num.ToString();

                    IsBusy = true;
                    //await GetRackRailShelfDatas(); // Update  RackRailShelfs property with rack_rail_shelf AWS table
                    InsertToSQLite(true);
                    IsBusy = false;
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Racks Updated Successfully", "Success"));
                    // Update Session's Rack save count 
                    Session.RackCount++;
                }
                else if (result.Trim().Equals("0"))
                {
                    Console.WriteLine();
                    InsertToSQLite(false);
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Racks Updated Fail", "Fail"));
                }

            }
            catch(Exception e)
            {
                InsertToSQLite(false);
                Console.WriteLine(e.ToString());
            }
        }
    }
}
