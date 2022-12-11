using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications
{
    internal interface IRxState
    {
        public IRxState ProcessState();
    }
}
