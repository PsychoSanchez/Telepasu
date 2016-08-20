using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Re_translator
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
                    if (String.Compare(ip[0], compIP) == 0)
                    {
                        return true;
                    }
                }
                else if (ip.Length == 2)
                {
                    if (String.Compare(ip[0], compIP) == -1 & String.Compare(compIP, ip[1]) == -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
