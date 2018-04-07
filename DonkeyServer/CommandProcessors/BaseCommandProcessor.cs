using Donkey.Common.Commands;
using Donkey.Common.Answers;
using Donkey.Common;

namespace Donkey.Server.CommandProcessors
{
	public abstract class BaseCommandProcessor
    {
        protected readonly ClientCommand Command;

        public ServerAnswer Answer { get; protected set; }

        public BaseCommandProcessor(ClientCommand command)
        {
            Command = command;
        }

        public bool ExecuteOn(GameServer server)
        {
            Answer = new ServerAnswer(false);

            Player player = null;
            var canExecute = true;

            var needAuth = ((RegisterClientCommandAttribute)Command.GetType().GetCustomAttributes(typeof(RegisterClientCommandAttribute), true)[0]).NeedAuth;
            if (needAuth)
            {
                var authResult = !Command.AuthData.Equals(ServerDefaults.Root) && server.HasPlayer(Command.AuthData);
                if (authResult)
                {
                    player = server.GetPlayer(Command.AuthData);
                    canExecute = player.CanApplyCommand(Command);
                }
            }

            if (canExecute)
                canExecute = Process(server);

            var needApply = needAuth && canExecute && player != null;
            if (needApply)
            {
                player.ApplyCommand(Command);
                canExecute &= ProcessAfter(server);
            }

            return canExecute;
        }

        protected virtual bool Process(GameServer server)
        {
            return true;
        }

        protected virtual bool ProcessAfter(GameServer server)
        {
            return true;
        }

    }
}
