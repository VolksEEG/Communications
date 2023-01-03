using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetHeaderChecksum : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        public GetHeaderChecksum(LowLevelCommunicationsData stateData) 
        {
            _StateData = stateData;
        }

        public IRxState ProcessState()
        {
            if (_StateData.ComsLink.GetReceivedData(1, out int readCount, out byte[] data))
            {
                if (!_StateData.MessageHeaderIsValid(data[0]))
                {
                    // Header checksum is not valid, so look for the next sync word
                    return new GetSynchronisationWord(_StateData);
                }

                if (0 != _StateData.PayloadLength)
                {
                    // there is a payload so get it.
                    return new GetPayloadData(_StateData);
                }

                // No payload, so just process the message
                return new ProcessMessage(_StateData);
            }

            return this;
        }
    }
}
