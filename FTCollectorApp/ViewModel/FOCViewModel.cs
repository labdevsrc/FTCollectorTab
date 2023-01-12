using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.View.SitesPage;
using SQLite;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public class FOCViewModel : BaseViewModel
    {


        public FOCViewModel()
        {
            ShowPopupCommand = new Command(async _ => await ExecuteShowPopupCommand());
            ColorSelectedCommand = new Command(ductcolor => ExecuteColorSelectedCommand(ductcolor as ColorCode));
        }
        /// get selected color from popup - start
        private ColorCode _selectedColor;
        public ColorCode SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }


        private Manufacturer _selectedManuf;
        public Manufacturer SelectedManuf
        {
            get => _selectedManuf;
            set => SetProperty(ref _selectedManuf, value);
        }


        public ICommand ShowPopupCommand { get; }
        public ICommand ColorSelectedCommand { get; }
        private Task ExecuteShowPopupCommand()
        {
            var popup = new MMColorCodePopUp(SelectedColor)
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
        /// get selected color from popup - end
        /// 
        public ObservableCollection<ColorCode> MMColorCode
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ColorCode>();
                    var table = conn.Table<ColorCode>().ToList();
                    foreach(var col in table)
                    {
                        if (col.ColorName != "Aqua" && col.ColorName != "Orange")
                            table.Remove(col);
                    }
                    Console.WriteLine();
                    return new ObservableCollection<ColorCode>(table);
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
                    return new ObservableCollection<CableType>(table);
                }
            }
        }


        public ObservableCollection<Sheath> SheathList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Sheath>();
                    var table = conn.Table<Sheath>().ToList();
                    return new ObservableCollection<Sheath>(table);
                }
            }
        }


        public ObservableCollection<ReelId> ReelIdList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ReelId>();
                    //var rwTable = conn.Table<ReelId>().ToList();
                    var table = conn.Table<ReelId>().Where(a => a.JobNum == Session.jobnum).ToList();
                    return new ObservableCollection<ReelId>(table);
                }
            }
        }

        public ObservableCollection<Manufacturer> ManufacturerList
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Manufacturer>();
                    var table = conn.Table<Manufacturer>().ToList();
                    foreach (var col in table)
                    {
                        col.ManufName = HttpUtility.HtmlDecode(col.ManufName); // should use for escape char "
                    }
                    return new ObservableCollection<Manufacturer>(table);
                }
            }
        }

        public ObservableCollection<ModelDetail> ModelDetailList
        {
            get
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ModelDetail>();

                    var table = conn.Table<ModelDetail>().Where(a => a.ManufKey == SelectedManuf.ManufKey).OrderBy(b => b.ModelNumber).ToList();

                    foreach (var col in table)
                    {
                        col.ModelNumber = HttpUtility.HtmlDecode(col.ModelNumber); // should use for escape char 
                        col.ModelDescription = HttpUtility.HtmlDecode(col.ModelDescription); // should use for escape char 
                        if (col.ModelCode1 == "")
                            col.ModelCode1 = col.ModelCode2;
                        if (col.ModelCode2 == "")
                            col.ModelCode2 = col.ModelCode1;
                    }
                    Console.WriteLine();
                    return new ObservableCollection<ModelDetail>(table);
                }
            }
        }
    }
}
