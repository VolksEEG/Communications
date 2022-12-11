using System;
using System.Threading;
using System.Threading.Tasks;

namespace VolksEEG.Communications
{
    public class VolksEegCommunications : IResponseParser
    {
        private LowLevelCommunications _Coms;

        public VolksEegCommunications(ICommunicationsLink comsLink)
        {
            _Coms = new LowLevelCommunications(comsLink, this);
        }

        public void StartCommunications()
        {
            if (_Coms == null)
            {
                //! \todo throw an exception
                return;
            }

            if (_Coms.CommunicationsIsActive)
            {
                // communications is already active 
                return;
            }

            _Coms.StartCommunications();
        }

        public void StartDataCapture()
        {
            _Coms.SendPayload();
        }

        public void StopDataCapture()
        {
            _Coms.SendPayload();
        }

        public void ParseResponse(byte[] response)
        {
            
        }
    }
}
