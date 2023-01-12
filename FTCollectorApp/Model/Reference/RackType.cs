using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class RackType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string RackTypeKey { get; set; }
        public string RackMaterialDesc { get; set; }
    }
}
