using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server.CommandProcessors
{
	[CommandProcessorInfo(CommandType = CommandType.Register)]
    public class RegisterCommandProcessor : BaseCommandProcessor
    {
        public RegisterCommandProcessor(ClientCommand command)
            : base(command)
        {
        }

        protected override bool Process(GameServer server)
        {
            var result = true;
            try
            {
                var playerExist = server.HasPlayer(Command.AuthData);
                if (!playerExist)
                    server.AddPlayer(Command.AuthData);
				result = !playerExist;
            }
            catch (GameServerException)
            {
                result = false;
            }

            Answer = new RegisterAnswer(result);
            return result;
        }
    }
}
