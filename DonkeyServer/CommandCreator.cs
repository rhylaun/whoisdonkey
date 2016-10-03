using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;

namespace Donkey.Server
{
    public static class CommandCreator
    {
        public static ClientCommand Create(CommandType type, string[] args)
        {
            ClientCommand result = null;
            switch (type)
            {
                case CommandType.CreateLobby:
                    result = new CreateLobbyCommand(ServerDefaults.Root, args[0]);
                    break;
                case CommandType.GetLobbies:
                    result = new GetLobbiesCommand(ServerDefaults.Root);
                    break;
                case CommandType.GetPlayers:
                    result = new GetPlayersCommand(ServerDefaults.Root);
                    break;
                case CommandType.Leave:
                    result = new LeaveCommand(ServerDefaults.Root);
                    break;
            }

            return result;
        }
    }
}
