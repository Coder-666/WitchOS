using System;
using Cosmos.Hardware;
using WitchOS.Core;

namespace WitchOS.Services
{
    public class SyslogEntry
    {
        public int ID;
        private string datetime;
        private string caller;
        private string message;
        public Type type;
        public enum Type : byte { Kernel = 0, Driver, Service, Thread, Application }
        public SyslogEntry(Type type, string caller, string message)
        {
            this.type = type;
            this.datetime =
                RTC.DayOfTheMonth.ToString() + "/" +
                RTC.Month.ToString() + "/" +
                RTC.Year.ToString() +
                RTC.Hour.ToString() + ":" +
                RTC.Minute.ToString();
            this.caller = caller;
            this.message = message;
        }
        public void Read(bool datetime = true, bool type = true, bool caller = true)
        {
            string output = "";
            if (datetime) output += this.datetime;
            if (type)
            {
                output += " [";
                if (this.type == Type.Kernel) output += "KERNEL";
                else if (this.type == Type.Driver) output += "DRIVER";
                else if (this.type == Type.Service) output += "SERVICE";
                else if (this.type == Type.Thread) output += "THREAD";
                else if (this.type == Type.Application) output += "APP";
                output += "] ";
            }
            if (caller) output += " " + this.caller;
            output += " >>> " + message;
            Out.printf("%s\n", output);
        }
    }
}
