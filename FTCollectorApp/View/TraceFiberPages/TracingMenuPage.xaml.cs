using FTCollectorApp.ViewModel;
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
    public partial class TracingMenuPage : ContentPage
    {
        public TracingMenuPage()
        {
            InitializeComponent();
            BindingContext = new TracingMenuViewModel();
        }

        private void btnResume_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ResumeTracer());
        }

        private void btnStartTracing_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DuctTracePage()); 
        }
    }
}