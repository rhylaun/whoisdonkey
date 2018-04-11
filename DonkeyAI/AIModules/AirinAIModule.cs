using Donkey.Common.AI;
using System;
using Donkey.Common;
using System.Linq;
using System.Collections.Generic;

namespace DonkeyAI.AIModules
{
	[AIModule(Name = "Airin")]
	public class AirinAIModule : IAIModule
	{
		private readonly IGameStrategy _strategy = new DropMaxPointsStrategy();

		public GameMove ProcessMove(GameMove[] history, PlayerCardSet hand)
		{
			var cardsInHand = hand.ToList();
			var isDonkeyRound = DetermineDonkeyRound(history);
			var cardsOnTable = FindCardsOnTable(history);

			var move = _strategy.CalculateMove(cardsInHand, cardsOnTable, isDonkeyRound, !isDonkeyRound);
			return move;
		}

		private bool DetermineDonkeyRound(GameMove[] moves)
		{
			try
			{
				var reversed = moves.Reverse().ToList();
				var clearMove = reversed.FirstOrDefault(x => x.MoveType == MoveType.Clear);
				var roundIndex = reversed.IndexOf(clearMove) - 1;
				return reversed[roundIndex].IsDonkeyMove();
			}
			catch
			{
			}
			return false;
		}

		private List<Card> FindCardsOnTable(GameMove[] moves)
		{
			try
			{
				var reversed = moves.Reverse().ToList();
				for (int i = 0; i < reversed.Count; i++)
				{
					if (reversed[i].MoveType == MoveType.Drop)
						return reversed[i].Cards;
					if (reversed[i].MoveType == MoveType.Clear)
						return new List<Card>();
				}
			}
			catch
			{
			}
			return new List<Card>();
		}
	}
}
