using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class County
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string CountyId { get; set; }
        public string CountyName { get; set; }
    }
}
