using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
    [CommandProcessorInfo(CommandType = CommandType.GetHistory)]
    public class GetHistoryCommandProcessor : BaseCommandProcessor
    {
		public GetHistoryCommandProcessor(ClientCommand command)
            : base(command)
        {
        }

        protected override bool Process(GameServer server)
        {
            var success = true;
			GameMove[] moveArray = null;
			var command = Command as GetHistoryCommand;
            try
            {
				var player = server.GetPlayer(command.AuthData);
				var game = server.GetGameByPlayer(player);
				moveArray = game.GetHistory(player, command.FromIndex);
            }
            catch (GameServerException)
            {
                success = false;
            }

            Answer = new GetHistoryAnswer(success, moveArray);
            return success;
        }
    }
}
