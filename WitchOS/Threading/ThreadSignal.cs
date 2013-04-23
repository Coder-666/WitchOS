using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitchOS.Threading
{
    public class ThreadSignal
    {
        public Thread thread;
        public string signal;
        public ThreadSignal(Thread trd, string sig)
        {
            this.thread = trd;
            this.signal = sig;
        }
    }
}
