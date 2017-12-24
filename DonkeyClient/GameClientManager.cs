using Donkey.Common;
using Donkey.Common.ClientServer;
using System.IO;

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

				if (File.Exists("server.cfg"))
				{
					_serverAddress = File.ReadAllLines("server.cfg")[0].Trim();
				}

				_instance = new GameClient<TcpNetworkClient>(auth, _serverAddress);

				return _instance;
			}
		}
	}
}
