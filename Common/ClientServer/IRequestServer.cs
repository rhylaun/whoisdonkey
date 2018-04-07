using Donkey.Common.Commands;

namespace Donkey.Common.ClientServer
{
	public interface IRequestServer
    {
        void Bind(int port);
        event CommandReceivedEventHandler CommandReceived;
    }

    public delegate void CommandReceivedEventHandler(ClientCommand command, INetworkClient client);
}
