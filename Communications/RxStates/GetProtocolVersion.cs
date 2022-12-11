using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetProtocolVersion : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        private static readonly byte _EXPECTED_VERSION = 0x01;

        public GetProtocolVersion(LowLevelCommunicationsData stateData) 
        {
            _StateData = stateData;
        }

        public IRxState ProcessState()
        {
            if (_StateData.ComsLink.GetReceivedData(1, out int readCount, out byte[] data))
            {
                if (_EXPECTED_VERSION != data[0])
                {
                    // version is not as expected, so look for the next sync word
                    return new GetSynchronisationWord(_StateData);
                }

                // Version is ok so carry on receiving the message
                return new GetPayloadLength(_StateData);
            }

            return this;
        }
    }
}
