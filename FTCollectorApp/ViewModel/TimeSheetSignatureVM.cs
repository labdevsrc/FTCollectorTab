using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{


    public partial class TimeSheetSignatureVM : ObservableObject
    {
        public ICommand SaveCommand { get; set; }

        [ObservableProperty] string clockIntime;
        [ObservableProperty] string clockOutTimeForm;
        [ObservableProperty] string lunchInTime;
        [ObservableProperty] string lunchOutTime;
        [ObservableProperty] string totalHoursForToday;
        [ObservableProperty] string empID;

        public TimeSheetSignatureVM()
        {



            /*ClockIntime = intime;
                ClockOutTimeForm = outtime;
                LunchInTime = lunchIn;
                LunchOutTime = lunchOut;
                TotalHoursForToday = total;
                empID = empid;*/

            SaveCommand = new Command(
                execute: async () =>
                {

                    // do nothing
                    /*if (!intime.Equals(ClockIntime))
                    {
                        Session.event_type = "15";
                        await CloudDBService.PostTimeSheet(empID, ClockIntime, "", 0);
                    }
                    if (!intime.Equals(ClockOutTimeForm))
                    {
                        Session.event_type = "16";
                        await CloudDBService.PostTimeSheet(empID, ClockOutTimeForm, "", 0);
                    }
                    if (!intime.Equals(LunchInTime))
                    {
                        Session.event_type = "14";
                        await CloudDBService.PostTimeSheet(empID, LunchInTime, "", 0);
                    }
                    if (!intime.Equals(LunchOutTime))
                    {
                        Session.event_type = "13";
                        await CloudDBService.PostTimeSheet(empID, LunchOutTime, "", 0);
                    }*/
                    // if (!outtime.Equals(ClockOutTimeForm) ||
                    //         !lunchIn.Equals(LunchInTime) || !lunchOut.Equals(LunchOutTime) || !total.Equals(TotalHoursForToday))

                },
                canExecute: () =>
                {
                    //if (!intime.Equals(ClockIntime) || !outtime.Equals(ClockOutTimeForm) ||
                    //!lunchIn.Equals(LunchInTime) || !lunchOut.Equals(LunchOutTime) || !total.Equals(TotalHoursForToday)
                    //)
                    //    return true;
                    return true;
                }
              );

        }
    }
}
