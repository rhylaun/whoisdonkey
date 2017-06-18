using System;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.GetPlayerList, NeedAuth = true)]
    [ConsoleCommandInfo(ConsoleLine = "getplayers")]
    public class GetPlayersCommand : ClientCommand
    {
        public GetPlayersCommand(AuthData authData)
            : base(authData, CommandType.GetPlayerList)
        {
        }
    }
}
