using Donkey.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Donkey.Server
{
	public class Statistic
	{
		private readonly object _locker = new object();

		public readonly List<StatisticRecord> Records = new List<StatisticRecord>();

		public void Insert(Guid gameId, GameHistory history)
		{
			var moveProcessor = new PlayProcessor(gameId);			
			var moves = history.ToArray(0);
			var players = moves.Select(x => x.Player).Distinct().ToArray();
			var playersCardSets = new List<PlayerCardSet>();
			for (int i = 0; i < players.Length; i++)
				playersCardSets.Add(new PlayerCardSet());
			var cardSet = new GameCardSet(playersCardSets);
			cardSet.BindPlayers(players);
			for (int i = 0; i < moves.Length; i++)
			{
				if (!moveProcessor.Append(moves[i]))
					throw new Exception("Cannot process saved game");

				if (moves[i].MoveType == MoveType.Take)
					cardSet.GetPlayerCardset(moves[i].Player).Add(moves[i].Cards);

				if(moves[i].MoveType == MoveType.Drop)
					cardSet.GetPlayerCardset(moves[i].Player).Extract(moves[i].Cards);
			}

			lock(_locker)
			{
				for (int i = 0; i < players.Length; i++)
				{
					if (!Records.Any(x => x.Name == players[i].Login))
						Records.Add(new StatisticRecord(players[i].Login));

					var record = GetRecord(players[i].Login);
					var playerCardSet = cardSet.GetPlayerCardset(i);

					record.GamesPlayed++;
					record.TotalScore += playerCardSet.GetTotal();
					record.FinishWithDonkey += playerCardSet.Contains(Card.Donkey) ? 1U : 0;
				}
			}
		}

		public StatisticRecord GetRecord(string name)
		{
			lock(_locker)
				return Records.Single(x => x.Name == name);
		}
	}
}
