using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

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
			PlayerInLobbyDescription[] players = null;
            try
            {
				var lobby = server.GetLobby(command.LobbyName);
				players = lobby.GetState();
            }
            catch (GameServerException)
            {
                result = true;
            }

			Answer = new GetLobbyStateAnswer(result, players);
            return result;
        }
    }
}
