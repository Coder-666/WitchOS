using System;
using System.Collections.Generic;

namespace WitchOS.Threading
{
    public class Thread : ThreadBase
    {
        public Thread(string name, ThreadMethod trdinvoke)
        {
            this.name = name;
            trdman.Register(this);
            this.action = trdinvoke;
        }
    }
}
