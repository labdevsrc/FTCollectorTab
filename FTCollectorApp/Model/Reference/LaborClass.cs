using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class LaborClass
    {
        [PrimaryKey,AutoIncrement]
        public int id { get; set; }
        public string?  text { get; set; }
        public string? LaborClassId { get; set; }
        public string? abbreviation { get; set; }
    }
}
