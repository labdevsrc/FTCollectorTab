using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Model.Reference
{
    public class CrewInfoDetail
    {
        public int id { get; set; }
        public int TeamUserKey { get; set; }
        public string FullName { get; set; }
        public string StartTime { get; set; }
        public string LunchInTime { get; set; }
        public string LunchOutTime { get; set; }
        public string EndOfDayTime { get; set; }

        public string LaborClass { get; set; }
        public int PerDiem { get; set; }
    }
}
