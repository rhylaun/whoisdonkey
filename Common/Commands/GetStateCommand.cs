using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.GetState, NeedAuth = true)]
    public class GetStateCommand : ClientCommand
    {
        public GetStateCommand(AuthData authData)
            : base(authData, CommandType.GetState)
        {
        }
    }
}
