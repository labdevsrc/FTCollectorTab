using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class ExcludeSite
    {
        [PrimaryKey,AutoIncrement]
        public int id{ get; set; }
        public string SiteName { get; set; }
        public string property_id { get; set; }
        public string intersection { get; set; }
        public string roadway { get; set; }
        public string direction_of_travel { get; set; }
        public string orientation { get; set; }
        public string site_street_address { get; set; }
        public string description { get; set; }

        public string depth { get; set; }

        public string height { get; set; }
        public string width { get; set; }
        public string weight { get; set; }
        public string diameter { get; set; }
        public string mounting { get; set; }
        public string has_apron { get; set; }
        public string apron_width { get; set; }
        public string apron_height { get; set; }
        public string gravel_bottom { get; set; }
        public string lid_pieces { get; set; }

        public string has_ground_rod { get; set; }
        public string ground_resistance { get; set; }
        public string has_key { get; set; }
        public string key_type { get; set; }

        public string serial_number { get; set; }
        public string filter_count { get; set; }
        public string sun_shield { get; set; }

        public string installed { get; set; }
        public string distance_to_eotl { get; set; }
        public string bucket_truck { get; set; }
        public string lane_closure_required { get; set; }
        public string clear_zone_ind { get; set; }
        public string has_power_disconnect { get; set; }

        public string has_comms { get; set; }
        public string comms_provider { get; set; }
        public string UDS_owner { get; set; }
        public string UDS_name { get; set; }
        public string uds_tag_key { get; set; }

        public string electric_site_key { get; set; }

        public string ELEC_DEV_ID { get; set; }

        public string ELEC_ADMIN_1_CD { get; set; }
        public string ELEC_ADMIN_2_CD { get; set; }
        public string ELEC_ADMIN_3_CD { get; set; }

        public string electric_company { get; set; }
        public string electric_account_number { get; set; }
        public string electric_circuit_name { get; set; }

        public string electric_demarc_type { get; set; }

        public string electric_service_type { get; set; }

    }
}
