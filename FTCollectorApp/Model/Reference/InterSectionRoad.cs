using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Model.Reference
{
    public class InterSectionRoad
    {
        public int id { get; set; }
        public string IntersectionId { get; set; }
        public string IntersectionName { get; set; }
        public string IntersectionKey { get; set; }
        public string major_roadway { get; set; }
        public string minor_roadway { get; set; }
        public string OWNER_CD { get; set; }

        public string MajorRoadwayName { get; set; }

        public string MinorRoadwayName { get; set; }
    }
}
