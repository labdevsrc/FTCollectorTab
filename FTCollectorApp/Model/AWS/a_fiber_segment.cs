using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.AWS
{
    public class a_fiber_segment
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int AWSid { get; set; }
        public int AWSid2 { get; set; }
        public string record_state { get; set; }
        public string owner_key { get; set; }
        public string job { get; set; }
        public string job_key { get; set; }
        public string OWNER_CD { get; set; }

        public string cable_id { get; set; }
        public string cable_id_key { get; set; }
        public string stage { get; set; }
        public string cable_type { get; set; }
        public string install_method { get; set; }
        public string from_site { get; set; }
        public string from_site_key { get; set; }

        public string from_site_duct { get; set; }
        public string from_site_duct_key { get; set; }

        public string from_site_duct_direction { get; set; }
        public string from_site_duct_direction_count { get; set; }
        public string sheath_out { get; set; }
        public string to_site { get; set; }
        public string to_site_key { get; set; }

        public string to_site_duct { get; set; }
        public string to_site_duct_key { get; set; }

        public string to_site_duct_direction { get; set; }
        public string to_site_duct_direction_count { get; set; }
        public string sheath_in { get; set; }
        public string cable_length { get; set; }
        public string geo_length { get; set; }
        public string uom { get; set; }
        public string created_on { get; set; }
        public int created_by { get; set; }
        public string SyncStatus { get; set; }
    }
}
