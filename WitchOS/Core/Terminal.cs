using System;
using System.Collections.Generic;

namespace WitchOS.Core
{
    public static class Terminal
    {
        public static void EnterLoop()
        {
            while (true)
            {
                Out.printf("root@~/home/> ");
                ParseInput();
            }
        }
        public static void ParseInput()
        {
            string full = Console.ReadLine().Trim();
            if (full == "help") { Applications.appman.Help(); return; }
            if (full[0] == '#' || full == "") return;
            string command = full.Split(' ')[0].Trim();
            string[] args = full.Substring(command.Length + 1).Split(' ');
            for (int i = 0; i < args.Length; i++) args[i] = args[i].Trim();
            bool ok = Applications.appman.Run(command, args);
            if (!ok) Out.printf("The application \"%s\" does not exist :/\n", command);
        }
    }
}
