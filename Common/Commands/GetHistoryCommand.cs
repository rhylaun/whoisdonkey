using System;

namespace Donkey.Common.Commands
{
	[Serializable]
    [RegisterClientCommand(CommandType = CommandType.GetHistory, NeedAuth = true)]
    public class GetHistoryCommand : ClientCommand
    {
        public readonly int FromIndex;

        public GetHistoryCommand(AuthData authData, int fromIndex)
            : base(authData, CommandType.GetHistory)
        {
            FromIndex = fromIndex;
        }
    }
}
