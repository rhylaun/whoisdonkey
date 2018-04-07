using System;
using System.Collections.Generic;

namespace Donkey.Common
{
	[Serializable]
	public class LobbyState
	{
		public readonly string Creator;
		public readonly List<PlayerDescription> Players;

		public LobbyState(string creator, List<PlayerDescription> players)
		{
			Creator = creator;
			Players = players;
		}
	}
}
