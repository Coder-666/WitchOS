using System;
using System.Collections.Generic;

namespace WitchOS.HAL
{
    public class drvman
    {
        public static FATFS fs;
        public static SCREENBUFFER screenbuffer;
        public static ACPI acpi;
        public enum verbosity : byte { none = 0, basic = 1, max = 2 };
        public static void LoadDrivers()
        {
            // ACPI
            acpi = new ACPI();

            // Filesystem
            fs = new FATFS(verbosity.max);

            // Screen buffers
            screenbuffer = new SCREENBUFFER();
        }
    }
}
