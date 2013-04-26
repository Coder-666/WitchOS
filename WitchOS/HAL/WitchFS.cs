using System;
using System.Collections.Generic;
using Cosmos.Hardware.BlockDevice;
using fs = Cosmos.System.Filesystem;
using Sys = Cosmos.System;
using System.Text;
using WitchOS.Core;
using System.Runtime.InteropServices;

namespace WitchOS.HAL
{
    public unsafe class WitchFS : Driver
    {
        public List<HDDMountPoint> Mounts;

        public WitchFS()
        {
            base.name = "witchfs";
            base.info = "Witch Filesystem Driver";

            Out.printf("Loading Witch Filesystem driver...\n");

            Mounts = new List<HDDMountPoint>();
        }

        public void DetectHarddrives()
        {
            int count = 0;
            for (int i = 0; i < BlockDevice.Devices.Count; i++)
            {
                var device = BlockDevice.Devices[i];
                if (device is Partition)
                {
                    Out.printf("Partition found!\n");
                    Mount((Partition)device, Out.formatf("part%i", count));
                    count++;
                }
            }
        }

        public void Mount(Partition device, string hdd)
        {
            Out.printf("Creating mountpoint for /dev/%s...\n", hdd);
            HDDMountPoint mountpoint = new HDDMountPoint(device, hdd);

            Out.printf("Mounting /dev/%s...\n", hdd);
            mountpoint.Init();

            Mounts.Add(mountpoint);
        }
    }
}
