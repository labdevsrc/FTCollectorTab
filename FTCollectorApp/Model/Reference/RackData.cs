using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class RackData
    {
        const string AWS_NOTSYNCED = "aws_notsynced";

        public string  id { get; set; }
        public string uid { get; set; }
        public string time { get; set; }
        public string OWNER_CD { get; set; }
        public string jobnum { get; set; }
        public string owner_key { get; set; }
        public string jobkey { get; set; }
        public string tag { get; set; }
        public string sitekey { get; set; }
        public string front_back { get; set; }
        public string type { get; set; }

        public string racknumber { get; set; }
        public string orientation { get; set; }
        public string xpos { get; set; }
        public string ypos { get; set; }
        public string manufacturer_key { get; set; }
        public string manufacturer { get; set; }
        public string model_key { get; set; }

        public string model { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string depth { get; set; }
        public string SYNC_STS { get; set; } = AWS_NOTSYNCED;

    }
}
