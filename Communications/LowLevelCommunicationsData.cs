using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications
{
    internal class LowLevelCommunicationsData
    {
        public ICommunicationsLink ComsLink { get; }
        public IResponseParser ResponseParser { get; }

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

    }
}
