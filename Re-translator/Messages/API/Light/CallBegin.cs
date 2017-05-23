namespace Proxy.Messages.API.Event
{
    class CallBegin: JsonMessage
    {
        public string From;
        public string To;
        public string UniqueId;
        public string UniqueId2;
        public string Channel;
        public string Destination;
    }
}
