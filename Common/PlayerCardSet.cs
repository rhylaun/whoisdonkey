using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common
{
	[Serializable]
	public class PlayerCardSet
	{
		private readonly Dictionary<Card, int> _cardset;

		public PlayerCardSet()
		{
			_cardset = new Dictionary<Card, int>();
			foreach (var card in Enum.GetValues(typeof(Card)).Cast<Card>())
				_cardset[card] = 0;
		}

		public PlayerCardSet(Dictionary<Card, int> cardset)
		{
			_cardset = new Dictionary<Card, int>(cardset);
		}

		public bool Contains(Card card, int amount)
		{
			return _cardset.ContainsKey(card) && _cardset[card] >= amount;
		}

		public bool Contains(List<Card> cards)
		{
			var counter = new Dictionary<Card, int>();
			foreach (var card in cards)
			{
				if (!counter.ContainsKey(card))
					counter[card] = 0;
				counter[card]++;
			}

			foreach (var card in counter.Keys)
				if (!Contains(card, counter[card]))
					return false;
			return true;
		}

		public bool Extract(Card card, int amount)
		{
			if (Contains(card, amount))
			{
				_cardset[card] -= amount;
				return true;
			}
			return false;
		}

		public bool Extract(List<Card> cards)
		{
			if (!Contains(cards))
				return false;
			//TODO: здессь возможны косяки, по идее надо предусмотреть механизм отката операции если по середине все развалится
			foreach (var card in cards)
				Extract(card, 1);

			return true;
		}

		public void Add(Card card)
		{
			_cardset[card]++;
		}

		public void Add(List<Card> cards)
		{
			foreach (var card in cards)
				Add(card);
		}

		public int this[Card card]
		{
			get
			{
				return _cardset[card];
			}
			set
			{
				_cardset[card] = value;
			}
		}

		public bool Contains(Card card)
		{
			return Contains(card, 1);
		}

		public bool Any()
		{
			return _cardset.Any(x => x.Value > 0);
		}

		public uint GetTotal()
		{
			uint result = 0;
			foreach (var key in _cardset.Keys)
				result += Convert.ToUInt32(_cardset[key] * (int)key);
			return result;
		}

		public List<Card> ToList()
		{
			var list = new List<Card>();
			foreach (var kvp in _cardset)
				for (int i = 0; i < kvp.Value; i++)
					list.Add(kvp.Key);

			return list;
		}
	}
}
