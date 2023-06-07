
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
    public partial class CrewCRUDVM : ObservableObject
    {
        [ObservableProperty] bool isBusy = false;
        List<CrewInfoDetail> CrewInfoDetailList = new List<CrewInfoDetail>();


        //[ObservableProperty] CrewInfoDetail SelectedCrewMember;

        public CrewCRUDVM()
        {
            GetEvent18Prop();

            SaveCommand = new Command(
                execute: async () =>
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
                }
            );

            RemoveCommand = new Command(
                execute: async () =>
                {

                }
            );

        }

        public ICommand SaveCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        [ICommand]
        async void Back()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
        }

        public ObservableCollection<CrewInfoDetail> ObsEvent18
        {            
            get {
                return new ObservableCollection<CrewInfoDetail>(CrewInfoDetailList);
            }
        }

        async void GetEvent18Prop()
        {
            // when employee 
            IsBusy = true;

            var StartTimeEvent18 = await CloudDBService.GetEvent18Time();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                //conn.CreateTable<CrewInfoDetail>();
                conn.DeleteAll<CrewInfoDetail>();
                conn.InsertAll(StartTimeEvent18);

                var event18 = conn.Table<CrewInfoDetail>();

                CrewInfoDetailList.Clear();
                foreach (var col in event18)
                {
                    CrewInfoDetailList.Add(new CrewInfoDetail
                    {
                        FullName = col.FullName,
                        TeamUserKey = col.TeamUserKey,
                        StartTime = col.StartTime
                    });
                }

                await Console.Out.WriteLineAsync("CrewInfoDetailList " + CrewInfoDetailList.ToString());
            }
            OnPropertyChanged(nameof(CrewInfoDetailList));
            IsBusy = false;
        }
    }
}
