using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using System.Web;
using FTCollectorApp.Model;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class PullCableViewModel : ObservableObject
    {
        AFiberCable selectedFiberCable;
        [ObservableProperty] bool newFiber;
        public AFiberCable SelectedFiberCable
        {
            get => selectedFiberCable;
            set
            {
                SetProperty(ref selectedFiberCable, value);
                if (value.CableIdDesc != null)
                {
                    NewFiber = value.CableIdDesc.Equals("New");
                    OnPropertyChanged(nameof(NewFiber));
                }
            }
        }
        public ObservableCollection<CableType> CableTypeList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<CableType>();
                    var table = conn.Table<CableType>().ToList();
                    if (SelectedFiberCable != null)
                        table = conn.Table<CableType>().Where(a => a.CodeCableKey == SelectedFiberCable.CableType).ToList();
                    return new ObservableCollection<CableType>(table);
                }
            }
        }


        public PullCableViewModel()
        {
            NewCableCommand = new Command(
                execute: () =>
                {
                    //todo
                }
            );
        }
        public ICommand NewCableCommand { get; set; }

    }
}
