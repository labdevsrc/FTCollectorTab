using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class CrewInfoDetail
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int TeamUserKey { get; set; } = 1;
        public string FullName { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string LunchInTime { get; set; } = string.Empty;
        public string LunchOutTime { get; set; } = string.Empty;
        public string EndOfDayTime { get; set; } = string.Empty;

        public string LaborClass { get; set; }
        public string Phase { get; set; }
        public int PerDiem { get; set; } = 0;

    }
}
