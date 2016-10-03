using System;
using System.Collections.Generic;
using System.Linq;

namespace Donkey.Common
{
	public class CardStack
	{
		private readonly Stack<List<Card>> _stack = new Stack<List<Card>>();

		public List<Card> Top
		{
			get
			{
				if (IsEmpty)
					return new List<Card>();

				return _stack.Peek();
			}
		}

		public bool IsEmpty
		{
			get
			{
				return _stack.Count == 0;
			}
		}

		public void Push(List<Card> cards)
		{
			if (cards.Count < 1 || cards.Count > 3) throw new ArgumentException("Неверное количество карт.");
			if (cards.Any(x => x != cards[0])) throw new ArgumentException("Неверная комбинация карт, карыт должны быть одинаковыми.");
			_stack.Push(cards);
		}

		public List<Card> PopAll()
		{
			var result = new List<Card>();

			while (_stack.Count > 0)
				result.AddRange(_stack.Pop());

			return result;
		}

		public void Clear()
		{
			_stack.Clear();
		}

	}
}
