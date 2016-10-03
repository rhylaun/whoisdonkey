using System.Net;
using System.Net.Sockets;
using System.Threading;
using Donkey.Common.ClientServer;
using Donkey.Common.Commands;
using Donkey.Common.Answers;

namespace Donkey.Server
{
    public class TcpRequestServer : IRequestServer
    {
        private GameSerializer _serializer = new GameSerializer();
		private Thread _receiveThread;
		private TcpListener _listner;

        public void Bind(int port)
        {
			_listner = new TcpListener(IPAddress.Any, port);
			_listner.Start();
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
				var client = _listner.AcceptTcpClient();
				var buffer = new byte[1500];
				var readed = client.Client.Receive(buffer);
                var obj = _serializer.ToObject(buffer);
                if (!(obj is ClientCommand))
                    continue;

				var tcpClient = new TcpNetworkClient(client);
                var handler = CommandReceived;
                if (handler != null)
                    handler((ClientCommand)obj, tcpClient);
            }
        }
    }
}
