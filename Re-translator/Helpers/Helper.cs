using Proxy.ServerEntities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Proxy.Helpers
{
    public class Helper
    {
        #region API CONSTANTS
        public static string MachineID = Helper.ConvertToTranslit(Environment.MachineName.Replace(" ", "")) + Helper.ConvertToTranslit(Environment.UserName.Replace(" ", ""));

        private static Dictionary<char, string> _rusEng;
        private static void InitDictionary()
        {
            _rusEng = new Dictionary<char, string>();
            var rulowercase = RussianAlphabetLowercase.Split(' ');
            var ruuppercase = RussianAlphabetUppercase.Split(' ');
            var enlowercase = ExpectedLowercase.Split(' ');
            var enuppercase = ExpectedUppercase.Split(' ');
            for (int i = 0; i < rulowercase.Length; i++)
            {
                _rusEng.Add(rulowercase[i][0], enlowercase[i]);
                _rusEng.Add(ruuppercase[i][0], enuppercase[i]);
            }
        }

        private static string ConvertToTranslit(string rustring)
        {
            if (_rusEng == null)
            {
                InitDictionary();
            }
            string enstring = string.Empty;
            var charstring = rustring.ToCharArray();
            for (int i = 0; i < charstring.Length; i++)
            {
                if (_rusEng != null && _rusEng.ContainsKey(charstring[i]))
                {
                    charstring[i] = _rusEng[charstring[i]][0];
                    enstring = enstring + charstring[i];
                }
                else
                {
                    enstring = enstring + charstring[i];
                }
            }
            return enstring;
        }
        private const string RussianAlphabetLowercase = "а б в г д е ё ж з и й к л м н о п р с т у ф х ц ч ш щ ъ ы ь э ю я";
        private const string RussianAlphabetUppercase = "А Б В Г Д Е Ё Ж З И Й К Л М Н О П Р С Т У Ф Х Ц Ч Ш Щ Ъ Ы Ь Э Ю Я";

        private const string ExpectedLowercase = "a b v g d e yo zh z i y k l m n o p r s t u f kh ts ch sh shch \" y ' e yu ya";
        private const string ExpectedUppercase = "A B V G D E Yo Zh Z I Y K L M N O P R S T U F Kh Ts Ch Sh Shch \" Y ' E Yu Ya";

        public const string DEFAULT_HOSTNAME = "localhost";

        public const int DEFAULT_PORT = 5038;

        public const char END_LINE = '\n';
        public const string LINE_SEPARATOR = "\r\n";
        private const string ASTERISK_MESSAGE_PARAMETER_DELIMETER = ": ";

        public static char[] RESPONSE_KEY_VALUE_SEPARATOR = { ':' };
        public static char[] MINUS_SEPARATOR = { '-' };
        public static char[] VAR_DELIMETER = { '|' };

        public static char INTERNAL_CATION_ID_DELIMETER = '#';

        public static string[] END_MESSAGE = new string[] { "\r\n\r\n" };
        private static string[] ChannelSplit = new string[] { " " };
        public static Dictionary<string, string> vars = new Dictionary<string, string>();
        #endregion

        public static string BuildMessage(AsteriskAction action)
        {
            StringBuilder sb = new StringBuilder();
            //if (string.IsNullOrEmpty(action.ActionID))
            //{
            //    action.ActionID = MachineID;
            //}
            //sb.Append(string.Concat("Action: ", action.Action, LINE_SEPARATOR));
            //if (action.ActionID != null)
            //{
            //    sb.Append(string.Concat("ActionID: ", action.ActionID, LINE_SEPARATOR));
            //}
            sb.Append(action.ToString());
            //sb.Append(LINE_SEPARATOR);
            return sb.ToString();
        }

        public static string GetAsteriskMessageType(string message)
        {
            var firstLine = message.Substring(0, message.IndexOf(LINE_SEPARATOR, StringComparison.Ordinal));
            if (firstLine.Contains("Asterisk Call Manager"))
            {
                return "Response";
            }
            var index = message.IndexOf(':');
            return index < 0 ? "" : message.Substring(0, index);
        }
        /// <summary>
        /// Поиск параметра в ответе из запроса.(ИСПРАВЛЕНО)
        /// </summary>
        /// <param name="msg">Информация в строковой переменной</param>
        /// <param name="parameter">Указатель на искомые данные</param>
        public static string GetValue(string msg, string parameter)
        {
            if (!parameter.Contains(ASTERISK_MESSAGE_PARAMETER_DELIMETER))
            {
                parameter += ASTERISK_MESSAGE_PARAMETER_DELIMETER;
            }
            var msgLower = msg.ToLower();
            if (!msg.Contains(parameter) && !msgLower.Contains(parameter.ToLower()))
            {
                return string.Empty;
            }

            var index = msgLower.IndexOf(parameter.ToLower(), StringComparison.Ordinal);
            var message = msg.Substring(index);
            message += LINE_SEPARATOR;

            int startPos = parameter.Length;
            int length = message.IndexOf(LINE_SEPARATOR, StringComparison.Ordinal) - startPos;
            message = message.Substring(startPos, length);

            return !string.IsNullOrEmpty(message) ? message : string.Empty;
        }

        /// <summary>
        /// Получает номер приписанный к сип каналу
        /// </summary>
        /// <param name="channelId">ID канала</param>
        /// <returns></returns>
        public static string GetNumberFromChannel(string channelId)
        {
            if (!channelId.ToLower().Contains("sip")) return string.Empty;

            channelId = channelId.Replace("SIP/", "");
            int length = channelId.IndexOf("-", StringComparison.Ordinal);
            channelId = channelId.Substring(0, length);
            return channelId;
        }

        #region ToString()
        /// <summary>
        ///     Convert object with all properties to string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static string ToString(object obj)
        {
            object value;
            var sb = new StringBuilder();
            //sb.Append(" {");
            //obj.GetType().Name, 1024
            string strValue;
            IDictionary getters = GetGetters(obj.GetType());
            bool notFirst = false;
            var arrays = new List<MethodInfo>();
            // First step - all values properties (not a list)
            foreach (string name in getters.Keys)
            {
                var getter = (MethodInfo)getters[name];
                Type propType = getter.ReturnType;
                if (propType == typeof(object))
                    continue;
                if (
                    !(propType == typeof(string) || propType == typeof(bool) || propType == typeof(double) ||
                      propType == typeof(DateTime) || propType == typeof(int) || propType == typeof(long)))
                {
                    string propTypeName = propType.Name;
                    if (propTypeName.StartsWith("Dictionary") || propTypeName.StartsWith("List"))
                    {
                        arrays.Add(getter);
                    }
                    continue;
                }

                try
                {
                    value = getter.Invoke(obj, new object[] { });
                }
                catch
                {
                    continue;
                }

                if (value == null)
                    continue;
                if (value is string)
                {
                    strValue = (string)value;
                    if (strValue.Length == 0)
                        continue;
                }
                else if (value is bool)
                {
                    strValue = ((bool)value ? "true" : "false");
                }
                else if (value is double)
                {
                    var d = (double)value;
                    if (d == 0.0)
                        continue;
                    strValue = d.ToString();
                }
                else if (value is DateTime)
                {
                    var dt = (DateTime)value;
                    if (dt == DateTime.MinValue)
                        continue;
                    strValue = dt.ToLongTimeString();
                }
                else if (value is int)
                {
                    var i = (int)value;
                    if (i == 0)
                        continue;
                    strValue = i.ToString();
                }
                else if (value is long)
                {
                    var l = (long)value;
                    if (l == 0)
                        continue;
                    strValue = l.ToString();
                }
                else
                    strValue = value.ToString();

                //if (notFirst)
                //    sb.Append("; ");
                //notFirst = true;
                sb.Append(string.Concat(getter.Name.Substring(4), ": ", strValue, LINE_SEPARATOR));
            }

            // Second step - all lists
            foreach (var getter in arrays)
            {
                value = null;
                try
                {
                    value = getter.Invoke(obj, new object[] { });
                }
                catch
                {
                    continue;
                }
                if (value == null)
                    continue;

                #region List 

                IList list;
                if (value is IList && (list = (IList)value).Count > 0)
                {
                    if (notFirst)
                        sb.Append("; ");
                    notFirst = true;
                    sb.Append(getter.Name.Substring(4));
                    sb.Append(":[");
                    bool notFirst2 = false;
                    foreach (var o in list)
                    {
                        if (notFirst2)
                            sb.Append("; ");
                        notFirst2 = true;
                        sb.Append(o);
                    }
                    sb.Append("]");
                }
                #endregion

                #region IDictionary 
                if (value is IDictionary && ((IDictionary)value).Count > 0)
                {
                    if (notFirst)
                        sb.Append("; ");
                    notFirst = true;
                    sb.Append(getter.Name.Substring(4));
                    sb.Append(":[");
                    bool notFirst2 = false;
                    foreach (var key in ((IDictionary)value).Keys)
                    {
                        object o = ((IDictionary)value)[key];
                        if (notFirst2)
                            sb.Append("; ");
                        notFirst2 = true;
                        sb.Append(string.Concat(key, ":", o));
                    }
                    sb.Append("]");
                }

                #endregion
            }

            sb.Append(LINE_SEPARATOR);
            return sb.ToString();
        }

        /// <summary>
        ///     Returns a Map of getter methods of the given class.<br />
        ///     The key of the map contains the name of the attribute that can be accessed by the getter, the
        ///     value the getter itself . A method is considered a getter if its name starts with "get",
        ///     it is declared internal and takes no arguments.
        /// </summary>
        /// <param name="clazz">the class to return the getters for</param>
        /// <returns> a Map of attributes and their accessor methods (getters)</returns>
        internal static Dictionary<string, MethodInfo> GetGetters(Type clazz)
        {
            string name;
            string methodName;
            MethodInfo method;

            var accessors = new Dictionary<string, MethodInfo>();
            MethodInfo[] methods = clazz.GetMethods();

            for (int i = 0; i < methods.Length; i++)
            {
                method = methods[i];
                methodName = method.Name;

                // skip not "get..." methods and  skip methods with != 0 parameters
                if (!methodName.StartsWith("get_") || method.GetParameters().Length != 0)
                    continue;

                name = methodName.Substring(4);
                if (name.Length == 0)
                    continue;
                accessors[name] = method;
            }
            return accessors;
        }

        #endregion
    }
}

