using System;
using System.Collections.Generic;

namespace WitchOS.Core
{
    public static class Terminal
    {
        public static string directory = "~/home/";
        public static void EnterLoop()
        {
            while (true)
            {
                Out.printf("root@%s> ", directory);
                ParseInput();
            }
        }
        public static void ParseInput()
        {
            string full = Console.ReadLine().Trim();
            if (full[0] == '#' || full == "") return;
            string command = full.Split(' ')[0].Trim();
            string[] args = full.Substring(command.Length + 1).Split(' ');
            for (int i = 0; i < args.Length; i++) args[i] = args[i].Trim();
            bool ok = Applications.appman.Run(command, args);
        }
    }
}
