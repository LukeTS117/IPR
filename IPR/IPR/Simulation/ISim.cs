using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.Simulation
{
    public interface ISim
    {
        bool SendCommand(string command);
    }
}
