using Proxy.Helpers;
using System.Text;
///
namespace Proxy.ServerEntities.Messages
{
    /// <summary>
    /// http://www.asteriskdocs.org/en/2nd_Edition/asterisk-book-html-chunk/asterisk-APP-F-24.html
    /// https://wiki.asterisk.org/wiki/display/AST/ManagerAction_Park
    /// </summary>
    class ParkAction : AsteriskAction
    {
        public ParkAction(string channel1)
        {
            Channel = channel1;
        }
        public ParkAction(string channel1, string channel2, string timeout)
        {
            Channel = channel1;
            Channel2 = channel2;
            Timeout = timeout;
        }
        public ParkAction(string channel1, string channel2, string timeout, string parkinglot)
        {
            Channel = channel1;
            Channel2 = channel2;
            Timeout = timeout;
            ParkingLot = parkinglot;
        }
        public override string Action
        {
            get
            {
                return "Park";
            }
        }
        public string Channel { get; set; }
        public string Channel2 { get; set; }
        public string Timeout { get; set; }
        public string AnnounceChannel { get; set; }
        public string ParkingLot { get; set; }
    }
}

