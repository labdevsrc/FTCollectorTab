using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using SQLite;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public class SheathMarkViewModel : BaseViewModel
    {
        ObservableCollection<ConduitsGroup> ConduitsGroupListTable;
        ObservableCollection<ColorCode> ColorHextList;
        public ObservableCollection<AFiberCable> aFiberCableList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<AFiberCable>();
                    // City of Port St Lucie for Demo purpose
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
        public ObservableCollection<ConduitsGroup> DuctConduitDatas
        {
            get
            {
                var table = ConduitsGroupListTable.Where(b => b.HosTagNumber == Session.tag_number).ToList();
                foreach (var col in table)
                {
                    col.DuctSize = HttpUtility.HtmlDecode(col.DuctSize);
                    col.WhichDucts = col.Direction + " " + col.DirCnt;
                }
                Console.WriteLine("WhichDuctLists ");
                return new ObservableCollection<ConduitsGroup>(table);
            }
        }

        public SheathMarkViewModel()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<ConduitsGroup>();
                var table1 = conn.Table<ConduitsGroup>().ToList();
                ConduitsGroupListTable = new ObservableCollection<ConduitsGroup>(table1);

                conn.CreateTable<ColorCode>();
                var table2 = conn.Table<ColorCode>().ToList();
                ColorHextList = new ObservableCollection<ColorCode>(table2);
            }

            ClosePageCommand = new Command(async _ => await ExecuteSaveCloseCommand());
        }

        private async Task ExecuteSaveCloseCommand()
        {
            //var Result = await CloudDBService.PostSheathMark();
            //GetResultCommand?.Execute("OK");
            await Application.Current.MainPage.Navigation.PopAsync(true);
        }

        public ICommand ClosePageCommand { get; }


    }
}
