using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model;
using System;
using System.Collections.Generic;

using System.Text;
using SQLite;

using Xamarin.Forms;
using System.Threading.Tasks;
using FTCollectorApp.View;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FTCollectorApp.Services;

namespace FTCollectorApp.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        //[ObservableProperty]
        
        string emailText;
        public string EmailText{
            get=> emailText;
            set
            {
                SetProperty(ref emailText, value );
                CheckEntriesCommand?.Execute(null);
                //LoginCommand.CanExecute(null);
            }
        }


        string passwordText;
        public string PasswordText
        {
            get => passwordText;
            set
            {
                SetProperty(ref passwordText, value);
                CheckEntriesCommand?.Execute(null);
                //LoginCommand.CanExecute(null);

            }
        }

        [ObservableProperty]
        string firstName;

        [ObservableProperty]
        string lastName;

        ObservableCollection<User> Users;

        public ICommand CheckEntriesCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            CheckEntriesCommand = new Command(ExecuteCheckEntriesCommand);
            LoginCommand = new Command(() => ExecuteLoginCommand());
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                //Type classname = object1.GetType();

                conn.CreateTable<User>();
                Console.WriteLine("CreateTable<User> ");
                var userdetails = conn.Table<User>().ToList();
                //conn.InsertAll(users);

                Users = new ObservableCollection<User>(userdetails);
            }
        }

        public void ExecuteCheckEntriesCommand()
        {
            try
            {
                FirstName = Users.Where(a => (a.email == EmailText) && (a.password == PasswordText)).Select(a => a.first_name).First();
                LastName = Users.Where(a => (a.email == EmailText) && (a.password == PasswordText)).Select(a => a.last_name).First();
                Console.WriteLine(FirstName + " " + LastName);

            }
            catch (Exception exception)
            {
                FirstName = "";
                LastName = "";

                Console.WriteLine(exception.ToString());

                
            }

            OnPropertyChanged(nameof(FirstName)); // update FirstName entry
            OnPropertyChanged(nameof(LastName)); // update LastName entry

        }

        private async void ExecuteLoginCommand()
        {
            if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Email or Password is wrong", "TRY AGAIN");
                Console.WriteLine();
                return;
            }

            Console.WriteLine();
            Session.uid = Users.Where(a => (a.email == EmailText) && (a.password == PasswordText)).Select(a => a.UserKey).First(); // populate uid to Static-class (session) property uid  
            Session.crew_leader = $"{FirstName} {LastName}";
           
            
            //await Shell.Current.GoToAsync($"{nameof(VerifyJobPage)}");
            Session.LoggedIn = true;
            Session.event_type = "1";
            await CloudDBService.PostJobEvent();


            await Shell.Current.GoToAsync("..");


            //await Application.Current.MainPage.Navigation.PushAsync(new VerifyJobPage());
        }
    }
}
