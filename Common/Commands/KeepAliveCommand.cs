using System;

namespace Donkey.Common.Commands
{
	[Serializable]
    [RegisterClientCommand(CommandType = CommandType.KeepAlive, NeedAuth = true)]
    public class KeepAliveCommand : ClientCommand
    {
        public KeepAliveCommand(AuthData authData)
            : base(authData, CommandType.KeepAlive)
        {
        }
    }
}
