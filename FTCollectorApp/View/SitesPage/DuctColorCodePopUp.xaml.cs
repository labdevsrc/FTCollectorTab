using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using FTCollectorApp.ViewModel;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Model;
using System.Windows.Input;
using Rg.Plugins.Popup.Pages;
using System.Collections.ObjectModel;
using SQLite;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuctColorCodePopUp : PopupPage
    {
        public ObservableCollection<ColorCode> DuctColorCode
        {
            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<ColorCode>();
                    var table = conn.Table<ColorCode>().ToList();
                    return new ObservableCollection<ColorCode>(table);
                }
            }
        }
        public DuctColorCodePopUp(ColorCode selectedColor) 
        {
            InitializeComponent();

            BindingContext = this;
            SelectedColor = selectedColor;
        }

        private ColorCode _selectedColor;
        public ColorCode SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                OnPropertyChanged(nameof(SelectedColor));
                Console.WriteLine();
            }
        }

        public ICommand ColorSelectedCommand { get; set; }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            ColorSelectedCommand?.Execute(SelectedColor); //with Mode=TwoWay, no need this ?
            Console.WriteLine();
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(true);
        }

        /*string selectedColor;

        private void listViewColorList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as ColorCode;
            SelectedColor = selected;
            selectedColor = selected.ColorKey;
        }*/
    }
}