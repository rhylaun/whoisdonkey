using System;

namespace Donkey.Common.Commands
{
	[Serializable]
    [RegisterClientCommand(CommandType = CommandType.GetStatistic, NeedAuth = true)]
    public class GetStatisticCommand : ClientCommand
    {
        public GetStatisticCommand(AuthData authdata)
            : base(authdata, CommandType.GetStatistic)
        {
        }
    }
}
