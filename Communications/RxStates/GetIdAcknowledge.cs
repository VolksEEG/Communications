using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetIdAcknowledge : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        public GetIdAcknowledge(LowLevelCommunicationsData stateData) 
        {
            _StateData = stateData;
        }

        public IRxState ProcessState()
        {
            if (_StateData.ComsLink.GetReceivedData(1, out int readCount, out byte[] data))
            {
                _StateData.IdToAcknowledge = data[0];

                // Carry on receiving the message
                return new GetPayloadChecksum(_StateData);
            }

            return this;
        }
    }
}
