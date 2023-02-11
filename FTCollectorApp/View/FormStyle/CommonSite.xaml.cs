using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FTCollectorApp.View.FormStyle;
using FTCollectorApp.ViewModel;

namespace FTCollectorApp.View.FormStyle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CommonSite : ContentPage
    {
        public CommonSite()
        {
            InitializeComponent();
            Console.WriteLine();
            BindingContext = new CommonsiteVM();
        }

        StackLayout parent;

        /*public void AddEntries(object sender, EventArgs e)
        {
            // Define a new button
            Button newButton = new Button { Text = "New Button" };

            // Creating a binding
            newButton.SetBinding(Button.CommandProperty, new Binding("ViewModelProperty"));

            // Set the binding context after SetBinding method calls for performance reasons
            newButton.BindingContext = CommonsiteVM;

            // Set StackLayout in XAML to the class field
            parent = newEntries;

            // Add the new button to the StackLayout
            parent.Children.Add(newButton);
        }*/
    }


}