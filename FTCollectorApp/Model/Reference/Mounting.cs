using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class Mounting
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string MountingKey { get; set; }
        public string MountingType { get; set; }
    }
}
