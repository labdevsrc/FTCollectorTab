using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.AWS
{
    public class AWS_Site
    {
        public int id { get; set; }
        public int AWSid { get; set; }
        public int AWSid2 { get; set; }
        public int AWSKey { get; set; }
        public string record_state { get; set; }

        public string owner_key { get; set; }
        public string job_key { get; set; }
        public string owner { get; set; }
        public string OWNER_CD { get; set; }
        public string tag_number { get; set; }
        public string site_id { get; set; }
        public string SiteName { get; set; }

        public string LONGITUDE { get; set; }
        public string LATITUDE { get; set; }
        public string altitude { get; set; }

        public string gps_offset_bearing { get; set; }
        public string gps_offset_distance { get; set; }
        public string gps_offset_longitude { get; set; }
        public string gps_offset_latitude { get; set; }
        public string gps_time { get; set; }
        public string gps__accuracy { get; set; }
        public string updated_on { get; set; }
        public string updated_by { get; set; }
    }
}
