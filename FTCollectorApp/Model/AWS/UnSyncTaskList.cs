using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.AWS
{
    public class UnSyncTaskList
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string SyncedTime { get; set; }
        public string TargetTable { get; set; }
        public string rowCount { get; set; }
        public string ajaxTarget { get; set; }
        public string taskName { get; set; }
        public string Status { get; set; } = "UNSYNC";
        public string TableID { get; set; }
        public string TaskIdList { get; set; }
        //public List<int> TaskIdList { get; set; }
    }
}
