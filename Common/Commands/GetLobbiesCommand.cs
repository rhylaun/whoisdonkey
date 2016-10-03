using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.GetLobbies, NeedAuth = true)]
    [ConsoleCommandInfo(ConsoleLine = "getlobbies")]
    public class GetLobbiesCommand : ClientCommand
    {
        public GetLobbiesCommand(AuthData authData)
            : base(authData, CommandType.GetLobbies)
        {
        }
    }
}
