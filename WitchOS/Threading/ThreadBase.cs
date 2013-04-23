using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WitchOS.Threading
{
    public abstract class ThreadBase
    {
        public int TID;
        public delegate void ThreadMethod();
        public ThreadMethod action = null;
        public string name;
        public void Start()
        {
            action.Invoke();
        }
        public void Pause()
        {
        }
        public void Resume()
        {
        }
        public void Stop()
        {
        }
        public void Kill()
        {
        }
    }
}
