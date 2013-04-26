using System;
using System.Collections.Generic;
using Cosmos.Hardware.BlockDevice;
using Sys = Cosmos.System;
using System.Text;
using WitchOS.Core;
using System.Runtime.InteropServices;

namespace WitchOS.HAL
{
    public unsafe class HDDMountPoint
    {
        public Partition partition;
        public string MountPoint;
        WitchHeader* Header;
        List<WitchFile> Files;

        public const string home = "/home/";
        public const string fs = "/fs/";
        public const string config = "/config/";

        public HDDMountPoint(Partition partition, string Device)
        {
            this.partition = partition;
            this.MountPoint = Out.formatf("%s%s", fs, Device);
            Files = new List<WitchFile>();
        }

        public void Format(string name)
        {
            byte[] data = new byte[512];
            fixed (byte* ptr = data)
            {
                WitchHeader* header = (WitchHeader*)ptr;
                for (int i = 0; i < name.Length; i++)
                {
                    header->name[i] = (byte)name[i];
                }
                header->name[name.Length] = 0;
                for (int i = 0; i < home.Length; i++)
                {
                    header->rootdir[i] = (byte)home[i];
                }
                header->rootdir[home.Length] = 0;
                for (int i = 0; i < MountPoint.Length; i++)
                {
                    header->mountpoint[i] = (byte)MountPoint[i];
                }
                header->mountpoint[MountPoint.Length] = 0;
                header->firstblock = 2;
            }
            partition.WriteBlock(1, 1, data);
            Out.printf("Header created!\n");
        }

        public void Init()
        {
            Out.printf("Reading WitchFS header...\n");
            byte[] data = new byte[512];
            partition.ReadBlock(1, 1, data);
            Header = (WitchHeader*)Cosmos.Core.Heap.MemAlloc((uint)sizeof(WitchHeader));
            fixed (byte* ptr = data)
            {
                Kernel.MemCopy((uint)ptr, (uint)Header, 0, (uint)sizeof(WitchHeader));
            }
            Out.printf("Validating header...\n");
            if (ByteToString(Header->name) != "WitchOS")
            {
                Out.printf("Invalid header!\n");
                Out.printf("Rewriting header...\n");
                Format("WitchOS");
            }
            else
            {
                Out.printf("The WitchOS header is valid\n");
            }
            Out.printf("Getting files...");
        }

        public void InitFiles()
        {
            byte[] firstblock = new byte[512];

        }

        // IMPLEMENT!!!
        public byte[] ReadFile(string directory, string filename, string extension)
        {
            WitchFile file = new WitchFile();
            return new byte[] { 0x0 };
        }

        private string ByteToString(byte* data)
        {
            string str = "";
            int pos = 0;
            while (true)
            {
                byte b = data[pos];
                if (b != 0)
                    str += ((char)b).ToString();
                else
                    break;
                pos++;
            }
            return str;
        }
        private void StringToByte(byte* data, string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                data[i] = (byte)str[i];
            }
            data[str.Length] = 0;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 512)]
        public struct WitchHeader
        {
            [FieldOffset(0)]
            public fixed byte name[25];
            [FieldOffset(25)]
            public fixed byte rootdir[230];
            [FieldOffset(255)]
            public fixed byte mountpoint[255];
            [FieldOffset(510)]
            public byte firstblock;
            [FieldOffset(511)]
            public byte lastblock;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct WitchUser
        {
            [FieldOffset(0)]
            public fixed byte username[25];
            [FieldOffset(25)]
            public fixed byte password[25];
            [FieldOffset(50)]
            public byte root;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct WitchFile
        {
            [FieldOffset(0)]
            public fixed byte filename[240];
            [FieldOffset(240)]
            public fixed byte extension[15];
            [FieldOffset(255)]
            public byte startblock;
            [FieldOffset(256)]
            public byte endblock;
            [FieldOffset(257)]
            public byte blocksize;
        }
    }
}
