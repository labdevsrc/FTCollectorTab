using FTCollectorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.ViewPortrait
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPageP : ContentPage
	{
		public MainPageP ()
		{
			InitializeComponent ();
			BindingContext = new SplashDownloadViewModel();
		}
	}
}