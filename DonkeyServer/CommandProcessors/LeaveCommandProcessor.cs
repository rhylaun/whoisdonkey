using System.Linq;
using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.Leave)]
	public class LeaveCommandProcessor : BaseCommandProcessor
	{
		public LeaveCommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool Process(GameServer server)
		{
			var result = true;
			try
			{
				var player = server.GetPlayer(Command.AuthData);
				switch (player.State)
				{
					case PlayerState.Lobby:
					case PlayerState.Ready:
						var lobby = server.GetLobbyByPlayer(player);
						lobby.RemovePlayer(player);
						if (lobby.GetPlayers().Count() == 0)
							server.RemoveLobby(lobby);
						break;
					case PlayerState.Game:
						var game = server.GetGameByPlayer(player);
						game.RemovePlayer(player);
						var humanCount = game.GetHumanCount();
						if (humanCount == 0)
							server.RemoveGame(game);
						break;
				}
			}
			catch (GameServerException)
			{
				result = false;
			}

			Answer = new LeaveAnswer(result);
			return result;
		}
	}
}
