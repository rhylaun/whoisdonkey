using System;

namespace Donkey.Common.Commands
{
	[Serializable]
    [RegisterClientCommand(CommandType = CommandType.CreateLobby, NeedAuth = true)]
    [ConsoleCommandInfo(ConsoleLine = "createlobby")]
    public class CreateLobbyCommand : ClientCommand
    {
        public readonly string LobbyName;

        public CreateLobbyCommand(AuthData authdata, string lobbyName)
            : base(authdata, Commands.CommandType.CreateLobby)
        {
            LobbyName = lobbyName;
        }
    }
}
