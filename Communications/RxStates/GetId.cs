using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetId : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        public GetId(LowLevelCommunicationsData stateData) 
        {
            _StateData = stateData;
        }

        public IRxState ProcessState()
        {
            if (_StateData.ComsLink.Read(1, out int readCount, out byte[] data))
            {
                // check the ID
                if (_StateData.ExpectedID != data[0])
                {
                    // ID is not as expected, so look for the next sync word
                    return new GetSynchronisationWord(_StateData);
                }

                // ID is ok so carry on receiving the message
                return new GetIdAcknowledge(_StateData);
            }

            return this;
        }
    }
}
