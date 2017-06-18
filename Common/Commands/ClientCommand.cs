using System;

namespace Donkey.Common.Commands
{
	[Serializable]
	public abstract class ClientCommand
	{
		public readonly AuthData AuthData;
		public readonly CommandType CommandType;

		protected ClientCommand(AuthData authData, CommandType commandType)
		{
			AuthData = authData;
			CommandType = commandType;
		}
	}

	public enum CommandType
	{
		Auth,
		CreateLobby,
		GameMove,
		GetHistory,
		GetGames,
		GetLobbies,
		GetPlayers,
		JoinLobby,
		KeepAlive,
		Leave,
		Start,
		JoinGame,
		Register,
		GetState,
		GetCardSet,
		GetCurrentGameState,
		GetStatistic
	}
}
