using System;

namespace Donkey.Common.Commands
{
	[Serializable]
    [RegisterClientCommand(CommandType = CommandType.Auth, NeedAuth = true)]
    public class AuthCommand : ClientCommand
    {
        public AuthCommand(AuthData authdata)
            : base(authdata, CommandType.Auth)
        {
        }
    }
}
