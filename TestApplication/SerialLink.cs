using System;
using VolksEEG.Communications;
using System.IO.Ports;

namespace TestApplication
{
    internal class SerialLink : ICommunicationsLink
    {
        private SerialPort _Serial;

        public SerialLink(string port) 
        {
            _Serial = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);
        }

        public void Close()
        {
            _Serial.Close();
        }

        public void Open()
        {
            _Serial.Open();
        }

        public bool Read(int maxCount, out int readCount, out byte[] data)
        {
            data = new byte[maxCount];

            readCount = _Serial.Read(data, 0, maxCount);

            return (0 != readCount);
        }

        public void Write(int count, byte[] data)
        {
            _Serial.Write(data, 0, count);
        }
    }
}
