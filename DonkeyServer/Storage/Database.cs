using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Linq;
using Donkey.Common;

namespace Donkey.Server.Storage
{
	public class Database
	{
		public const string DbFolderPath = "ddb";
		public const string DbFileName = "accounts.ddb";
		public const string DbGameSaveExtention = "dgs";

		private readonly Dictionary<Guid, string> _gameFilesCache;

		private Database()
		{
			_gameFilesCache = new Dictionary<Guid, string>();
		}

		public static Database GetDatabase()
		{
			var db = new Database();
			db.Initialize();
			return db;
		}

		public List<Guid> GetSavedGames()
		{
			lock(_gameFilesCache)
				return _gameFilesCache.Keys.ToList();
		}

		private void Initialize()
		{
			var dirInfo = new DirectoryInfo(DbFolderPath);
			if (!dirInfo.Exists) dirInfo.Create();

			var fpath = Path.Combine(DbFolderPath, DbFileName);
			var donkeyFileInfo = new FileInfo(fpath);
			if (!donkeyFileInfo.Exists)
				SQLiteConnection.CreateFile(fpath);


			using (var connection = new SQLiteConnection(DbRequestHelper.GenerateConnectionString(fpath)))
			{
				connection.Open();

				var request = DbRequestHelper.GenerateCreateAccountsTableRequest();
				using (var command = new SQLiteCommand(request, connection))
					command.ExecuteNonQuery();

				connection.Close();
			}

			var gameSaves = dirInfo.GetFiles("*." + DbGameSaveExtention).ToList();
			lock (_gameFilesCache)
			foreach (var gs in gameSaves)
				{
					var guidStr = gs.Name.Replace("." + DbGameSaveExtention, string.Empty);
					var gameId = new Guid(guidStr);
					_gameFilesCache.Add(gameId, gs.FullName);
				}

		}

		public List<AuthData> ReadAccounts()
		{
			var fpath = Path.Combine(DbFolderPath, DbFileName);
			var result = new List<AuthData>();
			using (var connection = new SQLiteConnection(DbRequestHelper.GenerateConnectionString(fpath)))
			{
				connection.Open();

				var request = DbRequestHelper.GenerateReadAccountsRequest();
				using (var command = new SQLiteCommand(request, connection))
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						var name = reader.GetString(0);
						var pswd = reader.GetValue(1);

						var authData = new AuthData(name, pswd is DBNull ? string.Empty : (string)pswd);
						result.Add(authData);
					}
				}
				connection.Close();
			}

			return result;
		}

		public void WriteAccounts(List<AuthData> accounts)
		{
			var fpath = Path.Combine(DbFolderPath, DbFileName);
			using (var connection = new SQLiteConnection(DbRequestHelper.GenerateConnectionString(fpath)))
			{
				connection.Open();

				var request = DbRequestHelper.GenerateWriteAccountsRequest(accounts);
				using (var command = new SQLiteCommand(request, connection))
					command.ExecuteNonQuery();

				connection.Close();
			}
		}

		public void WriteGameMove(GameMove gameMove)
		{
			var fpath = string.Empty;
			lock (_gameFilesCache)
			{
				if (!_gameFilesCache.ContainsKey(gameMove.GameId))
					InitializeGameHistoryFile(gameMove.GameId);
				fpath = _gameFilesCache[gameMove.GameId];
			}

			using (var connection = new SQLiteConnection(DbRequestHelper.GenerateConnectionString(fpath)))
			{
				connection.Open();
				var writeCommand = DbRequestHelper.GenerateWriteGameMoveRequest();
				using (var command = new SQLiteCommand(writeCommand, connection))
				{
					command.Parameters.AddWithValue("$moveindex", gameMove.Index);
					command.Parameters.AddWithValue("$move", gameMove.MoveType.ToString());
					command.Parameters.AddWithValue("$name", gameMove.Player.Login);
					command.Parameters.AddWithValue("$date", gameMove.Date);
					command.Parameters.AddWithValue("$cards", DbRequestHelper.ConvertCardList(gameMove.Cards));
					command.ExecuteNonQuery();
				}
				connection.Close();
			}
		}

		public bool TryReadGameHistory(Guid gameId, out GameHistory history)
		{
			var fpath = string.Empty;
			lock (_gameFilesCache)
				fpath = _gameFilesCache[gameId];

			history = new GameHistory();
			try
			{
				using (var connection = new SQLiteConnection(DbRequestHelper.GenerateConnectionString(fpath)))
				{
					connection.Open();
					var writeCommand = DbRequestHelper.GenerateReadGameHistory();
					using (var command = new SQLiteCommand(writeCommand, connection))
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							var move = new GameMove();
							move.Index = reader.GetInt32(0);
							move.MoveType = (MoveType)Enum.Parse(typeof(MoveType), reader.GetString(1));
							move.Player = new AuthData(reader.GetString(2), string.Empty);
							move.Date = reader.GetDateTime(3);
							move.Cards = DbRequestHelper.ConvertCardString(reader.GetString(4));
							move.GameId = gameId;
							history.Add(move);
						}
					}
					connection.Close();
				}
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine($"Save file corrupted: {gameId}");
				return false;
			}

			return true;
		}

		private void InitializeGameHistoryFile(Guid gameId)
		{
			var tmpPath = Path.Combine(DbFolderPath, gameId + "." + DbGameSaveExtention);
			if (!File.Exists(tmpPath))
				SQLiteConnection.CreateFile(tmpPath);
			using (var connection = new SQLiteConnection(DbRequestHelper.GenerateConnectionString(tmpPath)))
			{
				connection.Open();
				var createCommand = DbRequestHelper.GenerateCreateHistoryTableRequest();
				using (var command = new SQLiteCommand(createCommand, connection))
					command.ExecuteNonQuery();
				connection.Close();
			}
			_gameFilesCache.Add(gameId, tmpPath);
		}
	}
}
