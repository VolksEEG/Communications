using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VolksEEG.Communications
{
    internal class LowLevelCommunications
    {
        public delegate void CommunicationsLostEventHandler();
        public event CommunicationsLostEventHandler CommunicationsLost;

        public bool CommunicationsIsActive { get; internal set; }

        private IRxState _CurrentRxState;
        private LowLevelCommunicationsData _LowLevelCommunicationsData;

        private CancellationTokenSource _CancellationTokenSource;

        private ICommunicationsLink _ComsLink;
        private IResponseParser _ResponseParser;

        private ConcurrentQueue<byte[]> _TransmitQueue;

        public LowLevelCommunications(ICommunicationsLink comsLink, IResponseParser responseParser)
        {
            _ComsLink = comsLink;
            _ResponseParser = responseParser;

            _TransmitQueue = new ConcurrentQueue<byte[]>();

            ResetCommunications();
            CommunicationsIsActive = false;
        }

        public void StartCommunications()
        {
            ResetCommunications();

            _CancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(ReceiveTask, _CancellationTokenSource.Token);
            Task.Factory.StartNew(TransmitTask, _CancellationTokenSource.Token);

            _ComsLink.Open();

            CommunicationsIsActive = true;
        }

        private void ReceiveTask()
        {
            while (!_CancellationTokenSource.IsCancellationRequested)
            {
                _CurrentRxState.ProcessState();
            }

            // Communications has been cancelled so raise the coms lost event
            CommunicationsIsActive = false;
            CommunicationsLost?.Invoke();
        }

        private void TransmitTask()
        {
            while(!_CancellationTokenSource.IsCancellationRequested)
            {
                // pop the next message of the transmit queue if there is one
                if (_TransmitQueue.TryDequeue(out byte[] message))
                {
                    // finalize the header with the most recent data
                    message[_LowLevelCommunicationsData._ID_ACKNOWLEDGE_INDEX] = _LowLevelCommunicationsData.LastReceivedID;
                    message[_LowLevelCommunicationsData._HEADER_CHECKSUM_INDEX] = _LowLevelCommunicationsData.GetHeaderChecksum(message);

                    _ComsLink.Write(message.Length, message);
                }
            }
        }

        public void StopCommunications()
        {
            _CancellationTokenSource.Cancel();

            _ComsLink.Close();
        }

        public void SendPayload(byte[] payload)
        {
            if (!CommunicationsIsActive)
            {
                // TODO Raise Exception
                return;
            }

            // check if payload will fit
            if (payload.Length > 255)
            {
                return;
            }

            byte[] message = new byte[payload.Length + _LowLevelCommunicationsData._HEADER_LENGTH];

            message[_LowLevelCommunicationsData._SYNC_WORD_LSB_INDEX] = _LowLevelCommunicationsData._SYNCHRONISATION_WORD[0];
            message[_LowLevelCommunicationsData._SYNC_WORD_MSB_INDEX] = _LowLevelCommunicationsData._SYNCHRONISATION_WORD[1];
            message[_LowLevelCommunicationsData._PROTOCOL_VERSION_INDEX] = _LowLevelCommunicationsData._PROTOCOL_VERSION;
            message[_LowLevelCommunicationsData._PAYLOAD_LENGTH_INDEX] = (byte)payload.Length;
            message[_LowLevelCommunicationsData._ID_NUMBER_INDEX] = _LowLevelCommunicationsData.ExpectedID;
            message[_LowLevelCommunicationsData._ID_ACKNOWLEDGE_INDEX] = 0; // this will be set when it is transmitted
            message[_LowLevelCommunicationsData._PAYLOAD_CHECKSUM_INDEX] = _LowLevelCommunicationsData.GetPayloadChecksum(payload);
            message[_LowLevelCommunicationsData._HEADER_CHECKSUM_INDEX] = 0; // this can't be set when the acknowledge ID is set.

            Array.Copy(payload, 0, message, _LowLevelCommunicationsData._PAYLOAD_START_INDEX, payload.Length);

            _TransmitQueue.Enqueue(message);
        }

        private void ResetCommunications()
        {
            _LowLevelCommunicationsData = new LowLevelCommunicationsData(_ComsLink, _ResponseParser);
            _CurrentRxState = new RxStates.GetSynchronisationWord(_LowLevelCommunicationsData);
        }

    }
}
