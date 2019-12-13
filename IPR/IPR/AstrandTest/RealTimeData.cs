using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.AstrandTest
{
    class RealTimeData : IAstrandData
    {
        IAstrandDataListener listener;


        public int GetHeartFrequency()
        {
            throw new NotImplementedException();
        }


        public void Connect(IAstrandDataListener listener)
        {
            this.listener = listener;
        }
    }
}
