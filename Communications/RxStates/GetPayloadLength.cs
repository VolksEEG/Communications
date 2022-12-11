using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetPayloadLength : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        private static readonly byte _EXPECTED_VERSION = 0x01;

        public GetPayloadLength(LowLevelCommunicationsData stateData) 
        {
            _StateData = stateData;
        }

        public IRxState ProcessState()
        {
            if (_StateData.ComsLink.GetReceivedData(1, out int readCount, out byte[] data))
            {
                // set the received
                _StateData.PayloadLength = (int)data[0];

                return new GetId(_StateData); 
            }

            return this;
        }
    }
}
