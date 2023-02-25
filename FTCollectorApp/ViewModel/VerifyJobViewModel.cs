using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.Utils;
using FTCollectorApp.View;
using FTCollectorApp.View.BeginWork;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.View.TraceFiberPages;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class VerifyJobViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        bool isDisplayBeginWork = false;

        [ObservableProperty]
        bool isJobChanged = false;

        [ObservableProperty]
        bool isJobChanging = false;


        [ObservableProperty]
        bool isDisplayEndOfDay = false;

        [ObservableProperty]
        bool isDisplayEndOfDayForm = false;

        [ObservableProperty]
        string crewLeader = Session.crew_leader;


        string verifyStatusBadge = string.Empty;
        public string VerifyStatusBadge{
            get => verifyStatusBadge;
            set
            {
                if(IsVerified)
                    SetProperty(ref textStatus, "OK");
                else if (IsJobChanging)
                    SetProperty(ref textStatus, "");
            }
        }


        //[ObservableProperty]
        string textStatus = "Verify";
        public string TextStatus
        {
            get => textStatus;
            set
            {
                if(isVerified || IsJobChanged)
                    SetProperty(ref textStatus, "Verified");
                else if (IsJobChanging)
                    SetProperty(ref textStatus, "Verify");
            }
        }

        //[ObservableProperty]
        bool isVerified = false;
        public bool IsVerified{
            get => isVerified;
            set {
                SetProperty(ref isVerified, value);
                (ODOPopupCommand as Command).ChangeCanExecute();
                (DisplayEquipmentCheckInCommand as Command).ChangeCanExecute();
                (DisplayEquipmentCheckOutCommand as Command).ChangeCanExecute();
                (ToggleCrewListCommand as Command).ChangeCanExecute();
                Console.WriteLine();
            }
        }


        bool isVerifyBtnEnable = false;
        public bool IsVerifyBtnEnable
        {
            get => isVerifyBtnEnable;
            set
            {
                SetProperty(ref isVerifyBtnEnable, value);
                (ContinueCommand as Command).ChangeCanExecute();

                Console.WriteLine();
            }
        }


        Job selectedJob;
        public Job SelectedJob
        {
            get => selectedJob;
            set
            {
                Console.WriteLine();
                IsDisplayBeginWork = false;
                IsVerifyBtnEnable = false;

                try
                {

                    var QueryOwnerJobNumber = _jobdetails.Where(a => (a.OwnerName == value.OwnerName) && (a.JobNumber == value.JobNumber));

                    // SetProperty(ref selectedJob, value);

                    if (Equals(selectedJob, value) || string.IsNullOrEmpty(nameof(selectedJob)))
                    {
                        IsDisplayBeginWork = true;
                        IsVerifyBtnEnable = false;
                        return;
                    }
                    selectedJob = value;
                    OnPropertyChanged(nameof(SelectedJob));
                    //OnPropertyChanged(nameof(IsVerifyBtnEnable));
                    IsVerifyBtnEnable = true;

                    Session.stage = QueryOwnerJobNumber.Select(a => a.stage).First();
                    Session.ownerkey = QueryOwnerJobNumber.Select(a => a.OwnerKey).First();
                    Session.jobkey = QueryOwnerJobNumber.Select(a => a.JobKey).First();
                    Session.ownerCD = QueryOwnerJobNumber.Select(a => a.OWNER_CD).First();
                    Session.countycode = QueryOwnerJobNumber.Select(a => a.CountyCode).First();
                    Session.JobShowAll = QueryOwnerJobNumber.Select(a => a.ShowAll).First();
                    Session.jobnum = value.JobNumber;
                    Session.OwnerName = value.OwnerName;

                    // Put to property location
                    //Application.Current.Properties[JobNumberKey] = value.JobNumber;
                    //Application.Current.Properties[JobOwnerKey] = value.OwnerName;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                Console.WriteLine();

            }
        }

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(JobNumbers))]
        Job selectedJobOwner;


        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(JobNumbers))]
        Owner selectedOwner;

        public ObservableCollection<Job> JobsByOwner
        {
            get
            {
                Console.WriteLine();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Job>();
                    var jobdetails = conn.Table<Job>().ToList();
                    var ownerNames = jobdetails.GroupBy(b => b.OwnerName).Select(g => g.First()).ToList();
                    return new ObservableCollection<Job>(ownerNames);
                }
            }
        }

        public ObservableCollection<Owner> OwnerList
        {
            get
            {
                Console.WriteLine();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Owner>();
                    var jobdetails = conn.Table<Owner>().ToList();
                    Console.WriteLine();
                    var ownerNames = jobdetails.Where(a => a.EndUserKey == Session.uid.ToString() || a.AltOwnerKey == Session.uid.ToString()).GroupBy(b => b.OwnerName).Select(g => g.First()).ToList();
                    Console.WriteLine();
                    return new ObservableCollection<Owner>(ownerNames);
                }
            }
        }


        public ObservableCollection<Job> JobNumbers
        {
            get
            {
                Console.WriteLine();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Job>();
                    var table = conn.Table<Job>().ToList();
                    // add "?" to make an object nullable.
                    // without nullable var, it will raise exception when it is null value
                    if (SelectedOwner?.OwnerName != null){
                        Console.WriteLine();
                        table = conn.Table<Job>().Where(a => a.OwnerName == SelectedOwner.OwnerName).GroupBy(b => b.JobNumber).Select(g => g.First()).ToList();
                        Console.WriteLine();
                    }
                    return new ObservableCollection<Job>(table);
                }
            }
        }

        private ObservableCollection<Job> _jobdetails = new ObservableCollection<Job>();

        public ICommand GPSSettingCommand { get; set; }
        public ICommand ContinueCommand { get; set; }

        public ICommand ToggleCrewListCommand { get; set; }
        public ICommand ToggleJobEntriesCommand { get; set; }

        public ICommand ODOPopupCommand { get; set; }

        public ICommand ODOSaveCommand { get; set; }
        public ICommand LunchOutCommand { get; set; }
        public ICommand LunchInCommand { get; set; }
        public ICommand ToggleEndofDayCommand { get; set; }
        public ICommand TriggerEndOfDayCommand { get; set; }
        public ICommand CrewSaveCommand { get; set; }



        public ICommand DisplayEquipmentCheckInCommand { get; set; }
        public ICommand DisplayEquipmentCheckOutCommand { get; set; }

        [ObservableProperty]
        bool isDisplayCrewList = false;

        [ObservableProperty]
        bool isDisplayJobEntries = false;

        [ObservableProperty]
        bool isDisplayOdo = false;

        [ObservableProperty]
        string verified = string.Empty;

        [ObservableProperty]
        bool isLunchOut = true;

        [ObservableProperty]
        bool isLunchIn = false;

        private void ToggleExecute()
        {
            IsDisplayCrewList = !IsDisplayCrewList;
            IsDisplayEndOfDayForm = false;
        }

        private void ToggleJobEntriesExecute()
        {
            IsDisplayCrewList = false;
            IsDisplayOdo = false;
            IsDisplayEndOfDayForm = false;
            IsDisplayJobEntries = !IsDisplayJobEntries;
        }

        private void ToggleEndofDayExecute()
        {
            IsDisplayCrewList = false;
            IsDisplayOdo = false;

            IsDisplayEndOfDayForm = !IsDisplayEndOfDayForm;
        }

        [ObservableProperty]
        Color clrBkgndJob = Color.LightBlue;

        [ObservableProperty]
        Color clrBkgndCrew = Color.LightBlue;

        [ObservableProperty]
        Color clrBkgndECheckin = Color.LightBlue;

        [ObservableProperty]
        Color clrBkgndEChkOut = Color.LightBlue;

        [ObservableProperty]
        Color clrBkgndODO = Color.LightBlue;

        [ObservableProperty]
        string entryOdometer;

        [ObservableProperty] int perDiemEmp1;

        [ObservableProperty] int perDiemEmp2;

        [ObservableProperty] int perDiemEmp3;

        [ObservableProperty] int perDiemEmp4;

        [ObservableProperty] int perDiemEmp5;

        [ObservableProperty] string startTimeEmp1;
        [ObservableProperty] string startTimeEmp2;
        [ObservableProperty] string startTimeEmp3;
        [ObservableProperty] string startTimeEmp4;
        [ObservableProperty] string startTimeEmp5;

        [ObservableProperty] string employee1Labor;
        [ObservableProperty] string employee2Labor;
        [ObservableProperty] string employee3Labor;
        [ObservableProperty] string employee4Labor;
        [ObservableProperty] string employee5Labor;

        public VerifyJobViewModel()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Job>();
                var jobdetails = conn.Table<Job>().ToList();
                _jobdetails = new ObservableCollection<Job>(jobdetails);

            }
            Console.WriteLine();

            GPSSettingCommand = new Command(() => DisplayGPSSettingCommand());
            ToggleJobEntriesCommand = new Command(() => ToggleJobEntriesExecute());
            ToggleEndofDayCommand = new Command(() => ToggleEndofDayExecute());


            try
            {
                ToggleCrewListCommand = new Command(
                    execute: () =>
                    {
                        IsDisplayJobEntries = false;
                        IsDisplayOdo = false;
                        IsDisplayCrewList = !IsDisplayCrewList;
                        clrBkgndJob = Color.LightBlue;
                        clrBkgndCrew = Color.Green;
                        clrBkgndECheckin = Color.LightBlue;
                        clrBkgndEChkOut = Color.LightBlue;
                        clrBkgndODO = Color.LightBlue;

                    },
                    canExecute: () =>
                    {
                        Console.WriteLine();
                        return IsVerified;
                    }
                );

                CrewSaveCommand = new Command(
                    execute: async () =>
                    {
                        string curHHMM = DateTime.Now.ToString("H:m");
                        string curHour = DateTime.Now.ToString("H");
                        string curMinute = DateTime.Now.ToString("m");
                        // preserve LunchOut time here
                        IsLunchOut = true;
                        IsLunchIn = false;
                        (LunchInCommand as Command).ChangeCanExecute();
                        Console.WriteLine();
                        Application.Current.Properties["ClockInHH"] = curHour;
                        Application.Current.Properties["ClockInMM"] = curMinute;
                        Session.event_type = "15"; // Clock In
                        /*if (Employee1Name != null && StartTimeEmp1.Length > 2)
                            await JobSaveEvent(PerDiemEmp1, curHHMM);
                        await JobSaveEvent(PerDiemEmp2, curHHMM);
                        await JobSaveEvent(PerDiemEmp3, curHHMM);
                        await JobSaveEvent(PerDiemEmp4, curHHMM);
                        await JobSaveEvent(PerDiemEmp5, curHHMM);*/

                    },
                    canExecute: () =>
                    {
                        Console.WriteLine();
                        return Employee1Name.FullName.Length > 1;
                    }
                );


                LunchOutCommand = new Command(
                    execute: async () =>
                    {
                        string curHHMM = DateTime.Now.ToString("H:m");
                        string curHour = DateTime.Now.ToString("H");
                        string curMinute = DateTime.Now.ToString("m");

                        IsLunchOut = true;
                        IsLunchIn = false;
                        (LunchInCommand as Command).ChangeCanExecute();
                        Console.WriteLine();
                        Application.Current.Properties["LunchOutHH"] = curHour;
                        Application.Current.Properties["LunchOutMM"] = curMinute;
                        Session.event_type = "13"; // Lunch in
                        LunchOutTime = DateTime.Now.ToString("HH:mm");
                        await JobSaveEvent(0, "");

                    },
                    canExecute: () =>
                    {
                        Console.WriteLine();
                        return IsLunchIn;
                    }
                );

                LunchInCommand = new Command(
                    execute: async () =>
                    {

                        IsLunchOut = false;
                        IsLunchIn = true;

                        (LunchOutCommand as Command).ChangeCanExecute();
                        LunchInTime = DateTime.Now.ToString("HH:mm");
                        Session.event_type = "14"; // Lunch in
                        await JobSaveEvent(0, LunchInTime);

                    },
                    canExecute: () =>
                    {
                        Console.WriteLine();
                        return IsLunchOut;
                    }
                );

                TriggerEndOfDayCommand = new Command(
                    execute: async () =>
                    {

                        ClockOutTime = DateTime.Now.ToString("HH:mm");
                        Session.event_type = "16"; // ClockOut
                        await JobSaveEvent(0, ClockOutTime);

                    },
                    canExecute: () =>
                    {
                        Console.WriteLine();
                        return IsLunchOut;
                    }
                );

                ODOPopupCommand = new Command(
                    execute: async () =>
                    {
                        IsDisplayOdo = !IsDisplayOdo;
                        IsDisplayCrewList = false;
                        IsDisplayJobEntries = false;
                        clrBkgndJob = Color.LightBlue;
                        clrBkgndCrew = Color.LightBlue;
                        clrBkgndECheckin = Color.LightBlue;
                        clrBkgndEChkOut = Color.LightBlue;
                        clrBkgndODO = Color.Green;
                    },
                    canExecute: () =>
                    {
                        Console.WriteLine();
                        return IsVerified;
                    }


                );
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception "+e);
            }


            ODOSaveCommand = new Command(
                execute: async () =>
                { 
                    IsBusy = true;
                    try
                    {
                        await CloudDBService.PostJobEvent(EntryOdometer);


                        /* Tab Navigation */
                        await Application.Current.MainPage.DisplayAlert("Job Event", "Job uploaded. Please Continue to Site Menu", "CLOSE");

                        
                        /*if (answer)
                        {
                            if (Session.stage =="A")
                                await Navigation.PushAsync(new AsBuiltDocMenu());
                            if (Session.stage == "I")
                                await Navigation.PushAsync(new MainMenuInstall());
                        }*/
                        //await PopupNavigation.Instance.PopAsync(true);
                    }
                    catch
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Update JobEvent table failed. Check again internet connection", "CLOSE");
                    }
                    IsBusy = false;

                    //await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new OdometerPopup());
                }

            );

            DisplayEquipmentCheckInCommand = new Command(
                execute: () => 
                {
                    DisplayEquipmentCheckIn();
                    clrBkgndJob = Color.LightBlue;
                    clrBkgndCrew = Color.LightBlue;
                    clrBkgndECheckin = Color.Green;
                    clrBkgndEChkOut = Color.LightBlue;
                    clrBkgndODO = Color.LightBlue;
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return IsVerified;
                }
            );

            DisplayEquipmentCheckOutCommand = new Command(
                execute: () =>
                {
                    DisplayEquipmentCheckOut();
                    clrBkgndJob = Color.LightBlue;
                    clrBkgndCrew = Color.LightBlue;
                    clrBkgndECheckin = Color.LightBlue;
                    clrBkgndEChkOut = Color.Green;
                    clrBkgndODO = Color.LightBlue;
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return IsVerified;
                }
            );

            ContinueCommand = new Command(
                execute: async () => {
                    Session.event_type = Session.JOB_VERIFIED;
                    IsBusy = true;
                    try
                    {
                        await CloudDBService.PostJobEvent();
                        var speaker = DependencyService.Get<ITextToSpeech>();
                        speaker?.Speak("Job verified!");

                        Application.Current.Properties["PageNumber"] = 3;

                        IsJobChanged = IsVerified; // condition when job changed
                        IsVerified = true; // enable EqIn, EqOut, ODO input button
                        Session.IsVerified = true; // singleton session instance to notify Verify job done
                        Verified = "Verified";
                    }
                    catch
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Update JobEvent table failed", "OK");
                    }
                    IsBusy = false;
                },

                canExecute: () =>
                {
                    Console.WriteLine();
                    return IsVerifyBtnEnable;
                }
              );

        }

        [ObservableProperty]
        CrewInfoDetail selectedCrewForEndOfDay;

        public ObservableCollection<CrewInfoDetail> SelectedCrewInfoDetails = new ObservableCollection<CrewInfoDetail>();

        private CrewInfoDetail employee1Name;
        public CrewInfoDetail Employee1Name
        {
            get => employee1Name;
            set
            {
                SetProperty(ref employee1Name, value);
                // Check Existense before insert
                if (!SelectedCrewInfoDetails.Contains(value))
                {
                    SelectedCrewInfoDetails.Add(new CrewInfoDetail
                    {
                        id=1,
                        FullName = value.FullName,
                        TeamUserKey = value.TeamUserKey
                    });
                    Console.WriteLine();
                }
            }
        }

        private CrewInfoDetail employee2Name;
        public CrewInfoDetail Employee2Name
        {
            get => employee2Name;
            set
            {
                SetProperty(ref employee2Name, value);
                // Check Existense before insert
                if (!SelectedCrewInfoDetails.Contains(value))
                {
                    SelectedCrewInfoDetails.Add(new CrewInfoDetail
                    {
                        id = 2,
                        FullName = value.FullName,
                        TeamUserKey = value.TeamUserKey
                    });
                    Console.WriteLine();
                }
            }
        }

        private CrewInfoDetail employee3Name;
        public CrewInfoDetail Employee3Name
        {
            get => employee3Name;
            set
            {
                SetProperty(ref employee3Name, value);
                // Check Existense before insert
                if (!SelectedCrewInfoDetails.Contains(value))
                {
                    SelectedCrewInfoDetails.Add(new CrewInfoDetail
                    {
                        id = 3,
                        FullName = value.FullName,
                        TeamUserKey = value.TeamUserKey
                    });
                    Console.WriteLine();
                }
            }
        }

        private CrewInfoDetail employee4Name;
        public CrewInfoDetail Employee4Name
        {
            get => employee4Name;
            set
            {
                SetProperty(ref employee4Name, value);
                // Check Existense before insert
                if (!SelectedCrewInfoDetails.Contains(value))
                {
                    SelectedCrewInfoDetails.Add(new CrewInfoDetail
                    {
                        id = 4,
                        FullName = value.FullName,
                        TeamUserKey = value.TeamUserKey
                    });
                    Console.WriteLine();
                }
            }
        }

        private CrewInfoDetail employee5Name;
        public CrewInfoDetail Employee5Name
        {
            get => employee4Name;
            set
            {
                SetProperty(ref employee5Name, value);
                // Check Existense before insert
                if (!SelectedCrewInfoDetails.Contains(value))
                {
                    SelectedCrewInfoDetails.Add(new CrewInfoDetail
                    {
                        id = 5,
                        FullName = value.FullName,
                        TeamUserKey = value.TeamUserKey
                    });
                    Console.WriteLine();
                }
            }
        }

        [ObservableProperty] string startTime1;

        [ObservableProperty] string clockIntime;

        [ObservableProperty] string lunchOutTime;

        [ObservableProperty] string lunchInTime;

        [ObservableProperty] string clockOutTime;

        [ObservableProperty] string totalHoursForToday;

        public ObservableCollection<CrewInfoDetail> SelectableCrewList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    var table = conn.Table<User>();
                    List<CrewInfoDetail> temp = new List<CrewInfoDetail>();
                    foreach (var col in table)
                    {
                        temp.Add(new CrewInfoDetail
                        {
                            FullName = col.first_name + " " + col.last_name,
                            TeamUserKey = col.UserKey
                        });
                        // table.Select(a => a.first_name).First() + " " + table.Select(a => a.last_name).First(),
                        // TeamUserKey = table.Select(c => c.UserKey).First()
                    }

                    Console.WriteLine();
                    return new ObservableCollection<CrewInfoDetail>(temp);
                }
            } 
        }

        private ObservableCollection<Crewdefault> Crewtable = new ObservableCollection<Crewdefault>();



        private async void DisplayGPSSettingCommand()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup
        }


        private async void DisplayEquipmentCheckIn()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new EquipmentCheckIn()); // for Rg.plugin popup
        }

        private async void DisplayEquipmentCheckOut()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new EquipmentCheckOut()); // for Rg.plugin popup
        }


        async void XQtODOPopupCommand()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new OdometerPopup());
        }



        async Task JobSaveEvent(int perDiemIdx, string sStartTime)
        {
            //Session.event_type = Session.ClockIn;

            await LocationService.GetLocation();

            Session.accuracy = String.Format("{0:0.######}", LocationService.Coords.Accuracy);
            //Session.longitude2 = String.Format("{0:0.######}", LocationService.Coords.Longitude);
            //Session.lattitude2 = String.Format("{0:0.######}", LocationService.Coords.Latitude);
            Session.live_longitude = String.Format("{0:0.######}", LocationService.Coords.Longitude);
            Session.live_lattitude = String.Format("{0:0.######}", LocationService.Coords.Latitude);
            Session.altitude = String.Format("{0:0.######}", LocationService.Coords.Altitude);
            //{ String.Format("{0:0.#######}", _location.Latitude.ToString())}

            try
            {
                DateTime dt = DateTime.Parse(sStartTime.Trim());
                int user_hours = int.Parse(dt.ToString("HH"));
                int user_minutes = int.Parse(dt.ToString("mm"));

                await CloudDBService.PostJobEvent(user_hours.ToString(), user_minutes.ToString(),  perDiemIdx.ToString());

            }
            catch
            {

                await Application.Current.MainPage.DisplayAlert("Warning", "Time format must be HH:MM", "OK");
            }

        }

    }
}
