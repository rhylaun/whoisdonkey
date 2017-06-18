using System;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class CurrentGameStateAnswer : ServerAnswer
	{
		public readonly GameState GameState;

		public CurrentGameStateAnswer(bool success, string currentPlayerName, bool gameEnded) : base(success)
		{
			GameState = new GameState(currentPlayerName, gameEnded);
		}
	}
}
