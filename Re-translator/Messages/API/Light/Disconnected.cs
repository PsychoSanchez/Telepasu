namespace Proxy.Messages.API.Light
{
    class Disconnected: JsonMessage
    {
        public int Status;
        public string Message;

        public Disconnected()
        {
            Action = "Disconnected";
        }
    }
}
