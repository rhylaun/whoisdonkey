using System;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class GetPlayersAnswer : ServerAnswer
	{
		public string[] Players;

		public GetPlayersAnswer(bool success)
			: base(success)
		{
		}

		public override string ToString()
		{
			if (Players == null) return "null";
			var result = string.Empty;
			foreach (var p in Players) result += p + "; ";
			return result;
		}
	}
}
