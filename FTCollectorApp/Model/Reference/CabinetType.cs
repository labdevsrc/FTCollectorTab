using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace FTCollectorApp.Model.Reference
{
    public  class CabinetType
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string CabinetTypeKey { get; set; }
        public string CabinetTypeCode { get; set; }
        public string TYPE_DESC { get; set; }

    }
}
