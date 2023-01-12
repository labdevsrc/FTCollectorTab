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

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MMColorCodePopUp : PopupPage
    {
        public MMColorCodePopUp(ColorCode selectedColor) 
        {
            InitializeComponent();

            BindingContext = new FOCViewModel();
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
            }
        }

        public ICommand ColorSelectedCommand { get; set; }
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            ColorSelectedCommand?.Execute(SelectedColor);
            await PopupNavigation.Instance.PopAsync(true);
        }
        string selectedColor; // will be as KV
        private void listViewColorList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as ColorCode;
            SelectedColor = selected;
            selectedColor = selected.ColorKey;
        }

    }
}