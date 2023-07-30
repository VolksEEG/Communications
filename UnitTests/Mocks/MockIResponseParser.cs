using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolksEEG.Communications;

namespace UnitTests.Mocks
{
    internal class MockIResponseParser : IResponseParser
    {
        public void ParseResponse(byte[] response)
        {
            throw new NotImplementedException();
        }
    }
}
