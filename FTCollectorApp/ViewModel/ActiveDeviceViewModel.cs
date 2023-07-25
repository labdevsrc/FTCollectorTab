using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using FTCollectorApp.Model;
using System.Web;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using FTCollectorApp.Services;
using FTCollectorApp.View.SitesPage;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using FTCollectorApp.View;
using System.Threading.Tasks;
using FTCollectorApp.View.Utils;

namespace FTCollectorApp.ViewModel
{
    public partial class ActiveDeviceViewModel : ObservableObject
    {
        [ObservableProperty]
        string selectedPosition = "1";

        [ObservableProperty]
        string defaultHostTagNumber;

        string selectedActDevNumber = "1";
        public string SelectedActDevNumber
        {
            get => selectedActDevNumber;
            set
            {
                SetProperty(ref selectedActDevNumber, value);
                selectedPosition = selectedActDevNumber;
                Console.WriteLine();
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }

        [ObservableProperty]
        string selectedSlotBladeTray;

        [ObservableProperty]
        ChassisType? selectedCT;

        [ObservableProperty]
        RackNumber selectedRackNumber;

        //[ObservableProperty]
        //[AlsoNotifyChangeFor(nameof(ModelDetailList))]
        //Manufacturer selectedManufacturer;

        //[ObservableProperty]
        //ModelDetail selectedModelDetail;

        [ObservableProperty]
        bool isDisplayed = false;

        [ObservableProperty]
        bool isShow = false;

        [ObservableProperty]
        string comment;

        [ObservableProperty]
        string _IP1;

        [ObservableProperty]
        string _IP2;

        [ObservableProperty]
        string _IP3;

        [ObservableProperty]
        string _IP4;
        [ObservableProperty]
        string subnet1;

        [ObservableProperty]
        string subnet2;

        [ObservableProperty]
        string subnet3;

        [ObservableProperty]
        string subnet4;
        [ObservableProperty]
        string _GWIP1;

        [ObservableProperty]
        string _GWIP2;

        [ObservableProperty]
        string _GWIP3;

        [ObservableProperty]
        string _GWIP4;

        [ObservableProperty]
        string _MCast1;
        [ObservableProperty]
        string _MCast2;
        [ObservableProperty]
        string _MCast3;
        [ObservableProperty]
        string _MCast4;

        [ObservableProperty]
        string protocol;
        [ObservableProperty]
        string videoProtocol;
        [ObservableProperty]
        string _VLAN;

        [ObservableProperty]
        string selectedManufDate;
        [ObservableProperty]
        string selectedInstallDate;


        public ObservableCollection<ChassisType> ChassisTypeList
        {
            get
            {
                Console.WriteLine();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ChassisType>();
                    var table = conn.Table<ChassisType>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<ChassisType>(table);
                }
            }
        }

        [ObservableProperty]
        bool isBusy;



        public  ObservableCollection<RackNumber> RackRailShelfs
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<RackNumber>();
                    Console.WriteLine();
                    var table = conn.Table<RackNumber>().Where(a => a.SiteId == Session.tag_number).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<RackNumber>(table);
                }

            }
        }


        //Properties, Bindable object for manufacturer autocomplete dropdown - start
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


        /*public ObservableCollection<ModelDetail> ModelDetailList
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
        }*/


        //Properties, Bindable object for manufacturer dropdown list - end


        public ICommand ToggleWebViewCommand { get; set; }
        public ICommand ToggleIPEntriesCommand { get; set; }
        public ICommand SaveContinueCommand { get; set; }
        public ICommand FinishActiveDeviceCommand { get; set; }
        public ICommand PortPageCommand { get; set; }
        public ICommand PortConnectionCommand { get; set; }
        public ICommand BladePageCommand { get; set; }

        public ICommand UpdateChassisCommand { get; set; }
        public ActiveDeviceViewModel()
        {
            ToggleWebViewCommand = new Command(() => IsDisplayed = !IsDisplayed);
            ToggleIPEntriesCommand = new Command(() => IsShow = !IsShow);
            SaveContinueCommand = new Command(() => ExecuteSaveContinueCommand());
            FinishActiveDeviceCommand = new Command(() => ExecuteFinishActiveDeviceCommand());
            PortPageCommand = new Command(() => ExecutePortPageCommand());
            PortConnectionCommand = new Command(() => ExecutePortConnectionCommand());
            BladePageCommand = new Command(() => ExecuteBladePageCommand());
            UpdateChassisCommand = new Command(() => ExecuteUpdateChassisCommand());
            SendDialogResultCommand = new Command( result => ExecuteSendDialogResultCommand(result as BasicAllertResult));
            Session.RowId = "0"; // reset RowId chassis table
            Session.current_page = "active_device";

            DefaultHostTagNumber = Session.tag_number;


        }


        [ObservableProperty]
        BasicAllertResult result;

        private async void ExecuteUpdateChassisCommand()
        {
            if (Session.RowId == "0")
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Please record an active device first", "Warning"));
                return;
            }
            
            var KVPair = keyvaluepairIpAddr(); // update existed chassis
            var result = await CloudDBService.UpdateIPAddress(KVPair, null);
            if (result.Length > 30)
            {
                var contentResponse = JsonConvert.DeserializeObject<ResponseRes>(result);

                if (contentResponse.sts.Equals("1")) // update done
                {

                    Console.WriteLine();
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Update Succesfully", "Success"));
                }

                Console.WriteLine();
            }
        }

        private void InsertToSQLite()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {

                conn.CreateTable<Chassis>();
                var tableChassis = conn.Table<Chassis>().ToList();
                try
                {

                    foreach (var col in tableChassis)
                    {
                        if (col.ChassisKey != null)
                            col.temp = int.Parse(col.ChassisKey);
                        else
                        {
                            col.ChassisKey = "0";
                            col.temp = 0;
                        }
                    }

                    Console.WriteLine();
                    var maxChassisKey = tableChassis.Max(x => x.temp);
                    maxChassisKey += 1; // increment

                    Chassis chas = new Chassis();
                    chas.ChassisKey = maxChassisKey.ToString();
                    chas.ChassisNum = SelectedActDevNumber ??= "0";
                    chas.rack_number = SelectedRackNumber?.Racknumber == null ? "0" : SelectedRackNumber.Racknumber;
                    chas.TagNumber = Session.tag_number;
                    chas.Model = SelectedModelDetail?.ModelNumber == null ? "0": SelectedModelDetail.ModelNumber;
                    chas.ModelKey = SelectedModelDetail?.ModelKey == null ? "0" : SelectedModelDetail.ModelKey;
                    conn.Insert(chas);

                    Console.WriteLine();

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private async Task<string> UpdateSQLite()
        {
            int DeletedCnt = 0, InsertedCnt = 0;
            IsBusy = true;
            var contentChassis = await CloudDBService.GetChassis();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                conn.CreateTable<Chassis>();
                DeletedCnt = conn.DeleteAll<Chassis>();
                InsertedCnt = conn.InsertAll(contentChassis);
                
            }

            IsBusy = false;
            string returnVal = string.Empty;
            if (DeletedCnt > InsertedCnt || InsertedCnt > DeletedCnt)
                returnVal = "SQLite No Change";
            else
                returnVal = DeletedCnt > InsertedCnt ? String.Format("Removed SQLite {0} Item(s)", DeletedCnt - InsertedCnt) : String.Format("New SQLite {0} Item(s)", InsertedCnt - DeletedCnt);
            return returnVal;
        }

        List<KeyValuePair<string, string>> keyvaluepairIpAddr()
        {


            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("rowid", Session.RowId),

                new KeyValuePair<string, string>("ipaddr", IsIPAddressValid(IP1 + "." + IP2 + "." + IP3 + "." + IP4)),
                new KeyValuePair<string, string>("subnet", IsIPAddressValid(Subnet1 + "." + Subnet2 + "." + Subnet3 + "." + Subnet4)),
                new KeyValuePair<string, string>("protocol", Protocol ??= "0"),
                new KeyValuePair<string, string>("vidioproto", VideoProtocol ??= "0"),
                new KeyValuePair<string, string>("vlan", VLAN ??= "0"),

                new KeyValuePair<string, string>("gateway", IsIPAddressValid(GWIP1 + "." + GWIP2 + "." + GWIP3 + "." + GWIP4)),
                new KeyValuePair<string, string>("multicastip", IsIPAddressValid(MCast1 + "." + MCast2 + "." + MCast3 + "." + MCast4)),

            };

            return keyValues;

        }

        List<KeyValuePair<string, string>> keyvaluepair(bool Update)
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("host_tag_number", Session.tag_number),
                new KeyValuePair<string, string>("stage", Session.stage),
                new KeyValuePair<string, string>("tag", Session.tag_number),
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("manufacturer_key", SelectedManuf?.ManufKey is null ? "0":SelectedManuf.ManufKey),
                new KeyValuePair<string, string>("model_key", SelectedModelDetail?.ModelKey is null ? "0":SelectedModelDetail.ModelKey ),

                new KeyValuePair<string, string>("manufacturer", SelectedManuf?.ManufName is null ? "0":SelectedManuf.ManufName),
                new KeyValuePair<string, string>("model", SelectedModelDetail?.ModelDescription is null ? "0":SelectedModelDetail.ModelDescription ),
                new KeyValuePair<string, string>("actdevnumber", SelectedActDevNumber ??= "0"),
                new KeyValuePair<string, string>("position", selectedPosition ??= "0"),

                new KeyValuePair<string, string>("comment", Comment ??= "NA"),
                new KeyValuePair<string, string>("manufactured_date", SelectedManufDate), //Manufactured),
                new KeyValuePair<string, string>("installed2",SelectedInstallDate), //InstalledAt),


                new KeyValuePair<string, string>("ipaddr", IsIPAddressValid(IP1 + "." + IP2 + "." + IP3 + "." + IP4)),
                new KeyValuePair<string, string>("subnet", IsIPAddressValid(Subnet1 + "." + Subnet2 + "." + Subnet3 + "." + Subnet4)),
                new KeyValuePair<string, string>("protocol", Protocol ??= "0"),
                new KeyValuePair<string, string>("vidioproto", VideoProtocol ??= "0"),
                new KeyValuePair<string, string>("vlan", VLAN ??= "0"),

                new KeyValuePair<string, string>("getway", IsIPAddressValid(GWIP1 + "." + GWIP2 + "." + GWIP3 + "." + GWIP4)),
                new KeyValuePair<string, string>("multicastip", IsIPAddressValid(MCast1 + "." + MCast2 + "." + MCast3 + "." + MCast4)),

                new KeyValuePair<string, string>("slotblade", SelectedSlotBladeTray ??= "0"),
                new KeyValuePair<string, string>("position", SelectedPosition ??= "0"),
                new KeyValuePair<string, string>("rack_number", SelectedRackNumber?.Racknumber is null ? "0" : SelectedRackNumber.Racknumber),
                new KeyValuePair<string, string>("actsts", Update ? "1" : "0"),

            };

            return keyValues;

        }
        string IsIPAddressValid(string ipaddress)
        {
            //  Split string by ".", check that array length is 4
            string[] arrOctets = ipaddress.Split('.');
            if (arrOctets.Length != 4)
                return "0.0.0.0";

            //Check each substring checking that parses to byte
            byte obyte = 0;
            foreach (string strOctet in arrOctets)
                if (!byte.TryParse(strOctet, out obyte))
                    return "0.0.0.0";

            return ipaddress;
        }

        [ObservableProperty]
        BasicAllertResult dialogResult;

        public ICommand SendDialogResultCommand { get; set; }

        private async void ExecuteSendDialogResultCommand(BasicAllertResult resultDialog)
        {
            Console.WriteLine();
            DialogResult = resultDialog;
            if (resultDialog.OK)
            {
                var KVPair = keyvaluepair(true); // update existed chassis
                var result = await CloudDBService.PostActiveDevice(KVPair);
                if (result.Length > 30)
                {
                    var contentResponse = JsonConvert.DeserializeObject<ResponseRes>(result);

                    if (contentResponse.sts.Equals("3")) // update done
                    {
                        var itemUpdate = await UpdateSQLite();
                        Console.WriteLine();
                        var num = int.Parse(SelectedActDevNumber) + 1;
                        SelectedActDevNumber = num.ToString();
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert(String.Format("Update Succesfully. SQLite {0}", itemUpdate), "Success"));
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Cancel Update Existed");
            }
        }

        private async void ExecuteSaveContinueCommand()
        {
            if(SelectedModelDetail == null )
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Model is empty.\n Please Select One", "Warning"));
                return;
            }

            var KVPair = keyvaluepair(false); // insert new chassis

            var result = await CloudDBService.PostActiveDevice(KVPair);

            if (result.Length > 30)
            {
                // Do something
                Console.WriteLine();
                try
                {
                    var contentResponse = JsonConvert.DeserializeObject<ResponseRes>(result);
                    Console.WriteLine(contentResponse);
                    //Console.WriteLine($"status : {0}", contentResponse.sts);
                    //Console.WriteLine($"cnumber : {0}", contentResponse.cnumber);

                    if (contentResponse.sts.Equals("0"))
                    {
                        var allerdiag = new BasicAllert("Chasis already Existed. Update ?", "Warning")
                        {
                            GetDialogResultCommand = SendDialogResultCommand
                        };
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(allerdiag);
                    }
                    else if (contentResponse.sts.Equals("1"))
                    {
                        Session.RowId = contentResponse.cnumber;
                        InsertToSQLite();
                        var num = int.Parse(SelectedActDevNumber) + 1;
                        SelectedActDevNumber = num.ToString();
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Update Succesfully", "Success"));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }




            }
        }

        private async void ExecuteFinishActiveDeviceCommand()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void ExecutePortPageCommand()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new PortPage());
        }

        private async void ExecutePortConnectionCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PortConnection());
        }

        private async void ExecuteBladePageCommand()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SlotBladePage());
        }


        [ICommand]
        async void Capture()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new CameraViewPage());
        }

    }
}
