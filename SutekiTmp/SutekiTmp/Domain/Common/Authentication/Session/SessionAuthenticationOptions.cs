using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SutekiTmp.Domain.Common.Authentication.Session
{
    public class SessionAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string Scheme = "SessionAuth";
        public PathString LoginPath { get; set; } = "/Home/Login";
        public string SessionKeyName { get; set; } = "Uid";
    }
}
