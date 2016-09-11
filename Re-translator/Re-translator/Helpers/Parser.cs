using Proxy.ServerEntities;
using Proxy.ServerEntities.Messages;
using System;
using System.Collections.Generic;

namespace Proxy.Helpers
{
    public class Parser
    {
        public List<AsteriskMessage> ToMessagesList(string response)
        {
            List<AsteriskMessage> message_list = new List<AsteriskMessage>();
            var message_array = response.Split(Helper.END_MESSAGE, StringSplitOptions.RemoveEmptyEntries);
            foreach (var message in message_array)
            {
                string message_type = Helper.GetAsteriskMessageType(message);
                if (message_type == "Event")
                {
                    var inner_message = ParseEvent(message);
                    if (inner_message != null)
                    {
                        message_list.Add(inner_message);
                    }

                }
                else if (message_type == "Response")
                {
                    var inner_message = ParseResponse(message);
                    if (inner_message != null)
                    {
                        message_list.Add(inner_message);
                    }
                }

            }
            return message_list;
        }
        private AsteriskMessage ParseEvent(string message)
        {
            string event_type = Helper.GetParameterValue(message, "Event: ");
            AsteriskMessage innerMessage = null;

            switch (event_type)
            {
                case "Bridge":
                    break;
                case "Dial":
                    var SubEvent = Helper.GetParameterValue(message, "SubEvent: ");
                    if (SubEvent.Equals("End"))
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
                case "ExstensionStatus":
                    break;
                case "Hangup":
                    innerMessage = new HangupEvent(message);
                    break;
                case "Hold":
                    innerMessage = new HoldEvent(message);
                    break;
                case "MusicOnHold":
                    break;
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

                case "PeerStatus":
                    break;
                case "PeerlistComplete":
                    innerMessage = new PeerlistCompleteEvent(message);
                    break;
                case "SoftHangupRequest":
                    //new HangupEvent(_message);
                    break;
                case "Status":
                    innerMessage = new StatusEvent(message);
                    break;
                case "StatusComplete":
                    break;
                case "RTCPSent":
                    break;
                case "RTCPReceived":
                    break;
                case "Unhold":
                    innerMessage = new UnholdEvent(message);
                    break;
                default:
                    break;
            }

            return innerMessage;
        }
        private AsteriskMessage ParseResponse(string message)
        {
            string event_type = Helper.GetParameterValue(message, "Response: ");
            AsteriskMessage innerMessage = null;

            switch (event_type)
            {
                case "Success":
                    if (message.Contains("Ping: Pong"))
                    {
                        innerMessage = new PingEvent(message);
                    }
                    else  if (message.Contains("Message: "))
                    {
                        if (message.Contains("Authentication"))
                        {
                            innerMessage = new LoginEvent(message);
                        }
                        else if (message.Contains("Originate successfully"))
                        {
                            innerMessage = new OriginateEvent(message);
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
                    break;
                case "Follows":
                    if (message.Contains("Channel              Location"))
                    {
                        innerMessage = new CoreShowChannelsEvent(message);
                    }
                    break;
                default:
                    break;
            }

            return innerMessage;
        }
    }
}
