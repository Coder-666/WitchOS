using System;
using System.Collections.Generic;
using Cosmos.Hardware.BlockDevice;
using Sys = Cosmos.System;
using System.Text;
using WitchOS.Core;

namespace WitchOS.HAL
{
    public class FATFS : Driver
    {
        private AtaPio xATA = null;
        private Partition xPartition = null;
        private Sys.Filesystem.FAT.FatFileSystem xFS = null;
        private List<Sys.Filesystem.Listing.Base> xListing = null;
        private Sys.Filesystem.FAT.Listing.FatFile xRootFile = null;
        private byte vlevel;

        public FATFS(drvman.verbosity verbositylevel)
        {
            base.name = "fatfs";
            base.info = "FAT Filesystem Driver";
            this.vlevel = (byte)verbositylevel;
            Core.Out.printf("Loading filesystem driver...\n");
            try
            {
                this.Setup();
            }
            catch
            {
                //dewitcher.Core.Bluescreen.Init(ex, true);
                if ((byte)this.vlevel > 0) Core.Out.printf("ERROR: Could not load the FATFS driver\n");
            }
        }

        public void Setup()
        {
            this.Detect();
            this.Mount();
            if (this.vlevel == 2) Info();
        }

        public void Detect()
        {
            // Detect a hard drive
            if (this.vlevel == 1) Out.printf("+-- Detecting...\n");
            if (this.vlevel == 2) Out.printf("+-- Detecting hard drives...\n");
            for (int i = 0; i < BlockDevice.Devices.Count; i++)
            {
                var xDevice = BlockDevice.Devices[i];
                if (xDevice is AtaPio)
                {
                    if (this.vlevel == 2) Out.printf("|   +-- Hard drive found!\n");
                    this.xATA = (AtaPio)xDevice;
                }
            }
        }

        public void Mount()
        {
            // Detect a partition
            if (this.vlevel > 0) Out.printf("+-- Mounting...\n");
            if (this.vlevel == 2) Out.printf("|   +-- Checking for partitions...\n");
            if (BlockDevice.Devices.Count > 0)
            {
                for (int i = 0; i < BlockDevice.Devices.Count; i++)
                {
                    var xDevice = BlockDevice.Devices[i];
                    if (xDevice is Partition)
                    {
                        if (this.vlevel == 2) Out.printf("|   |   +-- Partition found!\n");
                        this.xPartition = (Partition)xDevice;
                        this.xFS = new Cosmos.System.Filesystem.FAT.FatFileSystem(this.xPartition);
                        if (this.vlevel > 0) Out.printf("|   +-- Mapping filesystem...\n");
                        Sys.Filesystem.FileSystem.AddMapping("WITCHOS", this.xFS);
                        this.xListing = xFS.GetRoot();
                        if (this.vlevel == 2) Out.printf("|   +-- Checking for root file...\n");
                        for (int j = 0; j < xListing.Count; j++)
                        {
                            var xItem = xListing[j];
                            if (xItem is Sys.Filesystem.Listing.File && xItem.Name == "WOSFSROOT")
                            {
                                if (this.vlevel == 2) Out.printf("|   |   +-- Root file found!\n");
                                this.xRootFile = (Cosmos.System.Filesystem.FAT.Listing.FatFile)this.xListing[j];
                                break;
                            }
                        }
                        if (this.vlevel == 2)
                        {
                            try
                            {
                                Out.printf("|   |   +-- Reading root file...\n");
                                if (this.xRootFile != null)
                                {
                                    var xStream = new Sys.Filesystem.FAT.FatStream(this.xRootFile);
                                    var xData = new byte[this.xRootFile.Size];
                                    xStream.Read(xData, 0, (int)this.xRootFile.Size);
                                    var xText = Encoding.ASCII.GetString(xData);
                                    Out.printf(xText);
                                }
                                else
                                {
                                    Out.printf("|   |   |   +-- Root file not found!\n");
                                }
                            }
                            catch (Exception ex)
                            {
                                Out.printf("Error: %s\n", ex.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                if (this.xRootFile != null)
                                {
                                    var xStream = new Sys.Filesystem.FAT.FatStream(this.xRootFile);
                                    var xData = new byte[this.xRootFile.Size];
                                    xStream.Read(xData, 0, (int)this.xRootFile.Size);
                                    var xText = Encoding.ASCII.GetString(xData);
                                }
                            }
                            catch (Exception ex)
                            {
                                //dewitcher.Core.Bluescreen.Init(ex, false);
                                Out.printf("Error while reading root file: %s\n", ex.Message);
                            }
                        }
                    }
                }
            }
        }

        public void Info()
        {
            Out.printf("+-- Filesystem info:\n");
            Out.printf("|   +-- Type: %s\n", (this.xATA.DriveType == AtaPio.SpecLevel.ATA ? "ATA" : "ATAPI"));
            Out.printf("|   +-- Serial No: %s\n", this.xATA.SerialNo);
            Out.printf("|   +-- Firmware Rev: %s\n", this.xATA.FirmwareRev);
            Out.printf("|   +-- Model No: %s\n", this.xATA.ModelNo);
            Out.printf("|   +-- Block Size: %d %s\n", this.xATA.BlockSize, "bytes");
            Out.printf("|   +-- Size: %d %s\n", (ulong)(this.xATA.BlockCount * this.xATA.BlockSize / 1024 / 1024), "MB");
        }

        public void List()
        {
            for (int j = 0; j < xListing.Count; j++)
            {
                var xItem = xListing[j];
                if (xItem is Sys.Filesystem.Listing.Directory)
                {
                    //Detecting Dir in HDD
                    Out.printf("<DIR> %s", this.xListing[j].Name);
                }
                else if (xItem is Sys.Filesystem.Listing.File)
                {
                    //Detecting File in HDD
                    // Console.WriteLine("<FILE> " + this.xListing[j].Name + " (" + this.xListing[j].Size + ")");
                    Out.printf("<FILE> %s (%i)", this.xListing[j].Name, this.xListing[j].Size);
                    if (this.xListing[j].Name == "WOSFSROOT")
                    {
                        this.xRootFile = (Cosmos.System.Filesystem.FAT.Listing.FatFile)this.xListing[j];
                    }
                }
            }
        }

        public void WriteFile(string filename, string data)
        {
            // Write does not work, there is a problem with the FatStream.Write method
        }

        public string ReadFile(string filename)
        {
            Sys.Filesystem.Listing.File file = GetFile(filename);
            if (file == null) return null;
            Sys.Filesystem.FAT.FatStream stream =
                new Sys.Filesystem.FAT.FatStream((Sys.Filesystem.FAT.Listing.FatFile)file);
            byte[] data = new byte[file.Size];
            stream.Read(data, 0, (int)file.Size);
            string text = Encoding.ASCII.GetString(data);
            return text;
        }

        public Sys.Filesystem.Listing.File GetFile(string filename)
        {
            for (int i = 0; i < xListing.Count; i++)
            {
                var xItem = xListing[i];
                if (xItem is Sys.Filesystem.Listing.File)
                {
                    if (xItem.Name == filename) return (Sys.Filesystem.Listing.File)xItem;
                }
            }
            return null;
        }
    }
}
