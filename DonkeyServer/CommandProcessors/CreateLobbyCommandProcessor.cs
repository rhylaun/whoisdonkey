using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.CreateLobby)]
	public class CreateLobbyCommandProcessor : BaseCommandProcessor
	{
		public CreateLobbyCommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool Process(GameServer server)
		{
			var result = true;
			var command = (CreateLobbyCommand)Command;
			try
			{
				var lobbies = server.GetLobbies();
				var alreadyExist = lobbies.Contains(command.LobbyName);
				Lobby lobby;
				if (!alreadyExist)
				{
					lobby = server.CreateLobby(command.LobbyName);
				}
				else
				{
					lobby = server.GetLobby(command.LobbyName);
				}

				var player = server.GetPlayer(command.AuthData);
				lobby.AddPlayer(player);
			}
			catch (GameServerException)
			{
				result = false;
			}
			Answer = new CreateLobbyAnswer(result);

			return result;
		}
	}
}
