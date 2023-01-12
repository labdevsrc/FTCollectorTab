using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FTCollectorApp.Model.AWS;
using FTCollectorApp.Model.Reference;
using FTCollectorApp.Service;
using SQLite;
using Xamarin.Forms;

namespace FTCollectorApp.ViewModel
{
    public partial class UnSyncViewModel : ObservableObject
    {

        UnSyncTaskList selectedTask;
        public UnSyncTaskList SelectedTask{
            get => selectedTask;
            set
            {
                SetProperty(ref (selectedTask), value);
                Console.WriteLine();
                if (value != null)
                {
                    Console.WriteLine();
                    //SendCommand?.Execute(null);
                    IsButtonSendEnable = true;
                    OnPropertyChanged(nameof(IsButtonSendEnable));
                }
            }
          }


        bool isButtonSendEnable = false;
        public bool IsButtonSendEnable
        {
            get => isButtonSendEnable;
            set
            {
                SetProperty(ref isButtonSendEnable, value);
            }
        }

        public ObservableCollection<UnSyncTaskList> TaskList
        {

            get
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<UnSyncTaskList>();
                    var table = conn.Table<UnSyncTaskList>().ToList();
                    Console.WriteLine();
                    return new ObservableCollection<UnSyncTaskList>(table);
                }
            }
        }

        public ICommand SendCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public UnSyncViewModel()
        {
            Console.WriteLine();
            SendCommand = new Command(SendExecute);
            RemoveCommand = new Command(RemoveExecute);
        }


        async void SendExecute()
        //async void ExecuteSendCommand()
        {
            Console.WriteLine();
            if (SelectedTask.ajaxTarget.Equals(Constants.ajaxSaveDuctTrace))
            {

                ObservableCollection<a_fiber_segment> AFSTable = new ObservableCollection<a_fiber_segment>();

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<a_fiber_segment>();
                    var table2 = conn.Table<a_fiber_segment>().ToList();
                    AFSTable = new ObservableCollection<a_fiber_segment>(table2);

                    conn.CreateTable<UnSyncTaskList>();
                    var tasklistTBL = conn.Table<UnSyncTaskList>();
                    Console.WriteLine();



                    ObservableCollection<KeyValuePair<string, string>> DuctTraceParam = new ObservableCollection<KeyValuePair<string, string>>();
                    try
                    {

                        // Populate KeyValue and upload 
                        foreach (var afs in AFSTable)
                        {
                            Console.WriteLine();
                            DuctTraceParam.Clear();

                            // populate column by column
                            foreach (var item in afs.GetType().GetProperties())
                            {

                                DuctTraceParam.Add(new KeyValuePair<string, string>(item.Name, item.GetValue(afs)?.ToString()));
                                Console.WriteLine();
                            }
                            Console.WriteLine();

                            var result = await CloudDBService.Post_a_fiber_segment(DuctTraceParam);
                            //if result = OK
                            afs.SyncStatus = "SYNC";

                        }

                        // update task list, assume API upload Post_a_fiber_segment
                        var data = tasklistTBL.Where(s => s.TableID == SelectedTask.TableID);
                        foreach(var col in data)
                        {
                            col.Status = "SYNCED";
                        }
                        conn.Update(data); // update a_fiber_segment table in SQLite
                        OnPropertyChanged(nameof(TaskList));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception : " + e.ToString());
                    }
                    // assume  all task sent
                    // TaskList.Clear();

                    Console.WriteLine();
                }
            }
        }


        async void RemoveExecute()
        {
            Console.WriteLine(  );
            if (SelectedTask.ajaxTarget.Equals(Constants.ajaxSaveDuctTrace))
            {
                bool Ok = await Application.Current.MainPage.DisplayAlert("Remove", "Are you Sure ?", "Remove", "Cancel");
                if (Ok)
                {
                    // assume  all task sent
                    TaskList.Clear();
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
