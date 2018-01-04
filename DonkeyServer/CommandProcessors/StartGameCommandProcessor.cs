using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.Start)]
	public class StartGameCommandProcessor : BaseCommandProcessor
	{
		public StartGameCommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool ProcessAfter(GameServer server)
		{
			var result = true;
			try
			{
				var player = server.GetPlayer(Command.AuthData);
				var lobby = server.GetLobbyByPlayer(player);
				if (lobby.Creator != player)
				{
					result = false;
					Answer = new StartGameAnswer(result);
					return result;
				}

				server.RemoveLobby(lobby);
				var game = server.CreateGame(lobby);
				game.Start();
			}
			catch (GameServerException)
			{
				result = false;
			}

			Answer = new StartGameAnswer(result);
			return result;
		}
	}
}
