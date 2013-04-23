using System;
using WitchOS.Core;

namespace WitchOS.Applications
{
    public class syslog : Application
    {
        public syslog() { this.call = "syslog"; this.usage = "syslog view all"; }
        public override void Run(string[] args)
        {
            HAL.drvman.screenbuffer.Push();
            Console.Clear();
            if (args.Length == 1)
            {
                Out.printf("ERROR: syslog needs two parameters\nUsage: %s\n", this.usage);
            }
            else if (args.Length == 2)
            {
                if (args[0].ToLower() == "view")
                {
                    if (args[1].ToLower() == "all")
                    {
                        Services.srvman.syslog.ReadAll();
                    }
                    else Out.printf("ERROR: The second argument must be \"all\"\nUsage: %s\n", this.usage);
                }
                else Out.printf("ERROR: The first argument must be \"view\"\nUsage: %s\n", this.usage);
            }
            Out.printf("--- Press any key to exit ---\n");
            Console.ReadKey();
            Console.Clear();
            HAL.drvman.screenbuffer.Pop();
        }
    }
}
