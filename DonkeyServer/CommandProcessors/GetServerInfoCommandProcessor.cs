using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;
using System.Collections.Generic;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.GetServerInfo)]
	public class GetServerInfoCommandProcessor : BaseCommandProcessor
	{
		public GetServerInfoCommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool Process(GameServer server)
		{
			var success = true;
			var botNames = new List<string>();
			try
			{
				botNames = server.GetBotNames();
			}
			catch (GameServerException)
			{
				success = false;
			}

			Answer = new GetServerInfoAnswer(success, botNames);
			return success;
		}
	}
}
