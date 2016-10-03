using Donkey.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Client
{
	public interface IGameClient : IDisposable
	{
		AuthData AuthData { get; }
		PlayerState State { get; }
		int CurrentGameStep { get; }
		List<Card> GetCards();
		GameMove[] GetHistory(int fromIndex);
        GameMove[] GetHistory();
        List<string> GetLobbies();
		List<string> GetPlayers();

		bool Register();
		bool Auth();
		bool JoinLobby(string lobbyName);
		bool CreateLobby(string lobbyName);
		bool Leave();
		bool StartGame();
		bool Pass();
		bool MakeMove(List<Card> cards);
		bool Update();
	}
}
