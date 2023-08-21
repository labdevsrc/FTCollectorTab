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
	public partial class PullBoxQuestions2 : ContentPage
	{
		public PullBoxQuestions2 ()
		{
			InitializeComponent ();
			BindingContext = new PullBoxQuestions2VM();
            Session.PullBoxPage2CreateCnt = 1;
        }
	}
}