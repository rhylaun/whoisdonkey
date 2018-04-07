using System;
using System.Collections.Generic;
using System.Linq;
using Donkey.Common;
using Donkey.Server.Storage;

namespace Donkey.Server
{
	public class Game
	{
		private readonly object _locker = new object();

		private readonly List<PlayerDescription> _registredPlayers;
		private readonly List<PlayerDescription> _ingamePlayers;
		private readonly GameCardSet _cardSet;
		private readonly PlayProcessor _moveProcessor;
		private readonly Database _database;

		private bool _ended = false;
		public bool Ended
		{
			get
			{
				lock (_locker)
					return _ended;
			}
		}

		public readonly string Name;
		public readonly Guid Id;

		private PlayerDescription _currentTurnPlayer;
		public PlayerDescription CurrentTurnPlayer
		{
			get
			{
				lock (_locker)
				{
					return _currentTurnPlayer;
				}
			}
		}

		public Game(Lobby lobby, Database database)
		{
			Id = Guid.NewGuid();
			Name = lobby.Name;
			_database = database;
			_moveProcessor = new PlayProcessor(Id);
			_registredPlayers = new List<PlayerDescription>(lobby.GetPlayers());
			_ingamePlayers = _registredPlayers.ToList();
			_cardSet = CardShuffler.GetCardSet(_registredPlayers.Count);
			_cardSet.BindPlayers(_registredPlayers.Select(x => x.Name).ToArray());

			foreach (var player in _registredPlayers)
			{
				var playerCardSet = _cardSet.GetPlayerCardset(player.Name);
				var takeMove = _moveProcessor.GenerateMove(player.Name, MoveType.Take, playerCardSet.ToList());
				_moveProcessor.Append(takeMove);
				_database.WriteGameMove(takeMove);
			}

			var emptyMove = _moveProcessor.GenerateMove(_registredPlayers.First().Name, MoveType.Clear, new List<Card>());
			_moveProcessor.Append(emptyMove);
			_database.WriteGameMove(emptyMove);
		}

		public void Start()
		{
			lock (_locker)
			{
				foreach (var player in _registredPlayers)
				{
					if (_cardSet.GetPlayerCardset(player.Name).Contains(Card.Donkey))
						_currentTurnPlayer = player;
				}
			}
		}

		public int GetHumanCount()
		{
			lock (_locker)
			{
				return _ingamePlayers.Count(x => x.PlayerType == PlayerType.Human);
			}
		}

		public void RemovePlayer(Player player)
		{
			lock (_locker)
			{
				var toDelete = _ingamePlayers.Single(x => x.Name == player.AuthData.Login);
				_ingamePlayers.Remove(toDelete);
			}
		}

		public bool HasPlayer(string playerName)
		{
			lock (_locker)
				return _ingamePlayers.Any(x => x.Name.Equals(playerName));
		}

		public bool HasPlayer(AuthData authData)
		{
			lock (_locker)
				return _ingamePlayers.Any(x => x.Name.Equals(authData.Login));
		}

		public PlayerCardSet GetPlayerCardSet(string playerName)
		{
			return _cardSet.GetPlayerCardset(playerName);
		}

		public GameMove CreateMove(string playerName, MoveType moveType, List<Card> cards)
		{
			lock (_locker)
			{
				var move = _moveProcessor.GenerateMove(playerName, moveType, cards);
				return move;
			}
		}

		public bool MakeMove(GameMove gameMove)
		{
			lock (_locker)
			{
				if (!HasPlayer(gameMove.PlayerName))
					return false;

				if (CurrentTurnPlayer.Name != gameMove.PlayerName)
					return false;

				if (gameMove.MoveType == MoveType.Drop && !_cardSet.GetPlayerCardset(gameMove.PlayerName).Contains(gameMove.Cards))
					return false;

				if (!_moveProcessor.Append(gameMove))
					return false;

				_cardSet.GetPlayerCardset(gameMove.PlayerName).Extract(gameMove.Cards);
				PassTurnToNextPlayer();
				var isRoundEnded = CheckForRoundEnd();
				if (isRoundEnded)
					CheckForGameEnd();

				_database.WriteGameMove(gameMove);
				return true;
			}
		}

		public bool MakeMove(string playerName, MoveType moveType, List<Card> cards)
		{
			var move = CreateMove(playerName, moveType, cards);
			return MakeMove(move);
		}

		public GameMove[] GetHistory(Player player, int fromIndex)
		{
			var moves = _moveProcessor.GetHistory(fromIndex);
			foreach (var move in moves)
				if (move.MoveType == MoveType.Take && !player.AuthData.Equals(move.PlayerName))
					for (int i = 0; i < move.Cards.Count; i++)
						move.Cards[i] = Card.Hidden;
			return moves;
		}

		private void EndGame()
		{
			_ended = true;
		}

		private void PassTurnToNextPlayer()
		{
			var oldIndex = _registredPlayers.IndexOf(_currentTurnPlayer);
			var newIndex = (oldIndex + 1) % _registredPlayers.Count;
			_currentTurnPlayer = _registredPlayers[newIndex];
		}

		private bool CheckForRoundEnd()
		{
			if (!_currentTurnPlayer.Name.Equals(_moveProcessor.GetEndRoundPlayerName()))
				return false;

			var winner = _moveProcessor.GetRoundWinner();
			if (winner == null)
				return true;

			_currentTurnPlayer = _registredPlayers.Single(x => x.Name.Equals(winner));
			var endRoundResult = _moveProcessor.EndRound();
			foreach (var move in endRoundResult)
			{
				_database.WriteGameMove(move);
				if (move.MoveType == MoveType.Take)
					_cardSet.GetPlayerCardset(move.PlayerName).Add(move.Cards);
			}
			return true;
		}

		private bool CheckForGameEnd()
		{
			foreach (var p in _registredPlayers)
				if (!_cardSet.GetPlayerCardset(p.Name).Any())
				{
					EndGame();
					return true;
				}

			return false;
		}
	}
}
