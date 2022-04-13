using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SutekiTmp.Domain.Common.Authentication.Session
{
    public class SessionAuthenticationOptions
    {
        public string LoginPath { get; set; } = "/Home/Login";
        public string SessionKeyName { get; set; } = "uid";
        public string ReturnUrlKey { set; get; } = "return";
    }
}
