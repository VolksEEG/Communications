using System;
using System.Collections.Generic;
using System.Text;

namespace VolksEEG.Communications
{
    public class EEGData
    {
        public byte NumberOfSamplesPerChannel { get; internal set; }

        public List<Int16[]> SampleData { get; internal set; }

        public EEGData(byte samplesPerChannel, List<Int16[]> sampleData) 
        {
            NumberOfSamplesPerChannel = samplesPerChannel;
            SampleData = sampleData;
        }
    }
}
