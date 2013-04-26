using System;
using System.Collections.Generic;
using WitchOS.Core;

namespace WitchOS.Applications
{
    public class system : Application
    {
        public system() { this.call = "system"; this.usage = "system <appman|srvman|drvman|trdman> <args>"; }
        public override void Run(string[] args)
        {
            switch (args[0])
            {
                case "appman": AppManager(CutArgs(args)); break;
                case "drvman": DriverManager(CutArgs(args)); break;
                case "srvman": ServiceManager(CutArgs(args)); break;
                case "trdman": ThreadManager(CutArgs(args)); break;
                case "failheader": HAL.drvman.witchfs.Mounts[0].Format("USELESS"); break;
                default: Out.printf("ERROR: invalid arguments"); break;
            }
        }

        public string[] CutArgs(string[] args, int cut = 1)
        {
            string[] output = new string[args.Length - cut];
            for (int i = cut; i < args.Length; i++)
            {
                output[i - cut] = args[i];
            }
            return output;
        }

        public void AppManager(string[] args)
        {
            appman.HandleRequest(args);
        }

        public void DriverManager(string[] args)
        {
            HAL.drvman.HandleRequest(args);
        }

        public void ServiceManager(string[] args)
        {
            Services.srvman.HandleRequest(args);
        }

        public void ThreadManager(string[] args)
        {
            Threading.trdman.HandleRequest(args);
        }
    }
}
