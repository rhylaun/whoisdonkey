using System;

namespace Donkey.Common.Answers
{
	[Serializable]
    public class GetStatisticAnswer : ServerAnswer
    {
		public readonly StatisticRecord[] Statistic;

        public GetStatisticAnswer(bool success, StatisticRecord[] statistic)
            : base(success)
        {
			Statistic = statistic;
        }
    }
}
