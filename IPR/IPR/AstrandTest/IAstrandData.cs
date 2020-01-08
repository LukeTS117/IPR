﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR.AstrandTest
{
    interface IAstrandData
    {
        void Connect(IAstrandDataListener listener);
        void SetResistance(int percentage);
    }
}
