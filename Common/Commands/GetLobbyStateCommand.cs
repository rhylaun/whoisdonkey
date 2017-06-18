using System;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.GetLobbyState, NeedAuth = true)]
    public class GetLobbyStateCommand : ClientCommand
    {
		public readonly string LobbyName;

        public GetLobbyStateCommand(AuthData authData, string lobbyName)
            : base(authData, CommandType.GetLobbyState)
        {
			LobbyName = lobbyName;
        }
    }
}
