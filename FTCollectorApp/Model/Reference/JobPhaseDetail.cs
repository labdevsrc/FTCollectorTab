using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{

    public class JobPhaseDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string JobPhaseKey { get; set; }
        public string PhaseNumber { get; set; }
        public string Description { get; set; }

        public string NumDesc { get; set; } = string.Empty;

    }
}
