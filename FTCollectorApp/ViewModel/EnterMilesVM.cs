﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        [ObservableProperty] string errorMessage;

        string milesHours;
        public string MilesHours
        {
            get => milesHours;
            set
            {
                ErrorMessage = string.Empty;
                SetProperty(ref milesHours, value);
                if (Session.event_type == "7" || Session.event_type == "8" || Session.event_type == "9")
                {
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
                (SaveCommand as Command).ChangeCanExecute();
            }
        }
        public ICommand SaveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        string PreviousMileshour;
        public EnterMilesVM(string previousmileshour)
        {
            if (Session.event_type == Session.JOBEVENT_LEFT_FOR_JOB) MilesTitle = "Left For Job Mileage";
            else if (Session.event_type == Session.JOBEVENT_ARRIVED_AT_JOB) MilesTitle = "Arrived at Job Mileage";
            else if (Session.event_type == Session.JOBEVENT_LEFT_JOB) MilesTitle = "Left Job Mileage";
            else if (Session.event_type == Session.JOBEVENT_ARRIVED_AT_YARD ) MilesTitle = "Arrive At Yard Mileage";

            PreviousMileshour = previousmileshour;
            Console.WriteLine(  );
            SaveCommand = new Command(
                execute: async () =>
                {
                    try
                    {

                        await CloudDBService.PostJobEvent(DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(),
                            0, Session.curphase, MilesHours, Session.uid.ToString());


                        await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();

                        MessagingCenter.Send<EnterMilesVM, string>(this, "ConfirmMiles", MilesHours);
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
