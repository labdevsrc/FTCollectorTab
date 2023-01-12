using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.TraceFiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TraceFiberMenu : ContentPage
    {
        public TraceFiberMenu()
        {
            InitializeComponent();
        }

        private void btnStart_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TracingMenuPage());
        }



        private async void btnSelectDuct(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FiberOpticCablePage());
        }
    }
}