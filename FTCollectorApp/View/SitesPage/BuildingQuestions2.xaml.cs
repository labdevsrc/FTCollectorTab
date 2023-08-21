using FTCollectorApp.Model;
using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View.SitesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuildingQuestions2 : ContentPage
    {
        /*public BuildingSitePageView(string minorType, string tagNumber)
        {
            InitializeComponent();
            var MajorMinorType = $"Building - {minorType}";
            BindingContext = new BuildingSitePageViewModel(MajorMinorType, tagNumber);
        }*/

        public BuildingQuestions2()
        {
            InitializeComponent();
            BindingContext = new BuildingQuestions2VM();
            Session.BuildingPage2CreateCnt = 1;
        }



    }
}