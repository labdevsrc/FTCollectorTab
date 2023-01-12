using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace FTCollectorApp.Model.Reference
{
    public class ModelDetail
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string ManufKey { get; set; }
        public string ModelKey { get; set; }
        public string ModelCode1 { get; set; }
        public string ModelCode2 { get; set; }
        public string ModelNumber { get; set; }
        public string ModelType { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string depth { get; set; }
        public string ModelDescription { get; set; }
        public string PictUrl { get; set; }
        
    }
}
