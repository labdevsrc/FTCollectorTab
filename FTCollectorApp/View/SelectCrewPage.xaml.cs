using FTCollectorApp.Model;
using FTCollectorApp.Service;
using Newtonsoft.Json;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCrewPage : ContentPage
    {
        List<int> crewlist = new List<int>();
        private ObservableCollection<User> Users = new ObservableCollection<User>();
        private ObservableCollection<Crewdefault> Crewtable = new ObservableCollection<Crewdefault>();
        ArrayList crewnamelist = new ArrayList();
       public SelectCrewPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void btnFinish_Clicked(object sender, EventArgs e)
        {
            //Session.event_type = Session.CrewAssembled;
            //await CloudDBService.PostJobEvent();             // need to update event_type 
            //await Navigation.PushAsync(new StartTimePage()); // simple page change - vicky

            ///////////// add Rajib's code - start //////////////////////
            ArrayList crewnamelist = new ArrayList();
            crewnamelist.Add(Session.crew_leader);
            String timenow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int x = 1;
            try
            {

                String OWNER_CD = Session.ownerCD;

                String name1 = "";
                String name2 = "";
                String name3 = "";
                String name4 = "";
                String name5 = "";
                String name6 = "";
                //
                if (employeePicker1.SelectedIndex != -1)
                {
                    var ind = employeePicker1.SelectedIndex;
                    name1 = crewlist[ind].ToString();
                    crewnamelist.Add(employeePicker1.Items[employeePicker1.SelectedIndex]);
                }
                if (employeePicker2.SelectedIndex != -1)
                {
                    var ind = employeePicker2.SelectedIndex;
                    name2 = crewlist[ind].ToString();
                    crewnamelist.Add(employeePicker2.Items[employeePicker2.SelectedIndex]);
                }
                if (employeePicker3.SelectedIndex != -1)
                {
                    var ind = employeePicker3.SelectedIndex;
                    name3 = crewlist[ind].ToString();
                    crewnamelist.Add(employeePicker3.Items[employeePicker3.SelectedIndex]);
                }
                if (employeePicker4.SelectedIndex != -1)
                {
                    var ind = employeePicker4.SelectedIndex;
                    name4 = crewlist[ind].ToString();
                    crewnamelist.Add(employeePicker4.Items[employeePicker4.SelectedIndex]);
                }
                if (employeePicker5.SelectedIndex != -1)
                {
                    var ind = employeePicker5.SelectedIndex;
                    name5 = crewlist[ind].ToString();
                    crewnamelist.Add(employeePicker5.Items[employeePicker5.SelectedIndex]);
                }
                if (employeePicker6.SelectedIndex != -1)
                {
                    var ind = employeePicker6.SelectedIndex;
                    name6 = crewlist[ind].ToString();
                    crewnamelist.Add(employeePicker6.Items[employeePicker6.SelectedIndex]);
                }

                Session.sessioncrew = crewnamelist;

                //                
                String diem1 = "";
                String diem2 = "";
                String diem3 = "";
                String diem4 = "";
                String diem5 = "";
                String diem6 = "";
                String driver11 = "";
                String driver12 = "";
                String driver13 = "";
                String driver14 = "";
                String driver15 = "";
                String driver16 = "";

                IsBusy = true;
                await CloudDBService.SaveCrewdata( OWNER_CD, "1",name1, name2, name3, name4, name5, name6, diem1, diem2, diem3, diem4, diem5, diem6, driver11, driver12, driver13, driver14, driver15, driver16);
                IsBusy = false;
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    var q = string.Format("delete from Crewdefault where crew_leader = " + Session.uid + ";");
                    conn.Execute(q);
                    if (employeePicker1.SelectedIndex != -1)
                    {
                        Crewdefault crew = new Crewdefault();
                        crew.crew_leader = Session.uid;
                        var ind = employeePicker1.SelectedIndex;
                        crew.team_member = crewlist[ind].ToString();
                        crew.created_on = timenow;
                        x = conn.Insert(crew);
                    }
                    if (employeePicker2.SelectedIndex != -1)
                    {
                        Crewdefault crew = new Crewdefault();
                        crew.crew_leader = Session.uid;
                        var ind = employeePicker2.SelectedIndex;
                        crew.team_member = crewlist[ind].ToString();
                        crew.created_on = timenow;
                        x = conn.Insert(crew);
                    }
                    if (employeePicker3.SelectedIndex != -1)
                    {
                        Crewdefault crew = new Crewdefault();
                        crew.crew_leader = Session.uid;
                        var ind = employeePicker3.SelectedIndex;
                        crew.team_member = crewlist[ind].ToString();
                        crew.created_on = timenow;
                        x = conn.Insert(crew);
                    }
                    if (employeePicker4.SelectedIndex != -1)
                    {
                        Crewdefault crew = new Crewdefault();
                        crew.crew_leader = Session.uid;
                        var ind = employeePicker4.SelectedIndex;
                        crew.team_member = crewlist[ind].ToString();
                        crew.created_on = timenow;
                        x = conn.Insert(crew);
                    }
                    if (employeePicker5.SelectedIndex != -1)
                    {
                        Crewdefault crew = new Crewdefault();
                        crew.crew_leader = Session.uid;
                        var ind = employeePicker5.SelectedIndex;
                        crew.team_member = crewlist[ind].ToString();
                        crew.created_on = timenow;
                        x = conn.Insert(crew);
                    }
                    if (employeePicker6.SelectedIndex != -1)
                    {
                        Crewdefault crew = new Crewdefault();
                        crew.crew_leader = Session.uid;
                        var ind = employeePicker6.SelectedIndex;
                        crew.team_member = crewlist[ind].ToString();
                        crew.created_on = timenow;
                        x = conn.Insert(crew);
                    }
                    conn.CreateTable<Crewdefault>();
                    Console.WriteLine("CreateTable<Crewdefault> ");
                    var crewdetails = conn.Table<Crewdefault>().ToList();
                    Crewtable = new ObservableCollection<Crewdefault>(crewdetails);
                    

                    foreach (var s in Crewtable)
                    {

                        Console.WriteLine(s.id + "- " + s.crew_leader + " " + s.team_member + " " + s.created_on);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }


            if (x == 1)
            {
                //  DisplayAlert("Success!!!", "Crew members save successfully.", "Ok");
                //Navigation.PopAsync();
                // await Navigation.PushAsync(new BeginWorkPage());
                await Navigation.PushAsync(new StartTimePage());
            }
            else
            {
                // DisplayAlert("Something went wrong!!!", "Please try again", "ERROR");
            }
        }

        private void btnLogOut_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
        //database user table

        protected override async void OnAppearing()
        {
            Console.WriteLine("[SelectCrewPage] Connectivity : " + Connectivity.NetworkAccess);

            txtCrewLeader.Text = Session.crew_leader;

            // https://stackoverflow.com/questions/40458842/internet-connectivity-listener-in-xamarin-forms
            // https://www.youtube.com/watch?v=aA-sA0ACum0
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityHandler;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await GetEndUserFromAWSMySQLTable();
                await GetcrewDefaultFromAWSMySQLTable();
            }
            else
            {
                // because no internet network
                // Read Job Tables to local SQLite. Model is in Model.Job
                // with using(SQLiteConnection) we didn't have to do conn.close()
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<User>();
                    var users = conn.Table<User>().ToList();
                    Users = new ObservableCollection<User>(users);

                    conn.CreateTable<Crewdefault>();
                    Console.WriteLine("CreateTable<Crewdefault> ");
                    var crewdetails = conn.Table<Crewdefault>().ToList();
                    Crewtable = new ObservableCollection<Crewdefault>(crewdetails);

                }
            }
            //

            var empNames = Users.GroupBy(b => b.first_name).Select(g => g.First()).ToList();
            // populate to employees
            foreach (var empName in empNames)
            {
                crewlist.Add(empName.UserKey);
                employeePicker1.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker2.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker3.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker4.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker5.Items.Add(empName.first_name + " " + empName.last_name);
                employeePicker6.Items.Add(empName.first_name + " " + empName.last_name);
            }

            base.OnAppearing();
        }

        private async Task GetEndUserFromAWSMySQLTable()
        {
            Users.Clear();

            // grab End User tables from Url https://collector.fibertrak.com/phonev4/xamarinLogin.php
            // var response = await httpClient.GetStringAsync(Constants.GetEndUserTableUrl);
            var users = await CloudDBService.GetEndUserFromAWSMySQLTable();
            Users = new ObservableCollection<User>(users);
            Console.WriteLine(users);

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<User>();
                conn.InsertAll(users);
            }
        }

        private async Task GetcrewDefaultFromAWSMySQLTable()
        {
            Crewtable.Clear();

            // grab Cre Table tables from Url https://collector.fibertrak.com/phonev4/getcrewtable.php
            var crewdefaults = await CloudDBService.GetCrewDefaultFromAWSMySQLTable();
            Crewtable = new ObservableCollection<Crewdefault>(crewdefaults);
            Console.WriteLine(crewdefaults);

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Crewdefault>();
                conn.InsertAll(crewdefaults);
            }
        }

        private async void OnConnectivityHandler(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await GetEndUserFromAWSMySQLTable();
                await GetcrewDefaultFromAWSMySQLTable();
            }
        }
    }
}