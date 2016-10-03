using Donkey.Common;
using Donkey.Common.ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Client
{
	public class GameClientManager
	{
		private static string _serverAddress = "127.0.0.1";

		private static object _locker = new object();
		private static IGameClient _instance;

		public static IGameClient Current
		{
			get
			{
				lock (_locker)
				{
#if DEBUG
					if (_instance == null)
						_instance = new FakeGameClient();
#endif

					return _instance;
				}

			}
		}

		public static IGameClient CreateNew(AuthData auth)
		{
			lock (_locker)
			{
				if (_instance != null)
				{
					while (_instance.State != PlayerState.Offline && _instance.Leave()) { }
					_instance.Dispose();	
				}

				_instance = new GameClient<TcpNetworkClient>(auth, _serverAddress);

				return _instance;
			}
		}
	}
}
