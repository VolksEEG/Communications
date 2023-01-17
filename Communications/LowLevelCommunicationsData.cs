using NullFX.CRC;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VolksEEG.Communications
{
    internal class LowLevelCommunicationsData
    {
        public delegate void NewIDToAcknowledgeEventHandler(byte id);
        public event NewIDToAcknowledgeEventHandler NewIdToAcknowledge;

        public readonly byte _HEADER_LENGTH = 8;
        public readonly byte[] _SYNCHRONISATION_WORD = { 0xAA, 0x55 };
        public readonly byte _SYNC_WORD_LSB_INDEX = 0;
        public readonly byte _SYNC_WORD_MSB_INDEX = 1;

        public readonly byte _PROTOCOL_VERSION = 0x01;
        public readonly byte _PROTOCOL_VERSION_INDEX = 2;

        public readonly byte _PAYLOAD_LENGTH_INDEX = 3;

        public readonly byte _ID_NUMBER_INDEX = 4;
        public readonly byte _ID_ACKNOWLEDGE_INDEX = 5;
        public readonly byte _PAYLOAD_CHECKSUM_INDEX = 6;
        public readonly byte _HEADER_CHECKSUM_INDEX = 7;
        public readonly byte _PAYLOAD_START_INDEX = 8;

        public ICommunicationsLink ComsLink { get; }
        public IResponseParser ResponseParser { get; }

        private byte _ExpectedID;
        public byte ExpectedID
        {
            get { return _ExpectedID; }
        }

        private byte _LastReceivedId;
        public byte LastReceivedID
        {
            get { return _LastReceivedId; }
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

            _ExpectedID = 0;
            _LastReceivedId = 0xFF;
        }

        public void SetNextExpectedID()
        {
            _LastReceivedId = _ExpectedID;
            _ExpectedID++;
        }

        public void AcknowledgeReceivedID()
        {
            NewIdToAcknowledge?.Invoke(_IdToAcknowledge);
        }

        public bool MessageHeaderIsValid(byte rxChecksum)
        {
            return true;
        }

        public bool MessagePayloadIsValid()
        {
            return _PayloadChecksum == GetPayloadChecksum(_Data);
        }

        public byte GetPayloadChecksum(byte[] payload)
        {
            return Crc8.ComputeChecksum(payload);
        }

        public byte GetHeaderChecksum(byte[] header)
        {
            return 0x01;
        }
    }
}
