using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model
{
    public class Site
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        // A Site is pretty much anything that gets a tag number, 

        public string SiteKey { get; set; }
        public string JobKey { get; set; }
        public string JobNumber { get; set; }
        public string OwnerKey { get; set; }
        public string OWNER_CD { get; set; }
        public string TagNumber { get; set; }

        public string SiteId { get; set; }

        public string SiteName { get; set; }
        //public string Stage { get; set; }

        public string SiteTypeKey { get; set; } // points into code_site_type
        public string SiteTypeDesc { get; set; } // points into code_site_type
        
        
        //public string DirOfTravel { get; set; } // points into code_site_type
        //public string CabinetTypeKey { get; set; } // points into code_cabinet_type table
        //public string BuildingTypeKey { get; set; }  // points into code_building_type
        public string LONGITUDE { get; set; }

        public string LATITUDE { get; set; }

        //public string GpsTime { get; set; }


        public string CreatedBy { get; set; }
    }
}
