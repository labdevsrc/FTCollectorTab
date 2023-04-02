using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.Service;
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
        [ObservableProperty] string milesHours;
        public ICommand SaveCommand { get; set; }
        public EnterMilesVM()
        {
            if (Session.event_type == Session.JOBEVENT_LEFT_FOR_JOB) MilesTitle = "Left For Job Mileage";
            else if (Session.event_type == Session.JOBEVENT_ARRIVED_AT_JOB) MilesTitle = "Arrived at Job Mileage";
            else if (Session.event_type == Session.JOBEVENT_LEFT_JOB) MilesTitle = "Left Job Mileage";
            else if (Session.event_type == Session.JOBEVENT_ARRIVED_AT_YARD ) MilesTitle = "Arrive At Yard Mileage";

            SaveCommand = new Command(
                execute: async () =>
                {
                    try
                    {
                        await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(),
                            0, Session.phases, MilesHours, Session.uid.ToString());


                        await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();

                        MessagingCenter.Send<EnterMilesVM>(this, "ConfirmMiles");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        await Application.Current.MainPage.DisplayAlert("Error", "Check Network Connection ", "Back");

                    }
                },
               canExecute: () =>
               {
                   Console.WriteLine();
                   return true;
               }
              );
        }



    }
}
