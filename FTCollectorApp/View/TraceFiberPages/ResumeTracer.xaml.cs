using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using FTCollectorApp.Model;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.ViewModel;

namespace FTCollectorApp.View.TraceFiberPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResumeTracer : ContentPage
    {

        public ResumeTracer()
        {
            InitializeComponent();
            BindingContext = new ResumeTraceViewModel();
        }

    }
}