using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Services;
using FTCollectorApp.View;
using Newtonsoft.Json;
using SQLite;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class PortConnectionViewModel : ObservableObject
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(FromChassisList))]
        RackNumber selectedFromRack;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(ToChassisList))]
        RackNumber selectedToRack;


        
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(FromBladeList))]
        Chassis selectedFromChassis;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(ToBladeList))]
        Chassis selectedToChassis;

        [ObservableProperty]
        SlotBladeTray selectedFromBlade;

        [ObservableProperty]
        SlotBladeTray selectedToBlade;

        [ObservableProperty]
        Ports selectedFromPort;

        [ObservableProperty]
        Ports selectedToPort;

        [ObservableProperty]
        string selectedPortConnection;

        [ObservableProperty]
        string selectedJumperLen;

        public ICommand ConnectCommand { get; set; }
        public ICommand PrintLabelCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public PortConnectionViewModel()
        {

            ConnectCommand = new Command(() => ExecuteConnectCommand());
            PrintLabelCommand = new Command(() => ExecutePrintLabelCommand());
            BackCommand = new Command(() => ExecuteBackCommand());
            SendDialogResultCommand = new Command(result => ExecuteSendDialogResultCommand(result as BasicAllertResult));
            Console.WriteLine();
            Session.current_page = "PortConn";
        }

        private async void ExecuteBackCommand()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        private void ExecutePrintLabelCommand()
        {
            Console.WriteLine();
        }

        private async void ExecuteConnectCommand()
        {
            var KVPair = keyvaluepair(false); //check first existed
            var result = await CloudDBService.PostSavePortConnection(KVPair);
            if (result.Trim().Equals("1"))
            {
                Console.WriteLine();
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Port Connection Updated Successfully","Success"));
            }
            else if (result.Trim().Equals("0"))
            {
                Console.WriteLine();
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Data Insert Fail", "Fail"));
            }
            else if (result.Trim().Equals("3"))
            {
                Console.WriteLine();
                var allerdiag = new BasicAllert("Port Connection already Existed. Update ?", "Warning")
                {
                    GetDialogResultCommand = SendDialogResultCommand
                };
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(allerdiag);

            }
        }

        public ICommand SendDialogResultCommand { get; set; }
        
        [ObservableProperty]
        BasicAllertResult dialogResult;

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

                    if (contentResponse.sts.Equals("1")) // update done
                    {
                        //var itemUpdate = await UpdateSQLite();
                        Console.WriteLine();
                        //var num = int.Parse(SelectedActDevNumber) + 1;
                        //SelectedActDevNumber = num.ToString();
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Update Succesfully.", "Success"));
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Cancel Update Existed");
            }
        }

        List<KeyValuePair<string, string>> keyvaluepair(bool update)
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("tag", Session.tag_number),
                new KeyValuePair<string, string>("torack", SelectedToRack?.RackNumKey  == null ? "0": SelectedToRack.RackNumKey),  
                new KeyValuePair<string, string>("fromrack", SelectedFromRack?.RackNumKey  == null ? "0": SelectedFromRack.RackNumKey),  // 
                new KeyValuePair<string, string>("fromchassis", SelectedFromChassis?.ChassisKey == null ? "0": SelectedFromChassis.ChassisKey),  // 
                new KeyValuePair<string, string>("tochassis", SelectedToChassis?.ChassisKey == null ? "0" : SelectedToChassis.ChassisKey),  // 
                new KeyValuePair<string, string>("fromblade",  SelectedFromBlade?.key == null ? "0": SelectedFromBlade.key),  // 5
                new KeyValuePair<string, string>("toblade",  SelectedToBlade?.key == null ? "0": SelectedToBlade.key),  // 5
                new KeyValuePair<string, string>("fromport",  SelectedFromPort?.PortKey == null ? "0": SelectedFromPort.PortKey),  // 5
                new KeyValuePair<string, string>("toport",  SelectedToPort?.PortKey == null ? "0": SelectedToPort.PortKey),  // 5
                new KeyValuePair<string, string>("jumperlen", SelectedJumperLen ??= "0"),
                new KeyValuePair<string, string>("stage",  Session.stage),  // 5
                new KeyValuePair<string, string>("jobkey",  Session.jobkey),  // 5
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD),
                new KeyValuePair<string, string>("actsts", update ? "UPDATE" : "CHECK"),
            };


            return keyValues;

        }

        public ObservableCollection<RackNumber> FromRackRailShelfs
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

        public ObservableCollection<RackNumber> ToRackRailShelfs
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

        public ObservableCollection<Chassis> ToChassisList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<Chassis>();
                    var table = conn.Table<Chassis>().Where(a => a.TagNumber == Session.tag_number).ToList();
                    foreach (var col in table)
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
                        if (SelectedToRack != null)
                        {
                            table = conn.Table<Chassis>().Where(a => (a.TagNumber == Session.tag_number) && (a.rack_number == SelectedToRack.Racknumber)).ToList();
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return new ObservableCollection<Chassis>(table);
                }

            }
        }

        public ObservableCollection<Chassis> FromChassisList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    Console.WriteLine();
                    conn.CreateTable<Chassis>();
                    var table = conn.Table<Chassis>().Where(a => a.TagNumber == Session.tag_number).ToList();
                    foreach (var col in table)
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

                        if (SelectedFromRack != null)
                        {
                            table = conn.Table<Chassis>().Where(a => (a.TagNumber == Session.tag_number) && (a.rack_number == SelectedFromRack.Racknumber)).ToList();
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return new ObservableCollection<Chassis>(table);
                }

            }
        }

        public ObservableCollection<SlotBladeTray> ToBladeList
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
                         if (SelectedToChassis != null)
                        {
                            table = conn.Table<SlotBladeTray>().Where(a => (a.site == Session.tag_number) && (a.rack_key == SelectedToRack.RackNumKey) && (a.chassis_key == SelectedToChassis.ChassisKey)).ToList();
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

        public ObservableCollection<SlotBladeTray> FromBladeList
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
                        if (SelectedFromChassis != null)
                        {
                            table = conn.Table<SlotBladeTray>().Where(a => (a.site == Session.tag_number) && (a.rack_key == SelectedFromRack.RackNumKey) && (a.chassis_key == SelectedFromChassis.ChassisKey)).ToList();
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


        public ObservableCollection<Ports> FromPortKeyList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Ports>();
                    var table = conn.Table<Ports>().Where(a => a.site == Session.tag_number).ToList();
                    foreach (var col in table)
                    {
                        if (string.IsNullOrEmpty(col.PortKey))
                            col.PortKey = "0"; // because DisplayBindingItem ={Binding key}, make sure no Model = null 
                    }
                    Console.WriteLine();
                    try
                    {
                        if (SelectedFromPort != null)
                        {
                            table = conn.Table<Ports>().Where(a => (a.site == Session.tag_number) 
                            && (a.rack_key == SelectedFromRack.RackNumKey) 
                            && (a.chassis_key == SelectedFromChassis.ChassisKey) 
                            && (a.slot_or_blade_number == SelectedFromBlade.slot_or_blade_number)
                            ).ToList();
                            Console.WriteLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return new ObservableCollection<Ports>(table);
                }
            }
        }


        public ObservableCollection<Ports> ToPortKeyList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Ports>();
                    var table = conn.Table<Ports>().Where(a => a.site == Session.tag_number).ToList();
                    foreach (var col in table)
                    {
                        if (string.IsNullOrEmpty(col.PortKey))
                            col.PortKey = "0"; // because DisplayBindingItem ={Binding key}, make sure no Model = null 
                    }
                    Console.WriteLine();
                    try
                    {
                        if (SelectedToChassis != null)
                        {
                            table = conn.Table<Ports>().Where(a => (a.site == Session.tag_number)
                            && (a.rack_key == SelectedToRack.RackNumKey)
                            && (a.chassis_key == SelectedToChassis.ChassisKey)
                            && (a.slot_or_blade_number == SelectedToBlade.slot_or_blade_number)).ToList();
                            Console.WriteLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return new ObservableCollection<Ports>(table);
                }
            }
        }
    }
}
