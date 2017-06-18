using System;

namespace Donkey.Common.Commands
{
	[Serializable]
	[RegisterClientCommand(CommandType = CommandType.GetCurrentGameState, NeedAuth = true)]
	public class GetCurrentGameStateCommand : ClientCommand
	{
		public GetCurrentGameStateCommand(AuthData authdata)
			: base(authdata, CommandType.GetCurrentGameState)
		{
		}
	}
}
