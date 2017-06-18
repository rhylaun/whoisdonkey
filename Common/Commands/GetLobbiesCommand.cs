using System;

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
