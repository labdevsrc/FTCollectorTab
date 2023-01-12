using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model
{
    public class Crewdefault
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int crew_leader { get; set; }
        public string team_member { get; set; }
        public string created_on { get; set; }

    }
}
