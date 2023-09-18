using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Model.AWS;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Services;
using FTCollectorApp.View;
using FTCollectorApp.View.SyncPages;
using SQLite;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FTCollectorApp.ViewModel
{
    public partial class LoginPopUpVM : ObservableObject
    {

        void EnableLoginButton()
        {
            IsBusy = false;
            (LoginCommand as Command).ChangeCanExecute();
        }

        void DisableLoginButton()
        {
            IsBusy = true;
            (LoginCommand as Command).ChangeCanExecute();
        }


        async Task LaunchDBDownloadBackgroundService()
        {
            Console.WriteLine();
            if (DependencyService.Resolve<IForegroundService>().IsForeGroundServiceRunning())
            {
                Console.WriteLine();
                await Application.Current.MainPage.DisplayAlert("Warning", "Background Download already started", "OK");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();

                if (Application.Current.Properties.ContainsKey("DownloadCount"))
                {

                    try
                    {
                        var sCount = Application.Current.Properties["DownloadCount"] as string; // application properties cannot directly convert to integer
                        int cnt = int.Parse(sCount);
                        Console.WriteLine();
                        if (cnt >= 1)
                        {
                            var redownload = await Application.Current.MainPage.DisplayAlert("Info", "DB already downloaded " + cnt + " times before.Redownload ?", "Yes", "No");
                            if (!redownload)
                            {
                                IsLoginVisible = true; // display username and password
                                EnableLoginButton();
                                Console.WriteLine();
                                return;
                            }
                        }
                        cnt++;
                        Application.Current.Properties["DownloadCount"] = cnt.ToString();
                        Application.Current.Properties["DBVersion"] = "1.0";
                        Console.WriteLine();
                    }
                    catch(Exception e)
                    {
                        Application.Current.Properties["DownloadCount"] = "1";
                        Console.WriteLine(e.ToString());
                    }
                }
                else
                {
                    Application.Current.Properties.Add("DownloadCount", "1");
                    Application.Current.Properties.Add("DBVersion", "1.0");
                    Application.Current.SavePropertiesAsync();
                    Console.WriteLine();
                }
                Console.WriteLine();
                DisableLoginButton();
                LoadingText = "Downloading DB ... ";
                IsDownloadBtnVisible = false;
                DependencyService.Resolve<IForegroundService>().StartMyForegroundService();

            }
        }

        public LoginPopUpVM()
        {

            DisplayPendingTaskCommand = new Command(async () => DisplayPendingTaskCommandExecute());
            DisplayAllertCommand = new Command(async () => DisplayWarning());
            //TestCommand = new Command(async () => Test());
            Console.WriteLine();

            CheckEntriesCommand = new Command(ExecuteCheckEntriesCommand);
            GPSSettingCommand = new Command(() => DisplayGPSSettingCommand());
            UpdateUserLoggin();
            DownloadTablesCommand = new Command(
                execute: async () => {
                    LaunchDBDownloadBackgroundService();
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
                execute: async () =>
                {
                    Console.WriteLine();
                    Session.uid = 0;
                    Session.crew_leader = "";

                    Session.LoggedIn = false;
                    LoggedIn = false;
                    IsLoginVisible = true;
                    IsLogoutBtnVisible = false;
                    IsNavBarDisplayed = false;
                    IsTabBarDisplayed = false;

                },
                canExecute: () =>
                {
                    return !IsBusy;
                }
                );


            MessagingCenter.Subscribe<IForegroundService>(this, "DOWNLOAD_ERROR", async (sender) =>
            {
                LoadingText = "Download ERROR";

                //EnableLoginButton();
                IsBusy = false;
                IsDownloadBtnVisible = true;
                Console.WriteLine("DOWNLOAD_ERROR");
                DependencyService.Resolve<IForegroundService>().StopMyForegroundService();
            });

            MessagingCenter.Subscribe<IForegroundService>(this, "DOWNLOAD_DONE", async (sender) =>
            {
                LoadingText = "Download COMPLETE";


                IsLoginVisible = true; // display username and password
                EnableLoginButton();

                Console.WriteLine("DOWNLOAD_DONE");

                UpdateUserLoggin();     // IPC User SQLite DB 
                DependencyService.Resolve<IForegroundService>().StopMyForegroundService();
            });

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // auto start download
                LaunchDBDownloadBackgroundService();
            }
            else
            {
                DisplayConnectionProblemWarning();

            }
        }

        async void DisplayConnectionProblemWarning()
        {
            await Application.Current.MainPage.DisplayAlert("Warning", "No Internet connection", "BACK");
        }

        void UpdateUserLoggin()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                //Type classname = object1.GetType();

                conn.CreateTable<User>();
                Console.WriteLine("CreateTable<User> ");
                var userdetails = conn.Table<User>().ToList();
                //conn.InsertAll(users);

                Users = new ObservableCollection<User>(userdetails);
            }
        }

        string version = "0912-1"; // change here for release

        string apkVersion;
        public string ApkVersion
        {
            get
            {
                if (Constants.BaseUrl.Equals(Constants.LiveDBurl))
                {
                    return version + "LiveDB";
                }
                else if (Constants.BaseUrl.Equals(Constants.MariettaDB))
                {
                    return version + "MariettaDB";
                }
                else if (Constants.BaseUrl.Equals(Constants.TestingDB))
                {
                    return version + "TestingDB";
                }
                else if (Constants.BaseUrl.Equals(Constants.FloridaDB))
                {
                    return version + "FloridaDB";
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
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new GpsSettingPopup());
            //await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup
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

            try { 
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {


                    LoadingText = "Insert code tables ...";

                    conn.CreateTable<Site>();
                    conn.DeleteAll<Site>();
                    conn.InsertAll(contentSite);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
        bool isLoginVisible = false;

        [ObservableProperty]
        bool isLogoutBtnVisible = false;

        [ObservableProperty]
        bool loggedIn = false;


        [ObservableProperty]
        bool isDownloadBtnVisible = false;


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




            await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();


            // open dialog
            GPSSettingCommand?.Execute(null);

            Session.event_type = "1"; // Login
            await CloudDBService.PostJobEvent();

            DependencyService.Resolve<IForegroundService>().StopMyForegroundService();
            MessagingCenter.Send<LoginPopUpVM>(this, "LoginToVerifyJobCh");
        }




    }

}
