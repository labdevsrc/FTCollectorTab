using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class Manufacturer
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string ManufName { get; set; }
        public string ManufId { get; set; }
        public string ManufKey { get; set; }

    }
}
