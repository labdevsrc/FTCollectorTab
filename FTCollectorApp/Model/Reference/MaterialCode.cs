using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class MaterialCode
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string MaterialId { get; set; }
        public string CodeDescription { get; set; }
        public string MaterialKey { get; set; }
    }
}
