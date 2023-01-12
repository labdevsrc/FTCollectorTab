using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.View;
using SQLite;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class PortViewModel : ObservableObject
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(BladeList))]
        Chassis selectedChassisKey;

        [ObservableProperty]
        SlotBladeTray selectedBladSlotTray;


        [ObservableProperty]
        string portLabel;

        [ObservableProperty]
        string selectedOrientation;

        //[ObservableProperty]
        string selectedTXRXOption;
        string TXRXOption;
        public string SelectedTXRXOption
        {
            get => selectedTXRXOption;
            set
            {
                SetProperty(ref selectedTXRXOption, value);
                
                if (value == "Transmit")
                    TXRXOption = "T";
                else if (value == "Receive")
                    TXRXOption = "R";
                else if (value == "Full Duplex")
                    TXRXOption = "F";
                else
                    TXRXOption = "U";

            }
        }

        [ObservableProperty]
        string selectedBlade;


        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(ChassisList))]
        RackNumber selectedRackNumber;  // harus class !!, onpropertychanged dependency tidak bisa untuk string
        /*public RackNumber SelectedRackNumber {
            get => selectedRackNumber;
            set
            {
                if (selectedRackNumber != value)
                    return; 
                selectedRackNumber = value;
                Console.WriteLine();
                OnPropertyChanged(nameof(SelectedRackNumber));
            }
        }*/


        [ObservableProperty]
        string selectedPortNumber = "1";

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("tag", Session.tag_number),
                new KeyValuePair<string, string>("chassisid", SelectedChassisKey?.ChassisKey  == null ? "0": SelectedChassisKey.ChassisKey),  // 3
                new KeyValuePair<string, string>("bladeid", SelectedBladSlotTray?.key  == null ? "0": SelectedBladSlotTray.key),  // 
                new KeyValuePair<string, string>("transmit", TXRXOption ??="0"),  // 
                new KeyValuePair<string, string>("portid", SelectedPortNumber ??= "0"),  // 
                new KeyValuePair<string, string>("ptype",  SelectedPortType?.CodeKey == null ? "0": SelectedPortType.CodeKey),  // 5
                new KeyValuePair<string, string>("rack_number",  SelectedRackNumber?.Racknumber == null ? "0": SelectedRackNumber.Racknumber),  // 5
                new KeyValuePair<string, string>("rack_key",  SelectedRackNumber?.RackNumKey == null ? "0": SelectedRackNumber.RackNumKey),  // 5

                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1

                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 

                new KeyValuePair<string, string>("labelid", PortLabel ??= "0"),  // 4


            };


            return keyValues;

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
                    foreach(var col in table)
                    {
                        //if(col?.Model == null)
                        if (string.IsNullOrEmpty(col?.Model))
                        {
                            col.Model = "Unknwon"; // because DisplayBindingItem ={Binding Model}, make sure no Model = null 
                            Console.WriteLine();
                        }
                    }

                    try
                    {

                        if (SelectedRackNumber != null)
                        {
                            table = conn.Table<Chassis>().Where(a => (a.TagNumber == Session.tag_number) && (a.rack_number == SelectedRackNumber.Racknumber)).ToList();
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                    }
                    catch(Exception e) {
                        Console.WriteLine(e.ToString());
                    }
                    return new ObservableCollection<Chassis>(table);
                }
            }
        }

        public ObservableCollection<SlotBladeTray> BladeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<SlotBladeTray>();
                    var table = conn.Table<SlotBladeTray>().Where(a => a.site == Session.tag_number).ToList();
                    foreach (var col in table)
                    {
                        if (string.IsNullOrEmpty(col.key))
                            col.key = "0"; // because DisplayBindingItem ={Binding key}, make sure no Model = null 
                    }
                    Console.WriteLine();
                    try
                    {
                        if (SelectedChassisKey != null)
                        {
                            table = conn.Table<SlotBladeTray>().Where(a => (a.site == Session.tag_number) && (a.rack_key == SelectedRackNumber.RackNumKey) && (a.chassis_key == SelectedChassisKey.ChassisKey)).ToList();
                            Console.WriteLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return new ObservableCollection<SlotBladeTray>(table);
                }
            }
        }
        [ObservableProperty]
        PortType selectedPortType;
        public ObservableCollection<PortType> PortTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<PortType>();
                    var table = conn.Table<PortType>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<PortType>(table);
                }
            }
        }

        public ObservableCollection<Ports> PortKeyList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Ports>();
                    var table = conn.Table<Ports>().Where(a=>a.site == Session.tag_number).ToList();
                    return new ObservableCollection<Ports>(table);
                }
            }
        }


        public ICommand SaveCommand { get; set; }
        public ICommand FinishCommand { get; set; }
        public ICommand RefreshPortsKeyListCommand { get; set; }
        public ICommand RefreshBladeListCommand { get; set; }

        public PortViewModel()
        {
            SaveCommand = new Command(async () => ExecuteSaveCommand());
            FinishCommand = new Command(async () => ExecuteFinishCommand());
            RefreshPortsKeyListCommand = new Command(() => ExecuteRefreshPortsKeyListCommand());
            RefreshBladeListCommand = new Command(() => ExecuteRefreshBladeListCommand());
            Session.current_page = "Ports";
        }
        private async void ExecuteFinishCommand()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        [ObservableProperty]
        bool isBusy;

        private async void ExecuteRefreshPortsKeyListCommand()
        {
            Console.WriteLine();
            IsBusy = true;
            var contentPorts = await CloudDBService.GetPortTable(); // async download from AWS table
            if (contentPorts.ToString().Length > 20)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Ports>();
                    conn.DeleteAll<Ports>();
                    conn.InsertAll(contentPorts);
                }

                Console.WriteLine();
                OnPropertyChanged(nameof(PortKeyList)); // update Ports dropdown list
            }
            IsBusy = false;
            Console.WriteLine();
        }


        private async void ExecuteRefreshBladeListCommand()
        {
            Console.WriteLine();
            IsBusy = true;
            var contentSlotBladeTray = await CloudDBService.GetBladeTableKey(); // async download from AWS table
            if (contentSlotBladeTray.ToString().Length > 20)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<SlotBladeTray>();
                    conn.DeleteAll<SlotBladeTray>();
                    conn.InsertAll(contentSlotBladeTray);
                }

                Console.WriteLine();
                OnPropertyChanged(nameof(BladeList)); // update Ports dropdown list
            }
            IsBusy = false;
            Console.WriteLine();
        }


        private async void ExecuteSaveCommand()
        {
            var KVPair = keyvaluepair();
            var result = await CloudDBService.PostPortsSave(KVPair);


            if (result.Trim().Equals("1"))
            {
                Console.WriteLine();

                var num = int.Parse(SelectedPortNumber) + 1;
                SelectedPortNumber = num.ToString();

                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Port Updated Successfully", "Success"));
                var contentPorts = await CloudDBService.GetPortTable(); // async download from AWS table
                if (contentPorts.ToString().Length > 20)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                    {
                        conn.CreateTable<Ports>();
                        conn.DeleteAll<Ports>();
                        conn.InsertAll(contentPorts);
                    }
                    Console.WriteLine();
                    OnPropertyChanged(nameof(PortKeyList)); // update Ports dropdown list
                }
            }
            else //"0" or fail
            {
                Console.WriteLine();
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert(result, "Fail"));
            }
        }
    }
}
