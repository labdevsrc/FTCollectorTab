using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model
{

    public class CrewRole
    {
        [AutoIncrement, PrimaryKey]
        public int id { get; set; }
        public string EmployeeName  { get; set; }
        public string EmployeeKey { get; set; }
        public string CrewLeader { get; set; }
        public string EmpRole { get; set; }
        public string IsDriver { get; set; }
    }
}
