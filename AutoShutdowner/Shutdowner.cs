﻿using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;

namespace AutoShutdowner
{
    public class Shutdowner
    {
        readonly Timer _shutdowner = new Timer(Minutes_15);//IDispose doesn't matter in this app
        const double Minutes_15 = 15 * 60 * 1000;
        public Shutdowner()
        {
            _shutdowner.Elapsed += (object sender, ElapsedEventArgs e) => { Shutdown(); };
        }

        private void Shutdown()
        {
            var psi = new ProcessStartInfo("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
            Application.Current.Shutdown();
        }

        public bool Enable
        {
            set
            {
                switch (value)
                {
                    case true:
                        _shutdowner.Start();
                        break;
                    case false:
                        _shutdowner.Interval = Minutes_15;
                        _shutdowner.Stop();
                        break;
                }
            }
        }
    }
}
