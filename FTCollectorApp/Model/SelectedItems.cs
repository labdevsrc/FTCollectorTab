using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model
{
    public class SelectedItems
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string SelectedCable1 { get; set; }
        public string SelectedCable2 { get; set; }
        public string SelectedCable3 { get; set; }
        public string SelectedCable4 { get; set; }
    }
}
