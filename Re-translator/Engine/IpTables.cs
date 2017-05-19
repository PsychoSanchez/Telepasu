using System.Collections.Generic;

namespace Proxy
{
    public class IpTables
    {
        List<string> AllowedIP = new List<string>();
        public IpTables(string filePath)
        {

        }
        public bool Compare(string compIP)
        {
            foreach (var temp in AllowedIP)
            {
                string[] ip = temp.Split('-');
                if (ip.Length == 1)
                {
                    if (string.CompareOrdinal(ip[0], compIP) == 0)
                    {
                        return true;
                    }
                }
                else if (ip.Length == 2)
                {
                    if (string.CompareOrdinal(ip[0], compIP) == -1 & string.CompareOrdinal(compIP, ip[1]) == -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
