using System;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class GetLobbiesAnswer : ServerAnswer
	{
		public string[] Lobbies;

		public GetLobbiesAnswer(bool success)
			: base(success)
		{
		}
	}
}
