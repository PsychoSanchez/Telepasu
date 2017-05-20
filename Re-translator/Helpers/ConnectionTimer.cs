using System;
using System.ComponentModel;
using System.Threading;

namespace Proxy.Helpers
{
    class ConnectionTimer
    {
        readonly AutoResetEvent _timer = new AutoResetEvent(true);
        readonly int _timeout;
        public event EventHandler TimeOut;
        private bool _stopTimer = false;
        BackgroundWorker _worker;
        public ConnectionTimer(int timeout)
        {
            this._timeout = timeout;
        }
        public void Start()
        {
            Init();
            _worker.RunWorkerAsync();
        }

        public void Reset()
        {
            _timer.Set();
        }
        public void Stop()
        {
            _stopTimer = true;
            _worker.CancelAsync();
        }
        private void Init()
        {
            _worker = new BackgroundWorker { WorkerSupportsCancellation = true };
            _worker.DoWork += TimerWork;
        }

        private void TimerWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (worker != null && !_stopTimer)
            {
                if (_timer.WaitOne(_timeout)) continue;

                if (worker.CancellationPending)
                {
                    return;
                }
                if (_stopTimer)
                {
                    return;
                }
                OnTimeOut();
            }
        }

        private void OnTimeOut()
        {
            Stop();
            TimeOut?.Invoke(this, null);
        }
    }
}
