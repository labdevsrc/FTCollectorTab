using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class Dimensions
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string height { get; set; }
        public string depth { get; set; }
        public string width { get; set; }
        public string DimKey { get; set; }
        public string DimId { get; set; }
    }
}
