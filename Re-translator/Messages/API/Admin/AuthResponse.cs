using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Messages.API.Admin
{
    public class AuthResponse
    {
        private const string WELCOME_MESSAGE = "Telepasu Proxy 2.0. Welcome and Have a nice day";
        public string Action = "Login";
        /// <summary>
        /// 200 - Ok
        /// 404 - User Not Found
        /// 401 - Login or Password incorrect
        /// </summary>
        public int Status = 200;
        public string Message = "Telepasu Proxy 2.0. Welcome and Have a nice day";

        private void UpdateStatus()
        {
            switch (Status)
            {
                case 200:
                    Message = WELCOME_MESSAGE;
                    break;
                case 404:
                    Message = WELCOME_MESSAGE;
                    break;
                default:
                    Message = WELCOME_MESSAGE;
                    break;
            }
        }
    }
}
