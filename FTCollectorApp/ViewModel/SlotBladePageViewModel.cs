using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Collections.ObjectModel;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Model;
using System.Web;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.Service;
using FTCollectorApp.View;
using System.Linq;

namespace FTCollectorApp.ViewModel
{
    public partial class SlotBladePageViewModel : ObservableObject
    {
        public ICommand SaveCommand { get; set; }
        public ICommand FinishSaveCommand { get; set; }
        public ICommand ShowPortPageCommand { get; set; }
        public ICommand ShowPortConnPageCommand { get; set; }
        public ICommand RefreshBladeKeyListCommand { get; set; }
        public SlotBladePageViewModel()
        {
            SaveCommand = new Command(async () => ExecuteSaveCommand());
            FinishSaveCommand = new Command(() => ExecuteFinishSaveCommand());
            RefreshBladeKeyListCommand = new Command(() => ExecuteRefreshBladeKeyListCommand());
            ShowPortPageCommand = new Command(async () => ExecuteShowPortPageCommand());
            ShowPortConnPageCommand = new Command(async () => ExecuteShowPortConnPageCommand());

            Session.current_page = "slot";
        }
        
        [ObservableProperty]
        bool isBusy;

        private async void ExecuteRefreshBladeKeyListCommand()
        {
            Console.WriteLine();
            IsBusy = true;
            var contentSBT = await CloudDBService.GetBladeTableKey(); // async download from AWS table
            if (contentSBT.ToString().Length > 20)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<SlotBladeTray>();
                    conn.DeleteAll<SlotBladeTray>();
                    conn.InsertAll(contentSBT);
                }

                Console.WriteLine();
                OnPropertyChanged(nameof(SlotBladeTrayTables)); // update CONDUIT_GROUPS dropdown list
            }
            IsBusy = false;
            Console.WriteLine();
        }

        private int InsertToSQLite()
        {
            int InsertedRows = 0;
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<SlotBladeTray>();
                var tableBlade = conn.Table<SlotBladeTray>().ToList();
                try
                {

                    foreach (var col in tableBlade)
                    {
                        if (col.key != null)
                            col.temp = int.Parse(col.key);
                        else
                        {
                            col.key = "0";
                            col.temp = 0;
                        }
                    }

                    Console.WriteLine();
                    var maxBladeKey = tableBlade.Max(x => x.temp);
                    maxBladeKey += 1; // increment

                    SlotBladeTray slotblade = new SlotBladeTray();
                    slotblade.key = maxBladeKey.ToString();
                    slotblade.slot_or_blade_number = SelectedBladeNum ??= "0";
                    slotblade.rack_key = SelectedRackNumber?.Racknumber == null ? "0" : SelectedRackNumber.Racknumber;
                    slotblade.chassis_key = SelectedChassisKey?.ChassisKey == null ? "0" : SelectedChassisKey.ChassisKey;
                    slotblade.model_key = SelectedModelDetail?.ModelKey == null ? "0" : SelectedModelDetail.ModelKey;
                    slotblade.site = Session.tag_number;
                    InsertedRows = conn.Insert(slotblade);

                    Console.WriteLine();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            return InsertedRows;
        }


        private async Task ExecuteSaveCommand()
        {
            if (SelectedModelDetail == null)
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Model is empty.\n Please Select One", "Warning"));
                return;
            }
            var KVPair = keyvaluepair();
            var result = await CloudDBService.PostBladeSave(KVPair);


            if (result.Trim().Equals("1"))
            {
                Console.WriteLine();

                var num = int.Parse(SelectedBladeNum) + 1;
                SelectedBladeNum = num.ToString();

                var InsertedRow = InsertToSQLite();


                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert(string.Format("Blade Updated Successfully\nSQLite Insert {0}", InsertedRow), "Success"));


            }
            else //"0" or fail
            {
                Console.WriteLine();
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert(result, "Fail"));
            }
        }


        private async Task ExecuteFinishSaveCommand()
        {
            var KVPair = keyvaluepair();
            var result = await CloudDBService.PostBladeSave(KVPair);
            //if (result.Equals("OK"))
            {
                Console.WriteLine();
                Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        private async Task ExecuteShowPortPageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PortPage());
        }

        private async Task ExecuteShowPortConnPageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PortConnection());
        }

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(ModelDetailList))]
        Manufacturer selectedManufacturer;

        [ObservableProperty]
        ModelDetail selectedModelDetail;


        [ObservableProperty]
        Chassis selectedChassisKey;

        [ObservableProperty]
        SlotBladeTray selectedBladSlotTray;


        [ObservableProperty]
        string textField;

        [ObservableProperty]
        string selectedOrientation;

        [ObservableProperty]
        string selectedPorts;

        [ObservableProperty]
        string selectedBladeNum = "1";

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(ChassisList))]
        RackNumber selectedRackNumber;  // harus class !!, onpropertychanged dependency tidak bisa untuk string

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("tag", Session.tag_number),  // 2
                new KeyValuePair<string, string>("direction", SelectedBladSlotTray?.orientation  == null ? "0": SelectedBladSlotTray.orientation),  // 3
                new KeyValuePair<string, string>("orientation", SelectedOrientation ??= "0"),  // 4
                new KeyValuePair<string, string>("chassis_key", SelectedChassisKey?.ChassisKey == null ? "0": SelectedChassisKey.ChassisKey),  // 4
                new KeyValuePair<string, string>("slot_or_blade_number", SelectedBladeNum ??= "0"),  // 4
                new KeyValuePair<string, string>("port", SelectedPorts ??= "0"),  // 4
                new KeyValuePair<string, string>("rack_key",  SelectedRackNumber?.RackNumKey == null ? "0": SelectedRackNumber.RackNumKey),  // 5
                new KeyValuePair<string, string>("rack_number",  SelectedRackNumber?.Racknumber == null ? "0": SelectedRackNumber.Racknumber),  // 5
                new KeyValuePair<string, string>("manufacturer_key",  SelectedManufacturer?.ManufKey == null ? "0": selectedManufacturer.ManufKey),  // 5
                new KeyValuePair<string, string>("model_key", SelectedModelDetail?.ModelKey == null ? "0": SelectedModelDetail.ModelKey),  // 6
                new KeyValuePair<string, string>("manufacturer",  SelectedManufacturer?.ManufName == null ? "0": selectedManufacturer.ManufName),  // 7
                new KeyValuePair<string, string>("model", SelectedModelDetail?.ModelDescription == null ? "0": SelectedModelDetail.ModelDescription),  // 8
                new KeyValuePair<string, string>("textField", textField ??= "0"),  // 9
                new KeyValuePair<string, string>("bnumber", SelectedBladeNum ??= "0"),
                new KeyValuePair<string, string>("stage", Session.stage),
                new KeyValuePair<string, string>("chasis_number", SelectedChassisKey?.ChassisKey == null ? "0": SelectedChassisKey.ChassisKey)
            };
            return keyValues;
        }

        public ObservableCollection<ChassisType> ChassisTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ChassisType>();
                    var table = conn.Table<ChassisType>().ToList();
                    return new ObservableCollection<ChassisType>(table);
                }
            }
        }
        public ObservableCollection<SlotBladeTray> SlotBladeTrayTables
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<SlotBladeTray>();
                    var table = conn.Table<SlotBladeTray>().Where(a => a.site == Session.tag_number).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<SlotBladeTray>(table);
                }
            }
        }

        public ObservableCollection<RackNumber> RackRailShelfs
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<RackNumber>();
                    var table = conn.Table<RackNumber>().Where(a => a.SiteId == Session.tag_number).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<RackNumber>(table);
                }
            }
        }

        public ObservableCollection<Chassis> ChassisList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<Chassis>();
                    var table = conn.Table<Chassis>().Where(a => a.TagNumber == Session.tag_number).ToList();
                    try
                    {
                        if (SelectedRackNumber != null)
                            table = conn.Table<Chassis>().Where(a => (a.TagNumber == Session.tag_number) && (a.rack_number == SelectedRackNumber.Racknumber)).ToList();
                        Console.WriteLine();
                        return new ObservableCollection<Chassis>(table);
                    }
                    catch(Exception e)
                    {
                        e.ToString();
                        Console.WriteLine();

                        return new ObservableCollection<Chassis>(table);

                    }

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
        }



    }
}
