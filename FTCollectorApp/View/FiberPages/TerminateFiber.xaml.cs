using FTCollectorApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using FTCollectorApp.ViewModel;

namespace FTCollectorApp.View.FiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TerminateFiber : ContentPage
    {



        public TerminateFiber()
        {
            InitializeComponent();
            BindingContext = new TerminateFiberViewModel();

        }

        private async void OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}