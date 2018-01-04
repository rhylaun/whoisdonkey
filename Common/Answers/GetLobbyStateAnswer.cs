using System;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class GetLobbyStateAnswer : ServerAnswer
	{
		public readonly LobbyState State;

		public GetLobbyStateAnswer(bool success, LobbyState lobbyState)
			: base(success)
		{
			State = lobbyState;
		}
	}
}
