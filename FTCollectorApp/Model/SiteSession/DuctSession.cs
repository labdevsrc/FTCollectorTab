using System;
using System.Collections.Generic;
using System.Text;
using FTCollectorApp.Model.Reference;
using SQLite;

namespace FTCollectorApp.Model.SiteSession
{
    public class DuctSession
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string ConduitKey { get; set; }

        public string Direction { get; set; }
        public string DirCnt { get; set; }
        public string HosTagNumber { get; set; }
        public string HostType { get; set; }
        public string HostTypeKey { get; set; }

        public string WhichDucts { get; set; }

        public string OwnerKey { get; set; }
        public string InUsePercent { get; set; }
        public string HostSiteKey { get; set; }


        // Duct Type - Material
        public string DuctTypeKey { get; set; }
        public string DuctTypeDesc { get; set; }

        // Color atribut
        public string ColorKey { get; set; } = "1";
        public string ColorHex { get; set; } = "#0000FF";
        public string ColorName { get; set; } = "Blue";

        public string Description { get; set; }

        // Diameter / Duct Size
        public string DuctSizeKey { get; set; } 
        public string DuctSizeDesc{ get; set; }

        // Compass Direction 
        public string CompasKey { get; set; }
        public string DirDesc { get; set; }

        public bool IsDuctPlug { get; set; }
        public bool IsOpen { get; set; }
        public bool HasTraceWire { get; set; }
        public int PullTapeKey { get; set; }

    }

}
