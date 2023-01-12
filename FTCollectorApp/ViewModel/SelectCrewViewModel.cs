using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using FTCollectorApp.Model;
using FTCollectorApp.View;
using SQLite;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public class SelectCrewViewModel : ObservableObject
    {

        public ObservableCollection<CrewRole> CrewRoles
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CrewRole>();
                    var table = conn.Table<CrewRole>().ToList();
                    return new ObservableCollection<CrewRole>(table);
                }
            }
        }

        CrewRole selectedCrewRole;
        public CrewRole SelectedCrewRole
        {
            get => selectedCrewRole;
            set
            {
                selectedCrewRole = value;
                OnPropertyChanged(nameof(SelectedCrewRole));
                OnPropertyChanged(nameof(CrewRoles));
            }
        }


        public ICommand ShowPopupCommand { get; }
        public ICommand CrewSelectedCommand { get; }

        public SelectCrewViewModel()
        {
            CrewSelectedCommand = new Command(crew => ExecuteCrewSelectedCommand(crew as string));
        }


        private Task ExecuteShowPopupCommand()
        {
            var popup = new StartTimePopupView(SelectedCrewRole.EmployeeKey)
            {
                FinishTimeSetCommand = CrewSelectedCommand
            };
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }

        private void ExecuteCrewSelectedCommand(string crew)
        {
            OnPropertyChanged(nameof(CrewRoles));
            //SelectedCountry = crew;
        }


    }
}
