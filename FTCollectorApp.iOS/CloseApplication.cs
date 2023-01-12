using Foundation;
using FTCollectorApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UIKit;

namespace FTCollectorApp.iOS
{
    public class CloseApplication:ICloseApps
    {
        public void closeApplication()
        {
            Thread.CurrentThread.Abort();
        }

        
    }
}