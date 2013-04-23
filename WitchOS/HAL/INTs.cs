using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cosmos.IL2CPU.Plugs;
using CPUx86 = Cosmos.Assembler.x86;
using Cosmos.Core;
using IRQContext = Cosmos.Core.INTs.IRQContext;
//
// Credits for STIEnabler goes to Grunt
//
namespace WitchOS.HAL
{
    [Plug(Target = typeof(Cosmos.Core.INTs))]
    public class INTs
    {
        public static void HandleInterrupt_Default(ref IRQContext aContext)
        {
            if (aContext.Interrupt >= 0x20 && aContext.Interrupt <= 0x2F)
            {
                if (aContext.Interrupt >= 0x28)
                {
                    Cosmos.Core.Global.PIC.EoiSlave();
                }
                else
                {
                    Cosmos.Core.Global.PIC.EoiMaster();
                }
            }
        }

        public static void HandleInterrupt_00(ref IRQContext aContext)
        {
            Kernel.Panic("Divide by zero Exception", aContext);
        }

        public static void HandleInterrupt_01(ref IRQContext aContext)
        {
            Kernel.Panic("Debug Exception", aContext);
        }

        public static void HandleInterrupt_02(ref IRQContext aContext)
        {
            Kernel.Panic("Non Maskable Interrupt Exception", aContext);
        }

        public static void HandleInterrupt_03(ref IRQContext aContext)
        {
            Kernel.Panic("Breakpoint Exception", aContext);
        }

        public static void HandleInterrupt_04(ref IRQContext aContext)
        {
            Kernel.Panic("Into Detected Overflow Exception", aContext);
        }

        public static void HandleInterrupt_05(ref IRQContext aContext)
        {
            Kernel.Panic("Out of Bounds Exception", aContext);
        }

        public static void HandleInterrupt_06(ref IRQContext aContext)
        {
            Kernel.Panic("Invalid Opcode", aContext);
        }

        public static void HandleInterrupt_07(ref IRQContext aContext)
        {
            Kernel.Panic("No Coprocessor Exception", aContext);
        }

        public static void HandleInterrupt_08(ref IRQContext aContext)
        {
            Kernel.Panic("Double Fault Exception", aContext);
        }

        public static void HandleInterrupt_09(ref IRQContext aContext)
        {
            Kernel.Panic("Coprocessor Segment Overrun Exception", aContext);
        }

        public static void HandleInterrupt_0A(ref IRQContext aContext)
        {
            Kernel.Panic("Bad TSS Exception", aContext);
        }

        public static void HandleInterrupt_0B(ref IRQContext aContext)
        {
            Kernel.Panic("Segment not present", aContext);
        }

        public static void HandleInterrupt_0C(ref IRQContext aContext)
        {
            Kernel.Panic("Stack Fault Exception", aContext);
        }

        public static void HandleInterrupt_0E(ref IRQContext aContext)
        {
            Kernel.Panic("Page Fault Exception", aContext);
        }

        public static void HandleInterrupt_0F(ref IRQContext aContext)
        {
            Kernel.Panic("Unknown Interrupt Exception", aContext);
        }

        public static void HandleInterrupt_10(ref IRQContext aContext)
        {
            Kernel.Panic("Coprocessor Fault Exception", aContext);
        }

        public static void HandleInterrupt_11(ref IRQContext aContext)
        {
            Kernel.Panic("Alignment Exception", aContext);
        }

        public static void HandleInterrupt_12(ref IRQContext aContext)
        {
            Kernel.Panic("Machine Check Exception", aContext);
        }

        // IRQ0
        public static void HandleInterrupt_20(ref IRQContext aContext)
        {
            Cosmos.Core.Global.PIC.EoiMaster();
            Kernel.outb(0x20, 0x20);
        }
    }




    public class STIEnabler
    {
        public void Enable()
        {

        }
    }
    [Plug(Target = typeof(STIEnabler))]
    public class Enable : AssemblerMethod
    {
        public override void AssembleNew(object aAssembler, object aMethodInfo)
        {
            new CPUx86.Sti();
        }
    }
}