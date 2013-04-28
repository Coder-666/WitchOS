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
            Out.printf("Writing test file...");
            WriteFile(2, 1, "HelloWorld", "Hello from WitchFS!");
        }

        public void WriteFile(byte block, byte blocksize, string filename, string content)
        {
            byte[] data = new byte[512];

            fixed (byte* ptr = data)
            {
                WitchFile* file = (WitchFile*)ptr;
                file->magic = 0x7cc;
                file->startblock = block;
                file->blocksize = blocksize;
                file->endblock = (byte)(block + blocksize);
                for (int i = 0; i < filename.Length; i++)
                {
                    file->filename[i] = (byte)filename[i];
                }
                for (int i = 0; i < "file".Length; i++)
                {
                    file->extension[i] = (byte)"file"[i];
                }
                for (int i = 0; i < content.Length; i++)
                {
                    file->content[i] = (byte)content[i];
                }
            }

            partition.WriteBlock(block, blocksize, data);
        }

        public void InitFiles()
        {
            byte firstblock = 1;
            byte position = (byte)(firstblock + 1);
            while (true)
            {
                byte[] data = new byte[512];
                partition.ReadBlock(position, 1, data);
                WitchFile* current;
                fixed (byte* ptr = data)
                {
                    current = (WitchFile*)ptr;
                    Kernel.MemCopy((uint)ptr * position, (uint)current, 0, (uint)sizeof(WitchFile));
                }
                if (current->magic == 0x7cc)
                {
                    WitchFile file = new WitchFile();
                    file.magic = 0x7cc;
                    file.startblock = current->startblock;
                    file.blocksize = current->blocksize;
                    file.endblock = current->endblock;
                    string x = ByteToString(current->filename);
                    StringToByte(file.filename, x);
                    Out.printf("File found: %s", ByteToString(current->filename));
                    string y = ByteToString(current->extension);
                    StringToByte(file.extension, y);
                    string z = ByteToString(current->content);
                    StringToByte(file.content, z);
                    Files.Add(file);
                }
                else break;
                position++;
            }
        }

        public void ListFiles()
        {
            for (int i = 0; i < Files.Count; i++)
            {
                Out.printf("File [%i]: %s", i, "Unknown Filename");
            }
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
            InitFiles();
            Out.printf("Listing files...");
            ListFiles();
        }

        public string ReadFile(string filename)
        {
            /*
            byte[] data = new byte[512];
            partition.ReadBlock(block, blocksize, data);
            WitchFile* file = (WitchFile*)Cosmos.Core.Heap.MemAlloc((uint)sizeof(WitchFile));
            fixed (byte* ptr = data)
            {
                Kernel.MemCopy((uint)ptr, (uint)file, 0, (uint)sizeof(WitchFile));
            }
            string output = ByteToString(file->content);
             * */
            for (int i = 0; i < Files.Count; i++)
            {
                byte[] data = new byte[512];
                fixed (byte* ptr = data)
                {
                    if (ByteToString(ptr) == filename)
                    {
                        
                    }
                }
            }
            return "ERROR";
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
        private byte[] ByteToByte(byte* data)
        {
            int length = 0;
            int pos = 0;
            while (true)
            {
                byte b = data[pos];
                if (b != 0)
                    length++;
                else
                    break;
                pos++;
            }
            byte[] output = new byte[length];
            pos = 0;
            while (true)
            {
                byte b = data[pos];
                if (b != 0)
                    output[pos] = b;
                else
                    break;
                pos++;
            }
            return output;
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

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 512)]
        public struct WitchFile
        {
            [FieldOffset(0)]
            public short magic;
            [FieldOffset(2)]
            public fixed byte filename[25];
            [FieldOffset(27)]
            public fixed byte extension[10];
            [FieldOffset(37)]
            public byte startblock;
            [FieldOffset(38)]
            public byte endblock;
            [FieldOffset(39)]
            public byte blocksize;
            [FieldOffset(40)]
            public fixed byte content[472];
        }
    }
}
