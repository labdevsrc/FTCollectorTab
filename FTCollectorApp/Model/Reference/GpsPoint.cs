using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class GpsPoint
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string MaxId { get; set; }
    }
}
