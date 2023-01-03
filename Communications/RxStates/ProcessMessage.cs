using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications.RxStates
{
    internal class ProcessMessage : IRxState
    {
        private LowLevelCommunicationsData _StateData;

        public ProcessMessage(LowLevelCommunicationsData stateData) 
        {
            _StateData = stateData;
        }

        public IRxState ProcessState()
        {
            // The message is Valid, so process it's contents

            // Process the IDs.
            _StateData.SetNextExpectedID();
            _StateData.AcknowledgeReceivedID();

            // and the payload, if there is one.
            if (0 == _StateData.PayloadLength)
            {
                // No more to do with the message so look for the start of the next message
                return new GetSynchronisationWord(_StateData);
            }

            if (!_StateData.MessagePayloadIsValid())
            {
                // Payload is not valid, so look for the start of the next message
                return new GetSynchronisationWord(_StateData);
            }

            //! \todo Process the message payload

            return new GetSynchronisationWord(_StateData);
        }
    }
}
