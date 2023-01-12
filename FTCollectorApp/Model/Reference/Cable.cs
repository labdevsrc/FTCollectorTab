using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class Cable
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string CableKey { get; set; }
        public string CableId { get; set; }
        public string JobKey { get; set; }
    }
}
