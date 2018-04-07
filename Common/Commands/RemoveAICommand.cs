using System;

namespace Donkey.Common.Commands
{
	[Serializable]
	[RegisterClientCommand(CommandType = CommandType.RemoveAI, NeedAuth = true)]
	public class RemoveAICommand : ClientCommand
	{
		public readonly string LobbyName;
		public readonly string BotName;

		public RemoveAICommand(AuthData authData, string lobbyName, string botName)
			: base(authData, CommandType.RemoveAI)
		{
			LobbyName = lobbyName;
			BotName = botName;
		}
	}
}
