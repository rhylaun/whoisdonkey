using System;

namespace Donkey.Common.Commands
{
	[Serializable]
    [RegisterClientCommand(CommandType = CommandType.JoinLobby, NeedAuth = true)]
    public class JoinLobbyCommand : ClientCommand
    {
        public readonly string LobbyName;

        public JoinLobbyCommand(AuthData authData, string lobbyName)
            : base(authData, CommandType.JoinLobby)
        {
            LobbyName = lobbyName;
        }
    }
}
