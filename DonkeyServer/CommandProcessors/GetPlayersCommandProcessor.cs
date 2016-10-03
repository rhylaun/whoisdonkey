using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
    [CommandProcessorInfo(CommandType = CommandType.GetPlayers)]
    public class GetPlayersCommandProcessor : BaseCommandProcessor
    {
        public GetPlayersCommandProcessor(ClientCommand command)
            : base(command)
        {
        }

        protected override bool Process(GameServer server)
        {
            var result = true;
            string[] players = null;
            try
            {
                players = server.GetPlayers().ToArray();
            }
            catch (GameServerException ex)
            {
                result = true;
            }

            Answer = new GetPlayersAnswer(result)
            {
                Players = players
            };
            return result;
        }
    }
}
