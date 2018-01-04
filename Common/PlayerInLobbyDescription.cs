using System;

namespace Donkey.Common
{
	[Serializable]
	public class PlayerInLobbyDescription
	{
		public readonly string Name;
		public readonly PlayerType PlayerType;

		public PlayerInLobbyDescription(string name, PlayerType playerType)
		{
			Name = name;
			PlayerType = playerType;
		}
	}
}
