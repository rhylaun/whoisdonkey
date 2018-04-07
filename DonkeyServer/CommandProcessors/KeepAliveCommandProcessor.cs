using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.KeepAlive)]
    public class KeepAliveCommandProcessor : BaseCommandProcessor
    {
        public KeepAliveCommandProcessor(ClientCommand command)
            : base(command)
        {
        }

        protected override bool Process(GameServer server)
        {
            var result = true;
            try
            {
                server.GetPlayer(Command.AuthData);
            }
            catch (GameServerException)
            {
                result = false;
            }

            Answer = new AuthAnswer(result);
            return result;
        }
    }
}
