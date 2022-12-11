using System;
using System.Threading;
using System.Threading.Tasks;

namespace VolksEEG.Communications
{
    public class VolksEegCommunications
    {
        public delegate void CommunicationsLostEventHandler();
        public event CommunicationsLostEventHandler CommunicationsLost;

        private ICommunicationsLink _ComsLink;
        private IResponseParser _ResponseParser;

        private IRxState _CurrentRxState;

        private CancellationTokenSource _CancellationTokenSource;

        public VolksEegCommunications(ICommunicationsLink comsLink, IResponseParser responseParser)
        {
            _ComsLink = comsLink;
            _ResponseParser = responseParser;
        }

        public void StartCommunicationsAsync()
        {
            ResetCommunications();

            _CancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(ReceiveTask, _CancellationTokenSource.Token);
        }

        private void ReceiveTask()
        {
            while(_CancellationTokenSource.IsCancellationRequested)
            {
                _CurrentRxState.ProcessState();
            }

            // Communications has been cancelled so raise the coms lost event
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
            _CurrentRxState = new RxStates.GetSynchronisationWord(_ComsLink, _ResponseParser);
        }
    }
}
