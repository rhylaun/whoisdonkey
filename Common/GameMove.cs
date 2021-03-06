﻿using System;
using System.Collections.Generic;

namespace Donkey.Common
{
	[Serializable]
	public class GameMove
	{
		public Guid GameId;
		public MoveType MoveType;
		public List<Card> Cards = new List<Card>();
		public DateTime Date;
		public string PlayerName;
		public int Index;

		public GameMove()
		{
			Cards = new List<Card>();
		}

		public bool IsDonkeyMove()
		{
			return MoveType == MoveType.Drop && Cards.Count == 1 && Cards[0] == Card.Donkey;
		}

		public GameMove GetCopy()
		{
			var result = new GameMove()
			{
				GameId = this.GameId,
				PlayerName = this.PlayerName,
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
			return CardListValueHelper.GetValue(Cards);
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
