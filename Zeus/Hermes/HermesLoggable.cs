using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Hermes
{
    public interface HermesLoggable
    {
        long ID { get; }
        string Type { get; }
    }
}
