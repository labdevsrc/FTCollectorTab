using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class Tracewaretag
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string SiteTagNumber { get; set; }
        public string SiteKey { get; set; }

        public string SiteOwnerKey { get; set; }
    }
}
