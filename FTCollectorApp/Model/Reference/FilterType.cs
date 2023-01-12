using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class FilterType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string FilterTypeKey { get; set; }
        public string FilterTypeDesc { get; set; }
    }
}
