using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class CableStructure
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string CableKey { get; set; }
        public string buffer_type { get; set; }
    }
}
