using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.GetCardSet)]
	public class GetCardSetCommandProcessor : BaseCommandProcessor
	{
		public GetCardSetCommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool Process(GameServer server)
		{
			var result = true;
			PlayerCardSet cardSet = null;
			try
			{
				var player = server.GetPlayer(Command.AuthData);
				var game = server.GetGameByPlayer(player);
				cardSet = game.GetPlayerCardSet(player.AuthData.Login);
			}
			catch (GameServerException)
			{
				result = false;
			}

			Answer = new GetCardSetAnswer(result, cardSet);
			return result;
		}
	}
}
