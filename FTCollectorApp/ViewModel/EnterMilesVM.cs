using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class EnterMilesVM : ObservableObject
    {
        [ObservableProperty] string milesTitle;
        [ObservableProperty] string errorMessage;

        string milesHours;
        public string MilesHours
        {
            get => milesHours;
            set
            {
                ErrorMessage = string.Empty;
                SetProperty(ref milesHours, value);
                (SaveCommand as Command).ChangeCanExecute();
                // Save button enable disable only for event 7,8,9
                if (Session.event_type == "7" || Session.event_type == "8" || Session.event_type == "9")
                {

                    if (PreviousMileshour.Equals("FinishJob"))
                        return;

                    try
                    {
                        if (value.Length > 0)
                        {
                            if (int.Parse(PreviousMileshour) + 1 > int.Parse(value))
                            {
                                ErrorMessage = "Miles hour must greater than " + PreviousMileshour;
                                Console.WriteLine(ErrorMessage);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine( e.ToString() );
                    }
                }

            }
        }
        public ICommand SaveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        string PreviousMileshour;
        public EnterMilesVM(string previousmileshour)
        {
            if (Session.event_type == Session.JOBEVENT_LEFT_FOR_JOB) MilesTitle = (Session.JobCnt > 1) ? "Left For Job #" + Session.JobCnt+ " Mileage" : "Left For Job Mileage";
            else if (Session.event_type == Session.JOBEVENT_ARRIVED_AT_JOB) MilesTitle = (Session.JobCnt > 1) ? "Arrived at Job #" + Session.JobCnt + " Mileage" : "Arrived at Job Mileage";
            else if (Session.event_type == Session.JOBEVENT_LEFT_JOB) MilesTitle = (Session.JobCnt > 1) ? "Left Job #" + Session.JobCnt + " Mileage" : "Left Job Mileage";
            else if (Session.event_type == Session.JOBEVENT_ARRIVED_AT_YARD) MilesTitle = (Session.JobCnt > 1) ? "Arrive At Yard of Job #" + Session.JobCnt + " Mileage" : "Arrive At Yard Mileage";

            PreviousMileshour = previousmileshour;
            Console.WriteLine(  );
            SaveCommand = new Command(
                execute: async () =>
                {
                    try
                    {
                        await CloudDBService.PostJobEvent(0, Session.curphase, MilesHours, Session.uid.ToString(), Session.event_type);
                        await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();

                        if (previousmileshour.Equals("FinishJob"))
                        {
                            Console.WriteLine();
                            MessagingCenter.Send<EnterMilesVM, string>(this, "FinishJob", MilesHours);
                            Console.WriteLine();
                        }
                        else
                            MessagingCenter.Send<EnterMilesVM, string>(this, "ConfirmMiles", MilesHours);
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
                   return MilesHours != null;
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
