using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetSynchronisationWord : IRxState
    {
        private ICommunicationsLink _ComsLink;
        private IResponseParser _ResponseParser;

        private static byte[] _SYNCHRONISATION_WORD = {0xAA, 0x55};
        private int _CheckIndex;

        public GetSynchronisationWord(ICommunicationsLink comsLink, IResponseParser responseParser)
        {
            _ComsLink = comsLink;
            _ResponseParser = responseParser;

            _CheckIndex = 0;
        }

        public IRxState ProcessState()
        {
            if (_ComsLink.GetReceivedData(1, out int readCount, out byte[] data))
            {
                // we have a byte, so  check it
                // at this point we should have a byte but it may be worth checking
                if (_SYNCHRONISATION_WORD[_CheckIndex] == data[0])
                {
                    _CheckIndex++;

                    // TODO - Finish off from here.
                }
            }

            // Sync word has not completely been received so stay in this state
            return this;
        }
    }
}
