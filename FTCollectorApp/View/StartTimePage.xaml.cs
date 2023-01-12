using FTCollectorApp.Model;
using FTCollectorApp.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartTimePage : ContentPage
    {
        ArrayList crewnames = new ArrayList();
        public StartTimePage()
        {
            InitializeComponent();
            BindingContext = new SelectCrewViewModel();            
            crewnames = (ArrayList)Session.sessioncrew;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("[StartTimePage] OnAppearing ");

            stackLayout.Children.Clear();

            int cntCrew = 0;
            foreach (var cname in crewnames)
            {
                cntCrew++;

                Button btnCrew1 = new Button { Text = "Start Time for " + cname };
                stackLayout.Children.Add(btnCrew1);
                btnCrew1.Clicked += (s, e) => OnClicked(cname.ToString());
            }
            Session.crewCnt = cntCrew;

        }

        private async void OnClicked(string crewname)
        {

            await PopupNavigation.Instance.PushAsync(new StartTimePopupView(crewname));

        }
    }
}