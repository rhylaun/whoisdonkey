using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
    [CommandProcessorInfo(CommandType = CommandType.GetState)]
    public class GetStateCommandProcessor : BaseCommandProcessor
    {
        public GetStateCommandProcessor(ClientCommand command)
            : base(command)
        {
        }

        protected override bool Process(GameServer server)
        {
            var success = true;
            var state = PlayerState.Error;
            try
            {
                var player = server.GetPlayer(Command.AuthData);
                state = player.State;
            }
            catch (GameServerException)
            {
                success = false;
            }

            Answer = new GetStateAnswer(success, state);
            return success;
        }
    }
}
