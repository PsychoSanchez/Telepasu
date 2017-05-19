using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.LocalDB.SubsTable
{
    public class Subscribtions
    {
        public virtual int ID { get; set; }
        public virtual int APP_ID { get; set; }
        public virtual string MESSAGE_TAG { get; set; }
        public virtual int USER_ID { get; set; }
    }
}
