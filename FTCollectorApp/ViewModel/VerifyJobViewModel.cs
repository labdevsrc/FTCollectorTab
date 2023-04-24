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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Converters;
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
                    Session.max_phases = QueryOwnerJobNumber.Select(a => a.JobPhases).First();
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
        public ICommand SaveTimeSheetLCommand { get; set; }
        public ICommand SaveTimeSheet1Command { get; set; }
        public ICommand SaveTimeSheet2Command { get; set; }
        public ICommand SaveTimeSheet3Command { get; set; }
        public ICommand SaveTimeSheet4Command { get; set; }
        public ICommand SaveTimeSheet5Command { get; set; }
        public ICommand SaveTimeSheet6Command { get; set; }
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

        [ObservableProperty] bool isLunchIn = false;

        [ObservableProperty] bool isEqCheckIn = false;

        [ObservableProperty] bool isLunchOutDisplay = false;

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

        [ObservableProperty] int perDiemL = 0; // 20230331
        [ObservableProperty] int perDiemEmp1 = 0;
        [ObservableProperty] int perDiemEmp2 = 0;
        [ObservableProperty] int perDiemEmp3 = 0;
        [ObservableProperty] int perDiemEmp4 = 0;
        [ObservableProperty] int perDiemEmp5 = 0;
        [ObservableProperty] int perDiemEmp6 = 0;
        [ObservableProperty] bool isCurrentEmpBlank = false;

        string startTimeLeader = string.Empty;
        string pattern = "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";
        //string pattern = "\\d{1,2}:\\d{2}"; // obsolete, too buggy

        public string StartTimeLeader
        {
            get => startTimeLeader;
            set
            {
                SetProperty(ref startTimeLeader, value);
                if (value.Length > 1)
                {
                    IsTimeLeaderInvalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    (CrewSaveCommand as Command).ChangeCanExecute();
                }
            }

        }


        string startTimeEmp1 = string.Empty;
        public string StartTimeEmp1
        {
            get => startTimeEmp1;
            set
            {
                SetProperty(ref startTimeEmp1, value);
                if (value.Length > 1)
                {
                    IsTimeEmp1Invalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    IsCurrentEmpBlank = false;
                    if (Employee1Name == null)
                        IsCurrentEmpBlank = true;
                    (CrewSaveCommand as Command).ChangeCanExecute();
                }
            }

        }

        string startTimeEmp2 = string.Empty;
        public string StartTimeEmp2
        {
            get => startTimeEmp2;
            set
            {
                SetProperty(ref startTimeEmp2, value);
                if (value.Length > 1)
                {
                    IsTimeEmp2Invalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    IsCurrentEmpBlank = false;
                    if (Employee2Name == null)
                        IsCurrentEmpBlank = true;
                    (CrewSaveCommand as Command).ChangeCanExecute();
                }
            }

        }

        string startTimeEmp3 = string.Empty;
        public string StartTimeEmp3
        {
            get => startTimeEmp3;
            set
            {
                SetProperty(ref startTimeEmp3, value);
                if (value.Length > 1)
                {
                    IsTimeEmp3Invalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    IsCurrentEmpBlank = false;
                    if (Employee3Name == null)
                        IsCurrentEmpBlank = true;
                    (CrewSaveCommand as Command).ChangeCanExecute();
                }
            }

        }

        string startTimeEmp4 = string.Empty;
        public string StartTimeEmp4
        {
            get => startTimeEmp4;
            set
            {
                SetProperty(ref startTimeEmp4, value);
                if (value.Length > 1)
                {
                    IsTimeEmp4Invalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    IsCurrentEmpBlank = Employee4Name == null ? true : false;
                    (CrewSaveCommand as Command).ChangeCanExecute();
                }
            }

        }

        string startTimeEmp5 = string.Empty;
        public string StartTimeEmp5
        {
            get => startTimeEmp5;
            set
            {
                SetProperty(ref startTimeEmp5, value);
                if (value.Length > 1)
                {
                    IsTimeEmp5Invalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    IsCurrentEmpBlank = Employee5Name == null ? true : false;
                    (CrewSaveCommand as Command).ChangeCanExecute();
                }
            }

        }

        string startTimeEmp6 = string.Empty;
        public string StartTimeEmp6
        {
            get => startTimeEmp6;
            set
            {
                SetProperty(ref startTimeEmp6, value);
                if (value.Length > 1)
                {

                    IsTimeEmp6Invalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    IsCurrentEmpBlank = Employee6Name == null ? true : false;
                    (CrewSaveCommand as Command).ChangeCanExecute();
                }
            }

        }


        [ObservableProperty] bool isTimeLeaderInvalid= false;
        [ObservableProperty] bool isTimeEmp1Invalid = false;
        [ObservableProperty] bool isTimeEmp2Invalid = false; 
        [ObservableProperty] bool isTimeEmp3Invalid = false;
        [ObservableProperty] bool isTimeEmp4Invalid = false;
        [ObservableProperty] bool isTimeEmp5Invalid = false;
        [ObservableProperty] bool isTimeEmp6Invalid = false;

        [ObservableProperty] bool isLOLeaderInvalid = false;
        [ObservableProperty] bool isLOEmp1Invalid = false;
        [ObservableProperty] bool isLOEmp2Invalid = false;
        [ObservableProperty] bool isLOEmp3Invalid = false;
        [ObservableProperty] bool isLOEmp4Invalid = false;
        [ObservableProperty] bool isLOEmp5Invalid = false;
        [ObservableProperty] bool isLOEmp6Invalid = false;


        static string INVALID_TIME_ERR_MESSAGE = "Invalid HH:MM";
        static string LUNCHOUTIME_ERR_MESSAGE = "Must be later than Clockin Time";
        static string LUNCHINTIME_ERR_MESSAGE = "Must be later than LunchOut Time";
        static string CLOCKOUTTIME_ERR_MESSAGE = "Must be later than Lunchin Time";


        [ObservableProperty] string errorMessageL = INVALID_TIME_ERR_MESSAGE;
        [ObservableProperty] string errorMessageEmp1 = INVALID_TIME_ERR_MESSAGE;
        [ObservableProperty] string errorMessageEmp2 = INVALID_TIME_ERR_MESSAGE;
        [ObservableProperty] string errorMessageEmp3 = INVALID_TIME_ERR_MESSAGE;
        [ObservableProperty] string errorMessageEmp4 = INVALID_TIME_ERR_MESSAGE;
        [ObservableProperty] string errorMessageEmp5 = INVALID_TIME_ERR_MESSAGE;
        [ObservableProperty] string errorMessageEmp6 = INVALID_TIME_ERR_MESSAGE;

        string? lunchOutTimeLeader = string.Empty;


        public string? LunchOutTimeLeader
        {
            get => lunchOutTimeLeader;
            set
            {
                SetProperty(ref lunchOutTimeLeader, value);
                (IsLOLeaderInvalid , ErrorMessageL) = IsTimeGapNegativeNew(StartTimeLeader, value, CrewLeader);
                (SaveLunchOutCommand as Command).ChangeCanExecute();

            }
        }
        [ObservableProperty] string errmessage;

        static int TIMEGAP_MINIMUM = 1;
        int cnt = 0;
        (bool, string) IsTimeGapNegativeNew(string baseTime, string currTime, string? employeename)
        {
            errmessage = INVALID_TIME_ERR_MESSAGE;

            if (baseTime == null)
                return (false, "Base Time not valid");

            if (currTime.Length > 1)
            {
                bool temp = !Regex.IsMatch(currTime.ToString(), pattern, RegexOptions.CultureInvariant);
                if (!temp)
                {
                    var gaptime = DateTime.Parse(currTime).Subtract(DateTime.Parse(baseTime));
                    var gapminutes = gaptime.TotalMinutes;
                    if (gapminutes < TIMEGAP_MINIMUM)
                    {
                        errmessage = "Must be later than " + baseTime;
                        Console.WriteLine();
                        cnt++;
                        return (true, errmessage);
                    }
                }
                Console.WriteLine("#1" + employeename  + " " + cnt);
                cnt++;
                return (temp, errmessage);
            }
            else if (currTime.Length == 0 && employeename?.Length > 1)
            {
                Console.WriteLine("#2" + employeename + " " + cnt);
                cnt++;
                return (true, errmessage);
            }
            else if (currTime.Length == 0 && string.IsNullOrEmpty(employeename)) 
            {
                Console.WriteLine("#3" + employeename + " " + cnt);
                cnt++;
                return (false, errmessage);
            }
            Console.WriteLine("#4" + employeename + " " + cnt);
            cnt++;
            return (false,"");
        }

        bool IsTimeGapNegative(string baseTime, string currTime)
        {
            var gaptime = DateTime.Parse(currTime).Subtract(DateTime.Parse(baseTime));
            var gapminutes = gaptime.TotalMinutes;
            return gapminutes < TIMEGAP_MINIMUM ? true : false;
        }

        string? lunchOutTime1 = string.Empty;
        public string? LunchOutTime1
        {
            get => lunchOutTime1;
            set
            {
                SetProperty(ref lunchOutTime1, value);
                ErrorMessageEmp1 = INVALID_TIME_ERR_MESSAGE;
                (IsLOEmp1Invalid, ErrorMessageEmp1) = IsTimeGapNegativeNew(StartTimeEmp1, value, Employee1Name?.FullName);
                (SaveLunchOutCommand as Command).ChangeCanExecute();
            }
        }

        string? lunchOutTime2 = string.Empty;
        public string? LunchOutTime2
        {
            get => lunchOutTime2;
            set
            {
                SetProperty(ref lunchOutTime2, value);
                ErrorMessageEmp2 = INVALID_TIME_ERR_MESSAGE;
                (IsLOEmp2Invalid, ErrorMessageEmp2) = IsTimeGapNegativeNew(StartTimeEmp2, value, Employee2Name?.FullName);
                (SaveLunchOutCommand as Command).ChangeCanExecute();
            }
        }


        string? lunchOutTime3 = string.Empty;
        public string? LunchOutTime3
        {
            get => lunchOutTime3;
            set
            {
                SetProperty(ref lunchOutTime3, value);
                ErrorMessageEmp3 = INVALID_TIME_ERR_MESSAGE;
                (IsLOEmp3Invalid, ErrorMessageEmp3) = IsTimeGapNegativeNew(StartTimeEmp3, value, Employee3Name?.FullName);
                (SaveLunchOutCommand as Command).ChangeCanExecute();
            }
        }


        string? lunchOutTime4 = string.Empty;
        public string? LunchOutTime4
        {
            get => lunchOutTime4;
            set
            {
                SetProperty(ref lunchOutTime4, value);
                ErrorMessageEmp4 = INVALID_TIME_ERR_MESSAGE;
                (IsLOEmp4Invalid, ErrorMessageEmp4) = IsTimeGapNegativeNew(StartTimeEmp4, value, Employee4Name?.FullName);
                (SaveLunchOutCommand as Command).ChangeCanExecute();
            }
        }

        string? lunchOutTime5 = string.Empty;
        public string? LunchOutTime5
        {
            get => lunchOutTime5;
            set
            {
                SetProperty(ref lunchOutTime5, value);
                ErrorMessageEmp5 = INVALID_TIME_ERR_MESSAGE;
                (IsLOEmp5Invalid, ErrorMessageEmp5) = IsTimeGapNegativeNew(StartTimeEmp5, value, Employee5Name?.FullName);
                (SaveLunchOutCommand as Command).ChangeCanExecute();
            }
        }

        string? lunchOutTime6 = string.Empty;
        public string? LunchOutTime6
        {
            get => lunchOutTime6;
            set
            {
                SetProperty(ref lunchOutTime6, value);
                ErrorMessageEmp6 = INVALID_TIME_ERR_MESSAGE;
                (IsLOEmp6Invalid, ErrorMessageEmp6) = IsTimeGapNegativeNew(StartTimeEmp6, value, Employee6Name?.FullName);
                cnt = 0;
                (SaveLunchOutCommand as Command).ChangeCanExecute();
            }
        }

        [ObservableProperty] bool isLILeaderInvalid = false;
        [ObservableProperty] bool isLIEmp1Invalid = false;
        [ObservableProperty] bool isLIEmp2Invalid = false;
        [ObservableProperty] bool isLIEmp3Invalid = false;
        [ObservableProperty] bool isLIEmp4Invalid = false;
        [ObservableProperty] bool isLIEmp5Invalid = false;
        [ObservableProperty] bool isLIEmp6Invalid = false;

        string lunchInTimeLeader = string.Empty;
        public string LunchInTimeLeader
        {
            get => lunchInTimeLeader;
            set
            {
                SetProperty(ref lunchInTimeLeader, value);
                ErrorMessageL = INVALID_TIME_ERR_MESSAGE;
                (IsLILeaderInvalid, ErrorMessageL) = IsTimeGapNegativeNew(LunchOutTimeLeader, value, CrewLeader);
                (SaveLunchInCommand as Command).ChangeCanExecute();
            }

        }

        string lunchInTime1 = string.Empty;
        public string LunchInTime1
        {
            get => lunchInTime1;
            set
            {
                SetProperty(ref lunchInTime1, value);
                ErrorMessageEmp1 = INVALID_TIME_ERR_MESSAGE;
                (IsLIEmp1Invalid, ErrorMessageEmp1) = IsTimeGapNegativeNew(LunchOutTime1, value, Employee1Name?.FullName);
                (SaveLunchInCommand as Command).ChangeCanExecute();
            }

        }

        string lunchInTime2 = string.Empty;
        public string LunchInTime2
        {
            get => lunchInTime2;
            set
            {
                SetProperty(ref lunchInTime2, value);
                ErrorMessageEmp2 = INVALID_TIME_ERR_MESSAGE;
                (IsLIEmp2Invalid, ErrorMessageEmp2) = IsTimeGapNegativeNew(LunchOutTime2, value, Employee2Name?.FullName);
                (SaveLunchInCommand as Command).ChangeCanExecute();
            }

        }

        string lunchInTime3 = string.Empty;
        public string LunchInTime3
        {
            get => lunchInTime3;
            set
            {
                SetProperty(ref lunchInTime3, value);
                ErrorMessageEmp3 = INVALID_TIME_ERR_MESSAGE;
                (IsLIEmp3Invalid, ErrorMessageEmp3) = IsTimeGapNegativeNew(LunchOutTime3, value, Employee3Name?.FullName);
                (SaveLunchInCommand as Command).ChangeCanExecute();
            }

        }


        string lunchInTime4 = string.Empty;
        public string LunchInTime4
        {
            get => lunchInTime4;
            set
            {
                SetProperty(ref lunchInTime4, value);
                ErrorMessageEmp4 = INVALID_TIME_ERR_MESSAGE;
                (IsLIEmp4Invalid, ErrorMessageEmp4) = IsTimeGapNegativeNew(LunchOutTime4, value, Employee4Name?.FullName);
                (SaveLunchInCommand as Command).ChangeCanExecute();
            }

        }

        string lunchInTime5 = string.Empty;
        public string LunchInTime5
        {
            get => lunchInTime5;
            set
            {
                SetProperty(ref lunchInTime5, value);
                ErrorMessageEmp5 = INVALID_TIME_ERR_MESSAGE;
                (IsLIEmp5Invalid, ErrorMessageEmp5) = IsTimeGapNegativeNew(LunchOutTime5, value, Employee5Name?.FullName);
                (SaveLunchInCommand as Command).ChangeCanExecute();
            }

        }

        string lunchInTime6 = string.Empty;
        public string LunchInTime6
        {
            get => lunchInTime6;
            set
            {
                SetProperty(ref lunchInTime6, value);
                ErrorMessageEmp6 = INVALID_TIME_ERR_MESSAGE;
                (IsLIEmp6Invalid, ErrorMessageEmp6) = IsTimeGapNegativeNew(LunchOutTime6, value, Employee6Name?.FullName);
                (SaveLunchInCommand as Command).ChangeCanExecute();
            }

        }

        public bool IsValidTimeFormat(string input)
        {
            TimeSpan dummyOutput;
            return TimeSpan.TryParse(input, out dummyOutput);
        }

        [ObservableProperty] JobPhaseDetail selectedPhase;

        [ObservableProperty] string errorMessageCrew = string.Empty;

        [ObservableProperty] int clockOutPerDiem;
        [ObservableProperty] bool isStartODOEntered = false;

        [ObservableProperty] string milesHour1 = string.Empty;
        [ObservableProperty] string milesHour2 = string.Empty;
        [ObservableProperty] string milesHour3 = string.Empty;
        [ObservableProperty] string milesHour4 = string.Empty;
        [ObservableProperty] string milesHour5 = string.Empty;
        [ObservableProperty] string milesHour6 = string.Empty;
        [ObservableProperty] string milesHour7 = string.Empty;



        [ObservableProperty] string eq1TypeTitle;
        [ObservableProperty] string eq2TypeTitle;
        [ObservableProperty] string eq3TypeTitle;
        [ObservableProperty] string eq4TypeTitle;
        [ObservableProperty] string eq5TypeTitle;
        [ObservableProperty] string eq6TypeTitle;
        [ObservableProperty] string eq7TypeTitle;

        [ObservableProperty] string eq1DescTitle;
        [ObservableProperty] string eq2DescTitle;
        [ObservableProperty] string eq3DescTitle;
        [ObservableProperty] string eq4DescTitle;
        [ObservableProperty] string eq5DescTitle;
        [ObservableProperty] string eq6DescTitle;
        [ObservableProperty] string eq7DescTitle;

        List<CrewInfoDetail> CrewInfoDetailList = new List<CrewInfoDetail>();

        [ObservableProperty] string? arrivedAtYardTime;
        [ObservableProperty] string? leftForJobMilesHour;
        [ObservableProperty] string? arrivedAtJobMilesHour;
        [ObservableProperty] string? leftJobMilesHour;
        [ObservableProperty] string? arrivedAtYardMilesHour;



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

                ClearAllPage();
                IsDisplayJobEntries = true;

                OnPropertyChanged(nameof(OwnerList));
                OnPropertyChanged(nameof(JobNumbers));
                OnPropertyChanged(nameof(JobPhaseDetailList));
                CrewLeader = Session.crew_leader;
                Console.WriteLine();
            });

            MessagingCenter.Subscribe<EnterMilesVM, string >(this, "ConfirmMiles", (senders, value) =>
            {

                ClearAllPage();
                Console.WriteLine();

                if (Session.event_type == "6") // Left For Job
                {
                    LeftForJobMilesHour = value;
                    // after save Miles Hours Left For Job
                    // 1. Display Arrived at Job button
                    IsArrivedAtSiteBtnEnabled = true;
                    (ArrivedAtSiteCommand as Command).ChangeCanExecute();
                    // 2. Disable Left For Job button
                    IsLeftForJobBtnEnabled = false;
                    (LeftForJobCommand as Command).ChangeCanExecute();

                }

                if (Session.event_type == "7") // Arrived At Job
                {
                    ArrivedAtJobMilesHour = value;
                    // after save Miles Hours Arrived at Job
                    // Display all Lunchout Lunchin dll
                    IsArrivedAtSiteBtnEnabled = false;
                    (ArrivedAtSiteCommand as Command).ChangeCanExecute();
                    IsStartODOEntered = true;
                    (LunchOutCommand as Command).ChangeCanExecute();
                    (DisplayEquipmentCheckInCommand as Command).ChangeCanExecute();
                }

                if (Session.event_type == "8") // Left Job
                {
                    LeftJobMilesHour = value;
                    IsStartODOEntered = false;
                    (LunchOutCommand as Command).ChangeCanExecute();

                    // after save Miles Hours Left Job
                    // 1. Display Arrived at Yard Button
                    IsArrivedAtYardBtnEnabled = true;
                    (ArriveAtYardCommand as Command).ChangeCanExecute();
                }

                if (Session.event_type == "9") // Arrived at Yard
                {
                    ArrivedAtYardMilesHour = value;

                    IsStartODOEntered = true;
                    Console.WriteLine(); // check Lunchin, LunchOut , Equipment Checkin
                    ArrivedAtYardTime = DateTime.Now.ToString("HH:mm");
                    (ToggleEndofDayCommand as Command).ChangeCanExecute();
                    RefreshFAButton(); // clean up
                }

            });



            // Initial ItemsSource for Job Entries and Equipment Check out/check in 
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


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<User>();
                var table = conn.Table<User>();

                foreach (var col in table)
                {
                    CrewInfoDetailList.Add(new CrewInfoDetail
                    {
                        FullName = col.first_name + " " + col.last_name,
                        TeamUserKey = col.UserKey
                    });

                }
            }


            Console.WriteLine();

            GPSSettingCommand = new Command(() => DisplayGPSSettingCommand());

            SaveTimeSheetLCommand = new Command(
                execute: async () =>
                {
                    ClearAllPage();
                    IsDisplayEndOfDay = true;

                    Session.event_type = "16";

                    SaveAndDisplayTimeSheetForm(LunchInTimeLeader, LunchOutTimeLeader, LClockOutTime, StartTimeLeader, LClockOutTime, CrewLeader,
                    LClockOutPerDiem, Session.uid.ToString());

                },
                canExecute:() =>
                {
                    return (LClockOutTime == null) ? false : !IsCOLTimeInvalid;
                    /*if (LClockOutTime == null)
                    {
                        Console.WriteLine();
                        return false;
                    }
                    else
                    {
                        Console.WriteLine(  );
                        return !IsCO1TimeInvalid;
                    }*/
                    //return LClockOutTime?.Length==0 ? false : !IsCOLTimeInvalid;
                }
                
            );

            SaveTimeSheet1Command = new Command(
                execute: async () =>
                {
                    ClearAllPage();
                    IsDisplayEndOfDay = true;

                    Session.event_type = "16";

                    SaveAndDisplayTimeSheetForm(LunchInTime1, LunchOutTime1, Emp1ClockOutTime, StartTimeEmp1, Emp1ClockOutTime, Employee1Name.FullName,
                    Emp1ClockOutPerDiem, SelectedCrewInfoDetails[1].TeamUserKey.ToString());

                },
                canExecute: () =>
                {
                    if (Emp1ClockOutTime == null) return false;
                    else return !IsCO1TimeInvalid;
                }
            );

            SaveTimeSheet2Command = new Command(
                execute: async () =>
                {
                    ClearAllPage();
                    IsDisplayEndOfDay = true;

                    Session.event_type = "16";

                    SaveAndDisplayTimeSheetForm(LunchInTime2, LunchOutTime2, Emp2ClockOutTime, StartTimeEmp2, Emp2ClockOutTime, Employee2Name.FullName,
                    Emp2ClockOutPerDiem, SelectedCrewInfoDetails[2].TeamUserKey.ToString());

                },
                canExecute: () =>
                {
                    return (Emp2ClockOutTime == null) ? false : !IsCO2TimeInvalid;
                }
            );

            SaveTimeSheet3Command = new Command(
                execute: async () =>
                {
                    ClearAllPage();
                    IsDisplayEndOfDay = true;

                    Session.event_type = "16";

                    SaveAndDisplayTimeSheetForm(LunchInTime3, LunchOutTime3, Emp3ClockOutTime, StartTimeEmp3, Emp3ClockOutTime, Employee3Name.FullName,
                    Emp3ClockOutPerDiem, SelectedCrewInfoDetails[3]?.TeamUserKey.ToString());

                },
                canExecute: () =>
                {
                    return (Emp3ClockOutTime == null ) ? false: !IsCO3TimeInvalid;
                }
            );

            SaveTimeSheet4Command = new Command(
                execute: async () =>
                {
                    ClearAllPage();
                    IsDisplayEndOfDay = true;

                    Session.event_type = "16";

                    SaveAndDisplayTimeSheetForm(LunchInTime4, LunchOutTime4, Emp4ClockOutTime, StartTimeEmp4, Emp4ClockOutTime, Employee4Name.FullName,
                    Emp4ClockOutPerDiem, SelectedCrewInfoDetails[4]?.TeamUserKey.ToString());

                },
                canExecute: () =>
                {
                    return (Emp4ClockOutTime == null) ? false :  !IsCO4TimeInvalid;
                }
            );
            SaveTimeSheet5Command = new Command(
                execute: async () =>
                {
                    ClearAllPage();
                    IsDisplayEndOfDay = true;

                    Session.event_type = "16";

                    SaveAndDisplayTimeSheetForm(LunchInTime4, LunchOutTime5, Emp5ClockOutTime, StartTimeEmp5, Emp5ClockOutTime, Employee5Name.FullName,
                    Emp5ClockOutPerDiem, SelectedCrewInfoDetails[5]?.TeamUserKey.ToString());

                },
                canExecute: () =>
                {
                    return (Emp5ClockOutTime == null) ? false : !IsCO5TimeInvalid;
                }
            );
            SaveTimeSheet6Command = new Command(
                execute: async () =>
                {
                    ClearAllPage();
                    IsDisplayEndOfDay = true;

                    Session.event_type = "16";
                },
                canExecute: () =>
                {
                    return false;
                }
            );

            /*SaveTimeSheetCommand = new Command<string>(
                execute: async (string param) =>
                {
                    ClearAllPage();
                    IsDisplayEndOfDay = true;

                    var CrewKey = "0";
                    var Fullname = string.Empty;
                    int PerDiemTemp = 0;

                    Session.event_type = "16";

                    if (param.Equals("Leader"))
                    {
                        LunchInTime = LunchInTimeLeader;
                        LunchOutTime = LunchOutTimeLeader;
                        ClockOutTimeForm = LClockOutTime;
                        ClockIntime = StartTimeLeader;
                        ClockOutTime = LClockOutTime;
                        Fullname = CrewLeader;
                        PerDiemTemp = LClockOutPerDiem;
                        CrewKey = Session.uid.ToString();
                        Console.WriteLine();
                    }
                    else if (param.Equals("Emp1"))
                    {
                        LunchInTime = LunchInTime1;
                        LunchOutTime = LunchOutTime1;
                        ClockOutTimeForm = Emp1ClockOutTime;
                        ClockIntime = StartTimeEmp1;
                        ClockOutTime = Emp1ClockOutTime;
                        Fullname = Employee1Name.FullName;
                        PerDiemTemp = Emp1ClockOutPerDiem;
                        CrewKey = SelectedCrewInfoDetails[1].TeamUserKey.ToString(); //.Where(a => a.id == 2).Select(b => b.TeamUserKey).ToString();
                        Console.WriteLine();
                    }
                    else if (param.Equals("Emp2"))
                    {
                        LunchInTime = LunchInTime2;
                        LunchOutTime = LunchOutTime2;
                        ClockOutTimeForm = Emp2ClockOutTime;
                        ClockIntime = StartTimeEmp2;
                        CrewKey = SelectedCrewInfoDetails[2].TeamUserKey.ToString(); //.Where(a => a.id == 3).Select(b => b.TeamUserKey).ToString();
                        ClockOutTime = Emp2ClockOutTime;
                        Fullname = Employee2Name.FullName;
                        PerDiemTemp = Emp2ClockOutPerDiem;
                        Console.WriteLine();
                    }
                    else if (param.Equals("Emp3"))
                    {
                        LunchInTime = LunchInTime3;
                        LunchOutTime = LunchOutTime3;
                        ClockOutTimeForm = Emp3ClockOutTime;
                        ClockIntime = StartTimeEmp3;
                        CrewKey = SelectedCrewInfoDetails[3].TeamUserKey.ToString(); //.Where(a => a.id == 4).Select(b => b.TeamUserKey).ToString();
                        ClockOutTime = Emp3ClockOutTime;
                        Fullname = Employee3Name.FullName;
                        PerDiemTemp = Emp3ClockOutPerDiem;
                        Console.WriteLine();
                    }
                    else if (param.Equals("Emp4"))
                    {
                        LunchInTime = LunchInTime4;
                        LunchOutTime = LunchOutTime4;
                        ClockOutTimeForm = Emp4ClockOutTime;
                        ClockIntime = StartTimeEmp4;
                        CrewKey = SelectedCrewInfoDetails[4].TeamUserKey.ToString(); //.Where(a => a.id == 5).Select(b => b.TeamUserKey).ToString();
                        ClockOutTime = Emp4ClockOutTime;
                        Fullname = Employee4Name.FullName;
                        PerDiemTemp = Emp4ClockOutPerDiem;
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    try
                    {
                        await CloudDBService.PostTimeSheet(CrewKey, ClockOutTime, SelectedPhase.PhaseNumber, PerDiemTemp);
                        await CloudDBService.PostJobEvent(DateTime.Parse(ClockOutTime).Hour.ToString(),
                            DateTime.Parse(ClockOutTime).Minute.ToString(), PerDiemTemp, SelectedPhase.PhaseNumber, "0", CrewKey);


                        TimeSpan totaltime = DateTime.Parse(LunchOutTime).Subtract(DateTime.Parse(ClockIntime)) + DateTime.Parse(ClockOutTime).Subtract(DateTime.Parse(LunchInTime));
                        TotalHoursForToday = totaltime.ToString(@"hh\:mm\:ss");
                        await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new TimeSheetSignature(
                            Fullname, CrewKey,
                            ClockIntime, ClockOutTimeForm, LunchInTime, LunchOutTime, TotalHoursForToday));

                        Console.WriteLine();
                    }
                    catch (Exception e)
                    {
                        await Application.Current.MainPage.DisplayAlert("Empty ClockOut Data", "Check again ClockOut time", "DONE");
                        Console.WriteLine(e);
                    }

                }
            );*/


                ToggleCrewListCommand = new Command(
                execute: () =>
                {
                    ClearAllPage();
                    IsDisplayCrewList = true;

                    IsTimeLeaderInvalid = true;
                    (CrewSaveCommand as Command).ChangeCanExecute();
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
                        if (IsCurrentEmpBlank)
                        {
                            Console.WriteLine();
                            await Application.Current.MainPage.DisplayAlert("Input Invalid", "Employee or Crew should not empty", "BACK");

                        }
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
                                PerDiem = PerDiemL
                            });

                            try
                            {
                                await CloudDBService.PostTimeSheet(Session.uid.ToString(), StartTimeLeader, SelectedPhase.PhaseNumber, PerDiemL); //20230331
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
                                await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(), StartTimeEmp1, SelectedPhase.PhaseNumber, PerDiemEmp1);
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
                            PerDiemL.ToString(),
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

                        // Update each entry in Lunchin page, Lunch out page
                        (SaveLunchInCommand as Command).ChangeCanExecute();
                        (SaveLunchOutCommand as Command).ChangeCanExecute();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception " + e);
                    }


                },
                //crewsavecommand_canexecute
                canExecute: () =>
                {
                    Console.WriteLine();


                    var retval = true;

                    if (IsCurrentEmpBlank ||
                    IsTimeLeaderInvalid || IsTimeEmp1Invalid || IsTimeEmp2Invalid || IsTimeEmp3Invalid
                    || IsTimeEmp4Invalid || IsTimeEmp5Invalid || IsTimeEmp6Invalid
                    )
                        retval = false;

                    return retval;

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

                    ClearAllPage();

                },
                canExecute: () =>
                {
                    var retval = false;
                    if (LunchOutTimeLeader.Length > 3)
                    {
                        retval = !IsLOLeaderInvalid;
                    }

                    if (Employee1Name?.FullName.Length > 1 && LunchOutTime1.Length > 3)
                    {
                        retval = !IsLOEmp1Invalid;
                    }

                    if (Employee2Name?.FullName.Length > 1 && LunchOutTime2.Length > 3)
                    {
                        retval = !IsLOEmp2Invalid;
                    }

                    if (Employee3Name?.FullName.Length > 1 && LunchOutTime3.Length > 3)
                    {
                        retval = !IsLOEmp3Invalid;
                    }

                    if (Employee4Name?.FullName.Length > 1 && LunchOutTime4.Length > 3)
                    {
                        retval = !IsLOEmp4Invalid;
                    }

                    if (Employee5Name?.FullName.Length > 1 && LunchOutTime5.Length > 3)
                    {
                        retval = !IsLOEmp5Invalid;
                    }

                    if (Employee6Name?.FullName.Length > 1 && LunchOutTime6.Length > 3)
                    {
                        retval = !IsLOEmp6Invalid;
                    }

                    //var retvalue = true;
                    //if (IsLILeaderInvalid || IsLIEmp1Invalid || IsLIEmp2Invalid ||
                    //IsLIEmp3Invalid || IsLIEmp4Invalid || IsLIEmp5Invalid || IsLIEmp6Invalid)
                    //    retvalue = false;
                    return retval;
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


                    // after lunchin, display Left Job button
                    IsLeftJobBtnEnabled = true;
                    (LeftJobCommand as Command).ChangeCanExecute();


                    ClearAllPage();
                },
                canExecute: () =>
                {
                    var retval = false;
                    if (LunchInTimeLeader.Length > 3)
                    {
                        retval = !IsLILeaderInvalid;
                    }

                    if (Employee1Name?.FullName.Length > 1 && LunchInTime1.Length > 3)
                    {
                        retval = !IsLIEmp1Invalid;
                    }

                    if (Employee2Name?.FullName.Length > 1 && LunchInTime2.Length > 3)
                    {
                        retval = !IsLIEmp2Invalid;
                    }

                    if (Employee3Name?.FullName.Length > 1 && LunchInTime3.Length > 3)
                    {
                        retval = !IsLIEmp3Invalid;
                    }

                    if (Employee4Name?.FullName.Length > 1 && LunchInTime4.Length > 3)
                    {
                        retval = !IsLIEmp4Invalid;
                    }

                    if (Employee5Name?.FullName.Length > 1 && LunchInTime5.Length > 3)
                    {
                        retval = !IsLIEmp5Invalid;
                    }
                    //var retvalue = true;
                    //if (IsLILeaderInvalid || IsLIEmp1Invalid || IsLIEmp2Invalid ||
                    //IsLIEmp3Invalid || IsLIEmp4Invalid || IsLIEmp5Invalid || IsLIEmp6Invalid)
                    //    retvalue = false;
                    return retval;
                }
            );


            LunchOutCommand = new Command(
                    execute: async () =>
                    {
                        Console.WriteLine();
                        try
                        {

                            IsLunchOut = false;
                            IsLunchIn = true; // when lunchout button clicked, it will enable Lunchin button

                            (LunchInCommand as Command).ChangeCanExecute();
                            (LunchOutCommand as Command).ChangeCanExecute();
                            (DisplayEquipmentCheckInCommand as Command).ChangeCanExecute();
                            (ToggleEndofDayCommand as Command).ChangeCanExecute();

                            ClearAllPage();
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
                        return IsLunchOut;
                    }
                );

            LunchInCommand = new Command(
                execute: async () =>
                {
                    ClearAllPage();
                    IsLunchInDisplay = true;


                    LunchInTime = DateTime.Now.ToString("HH:mm");

                    IsLunchIn = false;
                    (LunchInCommand as Command).ChangeCanExecute();
                    //TodoButton = string.Empty;


                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return IsLunchIn;
                }
            );

            ToggleEndofDayCommand = new Command(
                execute: () =>
                {
                    ClearAllPage();
                    IsDisplayEndOfDay = true;

                    (ToggleCrewListCommand as Command).ChangeCanExecute();
                    (DisplayEquipmentCheckOutCommand as Command).ChangeCanExecute();
                    //calculate from Clock in to Lunch out

                    //check if clockintime > lunchout
                    Console.WriteLine();
                    //set initial value of perdiem to crew list perdiem;
                    LClockOutPerDiem = 1;// PerDiemL;
                    Emp1ClockOutPerDiem = 1; // PerDiemEmp1;
                    Emp2ClockOutPerDiem = 1; // PerDiemEmp2;
                    Emp3ClockOutPerDiem = 1; //PerDiemEmp3;
                    Emp4ClockOutPerDiem = 2; //PerDiemEmp4;
                    Emp5ClockOutPerDiem = PerDiemEmp5;

                },
                canExecute: () =>
                {
                    return !IsLunchIn && !IsLunchOut && !isEqCheckIn;
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

            DisplayEquipmentCheckInCommand = new Command(
                execute: () =>
                {
                    ClearAllPage();
                    IsEqCheckInDisplayed = true;
                    IsEqCheckIn = true;
                    try
                    {

                        if (Application.Current.Properties.ContainsKey("Eq1Type"))
                        {
                            Eq1TypeTitle = Application.Current.Properties["Eq1Type"] as string;
                            Eq1DescTitle = Application.Current.Properties["Eq1Desc"] as string;
                        }
                        if (Application.Current.Properties.ContainsKey("Eq2Type"))
                        {
                            Eq2TypeTitle = Application.Current.Properties["Eq2Type"] as string;
                            Eq2DescTitle = Application.Current.Properties["Eq2Desc"] as string;
                        }
                        if (Application.Current.Properties.ContainsKey("Eq3Type"))
                        {
                            Eq3TypeTitle = Application.Current.Properties["Eq3Type"] as string;
                            Eq3DescTitle = Application.Current.Properties["Eq3Desc"] as string;
                        }
                        if (Application.Current.Properties.ContainsKey("Eq4Type"))
                        {
                            Eq4TypeTitle = Application.Current.Properties["Eq4Type"] as string;
                            Eq4DescTitle = Application.Current.Properties["Eq4Desc"] as string;
                        }
                        if (Application.Current.Properties.ContainsKey("Eq5Type"))
                        {
                            Eq5TypeTitle = Application.Current.Properties["Eq5Type"] as string;
                            Eq5DescTitle = Application.Current.Properties["Eq5Desc"] as string;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("DisplayEquipmentCheckInCommand " + e.ToString());
                    }
                    //TodoButton = string.Empty;

                    Console.WriteLine();


                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return !IsLunchIn & !IsLunchOut;
                }
            );

            SaveEqCheckInCommand = new Command(
                execute: async () =>
                {

                    try
                    {
                        if (Eq1IsReturned)
                        {
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq1?.EqKey, MilesHourIn1, "0");
                            Console.WriteLine(Eq1IsReturned);
                        };
                        if (Eq2IsReturned)
                        {
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq2?.EqKey, MilesHourIn2, "0");
                            Console.WriteLine(Eq2IsReturned);
                        };
                        if (Eq3IsReturned)
                        { 
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq3?.EqKey, MilesHourIn3, "0");
                            Console.WriteLine(Eq3IsReturned);
                        };

                        if (Eq4IsReturned)
                        {
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq4?.EqKey, MilesHourIn4, "0");
                            Console.WriteLine(Eq4IsReturned);
                        };

                        if (Eq5IsReturned)
                        {
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq5?.EqKey, MilesHourIn5, "0");
                            Console.WriteLine(Eq5IsReturned);
                        };

                        if (Eq6IsReturned)
                        {
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq6?.EqKey, MilesHourIn6, "0");
                            Console.WriteLine(Eq6IsReturned);
                        };

                        bool answer = await Application.Current.MainPage.DisplayAlert("Upload Done", "Job Equipment table uploaded", "Finish The Day", "Back");
                        if (answer) {
                            ClearAllPage();
                            IsDisplayEndOfDay = true;
                        }
                        IsEqCheckIn = false;

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        await Application.Current.MainPage.DisplayAlert("Network problem", "Check internet connection", "Retry");
                    }

                }
            );

            DisplayEquipmentCheckOutCommand = new Command(
                execute: () =>
                {
                    ClearAllPage();
                    IsEqCheckOutDisplayed = true;
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

                    IsEqCheckOutDisplayed = false;

                    Session.event_type = "4"; // Equipment out
                    await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), 
                        DateTime.Now.Minute.ToString(), 0, SelectedPhase.PhaseNumber,"", MilesHour1); //20230331

                    //Session.event_type = "8"; // Odo
                    //await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), SelectedPhase.PhaseNumber);


                    //IsStartODOEntered = true; // start ODO entered , enable all icon
                    IsDisplayOdoStart = false;
                    IsDisplayOdoEnd = false;

                    IsLeftForJobBtnEnabled = true;// enable fab button
                    //TodoButton = "Leave to Site";
                    (LeftForJobCommand as Command).ChangeCanExecute();

                    try
                    {
                        if (SelectedEq1?.EqDesc.Length > 1)
                        {
                            if (Application.Current.Properties.ContainsKey("Eq1Type"))
                            {
                                Application.Current.Properties["Eq1Type"] = SelectedEq1Type?.EquipCodeDesc;
                                Application.Current.Properties["Eq1Desc"] = SelectedEq1?.EqDesc;
                                Application.Current.Properties["COMiles1"] = MilesHour1;

                            }
                            else
                            {
                                Application.Current.Properties.Add("Eq1Type", SelectedEq1Type?.EquipCodeDesc);
                                Application.Current.Properties.Add("Eq1Desc", SelectedEq1?.EqDesc);
                                Application.Current.Properties.Add("COMiles1", MilesHour1);
                            }

                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq1?.EqKey, "0", MilesHour1);

                        }
                        if (SelectedEq2?.EqDesc.Length > 1)
                        {


                            if (Application.Current.Properties.ContainsKey("Eq2Type"))
                            {
                                Application.Current.Properties["Eq2Type"] = SelectedEq2Type?.EquipCodeDesc;
                                Application.Current.Properties["Eq2Desc"] = SelectedEq2?.EqDesc;
                                Application.Current.Properties["COMiles2"] = MilesHour2;
                            }
                            else
                            {
                                Application.Current.Properties.Add("Eq2Type", SelectedEq2Type?.EquipCodeDesc);
                                Application.Current.Properties.Add("Eq2Desc", SelectedEq2?.EqDesc);
                                Application.Current.Properties.Add("COMiles2", MilesHour2);
                            }

                            //await CloudDBService.PostTimeSheet(Employee1Name?.TeamUserKey.ToString(), LunchOutTime1, SelectedPhase, 0);
                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq2?.EqKey, "0", MilesHour2);
                        }
                        if (SelectedEq3?.EqDesc.Length > 1)
                        {
                            if (Application.Current.Properties.ContainsKey("Eq3Type"))
                            {
                                Application.Current.Properties["Eq3Type"] = SelectedEq3Type?.EquipCodeDesc;
                                Application.Current.Properties["Eq3Desc"] = SelectedEq3?.EqDesc;
                                Application.Current.Properties["COMiles3"] = MilesHour3;
                            }
                            else
                            {
                                Application.Current.Properties.Add("Eq3Type", SelectedEq3Type?.EquipCodeDesc);
                                Application.Current.Properties.Add("Eq3Desc", SelectedEq3?.EqDesc);
                                Application.Current.Properties.Add("COMiles3", MilesHour3);
                            }

                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq3?.EqKey, "0", MilesHour3);
                        }

                        if (SelectedEq4?.EqDesc.Length > 1)
                        {
                            if (Application.Current.Properties.ContainsKey("Eq4Type"))
                            {
                                Application.Current.Properties["Eq4Type"] = SelectedEq4Type?.EquipCodeDesc;
                                Application.Current.Properties["Eq4Desc"] = SelectedEq4?.EqDesc;
                                Application.Current.Properties["COMiles4"] = MilesHour4;
                            }
                            else
                            {
                                Application.Current.Properties.Add("Eq4Type", SelectedEq4Type?.EquipCodeDesc);
                                Application.Current.Properties.Add("Eq4Desc", SelectedEq4?.EqDesc);
                                Application.Current.Properties.Add("COMiles4", MilesHour4);
                            }

                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq4?.EqKey, "0", MilesHour4);
                        }

                        if (SelectedEq5?.EqDesc.Length > 1)
                        {
                            if (Application.Current.Properties.ContainsKey("Eq5Type"))
                            {
                                Application.Current.Properties["Eq5Type"] = SelectedEq5Type?.EquipCodeDesc;
                                Application.Current.Properties["Eq5Desc"] = SelectedEq5?.EqDesc;
                                Application.Current.Properties["COMiles5"] = MilesHour5;
                            }
                            else
                            {
                                Application.Current.Properties.Add("Eq5Type", SelectedEq5Type?.EquipCodeDesc);
                                Application.Current.Properties.Add("Eq5Desc", SelectedEq5?.EqDesc);
                                Application.Current.Properties.Add("COMiles5", MilesHour5);
                            }

                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq5?.EqKey, "0", MilesHour5);
                        }

                        if (SelectedEq6?.EqDesc.Length > 1)
                        {
                            if (Application.Current.Properties.ContainsKey("Eq6Type"))
                            {
                                Application.Current.Properties["Eq6Type"] = SelectedEq6Type?.EquipCodeDesc;
                                Application.Current.Properties["Eq6Desc"] = SelectedEq6?.EqDesc;
                                Application.Current.Properties["COMiles6"] = MilesHour6;
                            }
                            else
                            {
                                Application.Current.Properties.Add("Eq6Type", SelectedEq6Type?.EquipCodeDesc);
                                Application.Current.Properties.Add("Eq6Desc", SelectedEq6?.EqDesc);
                                Application.Current.Properties.Add("COMiles6", MilesHour6);
                            }

                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq6?.EqKey, "0", MilesHour6);
                        }

                        if (SelectedEq7?.EqDesc.Length > 1)
                        {
                            if (Application.Current.Properties.ContainsKey("Eq7Type"))
                            {
                                Application.Current.Properties["Eq7Type"] = SelectedEq7Type?.EquipCodeDesc;
                                Application.Current.Properties["Eq7Desc"] = SelectedEq7?.EqDesc;
                                Application.Current.Properties["COMiles7"] = MilesHour7;
                            }
                            else
                            {
                                Application.Current.Properties.Add("Eq7Type", SelectedEq7Type?.EquipCodeDesc);
                                Application.Current.Properties.Add("Eq7Desc", SelectedEq7?.EqDesc);
                                Application.Current.Properties.Add("COMiles7", MilesHour7);
                            }

                            await CloudDBService.PostJobEquipment(SelectedPhase.PhaseNumber, SelectedEq7?.EqKey, "0", MilesHour7);
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
                    Session.curphase = SelectedPhase.PhaseNumber;
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

            LeftForJobCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = "6"; //left for job site event                   
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new EnterMiles("NA")); // open miles hours window

                },
                canExecute: () => {

                    return IsLeftForJobBtnEnabled;// IsStartODOEntered;
                }
            );

            ArrivedAtSiteCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = "7"; //arrived@ job site
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new EnterMiles(LeftForJobMilesHour));


                },
                canExecute: () => {

                    return IsArrivedAtSiteBtnEnabled;
                }
            );



            LeftJobCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = "8"; //left for job site
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new EnterMiles(ArrivedAtJobMilesHour));

                },
                canExecute: () => {

                    return IsLeftJobBtnEnabled;
                }
            );

            ArriveAtYardCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = "9"; //left for job site
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new EnterMiles(LeftJobMilesHour));
                    RefreshFAButton(); // pull all Is FAB button state to false, then invoke all ChangeExecute
                },
                canExecute: () => {

                    return IsArrivedAtYardBtnEnabled;
                }
            );

            LogOutCommand = new Command(
                execute: async () =>
                {
                    Session.event_type = "12"; //left for job site
                    await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), SelectedPhase.PhaseNumber);

                    bool answer = await Application.Current.MainPage.DisplayAlert("Logout", "Ready to Logout ?", "Yes", "No");
                    if (answer)
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            );

        }


        //SaveAndDisplayTimeSheetForm(LunchInTimeLeader,LunchOutTimeLeader,LClockOutTime,StartTimeLeader,LClockOutTime,CrewLeader,
        //LClockOutPerDiem,Session.uid.ToString())

        async void SaveAndDisplayTimeSheetForm(string LunchInTime, string LunchOutTime, string ClockOutTimeForm,
           string ClockIntime, string ClockOutTime, string Fullname, int PerDiemTemp, string CrewKey)
        {
            /*LunchInTime = LunchInTimeLeader;
            LunchOutTime = LunchOutTimeLeader;
            ClockOutTimeForm = LClockOutTime;
            ClockIntime = StartTimeLeader;
            ClockOutTime = LClockOutTime;
            Fullname = CrewLeader;
            PerDiemTemp = LClockOutPerDiem;
            CrewKey = Session.uid.ToString();*/
            Console.WriteLine();

            try
            {
                await CloudDBService.PostTimeSheet(CrewKey, ClockOutTime, SelectedPhase.PhaseNumber, PerDiemTemp);
                await CloudDBService.PostJobEvent(DateTime.Parse(ClockOutTime).Hour.ToString(),
                    DateTime.Parse(ClockOutTime).Minute.ToString(), PerDiemTemp, SelectedPhase.PhaseNumber, "0", CrewKey);


                TimeSpan totaltime = DateTime.Parse(LunchOutTime).Subtract(DateTime.Parse(ClockIntime)) + DateTime.Parse(ClockOutTime).Subtract(DateTime.Parse(LunchInTime));
                TotalHoursForToday = totaltime.ToString(@"hh\:mm\:ss");
                await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new TimeSheetSignature(
                    Fullname, CrewKey,
                    ClockIntime, ClockOutTimeForm, LunchInTime, LunchOutTime, TotalHoursForToday));
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Exception", e.ToString(), "BACK");
            }
        }
        void RefreshFAButton()
        {
            IsLeftForJobBtnEnabled = false; (LeftForJobCommand as Command).ChangeCanExecute();
            IsArrivedAtSiteBtnEnabled = false; (ArrivedAtSiteCommand as Command).ChangeCanExecute();
            IsLeftJobBtnEnabled = false; (LeftJobCommand as Command).ChangeCanExecute();
            IsArrivedAtYardBtnEnabled = false; (ArriveAtYardCommand as Command).ChangeCanExecute();


        }
        void ClearAllPage()
        {
            IsDisplayJobEntries = false;
            IsDisplayCrewList = false;
            IsDisplayEndOfDayForm = false;
            IsEqCheckInDisplayed = false;
            IsEqCheckOutDisplayed = false;
            IsLunchInDisplay = false;
            IsLunchOutDisplay = false;
            IsDisplayOdoStart = false;
            IsDisplayOdoEnd = false;
            IsDisplayEndOfDay = false;
        }

        [ObservableProperty] bool isLeftForJobBtnEnabled = false;
        [ObservableProperty] bool isArrivedAtSiteBtnEnabled = false;
        [ObservableProperty] bool isLeftJobBtnEnabled = false;
        [ObservableProperty] bool isArrivedAtYardBtnEnabled = false;
        [ObservableProperty] bool disableLunchOutSaveBtn = false;
        //[ObservableProperty] string todoButton = string.Empty;

        [ObservableProperty] bool eq1IsReturned = false;
        [ObservableProperty] bool eq2IsReturned = false;
        [ObservableProperty] bool eq3IsReturned = false;
        [ObservableProperty] bool eq4IsReturned = false;
        [ObservableProperty] bool eq5IsReturned = false;
        [ObservableProperty] bool eq6IsReturned = false;
        [ObservableProperty] bool eq7IsReturned = false;


        public ICommand ArrivedAtSiteCommand { get; set; }
        public ICommand LeftForJobCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        public ICommand LeftJobCommand { get; set; }
        public ICommand ArriveAtYardCommand { get; set; }




        [ObservableProperty] bool isCOLTimeInvalid = false;
        [ObservableProperty] bool isCO1TimeInvalid = false;
        [ObservableProperty] bool isCO2TimeInvalid = false;
        [ObservableProperty] bool isCO3TimeInvalid = false;
        [ObservableProperty] bool isCO4TimeInvalid = false;
        [ObservableProperty] bool isCO5TimeInvalid = false;
        [ObservableProperty] bool isCO6TimeInvalid = false;


        [ObservableProperty] string clockOutTime;
        string? lClockOutTime;
        public string LClockOutTime
        {
            get => lClockOutTime;
            set
            {
                SetProperty(ref lClockOutTime, value);
                ErrorMessageL = INVALID_TIME_ERR_MESSAGE;
                (IsCOLTimeInvalid, ErrorMessageL) = IsTimeGapNegativeNew(ArrivedAtYardTime, value, CrewLeader);
                
                (SaveTimeSheetLCommand as Command).ChangeCanExecute();
                /*if (value.Length > 1)
                {
                    ErrorMessageL = INVALID_TIME_ERR_MESSAGE;
                    IsCOLTimeInvalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    if (!IsCOLTimeInvalid)
                    {
                        if (IsTimeGapNegative(LunchInTimeLeader, value))
                        {
                            IsCOLTimeInvalid = true;
                            ErrorMessageL = CLOCKOUTTIME_ERR_MESSAGE;
                        }
                    }

                    (SaveTimeSheetLCommand as Command).ChangeCanExecute();
                    //(SaveTimeSheetCommand as Command).ChangeCanExecute();
                }*/
            }
        }


        string? emp1ClockOutTime;
        public string Emp1ClockOutTime
        {
            get => emp1ClockOutTime;
            set
            {
                SetProperty(ref emp1ClockOutTime, value);
                ErrorMessageEmp1 = INVALID_TIME_ERR_MESSAGE;
                (IsCO1TimeInvalid, ErrorMessageEmp1) = IsTimeGapNegativeNew(ArrivedAtYardTime, value, Employee1Name?.FullName);
                (SaveTimeSheet1Command as Command).ChangeCanExecute();
                /*if (value.Length > 1)
                {
                    ErrorMessageEmp1 = INVALID_TIME_ERR_MESSAGE;
                    IsCO1TimeInvalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    if (!IsCO1TimeInvalid)
                    {
                        if (IsTimeGapNegative(LunchInTime1, value))
                        {
                            IsCO1TimeInvalid = true;
                            ErrorMessageEmp1 = CLOCKOUTTIME_ERR_MESSAGE;
                        }
                    }
                    (SaveTimeSheet1Command as Command).ChangeCanExecute();
                }*/
            }
        }

        string? emp2ClockOutTime;
        public string Emp2ClockOutTime
        {
            get => emp2ClockOutTime;
            set
            {
                SetProperty(ref emp2ClockOutTime, value);
                ErrorMessageEmp2 = INVALID_TIME_ERR_MESSAGE;
                (IsCO2TimeInvalid, ErrorMessageEmp2) = IsTimeGapNegativeNew(ArrivedAtYardTime, value, Employee2Name?.FullName);
                (SaveTimeSheet2Command as Command).ChangeCanExecute();
                /*if (value.Length > 1)
                {
                    ErrorMessageEmp2 = INVALID_TIME_ERR_MESSAGE;
                    IsCO2TimeInvalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    if (!IsCO2TimeInvalid)
                    {
                        if (IsTimeGapNegative(LunchInTime2, value))
                        {
                            IsCO2TimeInvalid = true;
                            ErrorMessageEmp2 = CLOCKOUTTIME_ERR_MESSAGE;
                        }
                    }
                    (SaveTimeSheet2Command as Command).ChangeCanExecute();
                }*/
            }
        }

        string? emp3ClockOutTime;
        public string Emp3ClockOutTime
        {
            get => emp3ClockOutTime;
            set
            {
                SetProperty(ref emp3ClockOutTime, value);
                ErrorMessageEmp3 = INVALID_TIME_ERR_MESSAGE;
                (IsCO3TimeInvalid, ErrorMessageEmp3) = IsTimeGapNegativeNew(ArrivedAtYardTime, value, Employee3Name?.FullName);
                (SaveTimeSheet3Command as Command).ChangeCanExecute();
                /*if (value.Length > 1)
                {
                    ErrorMessageEmp3 = INVALID_TIME_ERR_MESSAGE;
                    IsCO3TimeInvalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    if (!IsCO3TimeInvalid)
                    {
                        if (IsTimeGapNegative(LunchInTime3, value))
                        {
                            IsCO3TimeInvalid = true;
                            ErrorMessageEmp3 = CLOCKOUTTIME_ERR_MESSAGE;
                        }
                    }
                    (SaveTimeSheet3Command as Command).ChangeCanExecute();
                }*/
            }
        }

        string? emp4ClockOutTime;
        public string Emp4ClockOutTime
        {
            get => emp4ClockOutTime;
            set
            {
                SetProperty(ref emp4ClockOutTime, value);
                ErrorMessageEmp4 = INVALID_TIME_ERR_MESSAGE;
                (IsCO4TimeInvalid, ErrorMessageEmp4) = IsTimeGapNegativeNew(ArrivedAtYardTime, value, Employee4Name?.FullName);
                (SaveTimeSheet4Command as Command).ChangeCanExecute();
                /*if (value.Length > 1)
                {
                    ErrorMessageEmp4 = INVALID_TIME_ERR_MESSAGE;
                    IsCO4TimeInvalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    if (!IsCO4TimeInvalid)
                    {
                        if (IsTimeGapNegative(LunchInTime4, value))
                        {
                            IsCO4TimeInvalid = true;
                            ErrorMessageEmp4 = CLOCKOUTTIME_ERR_MESSAGE;
                        }
                    }
                    (SaveTimeSheet4Command as Command).ChangeCanExecute();
                }*/
            }
        }

        string? emp5ClockOutTime;
        public string Emp5ClockOutTime
        {
            get => emp5ClockOutTime;
            set
            {
                SetProperty(ref emp5ClockOutTime, value);
                ErrorMessageEmp5 = INVALID_TIME_ERR_MESSAGE;
                (IsCO5TimeInvalid, ErrorMessageEmp5) = IsTimeGapNegativeNew(ArrivedAtYardTime, value, Employee5Name?.FullName);
                (SaveTimeSheet5Command as Command).ChangeCanExecute();
                /*if (value.Length > 1)
                {
                    ErrorMessageEmp4 = INVALID_TIME_ERR_MESSAGE;
                    IsCO5TimeInvalid = !Regex.IsMatch(value.ToString(), pattern, RegexOptions.CultureInvariant);
                    if (!IsCO5TimeInvalid)
                    {
                        if (IsTimeGapNegative(LunchInTime5, value))
                        {
                            IsCO5TimeInvalid = true;
                            ErrorMessageEmp5 = CLOCKOUTTIME_ERR_MESSAGE;
                        }
                    }
                    (SaveTimeSheet5Command as Command).ChangeCanExecute();
                }*/
            }
        }

        [ObservableProperty] int lClockOutPerDiem = 0;
        [ObservableProperty] int emp1ClockOutPerDiem = 0;
        [ObservableProperty] int emp2ClockOutPerDiem = 0;
        [ObservableProperty] int emp3ClockOutPerDiem = 0;
        [ObservableProperty] int emp4ClockOutPerDiem = 0;
        [ObservableProperty] int emp5ClockOutPerDiem = 0;



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


        private CrewInfoDetail employee1Name;
        public CrewInfoDetail Employee1Name
        {
            get => employee1Name;
            set
            {
                //  remove already selected crew didn't worked
                // CrewInfoDetailList.Remove(value);
                // OnPropertyChanged(nameof(SelectableCrewList));

                SetProperty(ref employee1Name, value);
                if (string.IsNullOrEmpty(StartTimeEmp1))
                {
                    Console.WriteLine();
                    IsTimeEmp1Invalid = true;
                }

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
                if (string.IsNullOrEmpty(StartTimeEmp2)) { 
                    Console.WriteLine();
                    IsTimeEmp2Invalid = true;
                }
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
                if (string.IsNullOrEmpty(StartTimeEmp3))
                {
                    Console.WriteLine();
                    IsTimeEmp3Invalid = true;
                }
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
                if (string.IsNullOrEmpty(StartTimeEmp4))
                    IsTimeEmp4Invalid = true;
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
                if (string.IsNullOrEmpty(StartTimeEmp5))
                    IsTimeEmp5Invalid = true;
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
                if (string.IsNullOrEmpty(StartTimeEmp6))
                    IsTimeEmp6Invalid = true;
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



        [ObservableProperty] string totalHoursForToday;




        public ObservableCollection<CrewInfoDetail> SelectableCrewList
        {
            get
            {
                return new ObservableCollection<CrewInfoDetail>(CrewInfoDetailList);
            }
        }

        private ObservableCollection<Crewdefault> Crewtable = new ObservableCollection<Crewdefault>();



        private async void DisplayGPSSettingCommand()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup
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
                    if (Session.max_phases != null)
                    {
                        foreach (var col in table)
                        {
                            if (int.Parse(col.PhaseNumber) <= int.Parse(Session.max_phases))
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



        /// EquipmentCO = Equipment Check Out that use in Check out page

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment1CO))] EquipmentType selectedEq1Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment2CO))] EquipmentType selectedEq2Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment3CO))] EquipmentType selectedEq3Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment4CO))] EquipmentType selectedEq4Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment5CO))] EquipmentType selectedEq5Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment6CO))] EquipmentType selectedEq6Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment7CO))] EquipmentType selectedEq7Type;

        // 
        [ObservableProperty] EquipmentCO selectedEq1;
        [ObservableProperty] EquipmentCO selectedEq2;
        [ObservableProperty] EquipmentCO selectedEq3;
        [ObservableProperty] EquipmentCO selectedEq4;
        [ObservableProperty] EquipmentCO selectedEq5;
        [ObservableProperty] EquipmentCO selectedEq6;
        [ObservableProperty] EquipmentCO selectedEq7;



        /// EquipmentCO = Equipment Check Out that use in Check In page
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment1CI))] EquipmentType selectedEqIn1Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment2CI))] EquipmentType selectedEqIn2Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment3CI))] EquipmentType selectedEqIn3Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment4CI))] EquipmentType selectedEqIn4Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment5CI))] EquipmentType selectedEqIn5Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment6CI))] EquipmentType selectedEqIn6Type;

        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(Equipment7CI))] EquipmentType selectedEqIn7Type;

        [ObservableProperty] EquipmentCO selectedEqIn1;
        [ObservableProperty] EquipmentCO selectedEqIn2;
        [ObservableProperty] EquipmentCO selectedEqIn3;
        [ObservableProperty] EquipmentCO selectedEqIn4;
        [ObservableProperty] EquipmentCO selectedEqIn5;
        [ObservableProperty] EquipmentCO selectedEqIn6;
        [ObservableProperty] EquipmentCO selectedEqIn7;


        [ObservableProperty] bool isErrorMsg1Display;
        [ObservableProperty] bool isErrorMsg2Display;
        [ObservableProperty] bool isErrorMsg3Display;

        [ObservableProperty] string errorMsg1;
        [ObservableProperty] string errorMsg2;
        [ObservableProperty] string errorMsg3;

        /*string milesHourIn1;
        public string MilesHourIn1
        {
            get => milesHourIn1;
            set
            {
                if (Application.Current.Properties.ContainsKey("COMiles1"))
                {
                    var COMiles1 = Application.Current.Properties["COMiles1"] as string;
                    Console.WriteLine();
                    if (int.Parse(value) < int.Parse(COMiles1))
                    {
                        ErrorMsg1 = "Miles must be greater";
                        IsErrorMsg1Display = true;
                        Console.WriteLine();
                        return;
                    }
                }
                SetProperty(ref milesHourIn1, value);
            }
        }*/

        [ObservableProperty] string milesHourIn1;
        [ObservableProperty] string milesHourIn2;
        [ObservableProperty] string milesHourIn3;
        [ObservableProperty] string milesHourIn4;
        [ObservableProperty] string milesHourIn5;
        [ObservableProperty] string milesHourIn6;
        [ObservableProperty] string milesHourIn7;

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


        public ObservableCollection<EquipmentCO> Equipment4CO
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEq4Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }


        public ObservableCollection<EquipmentCO> Equipment5CO
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEq5Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        public ObservableCollection<EquipmentCO> Equipment6CO
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEq6Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }


        public ObservableCollection<EquipmentCO> Equipment7CO
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEq7Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        /// check in  equipment list ///
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

        public ObservableCollection<EquipmentCO> Equipment4CI
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEqIn4Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        public ObservableCollection<EquipmentCO> Equipment5CI
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEqIn5Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        public ObservableCollection<EquipmentCO> Equipment6CI
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEqIn6Type?.EquipCodeKey).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                return new ObservableCollection<EquipmentCO>(table);

            }
        }

        public ObservableCollection<EquipmentCO> Equipment7CI
        {
            get
            {
                var table = _equipmentCO.ToList();
                try
                {
                    table = _equipmentCO.Where(a => a.TypeKey == SelectedEqIn7Type?.EquipCodeKey).ToList();
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
