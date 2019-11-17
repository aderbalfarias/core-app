using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Api.Options.Authorization
{
    public class AuthenticationOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }
        
        public string TokenEndpoint { get; set; }
    }
}
