using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.AstrandTest
{
    class SymData : IAstrandData
    {
        IAstrandDataListener listener;

        public void Connect(IAstrandDataListener listener)
        {
            this.listener = listener;
        }


    }
}
