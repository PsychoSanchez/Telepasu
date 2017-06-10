using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Proxy.Messages.API;
using Proxy.Messages.API.Event;
using Proxy.ServerEntities.Messages;

namespace Proxy.Engine
{
    class MessageBrocker
    {
        private enum Messages
        {
            DialBeginEvent = 0,
            DialEndEvent,
            HangupEvent,
            HoldEvent,
            LoginEvent,
            NewChannelEvent,
            NewstateEvent,
            OriginateEvent,
            OriginateResponseEvent,
            ParkedCallEvent,
            ParkedCallsCompleteEvent,
            PeerEntryEvent,
            PeerlistCompleteEvent,
            SipShowPeerEvent,
            StatusEvent,
            UnholdEvent
        }

        private static readonly Dictionary<Type, Messages> AsteriskMessageTypes = new Dictionary<Type, Messages>
        {
            {typeof(DialBeginEvent), Messages.DialBeginEvent},
            {typeof(DialEndEvent),  Messages.DialEndEvent},
            {typeof(HangupEvent), Messages.HangupEvent},
            {typeof(HoldEvent), Messages.HoldEvent},
            {typeof(LoginEvent), Messages.LoginEvent},
            {typeof(NewChannelEvent),Messages.NewChannelEvent},
            {typeof(NewstateEvent), Messages.NewstateEvent},
            {typeof(OriginateEvent), Messages.OriginateEvent},
            {typeof(OriginateResponseEvent), Messages.OriginateResponseEvent},
            {typeof(ParkedCallEvent), Messages.ParkedCallEvent},
            {typeof(ParkedCallsCompleteEvent), Messages.ParkedCallsCompleteEvent},
            {typeof(PeerEntryEvent), Messages.PeerEntryEvent},
            {typeof(PeerlistCompleteEvent), Messages.PeerlistCompleteEvent},
            {typeof(SIPShowPeerEvent), Messages.SipShowPeerEvent},
            {typeof(StatusEvent), Messages.StatusEvent},
            {typeof(UnholdEvent), Messages.UnholdEvent}
        };
        public static string ConvertToApi(AsteriskMessage original)
        {
            Messages type = AsteriskMessageTypes[original.GetType()];

            JsonMessage json = null;
            switch (type)
            {
                case Messages.DialBeginEvent:
                    var converted = (DialBeginEvent)original;
                    json = new CallBegin()
                    {
                        Action = "Call Begin",
                        Channel = converted.ChannelID,
                        Destination = converted.DestinationID,
                        From = converted.CallerIDNum,
                        To = converted.ConnectedLineNum,
                        UniqueId = converted.Uniqueid,
                        UniqueId2 = converted.Uniqueid2
                    };
                    break;
                case Messages.DialEndEvent:
                    break;
                case Messages.HangupEvent:
                    break;
                case Messages.HoldEvent:
                    break;
                case Messages.LoginEvent:
                    break;
                case Messages.NewChannelEvent:
                    break;
                case Messages.NewstateEvent:
                    break;
                case Messages.OriginateEvent:
                    break;
                case Messages.OriginateResponseEvent:
                    break;
                case Messages.ParkedCallEvent:
                    break;
                case Messages.ParkedCallsCompleteEvent:
                    break;
                case Messages.PeerEntryEvent:
                    break;
                case Messages.PeerlistCompleteEvent:
                    break;
                case Messages.SipShowPeerEvent:
                    break;
                case Messages.StatusEvent:
                    break;
                case Messages.UnholdEvent:
                    break;
                default:
                    break;
            }
            return JsonConvert.SerializeObject(json);
        }
    }
}
