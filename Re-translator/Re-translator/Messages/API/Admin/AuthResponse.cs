using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Messages.API.Admin
{
    public class AuthResponse
    {
        public bool AuthStatus = false;
        public int Status = -1;
        public string Message = "Have a nice day";
        public AuthResponse(bool authStatus)
        {
            AuthStatus = authStatus;
        }
    }
}
