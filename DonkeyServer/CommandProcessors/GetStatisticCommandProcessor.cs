using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
    [CommandProcessorInfo(CommandType = CommandType.GetStatistic)]
    public class GetStatisticCommandProcessor : BaseCommandProcessor
    {
        public GetStatisticCommandProcessor(ClientCommand command)
            : base(command)
        {
        }

        protected override bool Process(GameServer server)
        {
            var result = true;
			var statistic = new StatisticRecord[0];
            try
            {
				statistic = server.Statistic.Records.ToArray();
            }
            catch (GameServerException)
            {
                result = false;
            }

            Answer = new GetStatisticAnswer(result, statistic);
            return result;
        }
    }
}
