using System;

namespace Donkey.Common
{
	[Serializable]
	public class StatisticRecord
	{
		public readonly string Name;
		public uint GamesPlayed;
		public uint TotalScore;
		public uint FinishWithDonkey;

		public StatisticRecord(string name)
		{
			Name = name;
		}
	}
}
