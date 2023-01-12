using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class ElectricCircuit
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string ELEC_DEV_ID { get; set; }
        public string ELEC_CIRCUIT_NAME { get; set; }

    }

}
