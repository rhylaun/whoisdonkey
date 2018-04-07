using System;
using System.Collections.Generic;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class GetServerInfoAnswer : ServerAnswer
	{
		public readonly List<string> BotNames;

		public GetServerInfoAnswer(bool success, List<string> botNames)
			: base(success)
		{
			BotNames = botNames;
		}
	}
}
