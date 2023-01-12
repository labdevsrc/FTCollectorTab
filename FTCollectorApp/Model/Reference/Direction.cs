using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class Direction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string DirDesc { get; set; }
        public string DirKey { get; set; }
    }
}
