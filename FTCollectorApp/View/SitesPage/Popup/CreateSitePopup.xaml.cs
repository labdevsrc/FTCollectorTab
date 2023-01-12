using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FTCollectorApp.View.SitesPage.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateSitePopup
    {
        public CreateSitePopup()
        {
            Console.WriteLine();
            InitializeComponent();
            Console.WriteLine();
            BindingContext = new CreateSitePopupVM();
            this.CloseWhenBackgroundIsClicked = false;
        }
    }
}