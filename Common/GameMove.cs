using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common
{
	[Serializable]
	public class GameMove
	{
		public Guid GameId;
		public MoveType MoveType;
		public List<Card> Cards = new List<Card>();
		public DateTime Date;
		public AuthData Player;
		public int Index;

		public GameMove()
		{
			Cards = new List<Card>();
		}

		public bool IsDonkeyMove()
		{
			return MoveType == Common.MoveType.Drop && Cards.Count == 1 && Cards[0] == Card.Donkey;
		}

		public GameMove GetCopy()
		{
			var result = new GameMove()
			{
				GameId = this.GameId,
				Player = this.Player,
				MoveType = this.MoveType,
				Index = this.Index,
				Date = this.Date
			};

			if (this.Cards != null)
				result.Cards = new List<Card>(this.Cards);

			return result;
		}

		public int GetValue()
		{
			if (Cards == null || Cards.Count == 0)
				return 0;

			if (!Cards.Any(x => x == Card.Joker))
				return (int)Cards.First();

			if (!Cards.All(x => x == Card.Joker))
				return (int)Card.Joker;

			return (int)(Cards.Where(x => x != Card.Joker).First());
		}
	}

	public enum MoveType
	{
		Pass,
		Drop,
		Clear,
		Take
	}
}
