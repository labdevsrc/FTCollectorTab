using FTCollectorApp.Model.Reference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseColorPopUp : ContentPage
    {
        #region Fields

        private static List<ColorCode> _colorcode;
        private ColorCode _selectedcolorcode;

        #endregion Fields

        #region Constructors

        public ChooseColorPopUp(ColorCode selectedColorCode)
        {
            InitializeComponent();
            if (_colorcode == null || !_colorcode.Any())
            {
                LoadCountries();
            }
            VisibleCountries = new ObservableCollection<ColorCode>(_colorcode);

            SelectedColorCode = selectedColorCode;
            //CommonCountriesList.SetBinding(ListView.ItemsSourceProperty, new Binding(nameof(VisibleCountries), source: this));
            //CurrentCountryControl.SetBinding(CountryControl.CountryProperty, new Binding(nameof(SelectedColorCode), source: this));
        }

        #endregion Constructors

        #region Properties

        public ICommand CountrySelectedCommand { get; set; }

        public ObservableCollection<ColorCode> VisibleCountries { get; }

        public ColorCode SelectedColorCode
        {
            get => _selectedcolorcode;
            set
            {
                _selectedcolorcode = value;
                OnPropertyChanged(nameof(SelectedColorCode));
            }
        }

        #endregion Properties

        #region Private Methods

        private void CloseBtn_Clicked(object sender, EventArgs e)
        {
            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        private void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            CountrySelectedCommand?.Execute(SelectedColorCode);
            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }

        private void LoadCountries()
        {
            //this is not Task, because it's really fast
            //var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            _colorcode = new List<ColorCode>();
            /*var isoCountries = CountryUtils.GetCountriesByIso3166();
            _colorcode.AddRange(isoCountries.Select(c => new ColorCode
            {
                CountryCode = phoneNumberUtil.GetCountryCodeForRegion(c.TwoLetterISORegionName).ToString(),
                CountryName = c.EnglishName,
                FlagUrl = $"https://hatscripts.github.io/circle-flags/flags/{c.TwoLetterISORegionName.ToLower()}.svg",
            }));*/
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            /*VisibleCountries.Clear();
            var filteredCountries = string.IsNullOrWhiteSpace(SearchBar.Text)
                ? _countries
                : _countries.Where(country => country.CountryName.Contains(SearchBar.Text, StringComparison.InvariantCultureIgnoreCase));
            filteredCountries.ForEach(сountry => VisibleCountries.Add(сountry));*/
        }

        private void CommonCountriesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SelectedColorCode = e.SelectedItem as ColorCode;
        }

        #endregion Private Methods
    }
}