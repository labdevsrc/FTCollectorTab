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
        public string JobNumber { get; set; } = string.Empty;
        public string JobKey { get; set; }
        public string JobLocation { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CountyCode { get; set; }

        public string ShowAll { get; set; }
        public string table_name { get; set; }

        public int UserId { get; set; } //FK

        public string TimesheetStatus { get; set; }
        public string JobPhases { get; set; } = "1";
    }
}
