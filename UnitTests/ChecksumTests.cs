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

            Random rng = new Random();
            byte[] random_values = new byte[3];

            rng.NextBytes(random_values);

            header[0] = random_values[0];   // These random values should have no impact on the the checksum
            header[1] = random_values[1];
            header[2] = 0x31;
            header[3] = 0x32;
            header[4] = 0x33;
            header[5] = 0x34;
            header[6] = 0x35;
            header[7] = random_values[2];

            MockICommunicationsLink mockICommunicationsLink = new MockICommunicationsLink();
            MockIResponseParser mockResponseParser = new MockIResponseParser();

            LowLevelCommunicationsData lowLevelCommunications = new LowLevelCommunicationsData(mockICommunicationsLink, mockResponseParser);

            // Act
            byte calc_checksum = lowLevelCommunications.GetHeaderChecksum(header);

            // Assert
            Assert.Equal(0x64, calc_checksum);
        }

        [Fact]
        public void HeaderChecksumComfirmCorrectValueIsGeneratedForRepresentativePacket()
        {
            // Arrange
            byte[] header = new byte[9];

            header[0] = 0xAA;
            header[1] = 0x55;
            header[2] = 0x01;
            header[3] = 0x0A;
            header[4] = 0x00;
            header[5] = 0x00;
            header[6] = 0x00;

            MockICommunicationsLink mockICommunicationsLink = new MockICommunicationsLink();
            MockIResponseParser mockResponseParser = new MockIResponseParser();

            LowLevelCommunicationsData lowLevelCommunications = new LowLevelCommunicationsData(mockICommunicationsLink, mockResponseParser);

            // Act
            byte calc_checksum = lowLevelCommunications.GetHeaderChecksum(header);

            // Assert
            Assert.Equal(0x6B, calc_checksum);
        }
    }
}