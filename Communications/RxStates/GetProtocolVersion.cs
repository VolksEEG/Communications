using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetProtocolVersion : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        public GetProtocolVersion(LowLevelCommunicationsData stateData) 
        {
            _StateData = stateData;
        }

        public IRxState ProcessState()
        {
            if (_StateData.ComsLink.Read(1, out int readCount, out byte[] data))
            {
                if (_StateData._PROTOCOL_VERSION != data[0])
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
