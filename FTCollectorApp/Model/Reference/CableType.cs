using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class CableType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string CodeCableKey { get; set; }
        public string CodeCableDesc { get; set; }
        public string ITSFM { get; set; }
    }
}
