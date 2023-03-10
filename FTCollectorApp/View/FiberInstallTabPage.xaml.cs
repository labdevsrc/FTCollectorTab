using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiberInstallTabPage : TabbedPage
    {
        public FiberInstallTabPage()
        {
            InitializeComponent();
        }


        private void GotoNewFiber(object sender, EventArgs e)
        {
            Console.WriteLine();
            //var tabbedPage = this.Parent.Parent as TabbedPage;
            //tabbedPage.CurrentPage = tabbedPage.Children[0];
            Console.WriteLine();
        }
    }
}