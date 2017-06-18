using System;

namespace Donkey.Common
{
	[Serializable]
	public class GameState
	{
		public readonly string ActivePlayerName;
		public readonly bool GameEnded;

		public GameState(string activePlayerName, bool gameEnded)
		{
			GameEnded = gameEnded;
			ActivePlayerName = activePlayerName;
		}
	}
}
