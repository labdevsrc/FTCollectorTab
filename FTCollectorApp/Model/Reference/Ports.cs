using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class Ports
    {
        [AutoIncrement, PrimaryKey]
        public int id { get; set; }
        public string PortKey { get; set; }
        public string site { get; set; }
        public string site_key { get; set; }
        public string rack_key { get; set; }
        public string chassis_key { get; set; }
        public string slot_or_blade_number { get; set; }

        public string port_number { get; set; }


        public string owner_key { get; set; }
        public string xmt_rcv { get; set; }
        
    }
}
