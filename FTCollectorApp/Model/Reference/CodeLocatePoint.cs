using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class CodeLocatePoint
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string IdLocatePoint { get; set; }
        public string description { get; set; }
    }
}
