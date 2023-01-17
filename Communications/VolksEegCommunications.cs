using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace VolksEEG.Communications
{
    public class VolksEegCommunications : IResponseParser
    {
        public delegate void NewEegDataEventHandler(EEGData eegData);
        public event NewEegDataEventHandler NewEegData;

        // command groups
        private static readonly byte ACKNOWLEDGE_COMMAND_GROUP = 0x00;
        private static readonly byte QUERY_COMMAND_GROUP = 0x20;
        private static readonly byte WRITE_COMMAND_GROUP = 0x40;

        // commands
        private static readonly byte ACK_COMMAND = 0x00;
        private static readonly byte NACK_COMMAND = 0x10;
        private static readonly byte EEG_DATA_MODE_COMMAND = 0x01;
        private static readonly byte EEG_DATA_VALUES_COMMAND = 0x02;

        // EEG Data Mode Payload
        private static readonly byte EEG_DATA_MODE_DISABLED = 0x00;
        private static readonly byte EEG_DATA_MODE_ENABLED = 0x01;

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
            byte[] payload = new byte[2];

            payload[0] = (byte)(WRITE_COMMAND_GROUP | EEG_DATA_MODE_COMMAND);
            payload[1] = EEG_DATA_MODE_ENABLED;

            _Coms.SendPayload(payload);
        }

        public void StopDataCapture()
        {
            byte[] payload = new byte[2];

            payload[0] = (byte)(WRITE_COMMAND_GROUP | EEG_DATA_MODE_COMMAND);
            payload[1] = EEG_DATA_MODE_DISABLED;

            _Coms.SendPayload(payload);
        }

        public void ParseResponse(byte[] response)
        {
            
        }
    }
}
