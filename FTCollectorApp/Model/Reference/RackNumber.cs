using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class RackNumber
    {
        public const string AWS_NOTSYNCED = "aws_notsynced";
        public const string  AWS_SYNCED = "aws_synced";
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string RackNumKey { get; set; }
        public string OWNER_CD { get; set; }
        public string Racknumber { get; set; }
        public string SiteId { get; set; }
        public string RackType { get; set; }
        public int temp { get; set; }

        public string SyncStatus { get; set; } = AWS_NOTSYNCED;
    }
}
