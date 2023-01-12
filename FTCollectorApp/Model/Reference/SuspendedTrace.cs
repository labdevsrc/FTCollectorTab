using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class SuspendedTrace
    {

        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string OwnerKey { get; set; }
        public string JobNumber { get; set; }
        public string FromDuctSite { get; set; }
        public string FromDuctSiteKey { get; set; }
        public string FromDuctKey { get; set; }
    }
}
