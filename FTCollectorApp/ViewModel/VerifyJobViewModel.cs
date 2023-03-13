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
                    Session.phases = QueryOwnerJobNumber.Select(a => a.JobPhases).First();
                    Session.JobShowAll = QueryOwnerJobNumber.Select(a => a.ShowAll).First();
                    Session.jobnum = value.JobNumber;
                    Session.OwnerName = value.OwnerName;

                    OnPropertyChanged(nameof(JobPhaseList));

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



        public ObservableCollection<string> JobPhaseList {
            get
            {
                var PhasesList = new List<string>();
                if (Session.phases != null)
                {
                    if (int.Parse(Session.phases) > 0)
                    {
                        for (int i = 0; i < int.Parse(Session.phases); i++)
                        {
                            PhasesList.Add(i.ToString());
                        }
                    }
                }
                return new ObservableCollection<string>(PhasesList);
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

        [ObservableProperty] string startTimeLeader;
        [ObservableProperty] string startTimeEmp1;
        [ObservableProperty] string startTimeEmp2;
        [ObservableProperty] string startTimeEmp3;
        [ObservableProperty] string startTimeEmp4;
        [ObservableProperty] string startTimeEmp5;
        [ObservableProperty] string startTimeEmp6;

        [ObservableProperty] string leaderPhase;
        [ObservableProperty] string employee1Phase;
        [ObservableProperty] string employee2Phase;
        [ObservableProperty] string employee3Phase;
        [ObservableProperty] string employee4Phase;
        [ObservableProperty] string employee5Phase;
        [ObservableProperty] string employee6Phase;

        [ObservableProperty] string errorMessageCrew = string.Empty;

        [ObservableProperty] int clockOutPerDiem;

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

            SaveTimeSheetCommand = new Command(
                execute: async () =>
                {
                    IsDisplayCrewList = false;
                    IsDisplayOdo = false;
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
            );*/

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
                    IsDisplayJobEntries = false;
                    IsDisplayCrewList = false;
                    IsEqCheckInDisplayed = false;
                    IsEqCheckOutDisplayed = false;
                    IsDisplayOdo = false;
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
                    return !IsDisplayCrewList;
                }

                );

            ToggleCrewListCommand = new Command(
                execute: () =>
                {
                    IsDisplayJobEntries = false;
                    IsDisplayOdo = false;
                    IsDisplayCrewList = !IsDisplayCrewList;
                    IsDisplayEndOfDay = false;
                    IsDisplayEndOfDayForm = false;
                    IsEqCheckInDisplayed = false;
                    IsEqCheckOutDisplayed = false;
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
                        if (StartTimeLeader.Length > 3 && StartTimeLeader.Length > 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 1, FullName = CrewLeader, TeamUserKey = Session.uid,
                                Phase = LeaderPhase,
                                StartTime = StartTimeLeader,
                            });

                            try {
                                await CloudDBService.PostTimeSheet(Session.uid.ToString(), StartTimeLeader, LeaderPhase, PerDiemEmp1);
                                ErrorMessageCrew = "";
                            }
                            catch
                            {
                                ErrorMessageCrew = "Invalid format (HH:MM)";
                                Console.WriteLine();
                            }
                        }


                        if (Employee1Name?.FullName.Length > 1 && StartTimeEmp1.Length > 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 2,
                                FullName = Employee1Name?.FullName,
                                TeamUserKey = Employee1Name.TeamUserKey,
                                StartTime = StartTimeEmp1, Phase = Employee1Phase, PerDiem = PerDiemEmp1
                            });

                            try
                            {
                                await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(),
                                    StartTimeEmp1, Employee1Phase, PerDiemEmp1);
                                ErrorMessageCrew = "";
                            }
                            catch
                            {
                                ErrorMessageCrew = "Invalid format (HH:MM)";
                                Console.WriteLine();
                            }
                        }

                        if (Employee2Name?.FullName.Length > 1 && StartTimeEmp2.Length > 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 3,
                                FullName = Employee2Name?.FullName, TeamUserKey = Employee2Name.TeamUserKey,
                                StartTime = StartTimeEmp2,
                                Phase = Employee2Phase, PerDiem = PerDiemEmp2

                            });
                            await CloudDBService.PostTimeSheet(Employee2Name?.TeamUserKey.ToString(),
                                StartTimeEmp2, Employee2Phase, PerDiemEmp2);

                        }

                        if (Employee3Name?.FullName.Length > 1 && StartTimeEmp3.Length > 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 4,
                                FullName = Employee3Name?.FullName,
                                TeamUserKey = Employee3Name.TeamUserKey,
                                StartTime = StartTimeEmp3,
                                Phase = Employee3Phase,
                                PerDiem = PerDiemEmp3

                            });
                            await CloudDBService.PostTimeSheet(Employee3Name?.TeamUserKey.ToString(),
                                StartTimeEmp3, Employee3Phase, PerDiemEmp3);

                        }

                        if (Employee4Name?.FullName.Length > 1 && StartTimeEmp4.Length > 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 5,
                                FullName = Employee4Name?.FullName,
                                TeamUserKey = Employee4Name.TeamUserKey,
                                StartTime = StartTimeEmp4,
                                Phase = Employee4Phase,
                                PerDiem = PerDiemEmp4

                            });
                            await CloudDBService.PostTimeSheet(Employee4Name?.TeamUserKey.ToString(), StartTimeEmp4, Employee4Phase, PerDiemEmp4);
                        }


                        if (Employee5Name?.FullName.Length > 1 && StartTimeEmp5.Length > 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 6,
                                FullName = Employee5Name?.FullName,
                                TeamUserKey = Employee5Name.TeamUserKey,
                                StartTime = StartTimeEmp5,
                                Phase = Employee5Phase,
                                PerDiem = PerDiemEmp5

                            });
                            await CloudDBService.PostTimeSheet(Employee5Name?.TeamUserKey.ToString(), StartTimeEmp5, Employee5Phase, PerDiemEmp5);
                        }

                        if (Employee6Name?.FullName.Length > 1 && StartTimeEmp6.Length > 3)
                        {
                            SelectedCrewInfoDetails.Add(new CrewInfoDetail
                            {
                                id = 6,
                                FullName = Employee6Name?.FullName,
                                TeamUserKey = Employee6Name.TeamUserKey,
                                StartTime = StartTimeEmp6,
                                Phase = Employee6Phase,
                                PerDiem = PerDiemEmp6

                            });
                            await CloudDBService.PostTimeSheet(Employee6Name?.TeamUserKey.ToString(),
                                StartTimeEmp6, Employee6Phase, PerDiemEmp6);
                        }


                        // Refresh FinishDay List
                        OnPropertyChanged(nameof(FinishDayList));


                        Session.event_type = "3";  //crew assembled
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

            SaveLunchOutCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = "13"; // Lunch out
                    //each employee
                    if (LunchOutTimeLeader.Length > 3)
                    {
                        var tmp = SelectedCrewInfoDetails.Where(a => a.id == 1).First();

                        await CloudDBService.PostTimeSheet(Session.uid.ToString(), LunchOutTimeLeader, "", 0);
                    }

                    if (Employee1Name?.FullName.Length > 1 && LunchOutTime1.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(), LunchOutTime1, "", 0);
                    }

                    if (Employee2Name?.FullName.Length > 1 && LunchOutTime2.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee2Name?.TeamUserKey.ToString(), LunchOutTime2, "", 0);
                    }

                    if (Employee3Name?.FullName.Length > 1 && LunchOutTime3.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee3Name?.TeamUserKey.ToString(), LunchOutTime3, "", 0);
                    }
                    if (Employee4Name?.FullName.Length > 1 && LunchOutTime4.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee4Name?.TeamUserKey.ToString(), LunchOutTime4, "", 0);
                    }
                    if (Employee5Name?.FullName.Length > 1 && LunchOutTime5.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee5Name?.TeamUserKey.ToString(), LunchOutTime5, "", 0);
                    }

                    if (Employee6Name?.FullName.Length > 1 && LunchOutTime6.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee6Name?.TeamUserKey.ToString(), LunchOutTime6, "", 0);
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
                        await CloudDBService.PostTimeSheet(Session.uid.ToString(), LunchInTimeLeader, "", 0);
                    }

                    if (Employee1Name?.FullName.Length > 1 && LunchInTime1.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee1Name.TeamUserKey.ToString(), LunchInTime1, "", 0);

                    }

                    if (Employee2Name?.FullName.Length > 1 && LunchInTime2.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee2Name.TeamUserKey.ToString(), LunchInTime2, "", 0);
                    }

                    if (Employee3Name?.FullName.Length > 1 && LunchInTime3.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee3Name.TeamUserKey.ToString(), LunchInTime3, "", 0);
                    }

                    if (Employee4Name?.FullName.Length > 1 && LunchInTime4.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee4Name.TeamUserKey.ToString(), LunchInTime4, "", 0);
                    }

                    if (Employee5Name?.FullName.Length > 1 && LunchInTime5.Length > 3)
                    {
                        await CloudDBService.PostTimeSheet(Employee5Name.TeamUserKey.ToString(), LunchInTime5, "", 0);
                    }


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
                            IsLunchIn = true;
                            (LunchInCommand as Command).ChangeCanExecute();
                            (LunchOutCommand as Command).ChangeCanExecute();

                            // hide other display
                            IsDisplayJobEntries = false;
                            IsDisplayCrewList = false;
                            IsEqCheckInDisplayed = false;
                            IsEqCheckOutDisplayed = false;
                            IsDisplayOdo = false;
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
                    IsDisplayOdo = false;
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
                    IsDisplayOdo = true;
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
                    IsDisplayOdo = false;
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
                    return !IsDisplayCrewList && IsVerified ;
                }
            );

            SaveEqCheckInCommand = new Command(
                execute: async () =>
                {
                    // do something
                    // goto equipment checkin

                    IsDisplayJobEntries = false;
                    IsDisplayCrewList = false;

                    IsEqCheckOutDisplayed = false;
                    IsEqCheckInDisplayed = false;
                    IsDisplayOdo = true;

                    //ODOPopupCommand.Execute(null);

                    Session.event_type = "10"; // Equipment out
                    await CloudDBService.PostJobEvent();
                    //await CloudDBService.PostTimeSheet(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), "12");

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

                    Session.event_type = "4"; // Equipment checkout
                    await CloudDBService.PostJobEvent();
                    //await CloudDBService.PostTimeSheet(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), "12");
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

        public ObservableCollection<CrewInfoDetail> FinishDayList {
            get {
                return new ObservableCollection<CrewInfoDetail>(SelectedCrewInfoDetails);
            }
        }
        List<CrewInfoDetail> SelectedCrewInfoDetails = new List<CrewInfoDetail>();

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

        private CrewInfoDetail employee4Name;
        public CrewInfoDetail Employee4Name
        {
            get => employee4Name;
            set
            {
                SetProperty(ref employee4Name, value);
                StartTimeEmp4 = DateTime.Now.ToString("HH:mm");
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
