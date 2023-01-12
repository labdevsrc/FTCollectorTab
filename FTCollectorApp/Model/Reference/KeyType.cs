using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class KeyType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string KeyTypeKey { get; set; }
        public string KeyTypeDesc { get; set; }
    }
}
