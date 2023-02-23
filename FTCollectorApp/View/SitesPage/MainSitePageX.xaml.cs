using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FTCollectorApp.ViewModel;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSitePageX : ContentPage
    {
        public MainSitePageX()
        {
            InitializeComponent();
            Console.WriteLine();
            BindingContext = new MainSitePageXVM();
        }

    }


}