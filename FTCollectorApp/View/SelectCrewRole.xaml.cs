using FTCollectorApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCrewRole : ContentPage
    {
        List<string> EmployeeNames = new List<string>();
        List<string> EmployeeRoles = new List<string>();
        List<string> EmployeeIsDriver = new List<string>();

        public CrewRole SelectedCrew
        {
            get => _crewRoleList;
            set
            {
                if (_crewRoleList == value)
                    return;
                _crewRoleList = value;
                OnPropertyChanged(nameof(SelectedCrew));
            }
        }
        CrewRole _crewRoleList;

        string CrewLeader;

        public SelectCrewRole(string crewLeader)
        {
            InitializeComponent();
            CrewLeader = crewLeader;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            EmployeeNames.Clear();
            empName1 = CrewLeader;
            EmployeeNames.Add(CrewLeader);
        }

        string empKey1, empName1, empRole1, IsDriver1;
        string empKey2, empName2, empRole2, IsDriver2;
        string empKey3, empName3, empRole3, IsDriver3;
        string empKey4, empName4, empRole4, IsDriver4;

        private void btnFinish_Clicked(object sender, EventArgs e)
        {
            List<CrewRole> crewRoles = new List<CrewRole>();

            crewRoles.Add(new CrewRole {
                EmployeeName = empName1, CrewLeader = Session.uid.ToString()
            });
            crewRoles.Add(new CrewRole
            {
                EmployeeName = empName2, EmpRole = empRole2,
                CrewLeader = Session.uid.ToString()
            });
            crewRoles.Add(new CrewRole
            {
                EmployeeName = empName3, EmpRole = empRole3,
                CrewLeader = Session.uid.ToString()
            });
            crewRoles.Add(new CrewRole
            {
                EmployeeName = empName4, EmpRole = empRole4,
                CrewLeader = Session.uid.ToString()
            });

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<CrewRole>();
                conn.DeleteAll<CrewRole>();
                conn.InsertAll(crewRoles);
            }
        }




        private void OnEmpSelect(object sender, EventArgs e)
        {
            if (employeePicker1.SelectedIndex != -1)
            {
                empName2 = employeePicker1.SelectedItem.ToString();
                EmployeeNames.Add(empName2);

            }
            else
                return;

            if (employeePicker2.SelectedIndex != -1)
            {
                empName3 = employeePicker2.SelectedItem.ToString();
                EmployeeNames.Add(empName3);
            }
            else
                return;

            if (employeePicker3.SelectedIndex != -1)
            {
                empName4 = employeePicker3.SelectedItem.ToString();
                EmployeeNames.Add(empName4);
            }
            else
                return;
        }

        private void OnRoleSelect(object sender, EventArgs e)
        {
            if (laborPicker1.SelectedIndex != -1)
            {
                empRole2 = laborPicker1.SelectedItem.ToString();
                EmployeeRoles.Add(empRole2);

            }
            else
                return;

            if (laborPicker2.SelectedIndex != -1)
            {
                empRole3 = laborPicker2.SelectedItem.ToString();
                EmployeeRoles.Add(empRole3);
            }
            else
                return;

            if (laborPicker3.SelectedIndex != -1)
            {
                empRole4 = laborPicker3.SelectedItem.ToString();
                EmployeeRoles.Add(empRole4);
            }
            else
                return;
        }

        private void OnIsDriver(object sender, EventArgs e)
        {
            if (driverPicker1.SelectedIndex != -1)
            {
                IsDriver2 = driverPicker1.SelectedItem.ToString();
                EmployeeIsDriver.Add(IsDriver2);

            }
            else
                return;

            if (driverPicker2.SelectedIndex != -1)
            {
                IsDriver3= driverPicker2.SelectedItem.ToString();
                EmployeeIsDriver.Add(IsDriver3);
            }
            else
                return;

            if (driverPicker3.SelectedIndex != -1)
            {
                IsDriver4 = driverPicker3.SelectedItem.ToString();
                EmployeeIsDriver.Add(IsDriver4);
            }
            else
                return;
        }
    }
}