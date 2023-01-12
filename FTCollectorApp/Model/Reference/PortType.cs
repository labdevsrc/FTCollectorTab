using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class PortType
    {
        [AutoIncrement, PrimaryKey]
        public int id { get; set; }
        public string CodeKey { get; set; }
        public string TextType { get; set; }
        public string TXRX { get; set; }
    }
}
