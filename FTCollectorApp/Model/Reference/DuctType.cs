using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class DuctType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string DucTypeDesc { get; set; }
        public string DucTypeKey { get; set; }
    }
}
