using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.AddAI)]
	public class AddAICommandProcessor : BaseCommandProcessor
	{
		public AddAICommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool Process(GameServer server)
		{
			var result = true;
			var command = (AddAICommand)Command;
			try
			{
				var player = server.GetPlayer(command.AuthData);
				var lobby = server.GetLobby(command.LobbyName);

				lobby.AddAI(command.BotName);
			}
			catch (GameServerException)
			{
				result = false;
			}
			Answer = new AddAIAnswer(result);

			return result;
		}
	}
}
