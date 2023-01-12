using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class Orientation
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string OrientationDetail { get; set; }
        public string OrientationHV { get; set; }
    }
}
