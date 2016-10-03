using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.GetPlayers, NeedAuth = true)]
    [ConsoleCommandInfo(ConsoleLine = "getplayers")]
    public class GetPlayersCommand : ClientCommand
    {
        public GetPlayersCommand(AuthData authData)
            : base(authData, CommandType.GetPlayers)
        {
        }
    }
}
