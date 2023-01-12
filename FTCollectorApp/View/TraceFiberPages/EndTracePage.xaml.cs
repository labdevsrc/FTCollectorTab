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
    public partial class EndTracePage : ContentPage
    {
        public EndTracePage()
        {
            InitializeComponent();
            BindingContext = new EndTraceViewModel();
        }
    }
}
