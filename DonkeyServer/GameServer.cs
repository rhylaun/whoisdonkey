using System;
using System.Collections.Generic;
using System.Linq;
using Donkey.Common;
using Donkey.Server.Storage;

namespace Donkey.Server
{
	public class GameServer : IDisposable
	{
		private readonly object _locker = new object();

		private readonly List<Player> _players = new List<Player>();
		private readonly List<Lobby> _lobbies = new List<Lobby>();
		private readonly List<Game> _games = new List<Game>();

		private Database _database;
		public Database Database
		{
			get { return _database; }
		}

		private readonly Statistic _statistic = new Statistic();
		public Statistic Statistic
		{
			get { return _statistic; }
		}

		public void Load()
		{
			if (_database == null)
				_database = Database.GetDatabase();
			var accoutns = _database.ReadAccounts();
			_players.Clear();
			_players.AddRange(accoutns.Select(x => new Player(x)));

			var saves = _database.GetSavedGames();
			foreach (var saveId in saves)
			{
				GameHistory gameHistory;
				if (_database.TryReadGameHistory(saveId, out gameHistory))
					_statistic.Insert(saveId, gameHistory);
			}
		}

		public Player GetPlayer(AuthData authData)
		{
			lock (_locker)
			{
				var player = _players.FirstOrDefault(x => x.AuthData.Equals(authData));
				if (player == null)
					throw new GameServerException("Player not found");

				return player;
			}
		}

		public Player GetPlayer(string playerName)
		{
			lock (_locker)
			{
				var player = _players.FirstOrDefault(x => x.AuthData.Login.Equals(playerName));
				if (player == null)
					throw new GameServerException("Player not found");

				return player;
			}
		}

		public bool HasPlayer(AuthData authData)
		{
			lock (_locker)
			{
				var player = _players.FirstOrDefault(x => x.AuthData.Equals(authData));
				return (player != null);
			}
		}

		public void AddPlayer(AuthData authData)
		{
			lock (_locker)
			{
				var player = _players.FirstOrDefault(x => x.AuthData.Equals(authData));
				if (player != null)
					throw new GameServerException("Player already exist");

				_players.Add(new Player(authData));
				_database.WriteAccounts(_players.Select(x => x.AuthData).ToList());
			}
		}

		public List<string> GetPlayers()
		{
			lock (_locker)
			{
				return _players.Select(x => x.AuthData.Login).ToList();
			}
		}

		public Lobby CreateLobby(string name)
		{
			lock (_locker)
			{
				var lobby = new Lobby(name);
				_lobbies.Add(lobby);
				return lobby;
			}
		}

		public List<string> GetLobbies()
		{
			lock (_locker)
			{
				return _lobbies.Select(x => x.Name).ToList();
			}
		}

		public Lobby GetLobby(string lobbyName)
		{
			lock (_locker)
			{
				var lobby = _lobbies.FirstOrDefault(x => x.Name == lobbyName);
				if (lobby == null)
					throw new GameServerException("Lobby does not found");

				return lobby;
			}
		}

		public Lobby GetLobbyByPlayer(Player player)
		{
			lock (_locker)
			{
				foreach (var lobby in _lobbies)
					if (lobby.GetPlayers().Any(x => x.Name == player.AuthData.Login))
						return lobby;

				throw new GameServerException("Lobby not found for specific player");
			}
		}

		public void RemoveLobby(Lobby lobby)
		{
			lock (_locker)
			{
				_lobbies.Remove(lobby);
			}
		}

		public void RemoveGame(Game game)
		{
			lock (_locker)
			{
				_games.Remove(game);
			}
		}

		public Game CreateGame(Lobby lobby)
		{
			lock (_locker)
			{
				var game = new Game(lobby, Database);
				_games.Add(game);
				_lobbies.Remove(lobby);
				return game;
			}
		}

		public Game GetGameByPlayer(Player player)
		{
			lock (_locker)
			{
				return _games.Single(x => x.HasPlayer(player.AuthData.Login));
			}
		}

		public void Dispose()
		{
			_players.Clear();
			_games.Clear();
			_lobbies.Clear();
		}
	}
}

