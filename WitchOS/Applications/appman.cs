using System;
using System.Collections.Generic;
using WitchOS.Core;
using WitchOS.Services;

namespace WitchOS.Applications
{
    public static class appman
    {
        public static List<Application> applications;
        private static byte vlevel = 0;
        public static void Init(byte verbosity)
        {
            applications = new List<Application>();
            vlevel = verbosity;
            Register(new keys());
            Register(new syslog());
        }
        public static void Register(Application application)
        {
            application.APPID = applications.Count + 1;
            Services.srvman.syslog.Write(new SyslogEntry(SyslogEntry.Type.Service,
                "AppMan", "Registered app with APPID " + application.APPID.ToString()));
            if (vlevel == 1) Out.printf("Registered app with APPID %i\n", application.APPID);
            applications.Add(application);
        }
        public static void Help()
        {
            HAL.drvman.screenbuffer.Push();
            Console.Clear();
            Out.printf("--- WitchOS help start\n");
            for (int i = 0; i < applications.Count; i++)
            {
                Out.printf("%s - Usage: %s\n", applications[i].call, applications[i].usage);
            }
            Out.printf("--- WitchOS help end\n");
            Out.printf("--- Press any key to exit ---\n");
            Console.ReadKey();
            Console.Clear();
            HAL.drvman.screenbuffer.Pop();
        }
        public static bool Run(string command, string[] args)
        {
            for (int i = 0; i < applications.Count; i++)
            {
                if (applications[i].call == command)
                {
                    applications[i].Run(args);
                    return true;
                }
            }
            return false;
        }
    }
}
