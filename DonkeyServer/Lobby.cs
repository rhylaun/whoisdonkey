using Donkey.Common;
using System.Collections.Generic;
using System.Linq;

namespace Donkey.Server
{
	public class Lobby
	{
		private readonly object _locker = new object();
		private readonly List<Player> _players = new List<Player>();

		public string Name { get; private set; }
		public bool Ready
		{
			get
			{
				lock (_locker)
				{
					return _players.All(x => x.State == PlayerState.Ready);
				}
			}
		}

		public Lobby(string name)
		{
			Name = name;
		}

		public void AddPlayer(Player player)
		{
			lock (_locker)
				_players.Add(player);
		}

		public void RemovePlayer(Player player)
		{
			lock (_locker)
				_players.Remove(player);
		}

		public IEnumerable<Player> GetPlayers()
		{
			lock (_locker)
				return _players.ToList();
		}

		public PlayerInLobbyDescription[] GetState()
		{
			lock (_locker)
			{
				var result = _players.Select(x => new PlayerInLobbyDescription(x.AuthData.Login, x.State == PlayerState.Ready)).ToArray();
				return result;
			}
		}
	}
}
