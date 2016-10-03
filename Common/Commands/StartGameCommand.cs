using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.Start, NeedAuth = true)]
    public class StartGameCommand : ClientCommand
    {
        public StartGameCommand(AuthData authData)
            : base(authData, CommandType.Start)
        {
        }
    }
}
