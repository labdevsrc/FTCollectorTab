using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{

    public class UnitOfMeasure
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string UOMKey { get; set; }
        public string UOMUnit { get; set; }
        public string UOM_Abv { get; set; }
    }
}
