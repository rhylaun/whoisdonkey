using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common;
using Donkey.Server.Storage;

namespace Donkey.Server
{
	public class Game
	{
		private readonly object _locker = new object();

		private readonly List<Player> _players;
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

		private Player _currentTurnPlayer;
		public Player CurrentTurnPlayer
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
			_players = new List<Player>(lobby.GetPlayers());
			_cardSet = CardShuffler.GetCardSet(_players.Count);
			_cardSet.BindPlayers(_players.Select(x => x.AuthData).ToArray());

			foreach (var player in _players)
			{
				var playerCardSet = _cardSet.GetPlayerCardset(player.AuthData);
				var takeMove = _moveProcessor.GenerateMove(player.AuthData, MoveType.Take, playerCardSet.ToList());
				_moveProcessor.Append(takeMove);
				_database.WriteGameMove(takeMove);
			}

			var emptyMove = _moveProcessor.GenerateMove(_players.First().AuthData, MoveType.Clear, new List<Card>());
			_moveProcessor.Append(emptyMove);
			_database.WriteGameMove(emptyMove);
		}

		public void Start()
		{
			lock (_locker)
			{
				foreach (var player in _players)
				{
					player.JoinGame();
					if (_cardSet.GetPlayerCardset(player.AuthData).Contains(Card.Donkey))
						_currentTurnPlayer = player;
				}
			}
		}

		public List<Player> GetPlayers()
		{
			lock (_locker)
			{
				return _players.ToList();
			}
		}

		public bool HasPlayer(Player player)
		{
			lock (_locker)
				return _players.Any(x => x.AuthData.Equals(player.AuthData));
		}

		public bool HasPlayer(AuthData authData)
		{
			lock (_locker)
				return _players.Any(x => x.AuthData.Equals(authData));
		}

		public PlayerCardSet GetPlayerCardSet(Player player)
		{
			return _cardSet.GetPlayerCardset(player.AuthData);
		}

		public GameMove CreateMove(Player player, MoveType moveType, List<Card> cards)
		{
			lock (_locker)
			{
				var move = _moveProcessor.GenerateMove(player.AuthData, moveType, cards);
				return move;
			}
		}

		public bool MakeMove(GameMove gameMove)
		{
			lock (_locker)
			{
				if (!HasPlayer(gameMove.Player))
					return false;

				if (CurrentTurnPlayer.AuthData != gameMove.Player)
					return false;

				if (gameMove.MoveType == MoveType.Drop && !_cardSet.GetPlayerCardset(gameMove.Player).Contains(gameMove.Cards))
					return false;

				if (!_moveProcessor.Append(gameMove))
					return false;

				_cardSet.GetPlayerCardset(gameMove.Player).Extract(gameMove.Cards);
				PassTurnToNextPlayer();
				var isRoundEnded = CheckForRoundEnd();
				if (isRoundEnded)
					CheckForGameEnd();

				_database.WriteGameMove(gameMove);
				return true;
			}
		}

		public bool MakeMove(Player player, MoveType moveType, List<Card> cards)
		{
			var move = CreateMove(player, moveType, cards);
			return MakeMove(move);
		}

		public GameMove[] GetHistory(Player player, int fromIndex)
		{
			var moves = _moveProcessor.GetHistory(fromIndex);
			foreach (var move in moves)
				if (move.MoveType == MoveType.Take && !player.AuthData.Equals(move.Player))
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
			var oldIndex = _players.IndexOf(_currentTurnPlayer);
			var newIndex = (oldIndex + 1) % _players.Count;
			_currentTurnPlayer = _players[newIndex];
		}

		private bool CheckForRoundEnd()
		{
			if (!_currentTurnPlayer.AuthData.Equals(_moveProcessor.GetEndRoundPlayer()))
				return false;

			var winner = _moveProcessor.GetRoundWinner();
			if (winner == null)
				return true;

			_currentTurnPlayer = _players.Single(x => x.AuthData.Equals(winner));
			var endRoundResult = _moveProcessor.EndRound();
			foreach (var move in endRoundResult)
			{
				_database.WriteGameMove(move);
				if (move.MoveType == MoveType.Take)
					_cardSet.GetPlayerCardset(move.Player).Add(move.Cards);
			}
			return true;
		}

		private bool CheckForGameEnd()
		{
			foreach (var p in _players)
				if (!_cardSet.GetPlayerCardset(p.AuthData).Any())
				{
					EndGame();
					return true;
				}

			return false;
		}
	}
}
