using Donkey.Common;
using Donkey.Common.Answers;
using Donkey.Common.Commands;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.GetCurrentPlayer)]
	public class CurrentPlayerCommandProcessor : BaseCommandProcessor
	{
		public CurrentPlayerCommandProcessor(ClientCommand command)
            : base(command)
        {
		}

		protected override bool Process(GameServer server)
		{
			var result = true;
			var name = string.Empty;
			try
			{
				var player = server.GetPlayer(Command.AuthData);
				var game = server.GetGameByPlayer(player);
				name = game.CurrentTurnPlayer.AuthData.Login;
			}
			catch (GameServerException)
			{
				result = false;
			}

			Answer = new CurrentPlayerAnswer(result, name);
			return result;
		}
	}
}
