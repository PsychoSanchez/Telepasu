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
            reciever.DoWork += RecieverWork;
            reciever.WorkerSupportsCancellation = true;
            reciever.RunWorkerAsync();
        }

        private void RecieverWork(object sender, DoWorkEventArgs e)
        {
            byte[] recvBuffer = new byte[100000];
            string Response = string.Empty;
            int buffersize;
            BackgroundWorker worker = sender as BackgroundWorker;
            try
            {
                do
                {
                    //Нужен для того, чтобы буфер заполнился
                    Thread.Sleep(10);
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        Console.WriteLine("Asterisk thread disconnected");
                        return;
                    }
                    buffersize = socket.Receive(recvBuffer);
                    //Перекодируем его к строковому типу
                    Response += Encoding.ASCII.GetString(recvBuffer, 0, buffersize);
                    if (Response.EndsWith(Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR))
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        ///Кладем сообщение в общую память парсера
                        OnMessageRecieved(new MessageArgs(Response));
                        Response = string.Empty;
                        //AsterParser.Response = Response;
                        //Разрешаем ему доступ к данным
                        //AsterParser.startParse.Set();
                        //Обнуляем строку
                    }
                }
                while (socket.Connected);
            }
            catch (SocketException ex)
            {
                //if (ex.SocketErrorCode == SocketError.TimedOut)
                //    if (ManagerConnect != null && ManagerConnect.IsSockConnected())
                //    {
                //        //SendPing.Set();
                //        //Thread.Sleep(3000);
                //        if (!ManagerConnect.IsSockConnected())
                //        {
                //            throw new AmiTimeOutException("Слишком долгое время ожидания ответа от сервера");
                //        }
                //    }

                //if (IsAlive)
                //    //Console.WriteLine(ex.Message);
                //    return;
                return;
            }
            catch (ObjectDisposedException ex)
            {
                //if (IsAlive)
                //    //Console.WriteLine(ex.Message);
                //    return;
                return;
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        private void OnMessageRecieved(MessageArgs e)
        {
            MessageRecieved?.Invoke(this, e);
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
    }
}
