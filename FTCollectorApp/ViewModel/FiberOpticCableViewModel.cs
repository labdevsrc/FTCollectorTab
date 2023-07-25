using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using System.Web;
using FTCollectorApp.Model;
using System.Linq;
using System.Windows.Input;
using FTCollectorApp.Services;
using Xamarin.Forms;
using System.Threading.Tasks;
using FTCollectorApp.View;
using FTCollectorApp.View.TraceFiberPages;

namespace FTCollectorApp.ViewModel
{


    public partial class FiberOpticCableViewModel : ObservableObject
    {


        public FiberOpticCableViewModel()
        {
            Session.current_page = "cable";
            Console.WriteLine();
        }



        [ICommand]
        async void Save()
        {
            Console.WriteLine();
            var KVPair = keyvaluepair();
            Console.WriteLine();
            string result = await CloudDBService.PostSaveFiberOpticCable(KVPair);
            if (result.Equals("OK"))
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Uploading Data Done","DONE");

                /*var choice = 
                if (choice)
                {
                    if (Session.stage.Equals("A"))
                        await Application.Current.MainPage.Navigation.PushAsync(new AsBuiltDocMenu());
                    if (Session.stage.Equals("I"))
                        await Application.Current.MainPage.Navigation.PushAsync(new MainMenuInstall());
                }*/
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fail", "Uploading Data Fail. Check internet Connection", "RETRY", "DONE");
            }
        }

        [ICommand]
        async void SaveBack()
        {
            var KVPair = keyvaluepair();
            string result = await CloudDBService.PostSaveFiberOpticCable(KVPair);
            if (result.Equals("OK"))
            {
                //await DisplayAlert("Success", "Uploading Data Done", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        [ObservableProperty] string newCableName;

        //[ObservableProperty]
        //[AlsoNotifyChangeFor(nameof(CableTypeList))]
        //[AlsoNotifyChangeFor(nameof(NewFiber))]


        AFiberCable selectedFiberCable;
        public AFiberCable SelectedFiberCable
        {
            get => selectedFiberCable;
            set
            {
                SetProperty(ref selectedFiberCable, value);
                if (value.CableIdDesc != null) {
                    IsNewCableSelected = value.CableIdDesc.Equals("New");
                    OnPropertyChanged(nameof(CableTypeList));
                    OnPropertyChanged(nameof(IsNewCableSelected));
                }
            }
        }

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(ModelDetailList))]
        Manufacturer selectedManufacturer;

        [ObservableProperty]
        ModelDetail? selectedModelDetail;

        [ObservableProperty]
        CableType? selectedCableType;

        [ObservableProperty]
        string _SMCount;

        [ObservableProperty]
        string _MMCount;

        [ObservableProperty]
        string selectedBufferCnt;

        [ObservableProperty]
        TwoColor? selectedColor;

        [ObservableProperty]
        Sheath selectedSheath;

        [ObservableProperty]
        ReelId? selectedReelId;

        [ObservableProperty]
        DateTime selectedManufacturedDate;


        [ObservableProperty]
        DateTime selectedInstalledAt;

        [ObservableProperty]
        FiberInstallType selectedInstallType;

        [ObservableProperty]
        string textLabel;

        // multi mode diameter color        
        public ObservableCollection<TwoColor> ClrCodeList
        {
            get
            {
                List<TwoColor> temp = new List<TwoColor>();
                temp.Add(new TwoColor { ClrKey = "12", ClrName = "Aqua", ClrHex = "#00FFFF" });
                temp.Add(new TwoColor { ClrKey = "2", ClrName = "Orange", ClrHex = "#FFA500" });
                return new ObservableCollection<TwoColor>(temp);
            }
        }


        public ObservableCollection<ReelId> ReelIdList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ReelId>();
                    var rwTable = conn.Table<ReelId>().ToList();
                    var table = rwTable.Where(a => a.JobNum == Session.jobnum).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<ReelId>(table);
                }
            }
        }

        [ObservableProperty] bool isNewCableSelected;

        public ObservableCollection<AFiberCable> aFiberCableList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    // create dummy list 
                    List<AFiberCable> temp = new List<AFiberCable>();
                    temp.Add(new AFiberCable
                    {
                        CableIdDesc = "New"
                    });

                    conn.CreateTable<AFiberCable>();
                    var table = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey && a.JobNumber == Session.jobnum).ToList();
                    temp.AddRange(table);
                    foreach (var col in table)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<AFiberCable>(temp);
                }
            }
        }

        ObservableCollection<ModelDetail> _modelDetailList;
        public ObservableCollection<ModelDetail> ModelDetailList
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ModelDetail>();
                    var table = conn.Table<ModelDetail>().ToList();
                    Console.WriteLine();
                    if (SelectedManufacturer?.ManufKey != null)
                        table = conn.Table<ModelDetail>().Where(a => a.ManufKey == SelectedManufacturer.ManufKey).ToList();


                    try
                    {
                        foreach (var col in table)
                        {
                            col.ModelNumber = HttpUtility.HtmlDecode(col.ModelNumber); // should use for escape char 
                            if (col.ModelCode1 == "") // sometimes this model entri is null
                                col.ModelCode1 = col.ModelCode2;
                            if (col.ModelCode2 == "")
                                col.ModelCode2 = col.ModelCode1;
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
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

        //[ObservableProperty] CableType selectedCableType;
        public ObservableCollection<CableType> CableTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CableType>();
                    var table = conn.Table<CableType>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<CableType>(table);
                }
            }
        }
        /*public ObservableCollection<CableType> CableTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CableType>();
                    var table = conn.Table<CableType>().ToList();
                    Console.WriteLine();
                    try
                    {
                        if (SelectedFiberCable != null)
                            table = conn.Table<CableType>().Where(a => a.CodeCableKey == SelectedFiberCable.CableType).ToList();
                        return new ObservableCollection<CableType>(table);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine();
                        return new ObservableCollection<CableType>();
                    }
                }
            }
        }*/

        public ObservableCollection<Sheath> SheathList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Sheath>();
                    var table = conn.Table<Sheath>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<Sheath>(table);
                }
            }
        }

        public ObservableCollection<FiberInstallType> InstallTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<FiberInstallType>();
                    var table = conn.Table<FiberInstallType>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<FiberInstallType>(table);
                }
            }
        }

        public ICommand ResultCommand { get; set; }
        //public ICommand SaveCommand { get; set; }
        //public ICommand SaveBackCommand { get; set; }

        List<KeyValuePair<string, string>> keyvaluepair()
        {

            string CableName = string.Empty;


            //if (string.IsNullOrWhiteSpace(NewCableName) ? SelectedFiberCable.CableIdDesc : NewCableName;

            // duct color convert
            string color = string.Empty;
            if (SelectedColor?.ClrKey is null)
                color = "0";
            else if (SelectedColor.ClrKey.Equals("Aqua"))
                color = "62.5";
            else if (SelectedColor.ClrKey.Equals("Orange"))
                color = "62.5";

            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("cable_id", NewCableName ), // this is should be 
                new KeyValuePair<string, string>("manufacturer", SelectedManufacturer?.ManufKey is null ? "0" : SelectedManufacturer.ManufKey),
                new KeyValuePair<string, string>("model", SelectedModelDetail?.ModelKey is null ? "0" : SelectedModelDetail.ModelKey ),
                new KeyValuePair<string, string>("manufactured_date", SelectedManufacturedDate.ToString("yyyy-MM-dd")),
                new KeyValuePair<string, string>("label", textLabel ??= "NA"),
                new KeyValuePair<string, string>("cablelen", "0"),



                new KeyValuePair<string, string>("singlemode_count", SMCount ??= "0" ), //3
                new KeyValuePair<string, string>("multimode_count", MMCount ??= "0"),  //4
                new KeyValuePair<string, string>("buffer_count", SelectedBufferCnt ??= "0" ), //1
                //new KeyValuePair<string, string>("reel", SelectedReelId.ReelKey ??= ""), // 6
                new KeyValuePair<string, string>("reel", SelectedReelId?.ReelKey is null ? "0" : SelectedReelId.ReelKey ), //8
                new KeyValuePair<string, string>("installed_date", SelectedInstalledAt.ToString("yyyy-MM-dd")), //  7 

                new KeyValuePair<string, string>("cabtype", SelectedCableType?.CodeCableKey is null ? "0" : SelectedCableType.CodeCableKey ), //8
                new KeyValuePair<string, string>("installtyp", SelectedInstallType?.FbrInstallKey is null ? "" : SelectedInstallType.FbrInstallKey),  /// site_id
                new KeyValuePair<string, string>("sheath", SelectedSheath?.SheathKey is null ? "0" : SelectedSheath.SheathKey ),  /// code_site_type.key
                new KeyValuePair<string, string>("multimode_diameter", color),

                new KeyValuePair<string, string>("oname", Session.OwnerName), //1
                new KeyValuePair<string, string>("owner", Session.ownerkey), //1
                new KeyValuePair<string, string>("oid", Session.ownerkey), //1
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // #1
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 6
                new KeyValuePair<string, string>("jobkey", Session.jobkey),
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),  // 2
                new KeyValuePair<string, string>("jobnum", Session.jobnum), //  7 
                new KeyValuePair<string, string>("stage", Session.stage),
                new KeyValuePair<string, string>("country", "USA"),
                new KeyValuePair<string, string>("cablelen", "1"),
                new KeyValuePair<string, string>("geo_length", "1"),
                new KeyValuePair<string, string>("asite", "0"),
                new KeyValuePair<string, string>("zsite", "0"),
            };
            return keyValues;
        }
    }
}
