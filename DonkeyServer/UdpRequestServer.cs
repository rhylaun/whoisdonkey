using System.Net;
using System.Net.Sockets;
using System.Threading;
using Donkey.Common.ClientServer;
using Donkey.Common.Commands;
using Donkey.Common.Answers;

namespace Donkey.Server
{
    public class UdpRequestServer : IRequestServer
    {
        private UdpClient _client;
        private Thread _receiveThread;
        private GameSerializer _serializer = new GameSerializer();

        public void Bind(int port)
        {
            _client = new UdpClient(port);
            _receiveThread = new Thread(ReceiveProcess);
            _receiveThread.IsBackground = true;
            _receiveThread.Start();
        }

        public event CommandReceivedEventHandler CommandReceived;

        private void ReceiveProcess()
        {
            var endPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                var buffer = _client.Receive(ref endPoint);
                var obj = _serializer.ToObject(buffer);
                if (!(obj is ClientCommand))
                    continue;

				var client = new UdpNetworkClient();
				client.Connect(endPoint);
                var handler = CommandReceived;
                if (handler != null)
                    handler((ClientCommand)obj, client);
            }
        }
    }
}
