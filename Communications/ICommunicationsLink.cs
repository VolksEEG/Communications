using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications
{
    public interface ICommunicationsLink
    {
        public bool GetReceivedData(int maxCount, out int readCount, out byte[] data);

        public bool TransmitData(int count, byte[] data);
    }
}
