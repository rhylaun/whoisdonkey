using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using Donkey.Common.Commands;
using Donkey.Common.Answers;
using System.Net;

namespace Donkey.Common.ClientServer
{
    public class UdpNetworkClient : INetworkClient
    {
        private readonly UdpClient _client;
        private IPEndPoint _remoteHost;
        private readonly GameSerializer _serializer = new GameSerializer();

        public UdpNetworkClient()
        {
            _client = new UdpClient();
            _client.DontFragment = true;
        }

        public UdpNetworkClient(IPEndPoint localEndPoint)
        {
            _client = new UdpClient(localEndPoint);
        }

        public bool Connect(IPEndPoint endPoint)
        {
            _remoteHost = endPoint;
			return true;
        }

        public void Close()
        {
            _client.Close();
        }

        public void Send(object obj)
        {
            var buffer = _serializer.ToBytes(obj);
            _client.Send(buffer, buffer.Length, _remoteHost);
        }

        public object Receive()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, ((IPEndPoint)_client.Client.LocalEndPoint).Port);
            var buffer = _client.Receive(ref ep);
            return _serializer.ToObject(buffer);
        }
    }
}
