using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Services;
using FTCollectorApp.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage.Popup
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimeSheetSignature
	{

        /*string data1, data2, data3, data4, data5;

        string clockInTime = data1;
        public string ClockInTime
        {
            get => clockInTime;
            set
            {
                if (clockInTime == value)
                    return;
                clockInTime = value;
                OnPropertyChanged(nameof(ClockInTime));
                ComputeSpanTime();
                OnPropertyChanged(nameof(TotalHoursForToday));
            }
        }

        string clockOutTime = data2;
        public string ClockOutTime
        {
            get => clockOutTime;
            set
            {
                if (clockOutTime == value)
                    return;
                clockOutTime = value;
                OnPropertyChanged(nameof(ClockOutTime));
                ComputeSpanTime();
                OnPropertyChanged(nameof(TotalHoursForToday));
            }
        }

        string lunchInTime;
        public string LunchInTime
        {
            get => lunchInTime;
            set
            {
                if (lunchInTime == value)
                    return;
                lunchInTime = value;
                OnPropertyChanged(nameof(LunchInTime));
                ComputeSpanTime();
                OnPropertyChanged(nameof(TotalHoursForToday));
            }
        }

        string lunchOutTime;
        public string LunchOutTime
        {
            get => lunchOutTime;
            set
            {
                if (lunchOutTime == value)
                    return;
                lunchOutTime = value;
                OnPropertyChanged(nameof(LunchOutTime));
                ComputeSpanTime();
                OnPropertyChanged(nameof(TotalHoursForToday));
            }
        }

        string totalHoursForToday;
        public string TotalHoursForToday
        {
            get => totalHoursForToday;
            set
            {
                if (totalHoursForToday == value)
                    return;
                TotalHoursForToday = value;
                OnPropertyChanged(nameof(TotalHoursForToday));
            }
        }*/
        public TimeSheetSignature(string empname,string empid , string intime, string outtime, string lunchIn, string lunchOut,
            string total)
        {


            InitializeComponent();
            BindingContext = this;

            clkIntime.Text = intime;
            clkOuttime.Text = outtime;
            lIntime.Text = lunchIn;
            lOuttime.Text = lunchOut;
            totalTime.Text = total;
            employeeName.Text = empname;
            ComputeSpanTime();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


        void ComputeSpanTime()
        {
            try
            {
                TimeSpan totaltime = DateTime.Parse(lOuttime.Text).Subtract(DateTime.Parse(clkIntime.Text)) + DateTime.Parse(clkOuttime.Text).Subtract(DateTime.Parse(lIntime.Text));
                Console.WriteLine();
                totalTime.Text = totaltime.ToString(@"hh\:mm\:ss");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
        }

        /* void ComputeSpanTime()
         {
             try
             {
                 Console.WriteLine("ClockInTime "+ ClockInTime);
                 Console.WriteLine("ClockOutTime " + ClockOutTime);
                 Console.WriteLine("LunchInTime " + LunchInTime);
                 Console.WriteLine("LunchOutTime " + LunchOutTime);
                 Console.WriteLine("TotalHoursForToday " + TotalHoursForToday);
                 TimeSpan totaltime1 = DateTime.Parse(LunchOutTime).Subtract(DateTime.Parse(ClockInTime));
                 Console.WriteLine("totaltime1 " + totaltime1.ToString());
                 TimeSpan totaltime2 = DateTime.Parse(ClockOutTime).Subtract(DateTime.Parse(LunchInTime));
                 Console.WriteLine("totaltime2 " + totaltime2.ToString());
                 TimeSpan totaltime = DateTime.Parse(LunchOutTime).Subtract(DateTime.Parse(ClockInTime)) + DateTime.Parse(ClockOutTime).Subtract(DateTime.Parse(LunchInTime));
                 Console.WriteLine();
                 TotalHoursForToday = totaltime.ToString(@"hh\:mm\:ss");
                 Console.WriteLine();
             }
             catch(Exception e)
             {
                 Console.WriteLine(e);
             }
         }*/
    }
}