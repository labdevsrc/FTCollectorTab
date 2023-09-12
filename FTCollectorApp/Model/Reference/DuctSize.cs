﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FTCollectorApp.Model.Reference
{
    public class DuctSize
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string DuctKey { get; set; }
        public string DUCTS_SIZE { get; set; }
    }
}
