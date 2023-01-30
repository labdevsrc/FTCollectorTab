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
                    Application.Current.Properties[JobNumberKey] = value.JobNumber;
                    Application.Current.Properties[JobOwnerKey] = value.OwnerName;
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

        private const int PageNumber = 3;
        private const string PageNumberKey = "PageNumber";
        private const string JobOwnerKey = "JobOwner";
        private const string JobNumberKey = "JobNumber";

        public ICommand GPSSettingCommand { get; set; }
        public ICommand ContinueCommand { get; set; }

        public ICommand ToggleCrewListCommand { get; set; }
        public ICommand ToggleJobEntriesCommand { get; set; }

        public ICommand ODOPopupCommand { get; set; }
        public ICommand DisplayEquipmentCheckInCommand { get; set; }
        public ICommand DisplayEquipmentCheckOutCommand { get; set; }

        [ObservableProperty]
        bool isDisplayCrewList = false;

        [ObservableProperty]
        bool isDisplayJobEntries = false;

        [ObservableProperty]
        string verified = string.Empty;

        private void ToggleExecute()
        {
            IsDisplayCrewList = !IsDisplayCrewList;
        }

        private void ToggleJobEntriesExecute()
        {
            IsDisplayJobEntries = !IsDisplayJobEntries;
        }


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

            ToggleCrewListCommand = new Command(
                execute: () => {
                    IsDisplayCrewList = !IsDisplayCrewList;
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return IsVerified;
                }
            );

            ODOPopupCommand = new Command(
                execute: async () =>
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new OdometerPopup());
                },
                canExecute: () =>
                {
                    Console.WriteLine();
                    return IsVerified;
                }
            );

            DisplayEquipmentCheckInCommand = new Command(
                execute: () => 
                {
                    DisplayEquipmentCheckIn();
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
                        IsVerified = true; // enable EqIn, EqOut, ODO input button
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


    }
}
