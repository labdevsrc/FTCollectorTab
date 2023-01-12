using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class OwnerRoadway
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string OR_Id { get; set; }
        public string OR_Owner { get; set; }
        public string OR_Roadway { get; set; }
    }
}
