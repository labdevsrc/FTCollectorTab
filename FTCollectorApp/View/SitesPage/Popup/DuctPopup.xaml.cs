using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FTCollectorApp.ViewModel;

namespace FTCollectorApp.View.SitesPage.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DuctPopup 
    {
        public DuctPopup()
        {
            InitializeComponent();
            BindingContext = new DuctViewModel();
        }
    }
}