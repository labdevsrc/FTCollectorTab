
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
using FTCollectorApp.Services;
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
            GetSelectableCrewMemberList();

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

        async void GetSelectableCrewMemberList()
        {
            IsBusy = true;

            // get Crew Member's name that already occupied
            var occupiedMember = await CloudDBService.GetOccupiedMember();
            Console.WriteLine("#1");
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                // dump to SQLite
                conn.CreateTable<CrewInfoDetail>();
                conn.DeleteAll<CrewInfoDetail>();
                conn.InsertAll(occupiedMember);
                var OccupiedMemberTable = conn.Table<CrewInfoDetail>().ToList();
               // get all selectable crew from User table
                conn.CreateTable<User>();
                var table2 = conn.Table<User>();

                Console.WriteLine("#2");
                // populate CrewInfoDetailList with selectablecrewmember 
                foreach (var col in OccupiedMemberTable)
                {
                    Console.WriteLine("#111 FullName : " + col.FullName + ", CrewID" + col.TeamUserKey);
                }



                foreach (var col in table2)
                {
                    if (col.UserKey.Equals(Session.uid))
                        continue;

                    var full_name = col.first_name + " " + col.last_name;
                    bool memberOccupied = false;
                    foreach (var col1 in OccupiedMemberTable)
                    {
                        if (col1.FullName.Equals(full_name)) // when a crew member already occupied
                        {
                            Console.WriteLine("#3 match FullName : " + col1.FullName + ", CrewID" + col1.TeamUserKey);
                            memberOccupied = true;
                            break;
                        }
                    }

                    if (!memberOccupied)
                    {
                        CrewInfoDetailList.Add(new CrewInfoDetail
                        {
                            FullName = full_name,
                            TeamUserKey = col.UserKey,
                            StartTime = ""
                        });
                    }


                    /*try
                    {

                        if (OccupiedMemberTable.Contains(new CrewInfoDetail
                        {
                            FullName = full_name,
                            TeamUserKey = col.UserKey
                        }))
                        {
                            Console.WriteLine("#2-A " + full_name);
                            continue;
                        }
                        else
                            CrewInfoDetailList.Add(new CrewInfoDetail
                            {
                                FullName = full_name,
                                TeamUserKey = col.UserKey,
                                StartTime = ""
                            });

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("#2-E " + e.ToString() );
                    }*/
                    Console.WriteLine("#2-EE ");
                }
                Console.WriteLine("#3");

                /*foreach (var col in CrewInfoDetailList)
                {
                    try
                    {
                        var colInstance = OccupiedMemberTable.Where(a => a.FullName == col.FullName).First();
                        if (colInstance != null)
                        {
                            CrewInfoDetailList.
                            CrewInfoDetailList.Remove(col); // remove occupied crew member
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("#3-E");
                    }
                }*/
                Console.WriteLine("#4");
            }
            OnPropertyChanged(nameof(SelectableCrewMember));
            IsBusy = false;
        }

        async void UpdateCrewList()
        { 



           if (Session.event_type == "17" || Session.event_type == "18")
           {
                Console.WriteLine();
                var event18 = await CloudDBService.GetEvent18Time();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CrewChangeInfoDetail>();
                    conn.DeleteAll<CrewChangeInfoDetail>();
                    conn.InsertAll(event18);
                    var table1 = conn.Table<CrewChangeInfoDetail>();



                    // before : ~ 21 June
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
