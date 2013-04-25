using System;
using WitchOS.Core;

namespace WitchOS
{
    public class Boot : Cosmos.System.Kernel
    {
        protected override void BeforeRun()
        {
            Out.printf("WitchOS v%d%s startup process\n", Kernel.Version, Kernel.VersionExt);

            // Load drivers
            Out.printf("Loading drivers...\n");
            HAL.drvman.LoadDrivers();

            // Load services
            Out.printf("Loading services...\n");
            Services.srvman.LoadServices();

            // Prepare threading
            Out.printf("Preparing threading...\n");
            Threading.trdman.PrepareThreading();

            // Initialize applications
            Out.printf("Initializing applications...\n");
            Applications.appman.Init(1);

            Out.printf("WitchOS has loaded all the drivers and services.\n");
            Console.Clear();
        }

        protected override void Run()
        {
            Core.Terminal.EnterLoop();
        }
    }
}