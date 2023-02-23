using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Model
{
    public class SiteColsType
    {
        public string ColName { get; set; }
        public int ColType { get; set; }
        public bool IsSwitch { get; set; } = false;
        public bool IsEntry { get; set; } = false;
        public bool IsDropDown { get; set; } = false;
    }
}
