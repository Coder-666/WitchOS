using System;
using System.Collections.Generic;
using WitchOS.Core;

namespace WitchOS.Services
{
    public static class srvman
    {
        public static Syslog syslog;
        public static void LoadServices()
        {
            syslog = new Syslog();
        }
        //
        // IMPLEMENT!!!
        //
        public static void HandleRequest(string[] args)
        {
            if (args.Length > 0) Out.printf("The command %s is currently not supported", args[0]);
        }
    }
}
