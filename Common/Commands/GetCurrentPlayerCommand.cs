using System;

namespace Donkey.Common.Commands
{
	[Serializable]
	[RegisterClientCommand(CommandType = CommandType.Auth, NeedAuth = true)]
	public class GetCurrentPlayerCommand : ClientCommand
	{
		public GetCurrentPlayerCommand(AuthData authdata)
            : base(authdata, CommandType.GetCurrentPlayer)
        {
		}
	}
}
