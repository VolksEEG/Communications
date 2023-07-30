using UnitTests.Mocks;
using VolksEEG.Communications;

namespace TestApplication
{
    public class ChecksumTests
    {
        [Fact]
        public void PayloadChecksumComfirmCorrectValueIsGenerated()
        {
            // Arrange
            byte[] payload = new byte[9];

            // Set all values to the test array 0x31 0x32 0x33 0x34 0x35 0x36 0x37 0x38 0x39
            for (byte i = 0; i < payload.Length; i++)
            {
                payload[i] = (byte)(0x31 + i);
            }

            MockICommunicationsLink mockICommunicationsLink = new MockICommunicationsLink();
            MockIResponseParser mockResponseParser = new MockIResponseParser();

            LowLevelCommunicationsData lowLevelCommunications = new LowLevelCommunicationsData(mockICommunicationsLink, mockResponseParser);

            // Act
            byte calc_checksum = lowLevelCommunications.GetPayloadChecksum(payload);

            // Assert
            Assert.Equal(0xBC, calc_checksum);
        }

        [Fact]
        public void HeaderChecksumComfirmCorrectValueIsGenerated()
        {
            // Arrange
            byte[] header = new byte[9];

            // Set all values to the test array 0x31 0x32 0x33 0x34 0x35 0x36 0x37 0x38 0x39
            for (byte i = 0; i < header.Length; i++)
            {
                header[i] = (byte)(0x31 + i);
            }

            MockICommunicationsLink mockICommunicationsLink = new MockICommunicationsLink();
            MockIResponseParser mockResponseParser = new MockIResponseParser();

            LowLevelCommunicationsData lowLevelCommunications = new LowLevelCommunicationsData(mockICommunicationsLink, mockResponseParser);

            // Act
            byte calc_checksum = lowLevelCommunications.GetHeaderChecksum(header);

            // Assert
            Assert.Equal(0xBC, calc_checksum);
        }
    }
}