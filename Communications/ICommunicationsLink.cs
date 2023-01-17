using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications
{
    public interface ICommunicationsLink
    {
        public void Open();

        public void Close();

        public bool Read(int maxCount, out int readCount, out byte[] data);

        public void Write(int count, byte[] data);
    }
}
