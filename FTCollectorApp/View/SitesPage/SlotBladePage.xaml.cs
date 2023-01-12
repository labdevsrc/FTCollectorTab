﻿using FTCollectorApp.ViewModel;
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
    public partial class SlotBladePage : ContentPage
    {
        public SlotBladePage()
        {
            InitializeComponent();
            BindingContext = new SlotBladePageViewModel();
        }
    }
}