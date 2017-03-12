using System.Collections.Generic;
using System.Linq;
using Donkey.Common;

namespace DonkeyAI
{
	class DropMaxPointsStrategy : IGameStrategy
	{
		public GameMove CalculateMove(List<Card> cardsInHand, List<Card> cardsOnTable, bool isDonkeyRound, bool canDropDonkey)
		{
			if (isDonkeyRound)
				return CalculateDonkeyRound(cardsInHand, cardsOnTable);

			return CalculateUsualRound(cardsInHand, cardsOnTable, canDropDonkey);

		}

		private GameMove CalculateUsualRound(List<Card> cardsInHand, List<Card> cardsOnTable, bool canDropDonkey)
		{
			var resultMove = new GameMove();
			resultMove.MoveType = MoveType.Drop;
			if (cardsOnTable.Count > 0)
			{
				var acceptableList = FindAcceptableDrop(cardsInHand, cardsOnTable);
				if (acceptableList.Count == 0)
					return new GameMove() { MoveType = MoveType.Pass };

				
				resultMove.Cards = acceptableList;
				return resultMove;
			}

			if (cardsInHand.Contains(Card.Donkey) && canDropDonkey)
			{
				var list = new List<Card> { Card.Donkey };
				resultMove.Cards = list;
				return resultMove;
			}

			var maxList = FindMaxDrop(cardsInHand, true);
			resultMove.Cards = maxList;
			return resultMove;
		}

		private GameMove CalculateDonkeyRound(List<Card> cardsInHand, List<Card> cardsOnTable)
		{
			var minCard = cardsInHand.Min();
			var resultMove = new GameMove() { MoveType = MoveType.Drop };
			resultMove.Cards.Add(minCard);
			return resultMove;
		}
		
		private List<Card> FindAcceptableDrop(List<Card> cardsInHand, List<Card> cardsOnTable)
		{
			var sorted = GetSortedCards(cardsInHand);
			var valueOnTable = CardListValueHelper.GetValue(cardsOnTable);
			foreach (var key in sorted.Keys)
			{
				if ((int)key <= valueOnTable || sorted[key] < cardsOnTable.Count)
					continue;
				var result = new List<Card>();
				for (int i = 0; i < cardsOnTable.Count; i++)
					result.Add(key);
				return result;
			}

			return new List<Card>();
		}

		private Dictionary<Card, int> GetSortedCards(List<Card> cards)
		{
			var sorted = new Dictionary<Card, int>();
			foreach (var c in cards)
				if (sorted.ContainsKey(c))
					sorted[c] += 1;
				else
					sorted[c] = 1;
			return sorted;
		}

		private List<Card> FindMaxDrop(List<Card> cards, bool ignoreDonkey)
		{
			var sorted = GetSortedCards(cards);

			var max = 0;
			var result = new List<Card>();

			foreach (var key in sorted.Keys)
			{
				if (ignoreDonkey && key == Card.Donkey)
					continue;

				var count = sorted[key] > 3 ? 3 : sorted[key];
				var current = (int)key * count;
				if (max > current)
					continue;

				max = current;
				result.Clear();
				for (int i = 0; i < count; i++)
					result.Add(key);
			}

			return result;
		}
	}
}
