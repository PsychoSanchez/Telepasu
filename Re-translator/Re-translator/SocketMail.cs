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
        public string Message;
        public MessageArgs(string _message)
        {
            Message = _message;
        }
    }
    public class SocketMail
    {
        public event EventHandler<MessageArgs> MessageRecieved;
        public event EventHandler<MessageArgs> Disconnected;
        BackgroundWorker reciever;
        Socket socket;
        public SocketMail(Socket _socket)
        {
            socket = _socket;
            initReciever();
        }
        private void initReciever()
        {
            reciever = new BackgroundWorker();
            reciever.DoWork += Listen;
            reciever.WorkerSupportsCancellation = true;
            reciever.RunWorkerAsync();
        }
        private bool SocketConnected()
        {
            return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);

            ///Simpler version
            //bool part1 = socket.Poll(1000, SelectMode.SelectRead);
            //bool part2 = (socket.Available == 0);
            //if (part1 && part2)
            //    return false;
            //else
            //    return true;
        }
        private void Listen(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            byte[] recvBuffer = new byte[100000];
            string Response = string.Empty;
            int buffersize;
            try
            {
                do
                {
                    Thread.Sleep(15);
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        Console.WriteLine("Asterisk thread disconnected");
                        return;
                    }
                    buffersize = socket.Receive(recvBuffer);

                    Response += Encoding.ASCII.GetString(recvBuffer, 0, buffersize);
                    if (Response.EndsWith(Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR))
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        OnMessageRecieved(new MessageArgs(Response));
                        Response = string.Empty;
                    }
                }
                while (SocketConnected());
            }
            catch (Exception ex)
            {
                LogMonitor.log(ex.Message + ex.StackTrace);
            }
            OnConnectionLost(null);
            Disconnect();
        }

        private void OnMessageRecieved(MessageArgs e)
        {
            MessageRecieved?.Invoke(this, e);
        }
        private void OnConnectionLost(MessageArgs e)
        {
            Disconnected?.Invoke(this, e);
        }
        public void SendMessage(string message)
        {
            byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
            if (socket.Connected)
            {
                try
                {
                    socket.Send(sendBuffer);
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
            reciever.CancelAsync();
            reciever.Dispose();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
