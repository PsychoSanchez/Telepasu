using System;
using System.Net.Sockets;
using System.Text;

namespace Re_translator.ServerEntities
{
    public abstract class ServerEntity
    {
        public ServerRole role = ServerRole.Guest;
        public bool connectionStatus = false;
        public TcpClient client;
        public int ThreadNUmber;

        public bool Autorize()
        {
            StringBuilder sb = new StringBuilder();

            using (NetworkStream stream = client.GetStream())
            {
                int i;
                while ((i = stream.ReadByte()) != -1)
                {
                    sb.Append((char)i);
                }
                Console.WriteLine(sb.ToString());
                Console.WriteLine("Client accepted");
            }
        }
    }
}
