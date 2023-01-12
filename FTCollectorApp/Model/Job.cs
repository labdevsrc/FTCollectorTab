using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model
{
    public class Job
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string OwnerKey { get; set; }
        public string OWNER_CD { get; set; }
        public string stage { get; set; }
        public string OwnerName { get; set; }
        public string JobNumber { get; set; }
        public string JobKey { get; set; }
        public string JobLocation { get; set; }
        public string ContactName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CountyCode { get; set; }

        public string ShowAll { get; set; }
        public string table_name { get; set; }

        public int UserId { get; set; } //FK

        public string TimesheetStatus { get; set; }
    }
}
