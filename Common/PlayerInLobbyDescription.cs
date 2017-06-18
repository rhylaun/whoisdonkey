using System;

namespace Donkey.Common
{
	[Serializable]
	public class PlayerInLobbyDescription
	{
		public readonly string Name;
		public readonly bool Ready;

		public PlayerInLobbyDescription(string name, bool ready)
		{
			Name = name;
			Ready = ready;
		}
	}
}
