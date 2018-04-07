using System;

namespace Donkey.Common.Commands
{
	[Serializable]
    [RegisterClientCommand(CommandType = CommandType.Start, NeedAuth = true)]
    public class StartGameCommand : ClientCommand
    {
        public StartGameCommand(AuthData authData)
            : base(authData, CommandType.Start)
        {
        }
    }
}
