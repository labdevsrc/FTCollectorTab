using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class JobSubmittal
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string JobSubKey { get; set; }
        public string JobSubJobId { get; set; }
        public string JobSubManuf { get; set; }
        public string JobSubModel { get; set; }
        public string JobSubDesc { get; set; }
        public string JobSubPayItem { get; set; }
    }
}
