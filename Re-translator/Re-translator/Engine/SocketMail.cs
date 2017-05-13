using Proxy.Helpers;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Proxy
{
    public class MessageArgs : EventArgs
    {
        public readonly string Message;
        public MessageArgs(string message)
        {
            Message = message;
        }
    }
    public class SocketMail
    {
        public event EventHandler<MessageArgs> MessageRecieved;
        public event EventHandler<MessageArgs> Disconnected;
        public event EventHandler TimeOut;
        //ConnectionTimer Timer = null;
        private BackgroundWorker _reciever;
        private readonly Socket _socket;
        public SocketMail(Socket socket)
        {
            _socket = socket;
            InitReciever();
        }
        public SocketMail(Socket socket, int timeout)
        {
            _socket = socket;
            InitReciever();
            //Timer = new ConnectionTimer(timeout);
            //Timer.TimeOut += OnTimeOut;
            //Timer.Start();
        }

        private void InitReciever()
        {
            _reciever = new BackgroundWorker();
            _reciever.DoWork += Listen;
            _reciever.WorkerSupportsCancellation = true;
            _reciever.RunWorkerAsync();
        }
        private bool SocketConnected()
        {
            return !(_socket.Poll(1000, SelectMode.SelectRead) && _socket.Available == 0 || !_socket.Connected);
        }
        private void Listen(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            byte[] recvBuffer = new byte[100000];
            string response = string.Empty;
            try
            {
                do
                {
                    Thread.Sleep(50);
                    if (worker != null && worker.CancellationPending)
                    {
                        e.Cancel = true;
                        telepasu.log("Stopped listening");
                        return;
                    }
                    var buffersize = _socket.Receive(recvBuffer);

                    response += Encoding.ASCII.GetString(recvBuffer, 0, buffersize);
                    if (!response.EndsWith(Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR)) continue;
                    if (worker != null && worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    OnMessageRecieved(new MessageArgs(response));
                    response = string.Empty;
                }
                while (SocketConnected());
            }
            catch (Exception ex)
            {
                telepasu.exc(ex);
            }
            OnConnectionLost(null);
            Disconnect();
        }
        public void StopListen()
        {
            _reciever.CancelAsync();
            _reciever.Dispose();
        }
        private void OnTimeOut(object sender, EventArgs e)
        {
            //Timer.Stop();
            TimeOut?.Invoke(this, null);
        }
        private void OnMessageRecieved(MessageArgs e)
        {
            //if (Timer != null)
            //{
            //    Timer.Reset();
            //}
            MessageRecieved?.Invoke(this, e);
        }
        private void OnConnectionLost(MessageArgs e)
        {
            Disconnected?.Invoke(this, e);
        }
        public void SendMessage(string message)
        {
            byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
            if (_socket.Connected)
            {
                try
                {
                    _socket.Send(sendBuffer);
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.TimedOut)
                    {
                        //if (IsConnected)
                        //{
                        //    if (Disconnecting != null)
                        //    {
                        //        Disconnecting(this, null);
                        //    }
                        //    log.WriteLog("##Disconnectiong socketexceps timeout 1033");
                        //    Abort();
                        //    return;
                        //}
                    }
                }
                catch (ObjectDisposedException e)
                {
                    //if (IsConnected)
                    //{
                    //    if (Disconnecting != null)
                    //    {
                    //        Disconnecting(this, null);
                    //    }
                    //    log.WriteLog("##Disconnectiong disposed 1047");
                    //    Abort();
                    //    return;
                    //}
                }
            }
            else
            {
                return;
            }
        }

        public void Disconnect()
        {
            _reciever.CancelAsync();
            _reciever.Dispose();
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                telepasu.log(e.Message + e.StackTrace);
            }
            finally
            {
                _socket?.Close();
            }
        }
    }
}
