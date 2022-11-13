using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Artbuk.Tests
{
    internal class MyIdentity : IIdentity
    {
        public string? AuthenticationType
        {
            get
            {
                return "something";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string? Name
        {
            get
            {
                return name;
            }
        }
        private string name;

        public MyIdentity(string name)
        {
            this.name = name;
        }
    }
}
