using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model
{
    public class CodeSiteType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string? CodeKey { get; set; }

        public string? SiteType { get; set; }
        public string? MajorType { get; set; }
        public string? MinorType { get; set; }
        public string? ITSFM { get; set; }
    }
}
