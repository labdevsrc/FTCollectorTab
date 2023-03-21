using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using FTCollectorApp.Utils;
using FTCollectorApp.View;
using FTCollectorApp.View.BeginWork;
using FTCollectorApp.View.SitesPage;
using FTCollectorApp.View.SitesPage.Popup;
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
        public string VerifyStatusBadge
        {
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
        public bool IsVerified
        {
            get => isVerified;
            set
            {
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
                    Session.phases = QueryOwnerJobNumber.Select(a => a.JobPhases).First();
                    Session.JobShowAll = QueryOwnerJobNumber.Select(a => a.ShowAll).First();
                    Session.jobnum = value.JobNumber;
                    Session.OwnerName = value.OwnerName;

                    OnPropertyChanged(nameof(JobPhaseDetailList));

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



        /*public ObservableCollection<string> JobPhaseList {
            get
            {
                var PhasesList = new List<string>();
                if (Session.phases != null)
                {
                    if (int.Parse(Session.phases) > 0)
                    {
                        for (int i = 1; i <= int.Parse(Session.phases); i++)
                        {
                            PhasesList.Add(i.ToString());
                        }
                    }
                }
                return new ObservableCollection<string>(PhasesList);
            }
        }*/



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
                    if (SelectedOwner?.OwnerName != null)
                    {
                        Console.WriteLine();
                        table = conn.Table<Job>().Where(a => a.OwnerName == SelectedOwner.OwnerName).GroupBy(b => b.JobNumber).Select(g => g.First()).ToList();
                        Console.WriteLine();
                    }
                    return new ObservableCollection<Job>(table);
                }
            }
        }

        private ObservableCollection<Job> _jobdetails = new ObservableCollection<Job>();
        private ObservableCollection<EquipmentCO> _equipmentCO = new ObservableCollection<EquipmentCO>();
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

        public ICommand SaveTimeSheetCommand { get; set; }
        public ICommand CrewSaveCommand { get; set; }
        public ICommand SaveLunchInCommand { get; set; }
        public ICommand SaveLunchOutCommand { get; set; }
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

        [ObservableProperty] bool isDisplayOdoStart = false;
        [ObservableProperty] bool isDisplayOdoEnd = false;

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

        [ObservableProperty] string selectedCrewForEq;
        [ObservableProperty] string crewEqIndex;


        [ObservableProperty] int perDiemEmp1 = 0;
        [ObservableProperty] int perDiemEmp2 = 0;
        [ObservableProperty] int perDiemEmp3 = 0;
        [ObservableProperty] int perDiemEmp4 = 0;
        [ObservableProperty] int perDiemEmp5 = 0;
        [ObservableProperty] int perDiemEmp6 = 0;

        [ObservableProperty] string startTimeLeader = string.Empty;
        [ObservableProperty] string startTimeEmp1 = string.Empty;
        [ObservableProperty] string startTimeEmp2 = string.Empty;
        [ObservableProperty] string startTimeEmp3 = string.Empty;
        [ObservableProperty] string startTimeEmp4 = string.Empty;
        [ObservableProperty] string startTimeEmp5 = string.Empty;
        [ObservableProperty] string startTimeEmp6 = string.Empty;

        public bool IsValidTimeFormat(string input)
        {
            TimeSpan dummyOutput;
            return TimeSpan.TryParse(input, out dummyOutput);
        }

        [ObservableProperty] JobPhaseDetail selectedPhase;

        [ObservableProperty] string errorMessageCrew = string.Empty;

        [ObservableProperty] int clockOutPerDiem;
        [ObservableProperty] bool isStartODOEntered = false;

        void DisplayJobEntry()
        {
            IsDisplayCrewList = false;
            IsDisplayOdoStart = false;
            IsDisplayOdoEnd = false;
            IsDisplayEndOfDayForm = false;
            IsDisplayEndOfDay = false;
            IsDisplayJobEntries = true;
            IsLunchOutDisplay = false;
            IsLunchInDisplay = false;
        }

        [ObservableProperty] string milesHour1 = string.Empty;
        [ObservableProperty] string milesHour2 = string.Empty;
        [ObservableProperty] string milesHour3 = string.Empty;


        async void LoginPopUpCall()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new LoginPopUp());
        }
        public VerifyJobViewModel()
        {
            bool SomethingWrong = false;

            LoginPopUpCall();


            MessagingCenter.Subscribe<LoginPopUpVM>(this, "LoginToVerifyJobCh", (sender) =>
            {

                DisplayJobEntry();

                OnPropertyChanged(nameof(OwnerList));
                OnPropertyChanged(nameof(JobNumbers));
                OnPropertyChanged(nameof(JobPhaseDetailList));
                CrewLeader = Session.crew_leader;
                Console.WriteLine();
            });



            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                try
                {

                    conn.CreateTable<Job>();
                    var jobdetails = conn.Table<Job>().ToList();
                    _jobdetails = new ObservableCollection<Job>(jobdetails);

                    conn.CreateTable<EquipmentCO>();
                    var equipmentco = conn.Table<EquipmentCO>().ToList();
                    Console.WriteLine();
                    _equipmentCO = new ObservableCollection<EquipmentCO>(equipmentco);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }
            Console.WriteLine();

            GPSSettingCommand = new Command(() => DisplayGPSSettingCommand());

            SaveTimeSheetCommand = new Command(
                execute: async () =>
                {
                    IsDisplayCrewList = false;
                    IsDisplayOdoStart = false;
                    IsDisplayOdoEnd = false;
                    IsDisplayJobEntries = false;
                    IsLunchOutDisplay = false;
                    IsLunchInDisplay = false;

                    // display both so user can select which crew 
                    IsDisplayEndOfDay = true;
                    //IsDisplayEndOfDayForm = true;

                    ClockIntime = SelectedCrewForEndOfDay.StartTime;
                    ClockOutTimeForm = ClockOutTime;

                    if (SelectedCrewForEndOfDay.id == 1)
                    {
                        LunchInTime = LunchInTimeLeader;
                        LunchOutTime = LunchOutTimeLeader;
                    }
                    else if (SelectedCrewForEndOfDay.id == 2)
                    {
                        LunchInTime = LunchInTime1;
                        LunchOutTime = LunchOutTime1;
                    }
                    else if (SelectedCrewForEndOfDay.id == 3)
                    {
                        LunchInTime = LunchInTime2;
                        LunchOutTime = LunchOutTime2;
                    }
                    else if (SelectedCrewForEndOfDay.id == 4)
                    {
                        LunchInTime = LunchInTime3;
                        LunchOutTime = LunchOutTime3;
                    }
                    else if (SelectedCrewForEndOfDay.id == 5)
                    {
                        LunchInTime = LunchInTime4;
                        LunchOutTime = LunchOutTime4;
                    }
                    else if (SelectedCrewForEndOfDay.id == 6)
                    {
                        LunchInTime = LunchInTime5;
                        LunchOutTime = LunchOutTime5;
                    }
                    else if (SelectedCrewForEndOfDay.id == 7)
                    {
                        LunchInTime = LunchInTime6;
                        LunchOutTime = LunchOutTime6;
                    }



                    // upload to timesheet Clockout
                    Session.event_type = "16";
                    if (ClockOutTime.Length > 3)
                        await CloudDBService.PostTimeSheet(SelectedCrewForEndOfDay.TeamUserKey.ToString(),
                        ClockOutTime, "", ClockOutPerDiem);
                    Console.WriteLine();
                    try
                    {
                        TimeSpan totaltime = DateTime.Parse(LunchOutTime).Subtract(DateTime.Parse(ClockIntime)) + DateTime.Parse(ClockOutTime).Subtract(DateTime.Parse(LunchInTime));
                        TotalHoursForToday = totaltime.ToString(@"hh\:mm\:ss");
                        await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new TimeSheetSignature(
                            SelectedCrewForEndOfDay.FullName.ToString(), SelectedCrewForEndOfDay.TeamUserKey.ToString(),
                            ClockIntime, ClockOutTimeForm, LunchInTime, LunchOutTime, TotalHoursForToday));

                        Console.WriteLine();
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e);
                    }

                },
                canExecute: () =>
                {
                    return true;
                }
            );

            /*MoveToSitePageCommand = new Command(
                execute: () =>
                {
                    var tabbedPage = this.Parent as TabbedPage;
                    tabbedPage.CurrentPage = tabbedPage.Children[1];
                }
            );

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
            );*/

            ToggleEndofDayCommand = new Command(
                execute: () =>
                {
                    IsDisplayJobEntries = false;
                    IsDisplayCrewList = false;
                    IsEqCheckInDisplayed = false;
                    IsEqCheckOutDisplayed = false;
                    IsDisplayOdoStart = false;
                    IsDisplayOdoEnd = false;

                    IsLunchInDisplay = false;
                    IsLunchOutDisplay = false;
                    IsDisplayEndOfDay = true;

                    (ToggleCrewListCommand as Command).ChangeCanExecute();
                    (DisplayEquipmentCheckOutCommand as Command).ChangeCanExecute();
                    //calculate from Clock in to Lunch out

                    //check if clockintime > lunchout
                    Console.WriteLine();

                },
                canExecute: () =>
                {
                    return !IsLunchIn;
                }

                );


            ToggleCrewListCommand = new Command(
                execute: () =>
                {
                    IsDisplayJobEntries = false;
                    IsDisplayOdoStart = false;
                    IsDisplayOdoEnd = false;

                    IsDisplayCrewList = true;
                    IsDisplayEndOfDay = false;
                    IsDisplayEndOfDayForm = false;
                    IsEqCheckInDisplayed = false;
                    IsEqCheckOutDisplayed = false;
                    IsLunchInDisplay = false;
                    IsLunchOutDisplay = false;

                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return IsVerified && !IsDisplayEndOfDay;
                }
            );

            CrewSaveCommand = new Command(
                execute: async () =>
                {
                    Console.WriteLine();
                    try
                    {
                        string curHHMM = DateTime.Now.ToString("HM:mm");
                        string curHour = DateTime.Now.ToString("HH");
                        string curMinute = DateTime.Now.ToString("mm");
                        // preserve LunchOut time here
                        Console.WriteLine();


                        // ClockIn , event Type 15
                        Session.event_type = "15";

                        // Clear each time save 
                        if (SelectedCrewInfoDetails.Count > 1)
                            SelectedCrewInfoDetails.Clear();
                        Console.WriteLine();

                        // Put to list view
                        if (StartTimeLeader.Length >= 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 1,
                                FullName = CrewLeader,
                                TeamUserKey = Session.uid,
                                Phase = SelectedPhase.PhaseNumber,
                                StartTime = StartTimeLeader,
                            });

                            try
                            {
                                await CloudDBService.PostTimeSheet(Session.uid.ToString(), StartTimeLeader, SelectedPhase.PhaseNumber, PerDiemEmp1);
                            }
                            catch
                            {
                                Console.WriteLine();
                            }
                        }


                        if (Employee1Name?.FullName.Length > 1 && StartTimeEmp1.Length >= 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 2,
                                FullName = Employee1Name?.FullName,
                                TeamUserKey = Employee1Name.TeamUserKey,
                                StartTime = StartTimeEmp1,
                                Phase = SelectedPhase.PhaseNumber,
                                PerDiem = PerDiemEmp1
                            });

                            try
                            {
                                await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(),
                                    StartTimeEmp1, SelectedPhase.PhaseNumber, PerDiemEmp1);
                            }
                            catch
                            {
                                Console.WriteLine();
                            }
                        }

                        if (Employee2Name?.FullName.Length > 1 && StartTimeEmp2.Length >= 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 3,
                                FullName = Employee2Name?.FullName,
                                TeamUserKey = Employee2Name.TeamUserKey,
                                StartTime = StartTimeEmp2,
                                Phase = SelectedPhase.PhaseNumber,
                                PerDiem = PerDiemEmp2

                            });
                            await CloudDBService.PostTimeSheet(Employee2Name?.TeamUserKey.ToString(),
                                StartTimeEmp2, SelectedPhase.PhaseNumber, PerDiemEmp2);

                        }

                        if (Employee3Name?.FullName.Length > 1 && StartTimeEmp3.Length >= 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 4,
                                FullName = Employee3Name?.FullName,
                                TeamUserKey = Employee3Name.TeamUserKey,
                                StartTime = StartTimeEmp3,
                                Phase = SelectedPhase.PhaseNumber,
                                PerDiem = PerDiemEmp3

                            });
                            await CloudDBService.PostTimeSheet(Employee3Name?.TeamUserKey.ToString(),
                                StartTimeEmp3, SelectedPhase.PhaseNumber, PerDiemEmp3);

                        }

                        if (Employee4Name?.FullName.Length > 1 && StartTimeEmp4.Length >= 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 5,
                                FullName = Employee4Name?.FullName,
                                TeamUserKey = Employee4Name.TeamUserKey,
                                StartTime = StartTimeEmp4,
                                Phase = SelectedPhase.PhaseNumber,
                                PerDiem = PerDiemEmp4

                            });
                            await CloudDBService.PostTimeSheet(Employee4Name?.TeamUserKey.ToString(), StartTimeEmp4, SelectedPhase.PhaseNumber, PerDiemEmp4);
                        }


                        if (Employee5Name?.FullName.Length > 1 && StartTimeEmp5.Length >= 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 6,
                                FullName = Employee5Name?.FullName,
                                TeamUserKey = Employee5Name.TeamUserKey,
                                StartTime = StartTimeEmp5,
                                Phase = SelectedPhase.PhaseNumber,
                                PerDiem = PerDiemEmp5

                            });
                            await CloudDBService.PostTimeSheet(Employee5Name?.TeamUserKey.ToString(), StartTimeEmp5, SelectedPhase.PhaseNumber, PerDiemEmp5);
                        }

                        if (Employee6Name?.FullName.Length > 1 && StartTimeEmp6.Length >= 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 6,
                                FullName = Employee6Name?.FullName,
                                TeamUserKey = Employee6Name.TeamUserKey,
                                StartTime = StartTimeEmp6,
                                Phase = SelectedPhase.PhaseNumber,
                                PerDiem = PerDiemEmp6

                            });
                            await CloudDBService.PostTimeSheet(Employee6Name?.TeamUserKey.ToString(),
                                StartTimeEmp6, SelectedPhase.PhaseNumber, PerDiemEmp6);
                        }


                        // Refresh FinishDay List
                        OnPropertyChanged(nameof(FinishDayList));


                        Session.event_type = "3";  //crew assembled
                        await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), SelectedPhase.PhaseNumber);
                        //await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
                        await CloudDBService.SaveCrewdata(Session.ownerCD, SelectedPhase.PhaseNumber,
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
                    //return true;
                    return Employee1Name?.FullName.Length > 1 || Employee2Name?.FullName.Length > 1 ||
                            Employee3Name?.FullName.Length > 1 || Employee4Name?.FullName.Length > 1 ||
                            Employee5Name?.FullName.Length > 1 || Employee6Name?.FullName.Length > 1;
                }
            );

            SaveLunchOutCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = "13"; // Lunch out
                    //each employee
                    if (LunchOutTimeLeader.Length > 3)
                    {
                        var tmp = SelectedCrewInfoDetails.Where(a => a.id == 1).First();

                        await CloudDBService.PostTimeSheet(Session.uid.ToString(), LunchOutTimeLeader, SelectedPhase.PhaseNumber, 0);
                    }

                    if (Employee1Name?.FullName.Length > 1 && LunchOutTime1.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(), LunchOutTime1, SelectedPhase.PhaseNumber, 0);
                    }

                    if (Employee2Name?.FullName.Length > 1 && LunchOutTime2.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee2Name?.TeamUserKey.ToString(), LunchOutTime2, SelectedPhase.PhaseNumber, 0);
                    }

                    if (Employee3Name?.FullName.Length > 1 && LunchOutTime3.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee3Name?.TeamUserKey.ToString(), LunchOutTime3, SelectedPhase.PhaseNumber, 0);
                    }
                    if (Employee4Name?.FullName.Length > 1 && LunchOutTime4.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee4Name?.TeamUserKey.ToString(), LunchOutTime4, SelectedPhase.PhaseNumber, 0);
                    }
                    if (Employee5Name?.FullName.Length > 1 && LunchOutTime5.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee5Name?.TeamUserKey.ToString(), LunchOutTime5, SelectedPhase.PhaseNumber, 0);
                    }

                    if (Employee6Name?.FullName.Length > 1 && LunchOutTime6.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee6Name?.TeamUserKey.ToString(), LunchOutTime6, SelectedPhase.PhaseNumber, 0);
                    }

                },
                canExecute: () =>
                {
                    return true;
                }
            );

            SaveLunchInCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = "14"; // Lunch in
                    //each employee
                    if (LunchInTimeLeader.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Session.uid.ToString(), LunchInTimeLeader, SelectedPhase.PhaseNumber, 0);
                    }

                    if (Employee1Name?.FullName.Length > 1 && LunchInTime1.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee1Name.TeamUserKey.ToString(), LunchInTime1, SelectedPhase.PhaseNumber, 0);

                    }

                    if (Employee2Name?.FullName.Length > 1 && LunchInTime2.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee2Name.TeamUserKey.ToString(), LunchInTime2, SelectedPhase.PhaseNumber, 0);
                    }

                    if (Employee3Name?.FullName.Length > 1 && LunchInTime3.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee3Name.TeamUserKey.ToString(), LunchInTime3, SelectedPhase.PhaseNumber, 0);
                    }

                    if (Employee4Name?.FullName.Length > 1 && LunchInTime4.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee4Name.TeamUserKey.ToString(), LunchInTime4, SelectedPhase.PhaseNumber, 0);
                    }

                    if (Employee5Name?.FullName.Length > 1 && LunchInTime5.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee5Name.TeamUserKey.ToString(), LunchInTime5, SelectedPhase.PhaseNumber, 0);
                    }

                    IsLunchIn = false; // end of Lunch in, refresh Equipment in & Endof day button
                    (DisplayEquipmentCheckInCommand as Command).ChangeCanExecute();
                    (ToggleEndofDayCommand as Command).ChangeCanExecute();
                    (ArriveFromSiteCommand as Command).ChangeCanExecute();

                },
                canExecute: () =>
                {
                    return true;
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
                            IsLunchIn = true; // when lunchout button clicked, it will enable Lunchin button
                            (LunchInCommand as Command).ChangeCanExecute();
                            (LunchOutCommand as Command).ChangeCanExecute();
                            (DisplayEquipmentCheckInCommand as Command).ChangeCanExecute();
                            (ToggleEndofDayCommand as Command).ChangeCanExecute();

                            // hide other display
                            IsDisplayJobEntries = false;
                            IsDisplayCrewList = false;
                            IsEqCheckInDisplayed = false;
                            IsEqCheckOutDisplayed = false;
                            IsDisplayOdoStart = false;
                            IsDisplayOdoEnd = false;
                            IsDisplayEndOfDayForm = false;
                            IsDisplayEndOfDay = false;
                            IsLunchInDisplay = false;
                            IsLunchOutDisplay = true;


                            var lotime = DateTime.Now; //.ToString("HH:mm");
                            LunchOutTime = DateTime.Now.ToString("HH:mm");
                            Session.event_type = "13"; // Lunch out

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

                    // hide other display
                    IsDisplayJobEntries = false;
                    IsDisplayCrewList = false;
                    IsEqCheckInDisplayed = false;
                    IsEqCheckOutDisplayed = false;
                    IsDisplayOdoStart = false;
                    IsDisplayOdoEnd = false;

                    IsDisplayEndOfDay = false;
                    IsDisplayEndOfDayForm = false;
                    IsLunchOutDisplay = false;
                    IsLunchInDisplay = true;


                    LunchInTime = DateTime.Now.ToString("HH:mm");

                    IsLunchIn = false;
                    (LunchInCommand as Command).ChangeCanExecute();

                    IsLunchOut = true; // enable button LuncOut
                    (LunchOutCommand as Command).ChangeCanExecute();


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

                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return true;
                }
            );

            ODOPopupCommand = new Command(
                execute: async () =>
                {
                    IsDisplayOdoStart = !IsStartODOEntered;
                    IsDisplayOdoEnd = IsStartODOEntered;
                    IsDisplayCrewList = false;
                    IsDisplayJobEntries = false;
                    IsDisplayEndOfDay = false;
                    IsDisplayEndOfDayForm = false;
                    IsLunchInDisplay = false;
                    IsLunchOutDisplay = false;

                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return true; // !IsDisplayCrewList && IsVerified;
                }


            );



            ODOSaveCommand = new Command(
                execute: async () =>
                {
                    IsBusy = true;
                    try
                    {
                        Session.event_type = "8";
                        await CloudDBService.PostJobEvent(EntryOdometer);

                        /* Tab Navigation */
                        //await Application.Current.MainPage.DisplayAlert("Job Event", "Job uploaded. Please Continue to Site Menu", "CLOSE");
                    }
                    catch
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Update JobEvent table failed. Check again internet connection", "CLOSE");
                    }
                    IsBusy = false;
                    IsStartODOEntered = true;
                    IsDisplayOdoStart = false;
                    IsDisplayOdoEnd = false;

                }

            );

            DisplayEquipmentCheckInCommand = new Command(
                execute: () =>
                {
                    IsDisplayCrewList = false;
                    IsDisplayJobEntries = false;
                    IsDisplayEndOfDay = false;
                    IsDisplayEndOfDayForm = false;
                    IsLunchInDisplay = false;
                    IsLunchOutDisplay = false;
                    IsDisplayOdoStart = false;
                    IsDisplayOdoEnd = false;
                    IsEqCheckOutDisplayed = false;
                    IsEqCheckInDisplayed = true;

                    clrBkgndJob = Color.LightBlue;
                    clrBkgndCrew = Color.LightBlue;
                    clrBkgndECheckin = Color.Green;
                    clrBkgndEChkOut = Color.LightBlue;
                    clrBkgndODO = Color.LightBlue;
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return !IsLunchIn;
                }
            );

            SaveEqCheckInCommand = new Command(
                execute: async () =>
                {
                    // do something
                    // goto equipment checkin

                    try
                    {
                        if (SelectedEqIn1?.EqDesc.Length > 1)
                        {
                            //await CloudDBService.PostTimeSheet(Session.uid.ToString(), LunchOutTimeLeader, SelectedPhase, 0);
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq1?.EqKey, MilesHourIn1, "0");

                        }
                        if (SelectedEqIn2?.EqDesc.Length > 1)
                        {
                            //await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(), LunchOutTime1, SelectedPhase, 0);
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq2?.EqKey, MilesHourIn2, "0");
                        }
                        if (SelectedEqIn3?.EqDesc.Length > 1)
                        {
                            //await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(), LunchOutTime1, SelectedPhase, 0);
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq3?.EqKey, MilesHourIn3, "0");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }
            );

            DisplayEquipmentCheckOutCommand = new Command(
                execute: () =>
                {
                    // DisplayEquipmentCheckOut();
                    IsDisplayJobEntries = false;
                    IsDisplayCrewList = false;
                    IsDisplayEndOfDay = false;
                    IsDisplayEndOfDayForm = false;
                    IsLunchInDisplay = false;
                    IsLunchOutDisplay = false;
                    IsEqCheckInDisplayed = false;
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
                    return !IsDisplayCrewList && IsVerified && !IsDisplayEndOfDay;
                }
            );

            SaveEqCheckOutCommand = new Command(
                execute: async () =>
                {
                    // do something
                    // goto equipment checkin


                    IsEqCheckOutDisplayed = false;
                    (ODOPopupCommand as Command).ChangeCanExecute();
                    ODOPopupCommand.Execute(null);

                    Session.event_type = "4"; // Equipment out
                    await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), SelectedPhase.PhaseNumber);

                    Session.event_type = "8"; // Odo
                    await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), SelectedPhase.PhaseNumber);

                    // start ODO entered , enable all icon
                    IsStartODOEntered = true;
                    IsDisplayOdoStart = false;
                    IsDisplayOdoEnd = false;
                    IsLeaveToSiteBtnEnabled = true;
                    (LeaveJobToSiteCommand as Command).ChangeCanExecute();

                    try
                    {
                        if (SelectedEq1?.EqDesc.Length > 1)
                        {
                            //await CloudDBService.PostTimeSheet(Session.uid.ToString(), LunchOutTimeLeader, SelectedPhase, 0);
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq1?.EqKey, "0", MilesHour1);

                        }
                        if (SelectedEq2?.EqDesc.Length > 1)
                        {
                            //await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(), LunchOutTime1, SelectedPhase, 0);
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq2?.EqKey, "0", MilesHour2);
                        }
                        if (SelectedEq3?.EqDesc.Length > 1)
                        {
                            //await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(), LunchOutTime1, SelectedPhase, 0);
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq3?.EqKey, "0", MilesHour3);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    //await CloudDBService.PostTimeSheet(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), "12");
                }
            );

            VerifyCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = Session.JOB_VERIFIED;
                    IsBusy = true;
                    try
                    {
                        await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), SelectedPhase.PhaseNumber);
                        var speaker = DependencyService.Get<ITextToSpeech>();
                        speaker?.Speak("Job verified!");

                        Application.Current.Properties["PageNumber"] = 3;

                        IsVerified = true; // enable EqIn, EqOut, ODO input button
                        IsJobChanged = IsVerified; // condition when job changed
                        Session.IsVerified = true; // singleton session instance to notify Verify job done

                        ToggleCrewListCommand?.Execute(null); // obsolete - CrewList Button removed - Mar 18th

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

            LeaveJobToSiteCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = "6"; //left for job site
                    await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), SelectedPhase.PhaseNumber);
                    IsLeaveToSiteBtnEnabled = false;
                    IsArrivedFromSiteBtnEnabled = true;
                    (ArriveFromSiteCommand as Command).ChangeCanExecute();
                },
                canExecute: () => {

                    return IsStartODOEntered;
                }
            );

            ArriveFromSiteCommand = new Command(
                execute: async  () =>
                {
                    Session.event_type = "7"; //left for job site
                    await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), SelectedPhase.PhaseNumber);
                    IsArrivedFromSiteBtnEnabled = false;
                    IsLeaveToSiteBtnEnabled = false;
                    (LeaveJobToSiteCommand as Command).ChangeCanExecute();
                },
                canExecute: () => {

                    return IsLunchIn;
                }
            );


        }

        [ObservableProperty] bool isArrivedFromSiteBtnEnabled = true;
        [ObservableProperty] bool isLeaveToSiteBtnEnabled = false;

        public ICommand LeaveJobToSiteCommand { get; set; }
        public ICommand ArriveFromSiteCommand { get; set; }


        [ObservableProperty] string lunchInTimeLeader = string.Empty;
        [ObservableProperty] string lunchInTime1 = string.Empty;
        [ObservableProperty] string lunchInTime2 = string.Empty;
        [ObservableProperty] string lunchInTime3 = string.Empty;
        [ObservableProperty] string lunchInTime4 = string.Empty;
        [ObservableProperty] string lunchInTime5 = string.Empty;
        [ObservableProperty] string lunchInTime6 = string.Empty;


        [ObservableProperty] string lunchOutTimeLeader = string.Empty;
        [ObservableProperty] string lunchOutTime1 = string.Empty;
        [ObservableProperty] string lunchOutTime2 = string.Empty;
        [ObservableProperty] string lunchOutTime3 = string.Empty;
        [ObservableProperty] string lunchOutTime4 = string.Empty;
        [ObservableProperty] string lunchOutTime5 = string.Empty;
        [ObservableProperty] string lunchOutTime6 = string.Empty;

        [ObservableProperty] string clockOutTimeForm = string.Empty;

        public ICommand SaveAndDisplaySignForm { get; set; }

        //[ObservableProperty]
        CrewInfoDetail selectedCrewForEndOfDay;
        public CrewInfoDetail SelectedCrewForEndOfDay
        {
            get => selectedCrewForEndOfDay;
            set
            {
                SetProperty(ref selectedCrewForEndOfDay, value);

                // clear entries on form
                ClockIntime = string.Empty;
                ClockOutTime = DateTime.Now.ToString("HH:mm");
                ClockOutTimeForm = string.Empty;
                LunchInTime = string.Empty;
                LunchOutTime = string.Empty;
                TotalHoursForToday = string.Empty;

                IsDisplayEndOfDayForm = false;

            }
        }

        public ObservableCollection<CrewInfoDetail> FinishDayList
        {
            get
            {
                return new ObservableCollection<CrewInfoDetail>(SelectedCrewInfoDetails);
            }
        }
        List<CrewInfoDetail> SelectedCrewInfoDetails = new List<CrewInfoDetail>();


        //[ObservableProperty] CrewInfoDetail employee1Name;
        private CrewInfoDetail employee1Name;
        public CrewInfoDetail Employee1Name
        {
            get => employee1Name;
            set
            {
                SetProperty(ref employee1Name, value);
                //StartTimeEmp1 = DateTime.Now.ToString("HH:mm");
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
                //StartTimeEmp2 = DateTime.Now.ToString("HH:mm");
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
                //StartTimeEmp3 = DateTime.Now.ToString("HH:mm");
                (CrewSaveCommand as Command).ChangeCanExecute();
            }
        }

        private CrewInfoDetail employee4Name;
        public CrewInfoDetail Employee4Name
        {
            get => employee4Name;
            set
            {
                SetProperty(ref employee4Name, value);
                //StartTimeEmp4 = DateTime.Now.ToString("HH:mm");
                (CrewSaveCommand as Command).ChangeCanExecute();
            }
        }
        private CrewInfoDetail employee5Name;
        public CrewInfoDetail Employee5Name
        {
            get => employee5Name;
            set
            {
                SetProperty(ref employee5Name, value);
                //StartTimeEmp4 = DateTime.Now.ToString("HH:mm");
                (CrewSaveCommand as Command).ChangeCanExecute();
            }
        }

        private CrewInfoDetail employee6Name;
        public CrewInfoDetail Employee6Name
        {
            get => employee6Name;
            set
            {
                SetProperty(ref employee6Name, value);
                //StartTimeEmp4 = DateTime.Now.ToString("HH:mm");
                (CrewSaveCommand as Command).ChangeCanExecute();
            }
        }

        [ObservableProperty] int employee1IsDriver = 0;
        [ObservableProperty] int employee2IsDriver = 0;
        [ObservableProperty] int employee3IsDriver = 0;
        [ObservableProperty] int employee4IsDriver = 0;
        [ObservableProperty] int employee5IsDriver = 0;
        [ObservableProperty] int employee6IsDriver = 0;

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



        async Task JobSaveEvent()
        {
            Console.WriteLine("JobSaveEvent");
            try
            {
                await LocationService.GetLocation();
                Session.accuracy = String.Format("{0:0.######}", LocationService.Coords.Accuracy);
                Session.live_longitude = String.Format("{0:0.######}", LocationService.Coords.Longitude);
                Session.live_lattitude = String.Format("{0:0.######}", LocationService.Coords.Latitude);
                Session.altitude = String.Format("{0:0.######}", LocationService.Coords.Altitude);

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



        public ObservableCollection<JobPhaseDetail> JobPhaseDetailList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<JobPhaseDetail>();
                    var table = conn.Table<JobPhaseDetail>().ToList();

                    var PhaseDescList = new List<JobPhaseDetail>();
                    if (Session.phases != null)
                    {
                        foreach (var col in table)
                        {
                            if (int.Parse(col.PhaseNumber) <= int.Parse(Session.phases))
                            {
                                col.NumDesc = col.PhaseNumber + " " + col.Description;
                                PhaseDescList.Add(col);
                            }
                        }
                    }
                    Console.WriteLine();
                    return new ObservableCollection<JobPhaseDetail>(PhaseDescList);

                    // max : job.phase
                    //foreach (var col in table)
                    //{
                    //    col.NumDesc = col.PhaseNumber + " " + col.Description;
                    //}
                    //return new ObservableCollection<JobPhaseDetail>(table);
                }
            }
        }



        /// Equipment 

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment1CO))] EquipmentType selectedEq1Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment2CO))] EquipmentType selectedEq2Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment3CO))] EquipmentType selectedEq3Type;


        [ObservableProperty] EquipmentCO selectedEq1;
        [ObservableProperty] EquipmentCO selectedEq2;
        [ObservableProperty] EquipmentCO selectedEq3;


        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment1CI))] EquipmentType selectedEqIn1Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment2CI))] EquipmentType selectedEqIn2Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment3CI))] EquipmentType selectedEqIn3Type;


        [ObservableProperty] EquipmentCO selectedEqIn1;
        [ObservableProperty] EquipmentCO selectedEqIn2;
        [ObservableProperty] EquipmentCO selectedEqIn3;

        [ObservableProperty] string milesHourIn1;
        [ObservableProperty] string milesHourIn2;
        [ObservableProperty] string milesHourIn3;

        [ObservableProperty] EquipmentType selectedCheckInEq;

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
        public ObservableCollection<EquipmentCO> Equipment1CO
        {
            get
            {
                var table = _equipmentCO.ToList();
                Console.WriteLine();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEq1Type?.EquipCodeKey).ToList();
                    Console.WriteLine();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        public ObservableCollection<EquipmentCO> Equipment2CO
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEq2Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        public ObservableCollection<EquipmentCO> Equipment3CO
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEq3Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        public ObservableCollection<EquipmentCO> Equipment1CI
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEqIn1Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        public ObservableCollection<EquipmentCO> Equipment2CI
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEqIn2Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        public ObservableCollection<EquipmentCO> Equipment3CI
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEqIn3Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

    }
        ////////////////////
        ///
    }
