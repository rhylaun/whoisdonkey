using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
    [CommandProcessorInfo(CommandType = CommandType.GetLobbies)]
    public class GetLobbiesCommandProcessor : BaseCommandProcessor
    {
        public GetLobbiesCommandProcessor(ClientCommand command)
            : base(command)
        {
        }

        protected override bool Process(GameServer server)
        {
            var result = true;
            string[] lobbies = null;
            try
            {
                lobbies = server.GetLobbies().ToArray();
            }
            catch (GameServerException)
            {
                result = true;
            }

            Answer = new GetLobbiesAnswer(result)
            {
                Lobbies = lobbies
            };
            return result;
        }
    }
}
