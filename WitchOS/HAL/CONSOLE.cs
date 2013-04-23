using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitchOS.HAL
{
    public unsafe class CONSOLE
    {
        public struct CColor
        {
            public static byte Black = 0x0;
            public static byte Blue = 0x1;
            public static byte Green = 0x2;
            public static byte Cyan = 0x3;
            public static byte Red = 0x4;
            public static byte Magenta = 0x5;
            public static byte Brown = 0x6;
            public static byte Lightgray = 0x7;
            public static byte Darkgray = 0x8;
            public static byte Lightblue = 0x9;
            public static byte Lightgreen = 0xA;
            public static byte Lightcyan = 0xB;
            public static byte LightRed = 0xC;
            public static byte LightMagenta = 0xD;
            public static byte Yellow = 0xE;
            public static byte White = 0xF;
        }
        public byte X, Y;
        public byte ForeColor;
        public byte BackColor;
        public CONSOLE()
        {
            X = 0;
            Y = 0;
            ForeColor = CColor.White;
            BackColor = CColor.Black;
        }
        public void PrintChar(char chr)
        {
            byte attrib = (byte)((BackColor << 4) | (ForeColor & 0x0F));
            ushort* address = (ushort*)((0xB8000 + ((Y * 80) + X)) * 2);
            *address = (ushort)(chr | (attrib << 8));
            if (X < 80) X++;
            else if (X == 80) { X = 0; Y++; }
        }
        public void Print(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                PrintChar(str[i]);
            }
        }
        public void SetCursor(byte x, byte y)
        {
            byte tmp = (byte)((byte)((byte)x * (byte)80) + (byte)y);
            Kernel.outb(new ushort[] { 0x3D4, 0x3D5, 0x3D4, 0x3D5 }, new byte[] { 14, (byte)((byte)tmp >> (byte)8), 15, tmp });
        }
    }
}
