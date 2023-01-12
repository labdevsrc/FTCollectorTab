using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class DuctInstallType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string DuctInstallKey { get; set; }
        public string DuctInstallDesc{ get; set; }
    }
}
