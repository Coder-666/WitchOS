using System;
using Cosmos.Core;

namespace WitchOS
{
    public static unsafe partial class Kernel
    {
        /// <summary>
        /// Reads a byte
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static byte inb(ushort port)
        {
            IOPort io = new IOPort(port);
            return io.Byte;
        }
        /// <summary>
        /// Reads a byte array
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static byte[] inb(ushort[] port)
        {
            byte[] output = new byte[port.Length];
            IOPort io;
            for (uint i = 0; i < port.Length; i++)
            {
                io = new IOPort(port[i]);
                output[i] = io.Byte;
            }
            return output;
        }
        /// <summary>
        /// Reads a 16 bit word
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ushort inw(ushort port)
        {
            IOPort io = new IOPort(port);
            return io.Word;
        }
        /// <summary>
        /// Reads a 16 bit word array
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ushort[] inw(ushort[] port)
        {
            ushort[] output = new ushort[port.Length];
            IOPort io;
            for (uint i = 0; i < port.Length; i++)
            {
                io = new IOPort(port[i]);
                output[i] = io.Word;
            }
            return output;
        }
        /// <summary>
        /// Reads a 32 bit word
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static uint inl(ushort port)
        {
            IOPort io = new IOPort(port);
            return io.DWord;
        }
        /// <summary>
        /// Reads a 32 bit word array
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static uint[] inl(ushort[] port)
        {
            uint[] output = new uint[port.Length];
            IOPort io;
            for (uint i = 0; i < port.Length; i++)
            {
                io = new IOPort(port[i]);
                output[i] = io.DWord;
            }
            return output;
        }
        /// <summary>
        /// Writes a byte
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void outb(ushort port, byte data)
        {
            IOPort io = new IOPort(port);
            io.Byte = data;
        }
        /// <summary>
        /// Writes a byte array
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void outb(ushort[] port, byte[] data)
        {
            IOPort io;
            for (uint i = 0; i < port.Length; i++)
            {
                io = new IOPort(port[i]);
                io.Byte = data[i];
            }
        }
        /// <summary>
        /// Writes a 16 bit word
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void outw(ushort port, ushort data)
        {
            IOPort io = new IOPort(port);
            io.Word = data;
        }
        /// <summary>
        /// Writes a 16 bit word array
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void outw(ushort[] port, ushort[] data)
        {
            IOPort io;
            for (uint i = 0; i < port.Length; i++)
            {
                io = new IOPort(port[i]);
                io.Word = data[i];
            }
        }
        /// <summary>
        /// Writes a 32 bit word
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void outl(ushort port, uint data)
        {
            IOPort io = new IOPort(port);
            io.DWord = data;
        }
        /// <summary>
        /// Writes a 32 bit word array
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void outl(ushort[] port, uint[] data)
        {
            IOPort io;
            for (uint i = 0; i < port.Length; i++)
            {
                io = new IOPort(port[i]);
                io.DWord = data[i];
            }
        }
    }
}