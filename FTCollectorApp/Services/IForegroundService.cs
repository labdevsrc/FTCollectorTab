using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Services
{
    public interface IForegroundService
    {
        void StartMyForegroundService();
        void StopMyForegroundService();

        bool IsForeGroundServiceRunning();
    }
}
