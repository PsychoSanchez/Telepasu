﻿using System;
using System.ComponentModel;
using System.Threading;

namespace Proxy.Helpers
{
    class ConnectionTimer
    {
        AutoResetEvent timer = new AutoResetEvent(true);
        int timeout;
        public event EventHandler TimeOut;
        BackgroundWorker worker;
        public ConnectionTimer(int timeout)
        {
            this.timeout = timeout;
        }
        public void Start()
        {
            init();
            worker.RunWorkerAsync();
        }
        public void Reset()
        {
            timer.Set();
        }
        public void Stop()
        {
            worker.CancelAsync();
        }
        private void init()
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += TimerWork;
        }

        private void TimerWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (!worker.CancellationPending)
            {
                if (!timer.WaitOne(timeout))
                {
                    if (worker.CancellationPending)
                    {
                        return;
                    }
                    OnTimeOut();
                }
            }
        }

        private void OnTimeOut()
        {
            TimeOut?.Invoke(this, null);
        }
    }
}
