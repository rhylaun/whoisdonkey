using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.RemoveAI)]
	public class RemoveAICommandProcessor : BaseCommandProcessor
	{
		public RemoveAICommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool Process(GameServer server)
		{
			var result = true;
			var command = (RemoveAICommand)Command;
			try
			{
				var player = server.GetPlayer(command.AuthData);
				var lobby = server.GetLobby(command.LobbyName);

				lobby.RemoveAI(command.BotName);
			}
			catch (GameServerException)
			{
				result = false;
			}
			Answer = new RemoveAIAnswer(result);

			return result;
		}
	}
}
