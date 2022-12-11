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
            if (_StateData.ComsLink.GetReceivedData(1, out int readCount, out byte[] data))
            {
                // check the ID

                //! \todo continue from here.
            }

            return this;
        }
    }
}
