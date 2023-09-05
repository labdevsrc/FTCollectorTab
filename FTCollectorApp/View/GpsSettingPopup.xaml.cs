
using FTCollectorApp.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;


namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GpsSettingPopup 
    {
        public GpsSettingPopup()
        {
            InitializeComponent();
            BindingContext = new GpsSettingPopupVM();
            this.CloseWhenBackgroundIsClicked = false;
        }
    }
}