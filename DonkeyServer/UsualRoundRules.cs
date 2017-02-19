using System.Collections.Generic;
using System.Linq;
using Donkey.Common;
using System;

namespace Donkey.Server
{
	internal class UsualRoundRules : IRoundRules
	{
		public bool ValidateMove(GameMove pendingMove, List<Card> topCards)
		{
			if (!topCards.Any())
			{
				if (pendingMove.MoveType == MoveType.Clear)
					return false;
				if (pendingMove.MoveType == MoveType.Pass)
					return false;
			}

			if (pendingMove.MoveType == MoveType.Drop)
			{
				if (pendingMove.Cards.Count < 1 || pendingMove.Cards.Count > 3)
					return false;
				if (pendingMove.Cards.Any(x => x != pendingMove.Cards[0]))
					return false;

				if (topCards.Count == 0)
					return true;

				if (topCards.Count != pendingMove.Cards.Count)
					return false;
				if (topCards[0] >= pendingMove.Cards[0]) //TODO: надо ввести move value
					return false;
				if (pendingMove.Cards.Contains(Card.Donkey))
					return false;
			}

			return true;
		}

		public bool ChangeWinner(GameMove currentWinnerMove, GameMove pendingMove)
		{
			if (currentWinnerMove == null)
				return true;

			return currentWinnerMove.GetValue() < pendingMove.GetValue();
		}

		public GameMove GetTakeMove(AuthData player, List<Card> cards)
		{
			var move = new GameMove()
			{
				MoveType = MoveType.Take,
				Date = DateTime.UtcNow,
				Player = player,
				Cards = new List<Card>()
			};
			return move;
		}
	}
}
