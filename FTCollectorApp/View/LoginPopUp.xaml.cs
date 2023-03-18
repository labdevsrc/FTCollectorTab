using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTCollectorApp.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPopUp
    {
        public LoginPopUp()
        {
            InitializeComponent();
            BindingContext = new LoginPopUpVM();
            this.CloseWhenBackgroundIsClicked = false;
        }
    }
}