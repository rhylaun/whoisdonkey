using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.JoinLobby)]
	public class JoinLobbyCommandProcessor : BaseCommandProcessor
	{
		public JoinLobbyCommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool Process(GameServer server)
		{
			var result = true;
			var command = (JoinLobbyCommand)Command;
			try
			{
				var player = server.GetPlayer(command.AuthData);
				var lobby = server.GetLobby(command.LobbyName);

				lobby.AddPlayer(player);
			}
			catch (GameServerException)
			{
				result = false;
			}
			Answer = new JoinLobbyAnswer(result);

			return result;
		}
	}
}
