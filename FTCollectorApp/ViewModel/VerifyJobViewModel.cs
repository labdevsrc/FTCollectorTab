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
        public string VerifyStatusBadge {
            get => verifyStatusBadge;
            set
            {
                if (IsVerified)
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
                if (isVerified || IsJobChanged)
                    SetProperty(ref textStatus, "Verified");
                else if (IsJobChanging)
                    SetProperty(ref textStatus, "Verify");
            }
        }

        //[ObservableProperty]
        bool isVerified = false;
        public bool IsVerified {
            get => isVerified;
            set {
                SetProperty(ref isVerified, value);
                (ODOPopupCommand as Command).ChangeCanExecute();
                (DisplayEquipmentCheckInCommand as Command).ChangeCanExecute();
                (DisplayEquipmentCheckOutCommand as Command).ChangeCanExecute();
                (ToggleCrewListCommand as Command).ChangeCanExecute();
                (ToggleEndofDayCommand as Command).ChangeCanExecute();
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
                (VerifyCommand as Command).ChangeCanExecute();

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
                catch (Exception e)
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
                    if (SelectedOwner?.OwnerName != null) {
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
        public ICommand VerifyCommand { get; set; }

        public ICommand ToggleCrewListCommand { get; set; }
        public ICommand ToggleJobEntriesCommand { get; set; }

        public ICommand ODOPopupCommand { get; set; }

        public ICommand ODOSaveCommand { get; set; }
        public ICommand LunchOutCommand { get; set; }
        public ICommand LunchInCommand { get; set; }
        public ICommand ToggleEndofDayCommand { get; set; }
        public ICommand SaveEndOfDayCommand { get; set; }
        public ICommand CrewSaveCommand { get; set; }

        public ICommand SaveEqCheckOutCommand { get; set; }
        public ICommand SaveEqCheckInCommand { get; set; }

        public ICommand DisplayEquipmentCheckInCommand { get; set; }
        public ICommand DisplayEquipmentCheckOutCommand { get; set; }

        [ObservableProperty]
        bool isEqCheckOutDisplayed = false;

        [ObservableProperty]
        bool isEqCheckInDisplayed = false;      

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

        [ObservableProperty]
        bool isLunchOutDisplay = false;

        [ObservableProperty]
        bool isLunchInDisplay = false;

        private void ToggleExecute()
        {
            IsDisplayCrewList = !IsDisplayCrewList;
            IsDisplayEndOfDayForm = false;
            IsDisplayEndOfDay = false;
            IsLunchOutDisplay = false;
            IsLunchInDisplay = false;
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

        [ObservableProperty] int perDiemEmp1 = 0;
        [ObservableProperty] int perDiemEmp2 = 0;
        [ObservableProperty] int perDiemEmp3 = 0;
        [ObservableProperty] int perDiemEmp4 = 0;
        [ObservableProperty] int perDiemEmp5 = 0;
        [ObservableProperty] int perDiemEmp6 = 0;

        [ObservableProperty] string startTimeLeader = DateTime.Now.ToString("HH:mm");
        [ObservableProperty] string startTimeEmp1;
        [ObservableProperty] string startTimeEmp2;
        [ObservableProperty] string startTimeEmp3;
        [ObservableProperty] string startTimeEmp4;
        [ObservableProperty] string startTimeEmp5;
        [ObservableProperty] string startTimeEmp6;

        [ObservableProperty] string employee1Labor;
        [ObservableProperty] string employee2Labor;
        [ObservableProperty] string employee3Labor;
        [ObservableProperty] string employee4Labor;
        [ObservableProperty] string employee5Labor;
        [ObservableProperty] string employee6Labor;


        public VerifyJobViewModel()
        {
            bool SomethingWrong = false;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Job>();
                var jobdetails = conn.Table<Job>().ToList();
                _jobdetails = new ObservableCollection<Job>(jobdetails);

            }
            Console.WriteLine();

            GPSSettingCommand = new Command(() => DisplayGPSSettingCommand());
            ToggleJobEntriesCommand = new Command(
                execute: () =>
                {
                    IsDisplayCrewList = false;
                    IsDisplayOdo = false;
                    IsDisplayEndOfDayForm = false;
                    IsDisplayEndOfDay = false;
                    IsDisplayJobEntries = !IsDisplayJobEntries;
                    IsLunchOutDisplay = false;
                    IsLunchInDisplay = false;
                },
                canExecute: () =>
                {
                    return !IsDisplayCrewList;
                }
            );

            ToggleEndofDayCommand = new Command(
                execute: () => {
                    IsDisplayEndOfDay = !IsDisplayEndOfDay;
                    IsDisplayCrewList = false;
                    IsDisplayJobEntries = false;
                    IsDisplayOdo = false;
                    IsLunchInDisplay = false;
                    IsLunchOutDisplay = false;

                    //calculate from Clock in to Lunch out

                    //check if clockintime > lunchout
                    Console.WriteLine();

                },
                canExecute: () =>
                {
                    return !IsDisplayCrewList;
                }
                
                ) ;

            ToggleCrewListCommand = new Command(
                execute: () =>
                {
                    IsDisplayJobEntries = false;
                    IsDisplayOdo = false;
                    IsDisplayCrewList = !IsDisplayCrewList;
                    IsDisplayEndOfDay = false;
                    IsDisplayEndOfDayForm = false;
                    IsLunchInDisplay = false;
                    IsLunchOutDisplay = false;

                    (ToggleEndofDayCommand as Command).ChangeCanExecute();
                    (DisplayEquipmentCheckOutCommand as Command).ChangeCanExecute();
                    (DisplayEquipmentCheckInCommand as Command).ChangeCanExecute();
                    (LunchOutCommand as Command).ChangeCanExecute();
                    (ToggleJobEntriesCommand as Command).ChangeCanExecute();
                    (ODOPopupCommand as Command).ChangeCanExecute();


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
                    Console.WriteLine(  );
                    try
                    {
                        string curHHMM = DateTime.Now.ToString("HM:mm");
                        string curHour = DateTime.Now.ToString("HH");
                        string curMinute = DateTime.Now.ToString("mm");
                        // preserve LunchOut time here
                        Console.WriteLine();



                        // Put to list view
                        if (StartTimeLeader.Length > 3 && StartTimeLeader.Length > 3)
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 1,
                                FullName = CrewLeader,
                                TeamUserKey = 22,
                                StartTime = StartTimeLeader

                            });

                        if (Employee1Name?.FullName.Length > 1 && StartTimeEmp1.Length > 3)
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 2,
                                FullName = Employee1Name?.FullName,
                                TeamUserKey = Employee1Name.TeamUserKey,
                                StartTime = StartTimeEmp1

                            });

                        if (Employee2Name?.FullName.Length > 1 && StartTimeEmp2.Length > 3)
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 3,
                                FullName = Employee2Name?.FullName,
                                TeamUserKey = Employee2Name.TeamUserKey,
                                StartTime = StartTimeEmp2

                            });

                        if (Employee3Name?.FullName.Length > 1 && StartTimeEmp3.Length > 3)
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 4,
                                FullName = Employee3Name?.FullName,
                                TeamUserKey = Employee3Name.TeamUserKey,
                                StartTime = StartTimeEmp3

                            });

                        if (Employee4Name?.FullName.Length > 1 && StartTimeEmp4.Length > 3)
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 5,
                                FullName = Employee4Name?.FullName,
                                TeamUserKey = Employee4Name.TeamUserKey,
                                StartTime = StartTimeEmp4

                            });

                        if (Employee5Name?.FullName.Length > 1 && StartTimeEmp5.Length > 3)
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 6,
                                FullName = Employee5Name?.FullName,
                                TeamUserKey = Employee5Name.TeamUserKey,
                                StartTime = StartTimeEmp5

                            });

                            OnPropertyChanged(nameof(FinishDayList));


                        Session.event_type = "3";
                        await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
                            await CloudDBService.SaveCrewdata(Session.ownerCD,
                                Employee1Name?.TeamUserKey.ToString(),
                                Employee2Name?.TeamUserKey.ToString(),
                                Employee3Name?.TeamUserKey.ToString(), Employee4Name?.TeamUserKey.ToString(),
                                Employee5Name?.TeamUserKey.ToString(), Employee6Name?.TeamUserKey.ToString(),
                                PerDiemEmp1.ToString(), PerDiemEmp2.ToString(), PerDiemEmp3.ToString(),
                                PerDiemEmp4.ToString(), PerDiemEmp5.ToString(), PerDiemEmp6.ToString(),
                                Employee1IsDriver.ToString(), Employee2IsDriver.ToString(),
                                Employee3IsDriver.ToString(), Employee4IsDriver.ToString(),
                                Employee5IsDriver.ToString(), Employee6IsDriver.ToString()
                                );

                            await Application.Current.MainPage.DisplayAlert("Updated", "Crew member updated", "OK");
                            DisplayEquipmentCheckInCommand?.Execute(null); // show Crew List 

                        IsDisplayCrewList = false;

                        (DisplayEquipmentCheckOutCommand as Command).ChangeCanExecute();
                        (DisplayEquipmentCheckInCommand as Command).ChangeCanExecute();
                        (LunchOutCommand as Command).ChangeCanExecute();
                        (ToggleEndofDayCommand as Command).ChangeCanExecute();

                        // Display Equipment Checkout
                        DisplayEquipmentCheckOutCommand.Execute(null);



                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception " + e);
                    }
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return true;
                    //return Employee1Name?.FullName.Length > 1 || Employee2Name?.FullName.Length > 1 ||
                    //        Employee3Name?.FullName.Length > 1 || Employee4Name?.FullName.Length > 1 ||
                    //        Employee5Name?.FullName.Length > 1 || Employee6Name?.FullName.Length > 1;
                }
            );


            LunchOutCommand = new Command(
                execute: async () =>
                {
                    Console.WriteLine();
                    try
                    {

                        string curHHMM = DateTime.Now.ToString("H:m");
                        string curHour = DateTime.Now.ToString("%H");
                        string curMinute = DateTime.Now.ToString("%m");
                        Application.Current.Properties["LunchOutHH"] = curHour;
                        Application.Current.Properties["LunchOutMM"] = curMinute;


                        IsLunchOut = false;
                        IsLunchIn = true;
                        (LunchInCommand as Command).ChangeCanExecute();
                        (LunchOutCommand as Command).ChangeCanExecute();

                        // hide other display
                        IsDisplayCrewList = false;
                        IsDisplayEndOfDay= false;
                        IsDisplayOdo = false;
                        IsDisplayEndOfDayForm = false;
                        IsDisplayEndOfDay = false;

                        Console.WriteLine();
                        Session.event_type = "13"; // Lunch out
                        Console.WriteLine();

                        IsLunchOutDisplay = !IsLunchOutDisplay;
                        IsLunchInDisplay = false;

                        LunchOutTime = DateTime.Now.ToString("HH:mm");
                        await JobSaveEvent();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception LunchOutCommand " + e);
                    }

                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return !IsDisplayCrewList && IsLunchOut;
                }
            );

            LunchInCommand = new Command(
                execute: async () =>
                {

                    IsLunchOut = true;
                    IsLunchIn = false;


                    LunchInTime = DateTime.Now.ToString("HH:mm");
                    IsLunchOutDisplay = false;
                    IsLunchInDisplay = !IsLunchInDisplay;
                    (LunchInCommand as Command).ChangeCanExecute();
                    (LunchOutCommand as Command).ChangeCanExecute();

                    // hide other display
                    IsDisplayCrewList = false;
                    IsDisplayEndOfDay = false;
                    IsDisplayOdo = false;



                    Session.event_type = "14"; // Lunch in

                    await JobSaveEvent();

                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return IsLunchIn;
                }
            );

            SaveEndOfDayCommand = new Command(
                execute: async () =>
                {



                    Session.event_type = "16"; // ClockOut
                    await JobSaveEvent();

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
                    IsDisplayJobEntries = false;
                    IsDisplayEndOfDay = false;
                    IsDisplayEndOfDayForm = false;
                    IsLunchInDisplay = false;
                    IsLunchOutDisplay = false;

                    clrBkgndJob = Color.LightBlue;
                    clrBkgndCrew = Color.LightBlue;
                    clrBkgndECheckin = Color.LightBlue;
                    clrBkgndEChkOut = Color.LightBlue;
                    clrBkgndODO = Color.Green;
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return !IsDisplayCrewList && IsVerified;
                }


            );



            ODOSaveCommand = new Command(
                execute: async () =>
                {
                    IsBusy = true;
                    try
                    {
                        await CloudDBService.PostJobEvent(EntryOdometer);

                        /* Tab Navigation */
                        await Application.Current.MainPage.DisplayAlert("Job Event", "Job uploaded. Please Continue to Site Menu", "CLOSE");
                    }
                    catch
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Update JobEvent table failed. Check again internet connection", "CLOSE");
                    }
                    IsBusy = false;

                }

            );

            DisplayEquipmentCheckInCommand = new Command(
                execute: () =>
                {
                    //IsEqCheckInDisplayed = true;



                    clrBkgndJob = Color.LightBlue;
                    clrBkgndCrew = Color.LightBlue;
                    clrBkgndECheckin = Color.Green;
                    clrBkgndEChkOut = Color.LightBlue;
                    clrBkgndODO = Color.LightBlue;
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return !IsDisplayCrewList && IsVerified;
                }
            );

            SaveEqCheckInCommand = new Command(
                execute: () =>
                {
                    // do something
                    // goto equipment checkin

                    IsDisplayJobEntries = false;
                    IsDisplayCrewList = false;

                    IsEqCheckOutDisplayed = false;
                    IsEqCheckInDisplayed = false;
                    IsDisplayOdo = true;

                    ODOPopupCommand.Execute(null);
                }
            );

            DisplayEquipmentCheckOutCommand = new Command(
                execute: () =>
                {
                    // DisplayEquipmentCheckOut();

                    IsEqCheckOutDisplayed = true;
                    clrBkgndJob = Color.LightBlue;
                    clrBkgndCrew = Color.LightBlue;
                    clrBkgndECheckin = Color.LightBlue;
                    clrBkgndEChkOut = Color.Green;
                    clrBkgndODO = Color.LightBlue;
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return !IsDisplayCrewList &&  IsVerified;
                }
            );

            SaveEqCheckOutCommand = new Command(
                execute : () =>
                {
                    // do something
                    // goto equipment checkin

                    IsDisplayJobEntries = false;
                    IsDisplayCrewList = false;
                    IsDisplayEndOfDay = false;
                    IsDisplayEndOfDayForm = false;
                    IsLunchInDisplay = false;
                    IsLunchOutDisplay = false;
                    IsEqCheckInDisplayed = true;
                    IsEqCheckOutDisplayed = false;

                    DisplayEquipmentCheckInCommand.Execute(null);
                }
            );

            VerifyCommand = new Command(
                execute: async () => {
                    Session.event_type = Session.JOB_VERIFIED;
                    IsBusy = true;
                    try
                    {
                        await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
                        var speaker = DependencyService.Get<ITextToSpeech>();
                        speaker?.Speak("Job verified!");

                        Application.Current.Properties["PageNumber"] = 3;

                        IsVerified = true; // enable EqIn, EqOut, ODO input button
                        IsJobChanged = IsVerified; // condition when job changed
                        Session.IsVerified = true; // singleton session instance to notify Verify job done
                        //Verified = "Verified";

                        ToggleCrewListCommand?.Execute(null); // show Crew List 

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception : " + e.ToString());
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

        //[ObservableProperty]
        CrewInfoDetail selectedCrewForEndOfDay;
        public CrewInfoDetail SelectedCrewForEndOfDay
        {
            get => selectedCrewForEndOfDay;
            set
            {
                SetProperty(ref selectedCrewForEndOfDay, value);
                IsDisplayEndOfDayForm = true; // display the Finish the Day 
                ClockIntime = value.StartTime;
                ClockOutTime = DateTime.Now.ToString("HH:mm");


                try
                {
                    //check if LunchInTime > ClockOutTime
                    //TimeSpan duration1 = DateTime.Parse(LunchOutTime) > DateTime.Parse(ClockIntime) ? DateTime.Parse(LunchOutTime).Subtract(DateTime.Parse(ClockIntime)) : new TimeSpan(0, 0, 0);
                    //check if LunchInTime > ClockOutTime
                    //TimeSpan duration2 = DateTime.Parse(ClockOutTime) > DateTime.Parse(LunchInTime) ? DateTime.Parse(ClockOutTime).Subtract(DateTime.Parse(LunchInTime)) : new TimeSpan(0, 0, 0);

                    TimeSpan totaltime = DateTime.Parse(LunchOutTime).Subtract(DateTime.Parse(ClockIntime)) + DateTime.Parse(ClockOutTime).Subtract(DateTime.Parse(LunchInTime));

                    TotalHoursForToday = totaltime.ToString(@"hh\:mm\:ss");
                    //Console.WriteLine("TotalHoursForToday " + TotalHoursForToday);

                    Console.WriteLine();
                }
                catch
                {


                }
            }
        }

        public ObservableCollection<CrewInfoDetail> FinishDayList {
            get {
                return new ObservableCollection<CrewInfoDetail>(SelectedCrewInfoDetails);
            }
        }




        List<CrewInfoDetail> SelectedCrewInfoDetails = new List<CrewInfoDetail>();
        //[ObservableProperty] CrewInfoDetail employee1Name;
        //[ObservableProperty] CrewInfoDetail employee2Name;
        //[ObservableProperty] CrewInfoDetail employee3Name;
        [ObservableProperty] CrewInfoDetail employee4Name;
        [ObservableProperty] CrewInfoDetail employee5Name;
        [ObservableProperty] CrewInfoDetail employee6Name;
        private CrewInfoDetail employee1Name;
        public CrewInfoDetail Employee1Name
        {
            get => employee1Name;
            set
            {
                SetProperty(ref employee1Name, value);
                StartTimeEmp1 = DateTime.Now.ToString("HH:mm");
                (CrewSaveCommand as Command).ChangeCanExecute();
            }
        }

        private CrewInfoDetail employee2Name;
        public CrewInfoDetail Employee2Name
        {
            get => employee2Name;
            set
            {
                SetProperty(ref employee2Name, value);
                StartTimeEmp2 = DateTime.Now.ToString("HH:mm");
                (CrewSaveCommand as Command).ChangeCanExecute();
            }
        }

        private CrewInfoDetail employee3Name;
        public CrewInfoDetail Employee3Name
        {
            get => employee3Name;
            set
            {
                SetProperty(ref employee3Name, value);
                StartTimeEmp3 = DateTime.Now.ToString("HH:mm");
                (CrewSaveCommand as Command).ChangeCanExecute();
            }
        }

        [ObservableProperty] int employee1IsDriver = 0;
        [ObservableProperty] int employee2IsDriver = 0;
        [ObservableProperty] int employee3IsDriver = 0;
        [ObservableProperty] int employee4IsDriver = 0;
        [ObservableProperty] int employee5IsDriver = 0; 
        [ObservableProperty] int employee6IsDriver = 0;

        [ObservableProperty] string clockIntime ;

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



        async Task JobSaveEvent()
        {
            Console.WriteLine("JobSaveEvent");
            try
            {
                /*await LocationService.GetLocation();
                Session.accuracy = String.Format("{0:0.######}", LocationService.Coords.Accuracy);
                Session.live_longitude = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                Session.live_lattitude = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                Session.altitude = String.Format("{0:0.######}", LocationService.Coords.Altitude);*/

                //DateTime dt = DateTime.Parse(sStartTime.Trim());
                //int user_hours = int.Parse(dt.ToString("HH"));
                //int user_minutes = int.Parse(dt.ToString("mm"));

                DateTime curTime = DateTime.Now;
                int user_hours = curTime.Hour;
                int user_minutes = curTime.Minute;
                Console.WriteLine("HH " + user_hours);
                Console.WriteLine("MM " + user_minutes);
                //int user_hours = int.Parse(DateTime.Now.ToString("%HH"));
                //int user_minutes = int.Parse(DateTime.Now.ToString("%mm"));
                Console.WriteLine();
                await CloudDBService.PostJobEvent(user_hours.ToString(), user_minutes.ToString());

            }
            catch
            {

                await Application.Current.MainPage.DisplayAlert("Warning", "Time format must be HH:MM", "OK");
            }

        }

        [ObservableProperty] string selectedEqType;
        [ObservableProperty] string selectedEqDetailType;
        [ObservableProperty] string selectedEqDetailAsset;
        [ObservableProperty] EquipmentType selectedCheckInEq;
        [ObservableProperty] bool equipment1Checked = false;
        [ObservableProperty] bool equipment2Checked = false;
        [ObservableProperty] bool equipment3Checked = false;
        [ObservableProperty] bool equipment4Checked = false;
        [ObservableProperty] bool equipment5Checked = false;
        [ObservableProperty] bool equipment6Checked = false;


        /////////// Equipment Checkout ////////////
        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<EquipmentType>();
                    var table = conn.Table<EquipmentType>().ToList();
                    return new ObservableCollection<EquipmentType>(table);
                }
            }
        }

        public ObservableCollection<EquipmentDetailType> EquipmentDetailTypes
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<EquipmentDetailType>();
                    var selected = SelectedEqType.ToString();
                    var table = conn.Table<EquipmentDetailType>().Where(g => g.EquipmentType == SelectedEqType).ToList();
                    return new ObservableCollection<EquipmentDetailType>(table);
                }
            }
        }

        public ObservableCollection<EquipmentDetailType> EquipmentDetailAssets
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<EquipmentDetailType>();
                    var table = conn.Table<EquipmentDetailType>().OrderBy(a => a.EquipmentNumber).ToList();
                    return new ObservableCollection<EquipmentDetailType>(table);
                }
            }
        }

    }

    ////////////////////
    ///
}
