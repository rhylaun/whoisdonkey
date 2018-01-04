using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;
using System.Linq;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.GetLobbyState)]
    public class GetLobbyStateCommandProcessor : BaseCommandProcessor
    {
        public GetLobbyStateCommandProcessor(ClientCommand command)
            : base(command)
        {
        }

        protected override bool Process(GameServer server)
        {
			var command = (GetLobbyStateCommand)Command;
            var result = true;
			LobbyState lobbyState = null;
            try
            {
				var lobby = server.GetLobby(command.LobbyName);
				var players = lobby.GetState().ToList();
				var creator = lobby.Creator.AuthData.Login;
				lobbyState = new LobbyState(creator, players);
            }
            catch (GameServerException)
            {
                result = true;
            }

			Answer = new GetLobbyStateAnswer(result, lobbyState);
            return result;
        }
    }
}
