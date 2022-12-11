using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications
{
    public interface IResponseParser
    {
        public void ParseResponse(byte[] response);
    }
}
