using FTCollectorApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using Xamarin.Essentials;
using System.Net.Http.Headers;
using Plugin.Connectivity;
using FTCollectorApp.View;
using FTCollectorApp.View.SitesPage;

using FTCollectorApp.Service;
using Rg.Plugins.Popup.Services;
using FTCollectorApp.Utils;
using FTCollectorApp.ViewModel;
//using Acr.UserDialogs;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyJobPage : ContentPage
    {

        public List<string> OwnerName;
        private const int PageNumber = 3;
        private const string PageNumberKey = "PageNumber";
        private const string JobOwnerKey = "JobOwner";
        private const string JobNumberKey = "JobNumber";
        private ObservableCollection<Job> _jobdetails = new ObservableCollection<Job>();

        public VerifyJobPage()
        {
            InitializeComponent();

            BindingContext = new VerifyJobViewModel();


        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var tabbedPage = this.Parent.Parent as TabbedPage;
            tabbedPage.CurrentPage = tabbedPage.Children[1];
        }




        /*protected override async void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;
            Console.WriteLine("Connection : " + Connectivity.NetworkAccess.ToString());

            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // grab Job tables from Url https://collector.fibertrak.com/phonev4/xamarinJob.php
                _jobdetails.Clear();

                var content = await CloudDBService.GetJobFromAWSMySQLTable();

                _jobdetails = new ObservableCollection<Job>(content);
                Console.WriteLine(content);

                // push Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Job>();
                    conn.InsertAll(content);
                }
            }
            else
            {
                // because no internet network
                // Read Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Job>();
                    var jobdetails = conn.Table<Job>().ToList();
                    _jobdetails = new ObservableCollection<Job>(jobdetails);


                }
            }

            if (Application.Current.Properties.ContainsKey(PageNumberKey))
            {
                var prevPageNumber = (int)Application.Current.Properties[PageNumberKey];
            }



            // select OwnerName from Job (LINQ command)
            var ownerNames = _jobdetails.GroupBy(b => b.OwnerName).Select(g => g.First()).ToList();
            // populate to JobOwnerPicker
            jobOwnersPicker.Items.Clear();
            jobNumbersPicker.Items.Clear();
            foreach (var ownerName in ownerNames)
                jobOwnersPicker.Items.Add(ownerName.OwnerName);

            if (Application.Current.Properties.ContainsKey(JobOwnerKey))
            {
                if(jobOwnersPicker != null)
                    jobOwnersPicker.SelectedIndex = (int) Application.Current.Properties[JobOwnerKey];
            }

            if (Application.Current.Properties.ContainsKey(JobNumberKey))
            {
                if (jobNumbersPicker != null)
                    jobNumbersPicker.SelectedIndex = (int)Application.Current.Properties[JobNumberKey];
                
            }


            await PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup

            IsBusy = false;
        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // grab Job tables from Url https://collector.fibertrak.com/phonev4/xamarinJob.php
                _jobdetails.Clear();
                var content = await CloudDBService.GetJobFromAWSMySQLTable();

                _jobdetails = new ObservableCollection<Job>(content);
                Console.WriteLine(content);

                // push Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Job>();
                    conn.InsertAll(content);
                }
            }
        }


        /*private void jobOwnersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

            // get selected owner Name
            if (jobOwnersPicker.SelectedIndex != -1)
            {
                var owner = jobOwnersPicker.Items[jobOwnersPicker.SelectedIndex];

                // SELECT JobNumber from Job where OwnerName = (selected) owner (LINQ command)
                var _jobNumbergrouped = _jobdetails.Where(a => a.OwnerName == owner).GroupBy(b => b.JobNumber).Select(g => g.First()).ToList();
                jobNumbersPicker.Items.Clear();
                foreach (var jobNumbergrouped in _jobNumbergrouped)
                    jobNumbersPicker.Items.Add(jobNumbergrouped.JobNumber);

                Application.Current.Properties["JobOwner"] = jobOwnersPicker.SelectedIndex;
            }
        }

        protected override void OnDisappearing()
        {
            Console.WriteLine("OnDisappearing()");
            base.OnDisappearing();
        }

        private void jobNumbersPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedOwner = jobOwnersPicker.SelectedIndex;
            if (selectedOwner == -1)
            {
                DisplayAlert("Warning", "Select Owner First", "OK");
                Console.WriteLine("jobOwnersPicker.SelectedIndex -1 ");
                if (Application.Current.Properties.ContainsKey(JobOwnerKey))
                {
                    if (jobOwnersPicker != null)
                        jobOwnersPicker.SelectedIndex = (int)Application.Current.Properties[JobOwnerKey];
                }
            }

            var selectedJobNumber = jobNumbersPicker.SelectedIndex;
            if (selectedJobNumber == -1)
            {
                //DisplayAlert("Warning", "Select Owner First", "OK");
                Console.WriteLine("jobNumbersPicker.SelectedIndex -1 ");
                return;
            }

            var jobNumber = jobNumbersPicker.Items[jobNumbersPicker.SelectedIndex];
            var owner = jobOwnersPicker.Items[jobOwnersPicker.SelectedIndex];

            Console.WriteLine($"jobNumber selected : {jobNumber}, owner selected : {owner} ");
            // Data Binding Trial
            // var owner = SelectedOwner;

            // SELECT JobLocation, ContactName, CustomerName, CustPhoneNum from Job where OwnerName = (selected) owner
            // and JobNumber = (selected) jobNumber (LINQ command)
            var QueryOwnerJobNumber = _jobdetails.Where(a => (a.OwnerName == owner) && (a.JobNumber == jobNumber));
            jobLocation.Text = QueryOwnerJobNumber.Select(a => a.JobLocation).First();
            contactName.Text = QueryOwnerJobNumber.Select(a => a.ContactName).First();
            custName.Text = QueryOwnerJobNumber.Select(a => a.CustomerName).First();
            custPhoneNum.Text = QueryOwnerJobNumber.Select(a => a.CustomerPhone).First();
            Session.stage = QueryOwnerJobNumber.Select(a => a.stage).First();
            Session.ownerkey = QueryOwnerJobNumber.Select(a => a.OwnerKey).First();
            Session.jobkey = QueryOwnerJobNumber.Select(a => a.JobKey).First();
            Session.ownerCD = QueryOwnerJobNumber.Select(a => a.OWNER_CD).First();
            Session.countycode = QueryOwnerJobNumber.Select(a => a.CountyCode).First();
            Session.JobShowAll = QueryOwnerJobNumber.Select(a => a.ShowAll).First();
            Session.jobnum = jobNumber.ToString();
            Session.OwnerName = owner.ToString();

            Application.Current.Properties[JobNumberKey] = jobNumbersPicker.SelectedIndex;
            Application.Current.Properties[JobOwnerKey] = jobOwnersPicker.SelectedIndex;

        }

        private async void submit_Clicked(object sender, EventArgs e)
        {

            await OnSubmit();

            var speaker = DependencyService.Get<ITextToSpeech>();
            speaker?.Speak("Job verified!");

            Application.Current.Properties["PageNumber"] = 3;

            //await Navigation.PushAsync(new SiteInputPage());
            //await Navigation.PushAsync(new EquipmenReturnPage());
            await Navigation.PushAsync(new BeginWorkPage());
        }

        async Task OnSubmit()
        {

            Session.event_type = Session.JOB_VERIFIED;
            IsBusy = true;
            try
            {
                await CloudDBService.PostJobEvent();
            }
            catch
            {
                await DisplayAlert("Error", "Update JobEvent table failed", "OK");
            }
            IsBusy = false;

        }

        private async void btnGPSSetting_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new GpsDevicePopUpView()); // for Rg.plugin popup
        }*/

        private void btnFindMe(object sender, EventArgs e)
        {


            var browser = new WebView
            {
                Source = "http://ec2-52-14-97-126.us-east-2.compute.amazonaws.com/FiberTrakArcGIS/UserMap.aspx?user=" + Session.uid
            };
        }


    }
}