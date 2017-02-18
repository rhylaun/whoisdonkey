using Donkey.Common;
using System.Collections.Generic;

namespace DonkeyAI
{
	public interface IGameStrategy
    {
		GameMove CalculateMove(List<Card> cardsInHand, List<Card> cardsOnTable, bool isDonkeyRound, bool canDropDonkey);
    }
}
