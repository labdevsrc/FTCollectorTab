using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class FiberInstallType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string FbrInstallKey { get; set; }
        public string FbrInstallDesc { get; set; }
    }
}
