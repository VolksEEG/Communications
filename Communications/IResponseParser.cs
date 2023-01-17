using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications
{
    internal interface IResponseParser
    {
        public void ParseResponse(byte[] response);
    }
}
