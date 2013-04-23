using System;
using System.Collections.Generic;
using WitchOS.Core;

namespace WitchOS.Applications
{
    public class witchedit : Application
    {
        public witchedit() { this.call = "wedit"; this.usage = "wedit"; }
        public override void Run(string[] args)
        {
            HAL.drvman.screenbuffer.Push();
            Console.Clear();

            List<string> bufferText = new List<string>();

            int lines = 1;
            int currentline = 1;
            bufferText.Add("");

            while (true)
            {
                Console.Clear();

                Out.printf("WitchEdit v%d%s\n", Kernel.Version, Kernel.VersionExt);
                Out.printf("Type #exit on a blank line to close WitchEdit\n\n");

                Console.CursorTop++;

                for (int i = 1; i < lines; i++)
                {
                    Out.printf("%i%s  %s", i, (i == currentline ? ")" : " "), bufferText[i - 1]);
                }

                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    if (currentline > 1) currentline--;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if (currentline < lines) currentline++;
                }

                if (bufferText.Count < lines) bufferText.Add(key.KeyChar.ToString());
                else bufferText[currentline] += key.KeyChar;
            }

            Console.Clear();
            HAL.drvman.screenbuffer.Pop();
        }
    }
}
