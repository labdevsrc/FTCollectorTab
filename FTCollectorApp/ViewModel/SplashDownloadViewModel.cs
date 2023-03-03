using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Model.AWS;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.View;
using FTCollectorApp.View.SyncPages;
using SQLite;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FTCollectorApp.ViewModel
{
    public partial class SplashDownloadViewModel : ObservableObject
    {

        public SplashDownloadViewModel()
        {

            DisplayPendingTaskCommand = new Command(async () => DisplayPendingTaskCommandExecute());
            //DownloadTablesCommand = new Command(async () => DownloadTablesCommandExecute());
            DisplayAllertCommand = new Command(async () => DisplayWarning());
            TestCommand = new Command(async () => Test());
            Console.WriteLine();

            CheckEntriesCommand = new Command(ExecuteCheckEntriesCommand);
            //LoginCommand = new Command(() => ExecuteLoginCommand());
            //LogoutCommand = new Command(() => ExecuteLogoutCommand());
            SettingCommand = new Command(() => ExecuteSettingCommand());

            GPSSettingCommand = new Command(() => DisplayGPSSettingCommand());
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                //Type classname = object1.GetType();

                conn.CreateTable<User>();
                Console.WriteLine("CreateTable<User> ");
                var userdetails = conn.Table<User>().ToList();
                //conn.InsertAll(users);

                Users = new ObservableCollection<User>(userdetails);
            }

            DownloadTablesCommand = new Command(
                execute: async () => DownloadTablesCommandExecute(),
                canExecute: () =>
                {
                    return !IsBusy;
                }
            );

            LoginCommand = new Command(
                execute: async () => ExecuteLoginCommand(),
                canExecute: () =>
                {
                    return !IsBusy;
                }
                );

            LogoutCommand = new Command(
                execute: async () => ExecuteLogoutCommand(),
                canExecute: () =>
                {
                    return !IsBusy;
                }
                );

        }


        string version = "20230303C"; // change here for release

        string apkVersion;
        public string ApkVersion
        {
            get
            {
                if (Constants.BaseUrl.Equals(Constants.LiveDBurl))
                {
                    return version + "LiveDB";
                }
                else if (Constants.BaseUrl.Equals(Constants.TestDBurl))
                {
                    return version + "TestDB";
                }
                else
                    return version;
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public ICommand DisplayPendingTaskCommand { get; set; }
        public ICommand DownloadTablesCommand {get;set;}

        public ICommand DisplayAllertCommand { get; set; }

        public ICommand TestCommand { get; set; }
        public ICommand GPSSettingCommand { get; set; }

        public ICommand SettingCommand { get; set; }
        private async void DisplayGPSSettingCommand()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup
        }



        public bool CheckButtonLogin()
        {
            Console.WriteLine();
            return Session.LoggedIn;

        }


        async void Test()
        {
            IsBusy = true;
            IsDisplayButton = false;
            LoadingText = "Test Download Site...";
            var contentSite = await CloudDBService.GetSite();

            Thread.Sleep(3000);
            LoadingText = "Download done! Populating SQLite...";
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {


                LoadingText = "Insert code tables ...";

                conn.CreateTable<Site>();
                conn.DeleteAll<Site>();
                conn.InsertAll(contentSite);
            }

            IsBusy = false;
            IsDisplayButton = true;
        }

       async void DisplayWarning()
        {
            Console.WriteLine();
            bool answer = await Application.Current.MainPage.DisplayAlert("Welcome", "Press DOWNLOAD for downloading require tables. Or choose LOGIN for LOGIN directly ", "DOWNLOAD", "LOGIN");
            if (answer)
            {
                Console.WriteLine(  );
                DownloadTablesCommand?.Execute(null);
            }
            else
            {
                Console.WriteLine();
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }

        }


        async void DisplayPendingTaskCommandExecute()
        {
            Console.WriteLine();
            await Application.Current.MainPage.Navigation.PushAsync(new SyncPage());
        }

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(IsDisplayButton))]
        bool isBusy = false;


        bool isDisplayButton = true;
        public bool IsDisplayButton {
            get => isDisplayButton;
            set
            {
                if(IsBusy)
                    SetProperty(ref isDisplayButton, false);
                else
                    SetProperty(ref isDisplayButton, value);
                Console.WriteLine();
            }
        }

        [ObservableProperty]
        bool isLoginVisible = true;

        [ObservableProperty]
        bool isLogoutBtnVisible = false;

        [ObservableProperty]
        bool loggedIn = false;


        [ObservableProperty]
        bool isDownloadBtnVisible = true;


        [ObservableProperty]
        string loadingText;


        [ObservableProperty]
        bool isNavBarDisplayed = false;

        [ObservableProperty]
        bool isTabBarDisplayed = false;
        /// from login page

        string emailText;
        public string EmailText
        {
            get => emailText;
            set
            {
                SetProperty(ref emailText, value);
                CheckEntriesCommand?.Execute(null);

            }
        }


        string passwordText;
        public string PasswordText
        {
            get => passwordText;
            set
            {
                SetProperty(ref passwordText, value);
                CheckEntriesCommand?.Execute(null);
            }
        }

        [ObservableProperty]
        string firstName;

        [ObservableProperty]
        string lastName;

        ObservableCollection<User> Users;

        public ICommand CheckEntriesCommand { get; set; }


        public void ExecuteCheckEntriesCommand()
        {
            try
            {
                FirstName = Users.Where(a => (a.email == EmailText) && (a.password == PasswordText)).Select(a => a.first_name).First();
                LastName = Users.Where(a => (a.email == EmailText) && (a.password == PasswordText)).Select(a => a.last_name).First();
                Console.WriteLine(FirstName + " " + LastName);

            }
            catch (Exception exception)
            {
                FirstName = "";
                LastName = "";

                Console.WriteLine(exception.ToString());


            }

            OnPropertyChanged(nameof(FirstName)); // update FirstName entry
            OnPropertyChanged(nameof(LastName)); // update LastName entry

        }

        private async void ExecuteLoginCommand()
        {
            if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email or Password is wrong", "TRY AGAIN");
                Console.WriteLine();
                return;
            }

            Console.WriteLine();
            Session.uid = Users.Where(a => (a.email == EmailText) && (a.password == PasswordText)).Select(a => a.UserKey).First(); // populate uid to Static-class (session) property uid  
            Session.crew_leader = $"{FirstName} {LastName}";

            Session.LoggedIn = true;
            LoggedIn = true;
            IsLoginVisible = false;
            IsLogoutBtnVisible = true;
            IsNavBarDisplayed = true;
            IsTabBarDisplayed = true;   // display bottom Tab Bar

            // open dialog
            GPSSettingCommand?.Execute(null);

            Session.event_type = "1"; // Login
            await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());

        }


        async void ExecuteSettingCommand()
        {
            await Application.Current.MainPage.DisplayAlert("Setting", "Under Construction", "Continue");
        }
        private async void ExecuteLogoutCommand()
        {
            Console.WriteLine();
            Session.uid = 0; 
            Session.crew_leader = "";

            Session.LoggedIn = false;
            LoggedIn = false;
            IsLoginVisible = true;
            IsLogoutBtnVisible = false;
            IsNavBarDisplayed = false;
            IsTabBarDisplayed =false;

        }

        async void DownloadTablesCommandExecute()
        {

            IsBusy = true;

            (LoginCommand as Command).ChangeCanExecute();
            (DownloadTablesCommand as Command).ChangeCanExecute();
            (LogoutCommand as Command).ChangeCanExecute();


            LoadingText = "Downloading...";
            try
            {

                LoadingText = "Downloading end_user table";
                var contentUser = await CloudDBService.GetEndUserFromAWSMySQLTable();

                LoadingText = "Site...";
                var contentSite = await CloudDBService.GetSite();


                LoadingText = "Downloading Job table";
                var contentJob = await CloudDBService.GetJobFromAWSMySQLTable();
                var contentCodeSiteType = await CloudDBService.GetCodeSiteTypeFromAWSMySQLTable();
                LoadingText = "Downloading Site table";
                //var contentSite = await CloudDBService.GetSite();
                //var contentSite = await CloudDBService.GetSiteFromAWSMySQLTable();

                var contentCrewDefault = await CloudDBService.GetCrewDefaultFromAWSMySQLTable();

                LoadingText = "Downloading manufacturer table";
                var contentManuf = await CloudDBService.GetManufacturerTable(); //manufacturer_list 
                var contentJobSumittal = await CloudDBService.GetJobSubmittalTable(); //job_submittal
                var contentKeyType = await CloudDBService.GetKeyTypeTable(); // keytype
                var equipmentType = await CloudDBService.GetEquipmentType(); //code_equipment_type
                var equipmentDetail = await CloudDBService.GetEquipmentDetail(); //equipment


                var unitOfmeasure = await CloudDBService.GetUOM(); //unit_of_measurement
                LoadingText = "Downloading code_material table";
                var contentMaterialCode = await CloudDBService.GetMaterialCodeTable(); // material
                var contentMounting = await CloudDBService.GetMountingTable(); // mounting

                LoadingText = "Downloading roadway table";
                var contentRoadway = await CloudDBService.GetRoadway();  // roadway
                //var contentOwnRoadway = await CloudDBService.GetOwnerRoadway();  // owner_roadway, this will be joined to roadway
                var contentElectCircuit = await CloudDBService.GetElectricCircuit(); //intersection
                LoadingText = "Downloading intersection table";
                var contentIntersection = await CloudDBService.GetIntersection(); //electric

                var contentDirection = await CloudDBService.GetDirection(); //direction
                LoadingText = "Downloading code_duct_size table";
                var contentDuctSize = await CloudDBService.GetDuctSize(); //dsize
                var contentDuctType = await CloudDBService.GetDuctType(); //ducttype
                var contentGroupType = await CloudDBService.GetGroupType(); //grouptype


                var contentDevType = await CloudDBService.GetDevType(); //devtype
                var contentModelDetail = await CloudDBService.GetModelDetail(); //model
                var contentRackNumber = await CloudDBService.GetRackNumber();
                var contentRackType = await CloudDBService.GetRackType(); //racktype
                LoadingText = "Downloading code_fiber_sheath_type table";
                var contentSheath = await CloudDBService.GetSheath(); // sheath
                var contentReelId = await CloudDBService.GetReelId(); // reelid
                var contentOrientation = await CloudDBService.GetOrientation();  // sbto
                var contentChassis = await CloudDBService.GetChassis();  // sbto

                LoadingText = "Downloading a_fiber_cable table";
                var contentAFCable = await CloudDBService.GetAFCable();  // frcable
                LoadingText = "Downloading a_fiber_reel table";
                var contentCabStructure = await CloudDBService.GetCableStructure(); //cable_structure

                //var contentSide = await CloudDBService.GetSide(); //side
                var contentTraceWareTag = await CloudDBService.GetTraceWareTag(); // tracewaretag
                LoadingText = "Downloading owner table";
                var contentOwner = await CloudDBService.GetOwners(); //owners
                LoadingText = "Downloading conduits table";
                var contentConduit = await CloudDBService.GetConduits(); // conduits

                LoadingText = "Downloading duct_installtype table";
                var ductInstallType = await CloudDBService.GetDuctInstallType(); // installtype

                LoadingText = "Downloading fiber_installtype table";
                var fiberInstallType = await CloudDBService.GetFiberInstallType(); // installtype

                LoadingText = "Downloading ductused table";
                var contentDuctUsed = await CloudDBService.GetDuctUsed(); // ductused


                LoadingText = "Downloading dimesnsions table";
                var contentDimension = await CloudDBService.GetDimensions();   // dimesnsions

                LoadingText = "Downloading fltrsizes table";
                var contentFilterSize = await CloudDBService.GetFilterSize(); //fltrsizes
                LoadingText = "Downloading  code_filter_type table";
                var contentFilterType = await CloudDBService.GetFilterType(); //fltrsizes
                var contentSpliceType = await CloudDBService.GetSpliceType();//splicetype
                var contentLaborClass = await CloudDBService.GetLaborClass();// laborclass


                var contentTravellen = await CloudDBService.GetCompassDir(); // travellen
                LoadingText = "Downloading building_type table";
                var contentBuildingType = await CloudDBService.GetBuildingType(); //bClassification

                LoadingText = "Downloading code_cable_type table";
                var contentCableType = await CloudDBService.GetCableType(); //code_cable_type

                var codeDuctInstallType = await CloudDBService.GetDuctInstallTypes(); //code_duct_installation

                var codeColor = await CloudDBService.GetColorCode(); //code_colors

                var contentChassisType = await CloudDBService.GetChassisTypes(); //code_colors
                var contentslotBladeTray = await CloudDBService.GetBladeTableKey(); //slotbladetray

                var portType = await CloudDBService.GetCodePortType(); //code_port_type
                var portTable = await CloudDBService.GetPortTable(); //port table


                LoadingText = "Downloading code_locate_point table";

                var codeLocatePoint = await CloudDBService.GetLocatePoint(); //code_locate_point

                var max_gps_point = await CloudDBService.GetMaxGpsPoint(); //gps_point

                var suspList = await CloudDBService.GetSuspendedTrace(); //gps_point
                var excludeSiteEntry = await CloudDBService.GetExcludeSite(); //exclude_site

                //Thread.Sleep(5000);
                LoadingText = "Download done! Populating SQLite...";
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<UnSyncTaskList>();


                    LoadingText = "Insert code tables ...";

                    conn.CreateTable<DuctUsed>();
                    conn.DeleteAll<DuctUsed>();
                    conn.InsertAll(contentDuctUsed);



                    conn.CreateTable<SuspendedTrace>();
                    conn.DeleteAll<SuspendedTrace>();
                    conn.InsertAll(suspList);

                    conn.CreateTable<GpsPoint>();
                    conn.DeleteAll<GpsPoint>();
                    conn.InsertAll(max_gps_point);


                    conn.CreateTable<Owner>();
                    conn.DeleteAll<Owner>();
                    conn.InsertAll(contentOwner);


                    conn.CreateTable<CodeLocatePoint>();
                    conn.DeleteAll<CodeLocatePoint>();
                    conn.InsertAll(codeLocatePoint);

                    conn.CreateTable<PortType>();
                    conn.DeleteAll<PortType>();
                    conn.InsertAll(portType);

                    conn.CreateTable<Ports>();
                    conn.DeleteAll<Ports>();
                    conn.InsertAll(portTable);

                    conn.CreateTable<DuctInstallType>();
                    conn.DeleteAll<DuctInstallType>();
                    conn.InsertAll(ductInstallType);


                    conn.CreateTable<DuctInstallType>();
                    conn.DeleteAll<DuctInstallType>();
                    conn.InsertAll(ductInstallType);

                    conn.CreateTable<FiberInstallType>();
                    conn.DeleteAll<FiberInstallType>();
                    conn.InsertAll(fiberInstallType);

                    conn.CreateTable<User>();
                    conn.DeleteAll<User>();
                    conn.InsertAll(contentUser);

                    conn.CreateTable<Job>();
                    conn.DeleteAll<Job>();
                    conn.InsertAll(contentJob);

                    conn.CreateTable<CodeSiteType>();
                    conn.DeleteAll<CodeSiteType>();
                    conn.InsertAll(contentCodeSiteType);

                    conn.CreateTable<Crewdefault>();
                    conn.DeleteAll<Crewdefault>();
                    conn.InsertAll(contentCrewDefault);

                    conn.CreateTable<Manufacturer>();
                    conn.DeleteAll<Manufacturer>();
                    conn.InsertAll(contentManuf);

                    conn.CreateTable<JobSubmittal>();
                    conn.DeleteAll<JobSubmittal>();
                    conn.InsertAll(contentJobSumittal);

                    conn.CreateTable<KeyType>();
                    conn.DeleteAll<KeyType>();
                    conn.InsertAll(contentKeyType);

                    conn.CreateTable<MaterialCode>();
                    conn.DeleteAll<MaterialCode>();
                    conn.InsertAll(contentMaterialCode);

                    conn.CreateTable<Mounting>();
                    conn.DeleteAll<Mounting>();
                    conn.InsertAll(contentMounting);

                    conn.CreateTable<Roadway>();
                    conn.DeleteAll<Roadway>();
                    conn.InsertAll(contentRoadway);


                    conn.CreateTable<InterSectionRoad>();
                    conn.DeleteAll<InterSectionRoad>();
                    conn.InsertAll(contentIntersection);


                    conn.CreateTable<ElectricCircuit>();
                    conn.DeleteAll<ElectricCircuit>();
                    conn.InsertAll(contentElectCircuit);

                    conn.CreateTable<Direction>();
                    conn.DeleteAll<Direction>();
                    conn.InsertAll(contentDirection);

                    conn.CreateTable<DuctSize>();
                    conn.DeleteAll<DuctSize>();
                    conn.InsertAll(contentDuctSize);

                    conn.CreateTable<DuctType>();
                    conn.DeleteAll<DuctType>();
                    conn.InsertAll(contentDuctType);

                    conn.CreateTable<GroupType>();
                    conn.DeleteAll<GroupType>();
                    conn.InsertAll(contentGroupType);


                    conn.CreateTable<DevType>();
                    conn.DeleteAll<DevType>();
                    conn.InsertAll(contentDevType);

                    conn.CreateTable<ModelDetail>();
                    conn.DeleteAll<ModelDetail>();
                    conn.InsertAll(contentModelDetail);

                    conn.CreateTable<RackNumber>();
                    conn.DeleteAll<RackNumber>();
                    conn.InsertAll(contentRackNumber);

                    conn.CreateTable<RackType>();
                    conn.DeleteAll<RackType>();
                    conn.InsertAll(contentRackType);

                    conn.CreateTable<Sheath>();
                    conn.DeleteAll<Sheath>();
                    conn.InsertAll(contentSheath);

                    conn.CreateTable<ReelId>();
                    conn.DeleteAll<ReelId>();
                    conn.InsertAll(contentReelId);

                    conn.CreateTable<Chassis>();
                    conn.DeleteAll<Chassis>();
                    conn.InsertAll(contentChassis);

                    conn.CreateTable<ChassisType>();
                    conn.DeleteAll<ChassisType>();
                    conn.InsertAll(contentChassisType);

                    conn.CreateTable<CableStructure>();
                    conn.DeleteAll<CableStructure>();
                    conn.InsertAll(contentCabStructure);

                    conn.CreateTable<Dimensions>();
                    conn.DeleteAll<Dimensions>();
                    conn.InsertAll(contentDimension);

                    conn.CreateTable<Orientation>();
                    conn.DeleteAll<Orientation>();
                    conn.InsertAll(contentOrientation);

                    conn.CreateTable<FilterSize>();
                    conn.DeleteAll<FilterSize>();
                    conn.InsertAll(contentFilterSize);

                    conn.CreateTable<FilterType>();
                    conn.DeleteAll<FilterType>();
                    conn.InsertAll(contentFilterType);

                    conn.CreateTable<SpliceType>();
                    conn.DeleteAll<SpliceType>();
                    conn.InsertAll(contentSpliceType);

                    conn.CreateTable<LaborClass>();
                    conn.DeleteAll<LaborClass>();
                    conn.InsertAll(contentLaborClass);

                    conn.CreateTable<CompassDirection>();
                    conn.DeleteAll<CompassDirection>();
                    conn.InsertAll(contentTravellen);

                    conn.CreateTable<BuildingType>();
                    conn.DeleteAll<BuildingType>();
                    conn.InsertAll(contentBuildingType);

                    conn.CreateTable<AFiberCable>();
                    conn.DeleteAll<AFiberCable>();
                    conn.InsertAll(contentAFCable);

                    conn.CreateTable<CableType>();
                    conn.DeleteAll<CableType>();
                    conn.InsertAll(contentCableType);

                    conn.CreateTable<EquipmentType>();
                    conn.DeleteAll<EquipmentType>();
                    conn.InsertAll(equipmentType);

                    conn.CreateTable<EquipmentDetailType>();
                    conn.DeleteAll<EquipmentDetailType>();
                    conn.InsertAll(equipmentDetail);

                    conn.CreateTable<UnitOfMeasure>();
                    conn.DeleteAll<UnitOfMeasure>();
                    conn.InsertAll(unitOfmeasure);

                    conn.CreateTable<DuctInstallType>();
                    conn.DeleteAll<DuctInstallType>();
                    conn.InsertAll(codeDuctInstallType);



                    conn.CreateTable<ConduitsGroup>();
                    conn.DeleteAll<ConduitsGroup>();
                    conn.InsertAll(contentConduit);


                    conn.CreateTable<ColorCode>();
                    conn.DeleteAll<ColorCode>();
                    conn.InsertAll(codeColor);

                    conn.CreateTable<SlotBladeTray>();
                    conn.DeleteAll<SlotBladeTray>();
                    conn.InsertAll(contentslotBladeTray);




                    conn.CreateTable<Site>();
                    conn.DeleteAll<Site>();
                    conn.InsertAll(contentSite);


                    conn.CreateTable<ExcludeSite>();
                    conn.DeleteAll<ExcludeSite>();
                    conn.InsertAll(excludeSiteEntry);

                }
                LoadingText = "Populating Local SQLite done!";
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.ToString());
                bool answer =
                await Application.Current.MainPage.DisplayAlert("Warning", "Error during download database", "RETRY", "CLOSE");
                //if (answer)
                //    DownloadTablesT();

                Console.WriteLine(e.ToString());

            }



            LoadingText = "SQLite Dumping done...";


            IsBusy = false;
            (LoginCommand as Command).ChangeCanExecute();
            (DownloadTablesCommand as Command).ChangeCanExecute();
            (LogoutCommand as Command).ChangeCanExecute();
        }

    }

}
