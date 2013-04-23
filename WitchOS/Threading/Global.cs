using System;
using System.Collections.Generic;
using WitchOS.Services;

namespace WitchOS.Threading
{
    public class Global
    {
        private static List<Thread> threads;
        private static Queue<ThreadSignal> signals;
        public static void PrepareThreading()
        {
            threads = new List<Thread>();
            signals = new Queue<ThreadSignal>();
        }
        public static void Register(Thread trd)
        {
            trd.TID = threads.Count + 1;
            Services.srvman.syslog.Write(new SyslogEntry(SyslogEntry.Type.Service,
                "Threading/Global", "Thread registered with TID " + trd.TID.ToString()));
            threads.Add(trd);
        }
        public static void ProcessSignals()
        {
            for (int i = 0; i < signals.Count; i++)
            {
                ThreadSignal trdsig = signals.Dequeue();
                if (trdsig.signal == "SIGKILL") trdsig.thread.Kill();
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
    }
}
