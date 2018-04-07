using System;
using System.Collections.Generic;
using System.Linq;
using Donkey.Common;

namespace Donkey.Server
{
	internal class PlayProcessor
	{
		private readonly Guid _gameId;
		private readonly GameHistory _history;
		private readonly CardStack _cardStack;
		private IRoundRules _roundRules;
		private AuthData _endRoundPlayer;
		private GameMove _winningMove;
		private int _denyDonkeyMoveInRounds = 0;

		public PlayProcessor(Guid gameId)
		{
			_gameId = gameId;
			_history = new GameHistory();
			_cardStack = new CardStack();
		}

		public bool Append(GameMove move)
		{
			if (_history.Count != 0 && move.Index != (_history.Last().Index + 1))
				return false;

			if (move.MoveType == MoveType.Clear)
			{
				_history.Add(move);
				_cardStack.Clear();
				_roundRules = new UsualRoundRules();
				if (_denyDonkeyMoveInRounds > 0)
					_denyDonkeyMoveInRounds--;
				_winningMove = null;
				_endRoundPlayer = move.Player;
				return true;
			}

			if (move.MoveType == MoveType.Take)
			{
				_history.Add(move);
				return true;
			}

			if (_cardStack.IsEmpty && move.IsDonkeyMove())
			{
				if (_denyDonkeyMoveInRounds > 0)
					return false;

				_history.Add(move);
				_cardStack.Push(move.Cards);
				_endRoundPlayer = move.Player;

				_roundRules = new DonkeyRoundRules();
				_denyDonkeyMoveInRounds = 2;
				return true;
			}

			var result = _roundRules.ValidateMove(move, _cardStack.Top);
			if (result)
			{
				_history.Add(move);
				if (move.MoveType == MoveType.Drop)
					_cardStack.Push(move.Cards);
				if (_roundRules.ChangeWinner(_winningMove, move))
					_winningMove = move;
			}
			return result;
		}

		public List<GameMove> EndRound()
		{
			var result = new List<GameMove>();

			var clearMove = GenerateMove(_winningMove.Player, MoveType.Clear, new List<Card>());
			result.Add(clearMove);

			var takeMove = _roundRules.GetTakeMove(_winningMove.Player, _cardStack.PopAll());
			takeMove.GameId = _gameId;
			takeMove.Index = clearMove.Index + 1;

			Append(clearMove);
			if (!takeMove.Cards.Any())
				return result;

			result.Add(takeMove);
			Append(takeMove);
			return result;
		}

		public GameMove GenerateMove(AuthData player, MoveType moveType, List<Card> cards)
		{
			var index = 0;
			if (_history.Count > 0)
				index = _history.Last().Index + 1;
			var result = new GameMove()
			{
				Cards = cards,
				GameId = _gameId,
				MoveType = moveType,
				Player = player,
				Date = DateTime.UtcNow,
				Index = index
			};
			return result;
		}

		public GameMove[] GetHistory(int fromIndex)
		{
			return _history.ToArray(fromIndex);
		}

		public AuthData GetEndRoundPlayer()
		{
			return _endRoundPlayer;
		}

		public AuthData GetRoundWinner()
		{
			if (_winningMove == null)
				return null;

			return _winningMove.Player;
		}
	}
}
