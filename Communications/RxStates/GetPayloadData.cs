using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class GetPayloadData : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        private byte _BytesReceived;

        public GetPayloadData(LowLevelCommunicationsData stateData) 
        {
            _StateData = stateData;

            _BytesReceived = 0;
        }

        public IRxState ProcessState()
        {
            if (_StateData.ComsLink.Read(1, out int readCount, out byte[] data))
            {
                _StateData.Data[_BytesReceived++] = data[0];

                if (_BytesReceived == _StateData.PayloadLength)
                {
                    // Payload has been received so process the message
                    return new ProcessMessage(_StateData);
                }
            }

            return this;
        }
    }
}
