using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Answers;
using Donkey.Common.Commands;
using System.Net;
using System.Threading;

namespace Donkey.Common.ClientServer
{
	public class CommandSender<TNetworkClient> where TNetworkClient : INetworkClient, new()
	{
		private readonly IPEndPoint _serverIp;

		public CommandSender(IPEndPoint serverIp)
		{
			_serverIp = serverIp;
		}

		public ServerAnswer Request(ClientCommand command)
		{
			var client = new TNetworkClient();
			client.Connect(_serverIp);
			client.Send(command);
			Thread.Sleep(100);
			var ans = (ServerAnswer)client.Receive();
			client.Close();
			return ans;
		}
	}
}
