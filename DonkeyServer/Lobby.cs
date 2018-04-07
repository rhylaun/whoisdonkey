using Donkey.Common;
using System.Collections.Generic;
using System.Linq;

namespace Donkey.Server
{
	public class Lobby
	{
		private readonly object _locker = new object();
		private readonly List<PlayerDescription> _players = new List<PlayerDescription>();

		public string Name { get; private set; }
		public Player Creator { get; private set; }

		public Lobby(string name)
		{
			Name = name;
		}

		public void AddPlayer(Player player)
		{
			lock (_locker)
			{
				if (_players.Any(x => x.Name == player.AuthData.Login))
					return;

				if (_players.Count == 0)
					Creator = player;

				var newPlayer = new PlayerDescription(player.AuthData.Login, PlayerType.Human);
				_players.Add(newPlayer);
			}
		}

		public void RemovePlayer(Player player)
		{
			lock (_locker)
			{
				var toDelete = _players.Single(x => x.Name == player.AuthData.Login);
				_players.Remove(toDelete);
			}
		}

		public PlayerDescription[] GetPlayers()
		{
			lock (_locker)
			{
				var result = _players.ToArray();
				return result;
			}
		}
	}
}
