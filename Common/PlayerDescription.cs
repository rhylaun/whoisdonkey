using System;

namespace Donkey.Common
{
	[Serializable]
	public class PlayerDescription
	{
		public readonly string Name;
		public readonly PlayerType PlayerType;

		public PlayerDescription(string name, PlayerType playerType)
		{
			Name = name;
			PlayerType = playerType;
		}
	}
}
