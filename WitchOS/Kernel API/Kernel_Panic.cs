using System;
using System.Collections.Generic;
using Cosmos.Core;
using WitchOS.Core;
using WitchOS.Services;

namespace WitchOS
{
    public static unsafe partial class Kernel
    {
        public static void Panic(string message, Cosmos.Core.INTs.IRQContext context)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < (25 * 80); i++) Console.Write(" ");
            Console.CursorTop = 1;
            Console.CursorLeft = 0;
            Services.srvman.syslog.Write(new SyslogEntry(SyslogEntry.Type.Kernel, "Kernel", "KERNELPANIC> " + message));
            Out.printf("KERNEL PANIC\n%s\n\n", message);
            Out.printf("Register values:\n");
            Out.printf("EAX: %i  EBP: %i  EBX: %i  ECX: %i  EDI: %i  EDX: %i  EIP: %i  ESI: %i  ESP: %i\n\n",
                context.EAX, context.EBP, context.EBX, context.ECX, context.EDI, context.EDX, context.EIP, context.ESI, context.ESP);
            while (true) ;
        }
    }
}