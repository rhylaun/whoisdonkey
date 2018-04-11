using Donkey.Common;
using Donkey.Common.Answers;
using Donkey.Common.Commands;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.GetCurrentGameState)]
	public class CurrentGameStateCommandProcessor : BaseCommandProcessor
	{
		public CurrentGameStateCommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool Process(GameServer server)
		{
			var result = true;
			var name = string.Empty;
			var gameEnded = false;
			try
			{
				var player = server.GetPlayer(Command.AuthData);
				var game = server.GetGameByPlayer(player);
				name = game.CurrentTurnPlayer.Name;
				gameEnded = game.Ended;
			}
			catch (GameServerException)
			{
				result = false;
			}

			Answer = new CurrentGameStateAnswer(result, name, gameEnded);
			return result;
		}
	}
}
