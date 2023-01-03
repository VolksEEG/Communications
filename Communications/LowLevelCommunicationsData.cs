using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VolksEEG.Communications
{
    internal class LowLevelCommunicationsData
    {
        public ICommunicationsLink ComsLink { get; }
        public IResponseParser ResponseParser { get; }

        private byte _ExpectedID;
        public byte ExpectedID
        {
            get { return _ExpectedID; }
        }

        private byte _IdToAcknowledge;

        public byte IdToAcknowledge
        {
            set => _IdToAcknowledge = value;
        }

        private byte _PayloadChecksum;

        public byte PayloadChecksum
        {
            set => _PayloadChecksum = value;
        }

        private byte[] _Data;
        public byte[] Data { get { return _Data; } }

        public int PayloadLength { 
            get { return _Data.Length; }
            set { 
                _Data = new byte[value];
            }
        }

        public LowLevelCommunicationsData(ICommunicationsLink comsLink, IResponseParser responseParser)
        {
            ComsLink = comsLink;
            ResponseParser = responseParser;
            PayloadLength = 0;
        }

        public void SetNextExpectedID()
        {
            _ExpectedID++;
        }

        public void AcknowledgeReceivedID()
        {
            //! \todo Add event to process the received ID.
        }

        public bool MessageHeaderIsValid(byte rxChecksum)
        {
            return true;
        }

        public bool MessagePayloadIsValid()
        {
            return true;
        }
    }
}
