using System;
using System.Net.Sockets;
using System.Net;

namespace Donkey.Common.ClientServer
{
	public class TcpNetworkClient : INetworkClient
	{
		private const int NetworkTimeout = 5000;

		private readonly TcpClient _client;
		private readonly GameSerializer _serializer = new GameSerializer();
		private IPEndPoint _endPoint;

		public TcpNetworkClient()
		{
			_client = new TcpClient();
		}

		public TcpNetworkClient(IPEndPoint localEndPoint)
		{
			_client = new TcpClient(localEndPoint);
		}

		public TcpNetworkClient(TcpClient client)
		{
			_client = client;
		}

		public bool Connect(IPEndPoint endPoint)
		{
			_client.ReceiveTimeout = NetworkTimeout;
			_client.SendTimeout = NetworkTimeout;
			_endPoint = endPoint;
			_client.Connect(_endPoint);
			return true;
		}

		public void Close()
		{
			if(_client != null)
				_client.Close();
		}

		public void Send(object obj)
		{
			var buffer = _serializer.ToBytes(obj);
			_client.GetStream().Write(buffer, 0, buffer.Length);
		}

		public object Receive()
		{
			var buffer = new byte[10000];
			var readed = _client.GetStream().Read(buffer, 0, buffer.Length);
			var bufferPrecise = new byte[readed];
			Buffer.BlockCopy(buffer, 0, bufferPrecise, 0, readed);
			return _serializer.ToObject(bufferPrecise);
		}
	}
}
