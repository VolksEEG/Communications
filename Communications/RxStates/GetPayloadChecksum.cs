using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetPayloadChecksum : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        public GetPayloadChecksum(LowLevelCommunicationsData stateData) 
        {
            _StateData = stateData;
        }

        public IRxState ProcessState()
        {
            if (_StateData.ComsLink.GetReceivedData(1, out int readCount, out byte[] data))
            {
                _StateData.PayloadChecksum = data[0];

                // ID is ok so carry on receiving the message
                return new GetPayloadLength(_StateData);
            }

            return this;
        }
    }
}
