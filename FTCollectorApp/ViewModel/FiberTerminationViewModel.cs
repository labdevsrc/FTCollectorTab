using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;
using FTCollectorApp.Model.Reference;
using System.Web;
using FTCollectorApp.Model;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using FTCollectorApp.View.SitesPage;

namespace FTCollectorApp.ViewModel
{
    public partial class FiberTerminationViewModel : ObservableObject
    {
        public FiberTerminationViewModel()
        {
            ShowPopupCommand = new Command(async _ => await ExecuteShowPopupCommand());
            ColorSelectedCommand = new Command(ductcolor => ExecuteColorSelectedCommand(ductcolor as ColorCode));
            Session.current_page = "cable";
        }

        // color popup implementation - start

        public ICommand ShowPopupCommand { get; set; }
        public ICommand ColorSelectedCommand { get; set; }

        [ObservableProperty]
        ColorCode selectedColor;

        private Task ExecuteShowPopupCommand()
        {
            var popup = new DuctColorCodePopUp(SelectedColor)
            {
                ColorSelectedCommand = ColorSelectedCommand
            };
            return Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);
        }
        private void ExecuteColorSelectedCommand(ColorCode ductcolor)
        {
            SelectedColor = ductcolor;
            Console.WriteLine();
        }
        // color popup implementation - end


        public ObservableCollection<ConduitsGroup> DuctConduitDatas
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ConduitsGroup>();

                    var table = conn.Table<ConduitsGroup>().Where(b => b.HosTagNumber == Session.tag_number).ToList();
                    foreach (var col in table)
                    {
                        col.DuctSize = HttpUtility.HtmlDecode(col.DuctSize);
                        col.WhichDucts = col.Direction + " " + col.DirCnt;
                    }
                    Console.WriteLine("WhichDuctLists ");
                    return new ObservableCollection<ConduitsGroup>(table);
                }
            }
        }

        public ObservableCollection<AFiberCable> aFiberCableList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<AFiberCable>();

                    var table = conn.Table<AFiberCable>().Where(a => a.OwnerKey == Session.ownerkey).ToList();
                    foreach (var col in table)
                    {
                        col.CableIdDesc = HttpUtility.HtmlDecode(col.CableIdDesc); // should use for escape char "
                    }
                    Console.WriteLine();
                    return new ObservableCollection<AFiberCable>(table);
                }
            }
        }

        public ObservableCollection<UnitOfMeasure> UnitOfMeasures
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<UnitOfMeasure>();
                    var table = conn.Table<UnitOfMeasure>().ToList();
                    return new ObservableCollection<UnitOfMeasure>(table);
                }
            }
        }
    }
}
