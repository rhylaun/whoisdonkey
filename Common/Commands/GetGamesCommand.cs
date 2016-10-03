using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.GetGames, NeedAuth = true)]
    [ConsoleCommandInfo(ConsoleLine = "getgames")]
    public class GetGamesCommand : ClientCommand
    {
        public GetGamesCommand(AuthData authData)
            : base(authData, CommandType.GetGames)
        {
        }
    }
}
