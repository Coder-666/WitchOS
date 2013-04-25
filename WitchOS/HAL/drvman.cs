using System;
using System.Collections.Generic;
using WitchOS.Core;

namespace WitchOS.HAL
{
    public static class drvman
    {
        public static FATFS fs;
        public static SCREENBUFFER screenbuffer;
        public static ACPI acpi;
        public static PCSpeaker pcspeaker;
        public static List<Driver> registered;
        public enum verbosity : byte { none = 0, basic = 1, max = 2 };
        public static void LoadDrivers()
        {
            registered = new List<Driver>();

            // ACPI
            acpi = new ACPI();
            registered.Add(acpi);

            // Filesystem
            fs = new FATFS(verbosity.max);
            registered.Add(fs);

            // Screen buffers
            screenbuffer = new SCREENBUFFER();
            registered.Add(screenbuffer);

            // PCSPeaker
            pcspeaker = new PCSpeaker();
            registered.Add(pcspeaker);
        }
        public static void HandleRequest(string[] args)
        {
            if (args.Length == 2 && args[0] == "info")
            {
                for (int i = 0; i < registered.Count; i++)
                {
                    if (args[1] == registered[i].name)
                    {
                        Out.printf("[%s] INFO: %s\n", registered[i].name, registered[i].info);
                    }
                }
            }
        }
    }
}