using IPR.BLEHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.AstrandTest
{
    interface IAstrandDataListener
    {
        void OnDataAvailable(DataTypes dataType, int value);
    }
}
