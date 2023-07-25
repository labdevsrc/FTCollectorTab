﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using FTCollectorApp.Services;
using FTCollectorApp.Model;
using FTCollectorApp.ViewModel;

namespace FTCollectorApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EnterMiles
	{
		public EnterMiles(string temp)
		{
			InitializeComponent();
			Console.WriteLine();
			BindingContext = new EnterMilesVM(temp);
			Console.WriteLine();
			this.CloseWhenBackgroundIsClicked= false;
		}


	}
}