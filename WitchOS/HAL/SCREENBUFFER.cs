using System;
using System.Collections.Generic;
using WitchOS.Core;

namespace WitchOS.HAL
{
    public unsafe class SCREENBUFFER
    {
        public class Buffer
        {
            public byte[] content;
            public int X, Y;
        }
        public byte* Address;
        public List<Buffer> buffers;
        public SCREENBUFFER()
        {
            Out.printf("Loading screenbuffer driver...\n");
            Address = (byte*)0xB8000;
            buffers = new List<Buffer>();
        }
        public Stack<Buffer> stack = new Stack<Buffer>();
        public void Push()
        {
            Buffer vb = new Buffer();
            vb.content = new byte[4250];
            for (int i = 0; i < 4250; i++)
            {
                byte b = this.Address[i];
                vb.content[i] = b;
            }
            vb.X = Console.CursorLeft;
            vb.Y = Console.CursorTop;
            this.stack.Push(vb);
        }
        public void Pop()
        {
            Buffer vb = stack.Pop();
            for (int i = 0; i < 4250; i++)
            {
                this.Address[i] = vb.content[i];
            }
            Console.CursorLeft = vb.X;
            Console.CursorTop = vb.Y;
        }
    }
}
