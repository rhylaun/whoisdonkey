using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common;

namespace Donkey.Server
{
    public static class ServerDefaults
    {
        public static AuthData Root
        {
            get { return new AuthData("root", "toor"); }
        }
    }
}
