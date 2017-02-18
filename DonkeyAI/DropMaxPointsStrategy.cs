using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			if (canDropDonkey && cardsInHand.Contains(Card.Donkey))
			{
				var donkeyMove = new GameMove() { MoveType = MoveType.Drop };
				donkeyMove.Cards.Add(Card.Donkey);
				return donkeyMove;
			}

			var tableCardsValue = CardListValueHelper.GetValue(cardsOnTable);
			var tableCardsCount = cardsOnTable.Count;

			var groupedCards = new Dictionary<Card, int>();
			foreach (var card in cardsInHand)
				if (groupedCards.ContainsKey(card))
					groupedCards[card] += 1;
				else
					groupedCards[card] = 1;

			var resultMove = new GameMove() { MoveType = MoveType.Pass };
			var moveValue = tableCardsValue;

			foreach (var card in groupedCards.Keys)
			{
				if (card == Card.Donkey)
					continue;

				if ((int)card <= moveValue)
					continue;

				if (groupedCards[card] >= tableCardsCount && (int)card > moveValue)
				{
					resultMove.MoveType = MoveType.Drop;
					moveValue = (int)card;
				}
			}

			if (resultMove.MoveType == MoveType.Pass)
				return resultMove;

			for (int i = 0; i < tableCardsCount; i++)
				resultMove.Cards.Add((Card)moveValue);

			return resultMove;
		}

		private GameMove CalculateDonkeyRound(List<Card> cardsInHand, List<Card> cardsOnTable)
		{
			var minCard = cardsInHand.Min();
			var resultMove = new GameMove() { MoveType = MoveType.Drop };
			resultMove.Cards.Add(minCard);
			return resultMove;
		}
	}
}
