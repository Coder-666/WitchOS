using System;
using System.Collections.Generic;
using Cosmos.Core;

namespace WitchOS
{
    public static unsafe partial class Kernel
    {
        public static void MemAlloc(uint length)
        {
            Cosmos.Core.Heap.MemAlloc(length);
        }
        public static unsafe void MemRemove(uint start, uint offset, uint length)
        {
            if (offset >= length) return;
            uint* ptr = (uint*)start;
            for (uint i = offset; i < offset + length; i++)
            {
                ptr[i] = (uint)0;
            }
        }
        public static unsafe void MemCopy(uint source, uint destination, uint offset, uint length)
        {
            if (offset >= length) return;
            uint* src = (uint*)source;
            uint* dst = (uint*)destination;
            for (uint i = offset; i < offset + length; i++)
            {
                dst[i] = src[i];
            }
        }
        public static unsafe void MemMove(uint source, uint destination, uint offset, uint length)
        {
            if (offset >= length) return;
            uint* src = (uint*)source;
            uint* dst = (uint*)destination;
            for (uint i = offset; i < offset + length; i++)
            {
                dst[i] = src[i];
                src[i] = 0;
            }
        }
        public static unsafe bool MemCompare(uint source1, uint source2, uint offset, uint length)
        {
            if (offset >= length) return false;
            uint* ptr1 = (uint*)source1;
            uint* ptr2 = (uint*)source2;
            for (uint i = offset; i < offset + length; i++)
            {
                if (ptr1[i] != ptr2[i]) return false;
            }
            return true;
        }
    }
}