using System;
using System.Collections.Generic;

namespace WitchOS.Services
{
    public abstract class Service
    {
        public string name;
        public enum ServiceType : byte { SystemService = 0 }
        public ServiceType type;
    }
}
