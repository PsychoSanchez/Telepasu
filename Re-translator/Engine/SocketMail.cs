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
        private const string ModuleName = "#(User Mail) ";
        public event EventHandler<MessageArgs> MessageRecieved;
        public event EventHandler<MessageArgs> Disconnected;
        public event EventHandler TimeOut;
        public bool IsApi = false;
        //ConnectionTimer Timer = null;
        private BackgroundWorker _reciever;
        private readonly Socket _socket;
        private readonly TcpClient _tcp;
        private NetworkStream _stream;
        private ConnectionTimer _timer;

        public SocketMail(TcpClient tcp)
        {
            _tcp = tcp;
            _timer = new ConnectionTimer(7500);
            _timer.TimeOut += _timer_TimeOut;
        }
        public SocketMail(Socket socket, int timeout)
        {
            _socket = socket;
            InitReciever();

            _timer = new ConnectionTimer(timeout);
            _timer.TimeOut += _timer_TimeOut;
        }

        private void _timer_TimeOut(object sender, EventArgs e)
        {
            Disconnected?.Invoke(this, null);
        }

        public SocketMail(Socket client)
        {
            _socket = client;
            InitReciever();
        }

        public void InitReciever()
        {
            _reciever = new BackgroundWorker();
            if (_tcp != null) _stream = _tcp.GetStream();
            _reciever.DoWork += Listen;
            _reciever.WorkerSupportsCancellation = true;
            _reciever.RunWorkerAsync();
            if (_timer != null) return;

            _timer = new ConnectionTimer(7500);
            _timer.TimeOut += _timer_TimeOut;
            _timer?.Start();
        }
        private bool SocketConnected()
        {
            return _socket != null && !(_socket.Poll(1000, SelectMode.SelectRead) && _socket.Available == 0 || !_socket.Connected);
        }
        private void Listen(object sender, DoWorkEventArgs e)
        {
            if (IsApi || !SocketConnected())
            {
                InnerListener(sender, e);
            }
            else
            {
                AsterListener(sender, e);
            }
        }

        /// <summary>
        /// Метод для прослушки сообщений с API от Asterisk
        /// </summary>
        private void AsterListener(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            byte[] recvBuffer = new byte[100000];
            string response = string.Empty;
            try
            {
                do
                {
                    Thread.Sleep(50);

                    e.Cancel = CheckCancel(worker);

                    var buffersize = _socket.Receive(recvBuffer);
                    response += Encoding.ASCII.GetString(recvBuffer, 0, buffersize);
                    if (!response.EndsWith(Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR)) continue;

                    OnMessageRecieved(new MessageArgs(response));
                    response = string.Empty;

                    e.Cancel = CheckCancel(worker);
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

        private bool CheckCancel(BackgroundWorker worker)
        {
            if (worker != null && !worker.CancellationPending) return false;

            telepasu.log(ModuleName + "Stopped listening");
            return true;
        }

        /// <summary>
        /// Метод для прослушки сообщений с внутренним API
        /// </summary>
        private void InnerListener(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Byte[] bytes = new Byte[1000000];
            int i;
            try
            {
                // Loop to receive all the data sent by the client.
                while (!e.Cancel && (i = _stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    var data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    // Process the data sent by the client.
                    e.Cancel = CheckCancel(worker);

                    OnMessageRecieved(new MessageArgs(data));

                    e.Cancel = CheckCancel(worker);
                    Thread.Sleep(5);
                }
            }
            catch (Exception exception)
            {
                OnConnectionLost(new MessageArgs(exception.Message));
                telepasu.exc(exception);
            }

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
            _timer?.Reset();
            MessageRecieved?.Invoke(this, e);
        }
        private void OnConnectionLost(MessageArgs e)
        {
            Disconnected?.Invoke(this, e);
        }
        public void SendMessage(string message)
        {
            telepasu.log(ModuleName + "Message send: " + Helper.END_LINE + message);
            byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
            if (_socket == null || !_socket.Connected)
            {
                SendApiMessage(message);
                return;
            }

            try
            {
                _socket.Send(sendBuffer);
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                    telepasu.exc(e);
                    OnConnectionLost(new MessageArgs(e.Message));
                }
            }
            catch (ObjectDisposedException e)
            {
                telepasu.exc(e);
                OnConnectionLost(new MessageArgs(e.Message));
            }
        }

        public void SendApiMessage(string message)
        {
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(message);
                // Send back a response.
                _stream.Write(msg, 0, msg.Length);

            }
            catch (Exception exception)
            {
                Disconnect();
                telepasu.exc(exception);
            }

        }

        public void Disconnect()
        {
            _reciever.CancelAsync();
            _reciever.Dispose();
            try
            {
                _socket?.Shutdown(SocketShutdown.Both);
                _tcp?.GetStream().Close();
                _tcp?.Close();
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
