using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class ReelId
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string ReelKey { get; set; }
        public string ReelNumber { get; set; }
        public string JobNum { get; set; }
    }
}
