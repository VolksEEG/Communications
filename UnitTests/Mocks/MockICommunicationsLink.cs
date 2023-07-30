using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolksEEG.Communications;

namespace UnitTests.Mocks
{
    internal class MockICommunicationsLink : ICommunicationsLink
    {
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public bool Read(int maxCount, out int readCount, out byte[] data)
        {
            throw new NotImplementedException();
        }

        public void Write(int count, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
