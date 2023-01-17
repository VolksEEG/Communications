using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetSynchronisationWord : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        private int _CheckIndex;

        public GetSynchronisationWord(LowLevelCommunicationsData stateData)
        {
            _StateData = stateData;

            _CheckIndex = 0;
        }

        public IRxState ProcessState()
        {
            if (_StateData.ComsLink.Read(1, out int readCount, out byte[] data))
            {
                // we have a byte, so  check it
                // at this point we should have a byte but it may be worth checking
                if (_StateData._SYNCHRONISATION_WORD[_CheckIndex] == data[0])
                {
                    _CheckIndex++;
                }
                else
                {
                    _CheckIndex = 0;
                }

                if (2 == _CheckIndex)
                {
                    // we have received 2 correct bytes so return the next state 
                    return new GetProtocolVersion(_StateData);
                }
            }

            // Sync word has not completely been received so stay in this state
            return this;
        }
    }
}
