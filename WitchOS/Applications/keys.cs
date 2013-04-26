using System;
using System.Collections.Generic;
using WitchOS.Core;

namespace WitchOS.Applications
{
    // change key layout
    public class keys : Application
    {
        public keys() { this.call = "keys"; this.usage = "keys <de|fr|<en|us>"; }
        public override void Run(string[] args)
        {
            if (args.Length == 0)
            {
                Out.printf("ERROR: keys needs one parameter\nUsage: %s\n", this.usage);
                Out.printf("-- Press any key to exit --\n");
            }
            else if (args.Length == 1)
            {
                string arg = args[0].ToLower();
                bool success = true;
                if (arg == "de" || arg == "qwertz") Core.Keylayout.QWERTZ();
                else if (arg == "en" || arg == "us" || arg == "qwerty") Core.Keylayout.QWERTY();
                else if (arg == "fr" || arg == "azerty") Core.Keylayout.AZERTY();
                else success = false;
                if (success) Out.printf("Changed keylayout to %s\n", arg);
                else Out.printf("ERROR: invalid parameter \"%s\"\nUsage: %s\n", arg, this.usage);
            }
            else if (args.Length > 1)
            {
                Out.printf("ERROR: Too much parameters for keys\nUsage: %s\n", this.usage);
            }
        }
    }
}
