using Proxy.Helpers;
using Proxy.Messages;
using Proxy.ServerEntities.Messages;
using System;
using System.Collections.Generic;

namespace Proxy.ServerEntities.Asterisk
{
    public class MessagesParser
    {
        public List<AsteriskAction> ToActionList(string messageLine)
        {
            List<AsteriskAction> list = new List<AsteriskAction>();
            var messageArray = messageLine.Split(Helper.END_MESSAGE, StringSplitOptions.RemoveEmptyEntries);
            foreach (var message in messageArray)
            {
                list.Add(new ResendAction(message));
            }
            return list;
        }
        public List<AsteriskMessage> ToMessagesList(string response)
        {
            List<AsteriskMessage> messageList = new List<AsteriskMessage>();
            var messageArray = response.Split(Helper.END_MESSAGE, StringSplitOptions.RemoveEmptyEntries);
            foreach (var message in messageArray)
            {
                var temp = message;
                temp += Helper.LINE_SEPARATOR + Helper.LINE_SEPARATOR;
                string messageType = Helper.GetAsteriskMessageType(temp);
                if (messageType == "Event")
                {
                    var innerMessage = ParseEvent(temp);
                    if (innerMessage != null)
                    {
                        messageList.Add(innerMessage);
                    }

                }
                else if (messageType == "Response")
                {
                    var innerMessage = ParseResponse(temp);
                    if (innerMessage != null)
                    {
                        messageList.Add(innerMessage);
                    }
                }
            }
            return messageList;
        }
        private AsteriskMessage ParseEvent(string message)
        {
            string eventType = Helper.GetValue(message, "Event: ");
            AsteriskMessage innerMessage = null;

            switch (eventType)
            {
                //case "Bridge":
                //    break;
                case "Dial":
                    var subEvent = Helper.GetValue(message, "SubEvent: ");
                    if (subEvent.Equals("End"))
                    {
                        innerMessage = new DialEndEvent(message);
                    }
                    else
                    {
                        innerMessage = new DialBegin11(message);
                    }
                    break;
                case "DialBegin":
                    innerMessage = new DialBeginEvent(message);
                    break;
                case "DialEnd":
                    innerMessage = new DialEndEvent(message);
                    break;
                //case "ExstensionStatus":
                //    break;
                case "Hangup":
                    innerMessage = new HangupEvent(message);
                    break;
                case "Hold":
                    innerMessage = new HoldEvent(message);
                    break;
                //case "MusicOnHold":
                //    break;
                case "NewCallerid":
                    innerMessage = new NewstateEvent(message);
                    break;
                case "Newchannel":
                    innerMessage = new NewChannelEvent(message);
                    break;
                case "Newstate":
                    innerMessage = new NewstateEvent(message);
                    break;
                case "OriginateResponse":
                    innerMessage = new OriginateResponseEvent(message);
                    break;
                case "ParkedCall":
                    innerMessage = new ParkedCallEvent(message);
                    break;
                case "ParkedCallsComplete":
                    innerMessage = new ParkedCallsCompleteEvent(message);
                    break;
                case "PeerEntry":
                    innerMessage = new PeerEntryEvent(message);
                    break;
                //case "PeerStatus":
                //    break;
                case "PeerlistComplete":
                    innerMessage = new PeerlistCompleteEvent(message);
                    break;
                case "SoftHangupRequest":
                    //new HangupEvent(_message);
                    break;
                case "Status":
                    innerMessage = new StatusEvent(message);
                    break;
                //case "StatusComplete":
                //    break;
                //case "RTCPSent":
                //    break;
                //case "RTCPReceived":
                //    break;
                case "Unhold":
                    innerMessage = new UnholdEvent(message);
                    break;
                default:
                    innerMessage = new UnknownEvent(message);
                    break;
            }

            return innerMessage;
        }
        private AsteriskMessage ParseResponse(string message)
        {
            string eventType = Helper.GetValue(message, "Response: ");
            AsteriskMessage innerMessage = null;

            switch (eventType)
            {
                case "Success":
                    if (message.Contains("Ping: Pong"))
                    {
                        innerMessage = new PingEvent(message);
                    }
                    else if (message.Contains("Message: "))
                    {
                        if (message.Contains("Authentication"))
                        {
                            innerMessage = new LoginEvent(message);
                        }
                        else if (message.Contains("Originate successfully"))
                        {
                            innerMessage = new OriginateEvent(message);
                        }
                        else
                        {
                            innerMessage = new UnknownEvent(message);
                        }
                    }
                    else if (message.Contains("Channeltype: ") && message.Contains("Context: ") && message.Contains("Address-IP: "))
                    {
                        innerMessage = new SIPShowPeerEvent(message);
                    }
                    else if (message.Contains("Challenge: "))
                    {
                        innerMessage = new ChallengeEvent(message);
                    }
                    else
                    {
                        innerMessage = new UnknownEvent(message);
                    }
                    break;
                case "Error":
                    if (message.Contains("Authentication"))
                    {
                        innerMessage = new LoginEvent(message);
                    }
                    else if (message.Contains("Peer") && message.Contains("not found"))
                    {
                        innerMessage = new SIPShowPeerEvent(message);
                    }
                    else if (message.Contains("Extension") && message.Contains("not exist"))
                    {
                        innerMessage = new OriginateEvent(message);
                    }
                    else
                    {
                        innerMessage = new UnknownEvent(message);
                    }
                    break;
                case "Follows":
                    if (message.Contains("Channel              Location"))
                    {
                        innerMessage = new CoreShowChannelsEvent(message);
                    }
                    else
                    {
                        innerMessage = new UnknownEvent(message);
                    }
                    break;
                default:
                    innerMessage = new UnknownEvent(message);
                    break;
            }

            return innerMessage;
        }
    }
}
