using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Commands
{
    [Serializable]
    [RegisterClientCommand(CommandType = CommandType.GetCardSet, NeedAuth = true)]
    public class GetCardSetCommand : ClientCommand
    {
        public GetCardSetCommand(AuthData authdata)
            : base(authdata, Commands.CommandType.GetCardSet)
        {
        }
    }
}
