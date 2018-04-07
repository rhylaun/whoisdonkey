using Donkey.Common.ClientServer;
using Donkey.Common.Commands;
using Donkey.Server.CommandProcessors;

namespace Donkey.Server
{
	public class NetworkServer<TReqServer> where TReqServer : IRequestServer, new()
    {
        private IRequestServer _requestServer;
        private readonly GameServer _gameServer;
        private readonly int _port;

        public NetworkServer(GameServer server, int port)
        {
            _gameServer = server;
            _port = port;
            _requestServer = new TReqServer();
        }

        public void Start()
        {
            _requestServer.Bind(_port);
            _requestServer.CommandReceived += CommandReceived;
        }

        private void CommandReceived(ClientCommand command, INetworkClient client)
        {
            var processor = CommandProcessorFactory.GetProcessor(command);
            processor.ExecuteOn(_gameServer);
            client.Send(processor.Answer);
            client.Close();
        }
    }
}
