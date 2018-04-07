using System;
using System.Collections.Generic;

namespace Donkey.Common
{
	[Serializable]
	public class LobbyState
	{
		public readonly string Creator;
		public readonly List<PlayerInLobbyDescription> Players;

		public LobbyState(string creator, List<PlayerInLobbyDescription> players)
		{
			Creator = creator;
			Players = players;
		}
	}
}
