using System;
using System.Collections.Generic;
using System.Text;
using SQLite;   
namespace FTCollectorApp.Model.Reference
{
    public class FilterSize
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string data { get; set; }
        public string FtSizeKey { get; set; }
    }
}
