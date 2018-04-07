using System;

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
