using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms;
using System.Threading.Tasks;
using FTCollectorApp.View.SitesPage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Services;
using Newtonsoft.Json;
using FTCollectorApp.View;
using FTCollectorApp.Model.SiteSession;
using System.Linq;

namespace FTCollectorApp.ViewModel
{
    public partial class DuctViewModel : ObservableObject
    {
        [ObservableProperty]
        string defaultHostTagNumber;

        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        bool isDuctLists;
        [ObservableProperty] int selectedDuctSessionIdx = 0;


        DuctSession selectedDuctSession;
        public DuctSession SelectedDuctSession
        {
            get => selectedDuctSession;
            set
            {
                SetProperty(ref selectedDuctSession, value);
                if(DuctSessions?.Count > 0)
                {
                    try
                    {
                        var newDuctColor = new ColorCode { ColorName = value.ColorName, ColorHex = value.ColorHex };
                        SelectedColor = newDuctColor;

                        var newDuctSize = new DuctSize { DuctKey = value.DuctSizeKey };
                        SelectedDuctSize = newDuctSize;
                        SelectedDuctSizeItemIdx = value.DuctSizeItemIdx;
                        Console.WriteLine($" SelectedDuctSizeItemIdx : {SelectedDuctSizeItemIdx}");

                        var newDuctMaterial = new DuctType { DucTypeKey = value.DuctTypeKey };                        
                        SelectedDuctType = newDuctMaterial;
                        SelectedMaterialIdx = value.DuctMaterialIdx;

                        //var newDirection = new CompassDirection { CompasKey = value.CompasKey, CompassDirDesc = value.DirDesc };
                        //SelectedDirection = newDirection;
                        SelectedDirItemIdx = value.DirectionItemIdx;
                        Console.WriteLine($" SelectedDirItemIdx : {SelectedDirItemIdx}");

                        DirectionCounter = value.SessionDirCounter;

                        SelectedDuctInstallTypeIdx = value.DuctInstallIdx;

                        IsPlugged = value.IsDuctPlug;
                        IsOpen = value.IsOpen;
                        HasTraceWire = value.HasTraceWire;
                        PullTapeIdx = value.PullTapeKey;

                        Console.WriteLine();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                Console.WriteLine();
                
            }
        }

        public ObservableCollection<DuctSession> DuctSessionList {
            get
            {
                Console.WriteLine();

                if (DuctSessions != null)
                {
                    Console.WriteLine();
                    return new ObservableCollection<DuctSession>(DuctSessions);
                }
                return new ObservableCollection<DuctSession>();


            }
        }

        List<DuctSession> DuctSessions = new List<DuctSession>();


        public DuctViewModel()
        {

            // from Duct Color Selection PopUp

            ColorSelectedCommand = new Command(ductcolor => ExecuteColorSelectedCommand(ductcolor as ColorCode));
            // from DuctPage
            ShowPopupCommand = new Command(async _ => await ExecuteShowPopupCommand());
            SaveCommand = new Command(
                execute: async () =>
                {
                    //Check direction_count
                    //if(int.Parse(DirectionCounter) > Session.MAX_DIR_CNT)
                    if(DirectionCounter > Session.MAX_DIR_CNT)

                    {
                        await Application.Current.MainPage.DisplayAlert("Warning", "Direction Counter Over than 9", "BACK");
                        //await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Direction Count already used", "Warning"));
                        return;
                    }
                    else if(DuctSessions?.Count > 0)
                    {
                        Console.WriteLine($"DuctSessions Size {DuctSessions?.Count}");

                        var testDuct = new DuctSession { SessionDirCounter = DirectionCounter };
                        if (DuctSessions.Contains(testDuct))
                        {
                            Console.WriteLine($"Already contain DuctSession num {DirectionCounter}");
                            DuctSessions.RemoveAt(SelectedDuctSessionIdx);
                            //DuctSessions.Add()
                            //Duc
                        }
                        OnPropertyChanged(nameof(DuctSessionList));
                        //return;

                    }

                    try
                    {
                        var KVPair = keyvaluepair();
                        string result = await CloudDBService.PostDuctSave(KVPair); // async upload to AWS table

                        var contentResponse = JsonConvert.DeserializeObject<ResponseRes>(result);
                        Console.WriteLine(contentResponse);
                        if (contentResponse.sts.Equals("0"))
                        {
                            Console.WriteLine();
                            //var num = int.Parse(DirectionCounter) + 1;
                            //DirectionCounter = num.ToString();

                            Session.DuctSaveCount++;

                            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert("Update Success with key ="+ contentResponse.d.ToString(), "Success"));
                            //ResultPageCommand?.Execute();
                            DuctSessions.Add(
                                new DuctSession
                                {
                                    HosTagNumber = DefaultHostTagNumber,
                                    ColorKey = SelectedColor?.ColorKey,
                                    ColorHex = SelectedColor?.ColorHex,
                                    ColorName = SelectedColor?.ColorName,
                                    DuctSizeKey = SelectedDuctSize?.DuctKey,
                                    DuctSizeItemIdx = SelectedDuctSizeItemIdx,
                                    CompasKey = SelectedDirection?.CompasKey,
                                    DirectionItemIdx = SelectedDirItemIdx,

                                    DuctTypeKey = SelectedDuctType?.DucTypeKey,
                                    DuctMaterialIdx = SelectedMaterialIdx,
                                    DuctInstallIdx = SelectedDuctInstallTypeIdx,
                                    IsDuctPlug = IsPlugged,
                                    IsOpen = IsOpen,
                                    HasTraceWire = HasTraceWire,
                                    PullTapeKey = PullTapeIdx,
                                    SessionDirCounter = DirectionCounter,

                                    Description =  $"{DirectionCounter} :  {SelectedDuctSize?.DUCTS_SIZE} : {SelectedDuctType?.DucTypeDesc}"
                                }
                                );
                            OnPropertyChanged(nameof(DuctSessionList));
                            DirectionCounter++;


                        }
                        else if (contentResponse.sts.Equals("4"))
                        {
                            Console.WriteLine();

                            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert(contentResponse.d.ToString(), "Fail"));
                        }
                        else if (contentResponse.sts.Equals("5"))
                        {
                            Console.WriteLine();

                            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert(contentResponse.d.ToString(), "Warning"));
                        }
                    }
                    catch(Exception e)
                    {
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new BasicAllert(e.ToString(),"FAIL"));
                    }
                    Console.WriteLine();
                });
            SaveBackCommand = new Command(
                execute: async () =>
                {
                    Console.WriteLine();
                    await Application.Current.MainPage.Navigation.PopAsync();

                });

            RemoveDuctItemCommand = new Command(
                execute: async () =>
                {
                    DuctSessions.RemoveAt(SelectedDuctSessionIdx);

                    var KVPair = keyvaluepair();
                    string result = await CloudDBService.RemoveDuctAt(KVPair); // async upload to AWS table
                    
                    var contentResponse = JsonConvert.DeserializeObject<ResponseRes>(result);
                    Console.WriteLine(contentResponse);
                    if (contentResponse.sts.Equals("0"))
                    {
                        Console.WriteLine();
                        
                    }

                });

            DefaultHostTagNumber = Session.tag_number;


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<DuctSession>();
            }
            Session.current_page = "Duct";

        }

        
        public ICommand ResultPageCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveBackCommand { get; set; }
        public ICommand RefreshDuctKeyListCommand { get; set; }

        public ICommand RemoveDuctItemCommand { get; set; }

        void RefreshCanExecutes()
        {
            (SaveCommand as Command).ChangeCanExecute();
            (SaveBackCommand as Command).ChangeCanExecute();
        }

        /// get selected color from popup - start
        [ObservableProperty]
        ColorCode selectedColor;

        public ICommand ShowPopupCommand { get; set; }
        public ICommand ColorSelectedCommand { get; set; }
        private Task ExecuteShowPopupCommand()
        {
            var popup = new DuctColorCodePopUp(SelectedColor)
            {
                ColorSelectedCommand = ColorSelectedCommand
            };
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }
        // with Mode=TwoWay, no need this ?
        private void ExecuteColorSelectedCommand(ColorCode ductcolor)
        {
            SelectedColor = ductcolor;
            Console.WriteLine();
        }


        [ICommand]
        async void ReturnToMain()
        {
            if (Session.stage.Equals("A"))
                await Application.Current.MainPage.Navigation.PushAsync(new AsBuiltDocMenu());
            if (Session.stage.Equals("I"))
                await Application.Current.MainPage.Navigation.PushAsync(new MainMenuInstall());
        }

        [ICommand]
        async void RecordInner()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RecordInnerDuct());
        }


        public ObservableCollection<DuctType> DuctMaterialList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctType>();
                    var table = conn.Table<DuctType>().ToList();
                    return new ObservableCollection<DuctType>(table);
                }
            }
        }
        public ObservableCollection<DuctUsed> DuctUsageList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctUsed>();
                    var table = conn.Table<DuctUsed>().ToList();
                    return new ObservableCollection<DuctUsed>(table);
                }
            }
        }

        public ObservableCollection<DuctSize> DuctSizeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DuctSize>();
                    var table = conn.Table<DuctSize>().ToList();
                    return new ObservableCollection<DuctSize>(table);
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
                    return new ObservableCollection<DuctInstallType>(table);
                }
            }
        }
        // Duct Page - end

        public ObservableCollection<CompassDirection> TravelDirectionList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CompassDirection>();
                    var data = conn.Table<CompassDirection>().ToList();
                    return new ObservableCollection<CompassDirection>(data);
                }
            }
        }


        [ObservableProperty]
        bool isPlugged = false;

        [ObservableProperty]
        bool isOpen = false;

        [ObservableProperty]
        int pullTapeIdx = 0;

        [ObservableProperty]
        bool hasInnerDuct = false;

        [ObservableProperty]
        bool hasTraceWire = false;

        [ObservableProperty]
        string percentOpen;

        [ObservableProperty]
        DuctSize selectedDuctSize;

        [ObservableProperty]
        DuctUsed selectedDuctUsage;

        [ObservableProperty]
        DuctType selectedDuctType;

        [ObservableProperty]
        DuctInstallType selectedDuctInstallType;

        [ObservableProperty]
        CompassDirection selectedDirection;

        [ObservableProperty] int selectedDuctSizeItemIdx;
        [ObservableProperty] int directionCounter = 1;
        [ObservableProperty] int selectedDirItemIdx;
        [ObservableProperty] int selectedMaterialIdx;
        [ObservableProperty] int selectedDuctInstallTypeIdx;

        List<KeyValuePair<string, string>> keyvaluepair()
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("host_tag_number", Session.tag_number),  // 2
                new KeyValuePair<string, string>("direction", SelectedDirection?.CompasKey  == null ? "0": SelectedDirection.CompasKey),  // 3
                new KeyValuePair<string, string>("direction_count", DirectionCounter.ToString() ),  // 4
                new KeyValuePair<string, string>("duct_size",  SelectedDuctSize?.DuctKey == null ? "0": SelectedDuctSize.DuctKey),  // 5
                new KeyValuePair<string, string>("duct_color", SelectedColor?.ColorKey == null ? "0": SelectedColor.ColorKey),  // 6
                new KeyValuePair<string, string>("duct_type",  SelectedDuctType?.DucTypeKey == null ?"0" : SelectedDuctType.DucTypeKey),  // 7
                new KeyValuePair<string, string>("site_type_key", Session.site_type_key),  // 8
                new KeyValuePair<string, string>("duct_usage", "0"),  // 9
                new KeyValuePair<string, string>("duct_grouptype", "0"),  // 9
                new KeyValuePair<string, string>("duct_groupid", "0"),  // 9
                new KeyValuePair<string, string>("duct_inuse", "1"),  // 9
                new KeyValuePair<string, string>("duct_trace", "0"),  // 9

                new KeyValuePair<string, string>("install", SelectedDuctInstallType?.DuctInstallKey == null ? "0":  SelectedDuctInstallType.DuctInstallKey),  // 10


                new KeyValuePair<string, string>("openpercent", PercentOpen is null ? "" : PercentOpen),  // 11
                new KeyValuePair<string, string>("has_trace_wire", HasTraceWire ?"1" : "0"),  // 12
                new KeyValuePair<string, string>("has_pull_tape", PullTapeIdx .ToString()),  // 12
            };


            return keyValues;

        }


    }
}
