using System;
using System.Collections.Generic;
using WitchOS.Core;

namespace WitchOS.Services
{
    public class Syslog : Service
    {
        public List<SyslogEntry> entries;
        public Syslog()
        {
            Out.printf("Loading syslog service...\n");
            this.entries = new List<SyslogEntry>();
            this.Write(new SyslogEntry(SyslogEntry.Type.Service, "Core/Syslog", "System log initialized"));
        }
        public void Write(SyslogEntry entry)
        {
            entry.ID = this.entries.Count + 1;
            this.entries.Add(entry);
        }
        public void ReadAll()
        {
            for (int i = 0; i < entries.Count; i++)
            {
                this.entries[i].Read();
            }
        }
    }
}
