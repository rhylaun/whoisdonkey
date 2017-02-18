using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common
{
	public static class CardListValueHelper
	{
		public static int GetValue(List<Card> cards)
		{
			if (cards == null || cards.Count == 0)
				return 0;

			if (!cards.Any(x => x == Card.Joker))
				return (int)cards.First();

			if (!cards.All(x => x == Card.Joker))
				return (int)Card.Joker;

			return (int)(cards.Where(x => x != Card.Joker).First());
		}
	}
}
