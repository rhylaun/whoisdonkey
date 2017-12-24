using System;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class GetLobbyStateAnswer : ServerAnswer
	{
		public readonly PlayerInLobbyDescription[] Players;

		public GetLobbyStateAnswer(bool success, PlayerInLobbyDescription[] players)
			: base(success)
		{
			Players = players;
		}
	}
}
