using System;
using System.Collections.Generic;
using WitchOS.Services;
using WitchOS.Core;

namespace WitchOS.Threading
{
    public static class trdman
    {
        private static List<Thread> threads;
        private static Queue<ThreadSignal> signals;
        public static void PrepareThreading()
        {
            Out.printf("Initializing thread list...\n");
            threads = new List<Thread>();
            Out.printf("Initializing signal queque...\n");
            signals = new Queue<ThreadSignal>();
        }
        public static void Register(Thread trd)
        {
            trd.TID = threads.Count + 1;
            Services.srvman.syslog.Write(new SyslogEntry(SyslogEntry.Type.Service,
                "threading/trdman", "Thread registered with TID " + trd.TID.ToString()));
            threads.Add(trd);
        }
        public static void ProcessSignals()
        {
            for (int i = 0; i < signals.Count; i++)
            {
                ThreadSignal trdsig = signals.Dequeue();
                trdsig.thread.HandleSignal(trdsig.signal);
            }
        }
        public static void SendSignal(int tid, string signal)
        {
            for (int i = 0; i < threads.Count; i++)
            {
                if (threads[i].TID == tid)
                {
                    signals.Enqueue(new ThreadSignal(threads[i], signal));
                    ProcessSignals();
                    return;
                }
            }
        }
        public static void SendSignal(string threadName, string signal)
        {
            for (int i = 0; i < threads.Count; i++)
            {
                if (threads[i].name == threadName)
                {
                    SendSignal(threads[i].TID, signal);
                    return;
                }
            }
        }
        //
        // IMPLEMENT!!!
        //
        public static void HandleRequest(string[] args)
        {
            if (args.Length > 0) Out.printf("The command %s is currently not supported", args[0]);
        }
    }
}
