using System;

namespace Donkey.Common.Commands
{
	[Serializable]
    [RegisterClientCommand(CommandType = CommandType.Leave, NeedAuth = true)]
    public class LeaveCommand : ClientCommand
    {
        public LeaveCommand(AuthData authData)
            : base(authData, CommandType.Leave)
        {
        }
    }
}
