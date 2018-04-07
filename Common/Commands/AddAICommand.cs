using System;

namespace Donkey.Common.Commands
{
	[Serializable]
	[RegisterClientCommand(CommandType = CommandType.AddAI, NeedAuth = true)]
	public class AddAICommand : ClientCommand
	{
		public readonly string LobbyName;
		public readonly string BotName;

		public AddAICommand(AuthData authData, string lobbyName, string botName)
			: base(authData, CommandType.AddAI)
		{
			LobbyName = lobbyName;
			BotName = botName;
		}
	}
}
