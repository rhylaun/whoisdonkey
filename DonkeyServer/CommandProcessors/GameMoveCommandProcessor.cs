using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.GameMove)]
	public class GameMoveCommandProcessor : BaseCommandProcessor
	{
		public GameMoveCommandProcessor(ClientCommand command)
			: base(command)
		{
		}

		protected override bool Process(GameServer server)
		{
			var result = false;
			var command = (GameMoveCommand)Command;
			try
			{
				var player = server.GetPlayer(Command.AuthData);
				var game = server.GetGameByPlayer(player);
				if (game.Ended)
				{
					Answer = new GameMoveAnswer(false);
					return false;					
				}

				var move = game.CreateMove(player, command.MoveType, command.Cards);
				result = game.MakeMove(move);
			}
			catch (GameServerException)
			{
				result = false;
			}

			Answer = new GameMoveAnswer(result);
			return result;
		}
	}
}
