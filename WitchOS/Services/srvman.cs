using System;
using System.Collections.Generic;

namespace WitchOS.Services
{
    public static class srvman
    {
        public static Syslog syslog;
        public static void LoadServices()
        {
            syslog = new Syslog();
        }
    }
}
