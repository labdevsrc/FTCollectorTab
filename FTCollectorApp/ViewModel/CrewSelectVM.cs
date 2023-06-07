
using System;
using System.Collections.Generic;
using System.Text;
using Rg.Plugins.Popup.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Xamarin.Forms;
using System.Windows.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using SQLite;
using System.Collections.ObjectModel;

namespace FTCollectorApp.ViewModel
{
    public partial class CrewSelectVM : ObservableObject
    {
        [ObservableProperty] bool isBusy = false;
        List<CrewInfoDetail> CrewInfoDetailList = new List<CrewInfoDetail>();
        CrewInfoDetail selectedCrewMember;
        public CrewInfoDetail SelectedCrewMember
        {
            get => selectedCrewMember;
            set
            {
                SetProperty(ref selectedCrewMember, value);
                (SaveCommand as Command).ChangeCanExecute();
            }
        }

        string CrewNumber = string.Empty;

        //[ObservableProperty] CrewInfoDetail SelectedCrewMember;

        public CrewSelectVM(string crewnumber)
        {
            GetCrewList();

            SaveCommand = new Command(
                execute: async () =>
                {
                    //var idx = CrewInfoDetailList.FindIndex(a => a.FullName == Employee1Name?.FullName);
                    //CrewInfoDetailList[idx].StartTime = value;

                    // send crewmember selected

                    if (SelectedCrewMember != null)
                    {
                        MessagingCenter.Send<CrewSelectVM, string>(this, "CrewMemberSelected",
                            crewnumber + "#" + SelectedCrewMember.TeamUserKey
                            + "#" + SelectedCrewMember.FullName
                            + "#" + SelectedCrewMember.StartTime);

                        using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                        {
                            conn.CreateTable<User>();

                        }
                    }
                    await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
                },
                canExecute: () =>
                {
                    return SelectedCrewMember != null;

                }
            );


            UpdateCrewList();

        }

        public ICommand SaveCommand { get; set; }

        [ICommand]
        async void Back()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
        }

        public ObservableCollection<CrewInfoDetail> SelectableCrewMember
        {            
            get {
                if (CrewInfoDetailList?.Count > 0)
                    return new ObservableCollection<CrewInfoDetail>(CrewInfoDetailList);
                else
                    return new ObservableCollection<CrewInfoDetail>();
            }
        }

        async void GetCrewList()
        {
            IsBusy = true;


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<User>();
                var table2 = conn.Table<User>();

                foreach (var col in table2)
                {
                    if (col.UserKey.Equals(Session.uid))
                        continue;

                    CrewInfoDetailList.Add(new CrewInfoDetail
                    {
                        FullName = col.first_name + " " + col.last_name,
                        TeamUserKey = col.UserKey,
                        StartTime = ""
                    });

                    Console.WriteLine();
                }
            }
            OnPropertyChanged(nameof(SelectableCrewMember));
            IsBusy = false;
        }

        async void UpdateCrewList()
        { 


           if (Session.event_type == "17" || Session.event_type == "18")
           {
                var event18 = await CloudDBService.GetEvent18Time();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CrewChangeInfoDetail>();
                    conn.InsertAll(event18);
                    var table1 = conn.Table<CrewChangeInfoDetail>();


                    foreach (var col in CrewInfoDetailList)
                    {
                        var ClockIn = "";
                        try
                        {
                            var colInstance = table1.Where(a => a.FullName == col.FullName).First();
                            if (colInstance != null)
                            {
                                col.StartTime = colInstance.StartTime;
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                OnPropertyChanged(nameof(SelectableCrewMember));
            }

        }
    }
}
