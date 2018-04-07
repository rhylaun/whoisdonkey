using System;

namespace Donkey.Common.Commands
{
	[Serializable]
    [RegisterClientCommand(CommandType = CommandType.Register, NeedAuth = false)]
    public class RegisterCommand : ClientCommand
    {
        public readonly bool Ready;

        public RegisterCommand(AuthData authData)
            : base(authData, CommandType.Register)
        {
        }
    }
}
