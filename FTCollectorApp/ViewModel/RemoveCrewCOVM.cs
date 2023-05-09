using CommunityToolkit.Mvvm.ComponentModel;

using FTCollectorApp.Model;
using FTCollectorApp.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class RemoveCrewCOVM : ObservableObject
    {
        public ICommand SaveCommand { get; set; }
        public ICommand BackCommand { get; set; }

        [ObservableProperty] string crewMemberName;

        [ObservableProperty] string errorMessage;
        [ObservableProperty] string errmessage;
        [ObservableProperty] bool isTimeInvalid;
        static string INVALID_TIME_ERR_MESSAGE = "Invalid HH:MM";


        string? clockOutCrewMember = string.Empty;
        public string? ClockOutCrewMember
        {
            get => clockOutCrewMember;
            set
            {
                SetProperty(ref clockOutCrewMember, value);
                Console.WriteLine(  value);
                ErrorMessage = INVALID_TIME_ERR_MESSAGE;
                (IsTimeInvalid, ErrorMessage) = IsTimeGapNegativeNew(value);

            }
        }
        static int TIMEGAP_MINIMUM = 1;
        int cnt = 0;
        (bool, string) IsTimeGapNegativeNew(string currTime)
        {
            errmessage = INVALID_TIME_ERR_MESSAGE;
            string pattern = "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";


            if (currTime.Length > 1)
            {
                bool temp = !Regex.IsMatch(currTime.ToString(), pattern, RegexOptions.CultureInvariant);
                Console.WriteLine("RemoveCrewCOVM#1" + " " + cnt);
                cnt++;
                return (temp, errmessage);
            }
            Console.WriteLine("RemoveCrewCOVM#4" + " " + cnt);
            cnt++;
            return (false, errmessage);
        }

        public RemoveCrewCOVM(string crewmember)
        {
            string[] CrewProp = crewmember.Split('#');
            var CrewNumber = CrewProp[0];
            CrewMemberName = CrewProp[1];
            var CrewId = CrewProp[2];

            SaveCommand = new Command(
                execute: async () =>
                {
                    try
                    {
                        await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();

                        MessagingCenter.Send<RemoveCrewCOVM, string>(this, "Crew#"+CrewNumber+"RemoveTime", ClockOutCrewMember);
                        Console.WriteLine("ClockOutCrewMember : " + ClockOutCrewMember);
                        Session.event_type = "18";  // employee removed from job 
                        await CloudDBService.PostJobEvent(0, Session.curphase, "", CrewId);
                        
                        Session.event_type = "16";  // Timesheet Clockout 
                        await CloudDBService.PostTimeSheet(CrewId, ClockOutCrewMember, Session.curphase, 0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        await Application.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Back");

                    }
                },
               canExecute: () =>
               {
                   Console.WriteLine();
                   return !IsTimeInvalid;
               }
            );

            BackCommand = new Command(
                execute: async () =>
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
                }
            );
        }
    }
}
