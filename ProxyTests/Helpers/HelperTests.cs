using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Proxy.Helpers.Tests
{
    [TestClass()]
    public class HelperTests
    {
        private const string LoginMessage =
            "Action: Login\r\n" +
            "ActionID: ALEXAdmin2\r\n" +
            "UserName: mark\r\n" +
            "AuthType: MD5\r\n" +
            "Key: 431f97d578354510c987ddfb5f33d8dd\r\n\r\n";

        private const string WelcomMessage =
            "Asterisk Call Manager/1.1\r\n" +
            "Response: Success\r\n" +
            "ActionID: ALEXAdmin1\r\n" +
            "Challenge: 9ZZRDeIRK8\r\n\r\n";

        private const string BridgeEvent =
            "Event: Bridge\r\n"+
            "Privilege: call,all\r\n"+
            "Bridgestate: Link\r\n"+
            "Bridgetype: core\r\n"+
            "Channel1: SIP/106-00000008\r\n"+
            "Channel2: SIP/105-00000009\r\n"+
            "Uniqueid1: 1473975752.8\r\n"+
            "Uniqueid2: 1473975754.9\r\n"+
            "CallerID1: 106\r\n"+
            "CallerID2: 105\r\n\r\n";

        private const string JsonMessage = "{\"Action\":\"Add Module\",\"Id\":\"1\",\"IP\":\"localhost\",\"Port\":\"5000\"}";

        [TestMethod()]
        public void GetValueTest()
        {
            var value = Helper.GetValue(LoginMessage, "action");
            var value1 = Helper.GetValue(LoginMessage, "Action");
            var value2 = Helper.GetValue(LoginMessage, "Action: ");
            var value3 = Helper.GetValue(LoginMessage, "");
            var value4 = Helper.GetValue(LoginMessage, null);
            var value5 = Helper.GetValue(null, null);
            var value6 = Helper.GetValue(null, "action");
            Assert.AreEqual("Login", value);
            Assert.AreEqual("Login", value1);
            Assert.AreEqual("Login", value2);
            Assert.AreEqual("", value3);
            Assert.AreEqual("", value4);
            Assert.AreEqual("", value5);
            Assert.AreEqual("", value6);

            var notValue = Helper.GetValue(LoginMessage, "action:");
            Assert.AreNotEqual("Login", notValue);
        }

        [TestMethod()]
        public void GetAsteriskMessageTypeTest()
        {
            var value = Helper.GetAsteriskMessageType(WelcomMessage);
            var value2 = Helper.GetAsteriskMessageType(LoginMessage);
            var value3 = Helper.GetAsteriskMessageType(null);
            var value4 = Helper.GetAsteriskMessageType("");
            Console.WriteLine(value);
            Assert.AreEqual("Response", value);
            Assert.AreEqual("Action", value2);
            Assert.AreEqual("", value3);
            Assert.AreEqual("", value4);
        }

        [TestMethod()]
        public void GetJsonValueTest()
        {
            var value = Helper.GetJsonValue(JsonMessage, "Action");
            var value2 = Helper.GetJsonValue(JsonMessage, "IP");
            var value3 = Helper.GetJsonValue(JsonMessage, null);
            var value4 = Helper.GetJsonValue(null, null);
            var value5 = Helper.GetJsonValue(null, "Action");
            Assert.AreEqual("Add Module", value);
            Assert.AreEqual("localhost", value2);
            Assert.AreEqual("", value3);
            Assert.AreEqual("", value4);
            Assert.AreEqual("", value5);
        }

        [TestMethod()]
        public void GetNumberFromChannelTest()
        {
            var channel = Helper.GetValue(BridgeEvent, "Channel1");
            var value = Helper.GetNumberFromChannel(channel);
            var value2 = Helper.GetNumberFromChannel(null);
            var value3 = Helper.GetNumberFromChannel("");
            Assert.AreEqual("106", value);
            Assert.AreEqual("", value2);
            Assert.AreEqual("", value3);
        }
    }
}