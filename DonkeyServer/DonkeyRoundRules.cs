using System;
using System.Collections.Generic;
using Donkey.Common;

namespace Donkey.Server
{
	internal class DonkeyRoundRules : IRoundRules
	{
		public bool ValidateMove(GameMove pendingMove, List<Card> topCards)
		{
			if (pendingMove.MoveType == MoveType.Pass) return false;
			if (pendingMove.MoveType == MoveType.Clear) return true;

			//Остался только вариант с хода картами
			if (pendingMove.Cards.Count != 1) return false;
			//Проверка на дурака, вдруг кто второго осла в колоду подкинет
			if (pendingMove.Cards[0] == Card.Donkey) return false;

			return true;
		}
		
		public bool ChangeWinner(GameMove currentWinnerMove, GameMove pendingMove)
		{
			if (currentWinnerMove == null)
				return true;

			return currentWinnerMove.GetValue() <= pendingMove.GetValue();
		}

		public GameMove GetTakeMove(AuthData player, List<Card> cards)
		{
			var move = new GameMove()
			{
				MoveType = MoveType.Take,
				Cards = cards,
				Date = DateTime.UtcNow,
				Player = player
			};
			return move;

		}
	}
}
