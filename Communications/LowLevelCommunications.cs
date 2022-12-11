using System;
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

        public bool CommunicationsIsActive;


        private IRxState _CurrentRxState;

        private CancellationTokenSource _CancellationTokenSource;

        private ICommunicationsLink _ComsLink;
        private IResponseParser _ResponseParser;

        public LowLevelCommunications(ICommunicationsLink comsLink, IResponseParser responseParser)
        {
            _ComsLink = comsLink;
            _ResponseParser = responseParser;

            CommunicationsIsActive = false;
        }

        public void StartCommunications()
        {
            ResetCommunications();

            _CancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(ReceiveTask, _CancellationTokenSource.Token);

            CommunicationsIsActive = true;
        }

        private void ReceiveTask()
        {
            while (_CancellationTokenSource.IsCancellationRequested)
            {
                _CurrentRxState.ProcessState();
            }

            // Communications has been cancelled so raise the coms lost event
            CommunicationsIsActive = false;
            CommunicationsLost?.Invoke();
        }

        public void StopCommunications()
        {
            _CancellationTokenSource.Cancel();
        }

        public void SendPayload()
        {

        }

        private void ResetCommunications()
        {
            _CurrentRxState = new RxStates.GetSynchronisationWord(new LowLevelCommunicationsData(_ComsLink, _ResponseParser));
        }

    }
}
