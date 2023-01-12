using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class BuildingType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string BuildingTypeKey { get; set; }
        public string TYPE_DESC { get; set; }
    }
}
