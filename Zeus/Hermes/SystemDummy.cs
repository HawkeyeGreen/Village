using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Hermes
{
    class SystemDummy : HermesLoggable
    {
        public long ID => 1337;

        private string typeStorage = "System";
        public string Type => typeStorage;

        public SystemDummy()
        {

        }

        public SystemDummy(string Type)
        {
            typeStorage = Type;
        }
    }
}
