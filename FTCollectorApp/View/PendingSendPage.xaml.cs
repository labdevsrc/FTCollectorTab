using FTCollectorApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PendingSendPage : ContentPage
    {
        Dictionary<string, List<KeyValuePair<string, string>>> dictionaryTask;

        public PendingSendPage()
        {
            InitializeComponent();
            
            BindingContext = this;
            dictionaryTask = new Dictionary<string, List<KeyValuePair<string, string>>>();
        }

        protected override void OnAppearing()
        {

            // Open the file containing the data that you want to deserialize.
            using (FileStream fs = new FileStream(App.InternalStorageLocation, FileMode.Open))
            {         
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    // Deserialize the hashtable from the file and
                    // assign the reference to the local variable.
                    dictionaryTask = (Dictionary<string,List<KeyValuePair<string,string>>>)formatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
            }
            // To prove that the table deserialized correctly,
            // display the key/value pairs.
            var listPendingTask = new ObservableCollection<string>();
            foreach (var de in dictionaryTask)
                listPendingTask.Add(de.Key as string);
            Console.WriteLine($" listPendingTask {listPendingTask}");
            
            listviewPost.ItemsSource = listPendingTask;
            base.OnAppearing();
        }

        private async void syncWorkItem_Clicked(object sender, EventArgs e)
        {

        }

        private async void listviewPost_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedTask = listviewPost.SelectedItem as string;
            
            await CloudDBService.PostPendingTask(selectedTask);
        }

        private void delWorkItem_Clicked(object sender, EventArgs e)
        {

        }
    }
}