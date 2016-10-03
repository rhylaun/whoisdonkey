using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.Auth, NeedAuth = true)]
    public class AuthCommand : ClientCommand
    {
        public AuthCommand(AuthData authdata)
            : base(authdata, Commands.CommandType.Auth)
        {
        }
    }
}
