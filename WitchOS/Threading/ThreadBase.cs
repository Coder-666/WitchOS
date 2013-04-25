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
        public void HandleSignal(string signal)
        {
            switch (signal)
            {
                case "START":
                    Start();
                    break;
            }
        }
        private void Start()
        {
            action.Invoke();
        }
        private void Pause()
        {
        }
        private void Resume()
        {
        }
        private void Stop()
        {
        }
        private void Kill()
        {
        }
    }
}
