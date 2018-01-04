using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common;
using Donkey.Common.Commands;
using System.Threading;

namespace Donkey.Client
{
	public class FakeGameClient : IGameClient
	{
		private readonly PlayerStateMachine _stateMachine = new PlayerStateMachine();
		private readonly List<string> _lobbies = new List<string>();

		private string _currentPlayer = string.Empty;

		private Thread _changePlayerThread;

		public FakeGameClient()
		{
			Auth();
			_lobbies.Add("test_lobby");

			_history.Add(new GameMove() { MoveType = MoveType.Clear, Index = 0, });
			_history.Add(new GameMove() { MoveType = MoveType.Drop, Index = 1, Player = new AuthData("test2", ""), Cards = new List<Card> { Card.One, Card.One, Card.One } });
			_history.Add(new GameMove() { MoveType = MoveType.Pass, Index = 2, Player = new AuthData("test3", ""), });
			_history.Add(new GameMove() { MoveType = MoveType.Clear, Index = 3, });

			_cardSet = new PlayerCardSet();
			_cardSet.Add(new List<Card>() { Card.Donkey, Card.One, Card.Two, Card.Three, Card.Four, Card.Five, Card.Six,
				Card.Seven, Card.Eight, Card.Nine, Card.Ten, Card.Thirteen, Card.Joker });
			_currentPlayer = AuthData.Login;
			_changePlayerThread = new Thread(RotatePlayer);
			_changePlayerThread.Start();
		}

		public AuthData AuthData
		{
			get
			{
				return new AuthData("tester", "");
			}
		}

		public int CurrentGameStep
		{
			get
			{
				return _history.Count - 1;
			}
		}

		public PlayerState State
		{
			get
			{
				return PlayerState.Game;
			}
		}

		public bool IsMyTurn
		{
			get
			{
				return _currentPlayer == AuthData.Login;
			}
		}

		public string CurrentTurnPlayer
		{
			get
			{
				return _currentPlayer;
			}
		}

		public GameState CurrentGameState
		{
			get
			{
				return new GameState(_currentPlayer, false);
			}
		}

		public bool Auth()
		{
			_stateMachine.ApplyCommandType(CommandType.Auth);
			return true;
		}

		public bool CreateLobby(string lobbyName)
		{
			if (_lobbies.Contains(lobbyName)) return false;
			_lobbies.Add(lobbyName);
			return true;
		}

		public void Dispose()
		{
		}

		private PlayerCardSet _cardSet;
		public List<Card> GetCards()
		{
			return _cardSet.ToList();
		}

		private GameHistory _history = new GameHistory();
		public GameMove[] GetHistory(int fromIndex)
		{
			return _history.ToArray(fromIndex);
		}

		public GameMove[] GetHistory()
		{
			return GetHistory(CurrentGameStep);
		}

		public List<string> GetLobbies()
		{
			return _lobbies;
		}

		public bool JoinLobby(string lobbyName)
		{
			if (!_lobbies.Contains(lobbyName))
				return false;

			_stateMachine.ApplyCommandType(CommandType.JoinLobby);
			return true;
		}

		public bool Leave()
		{
			_stateMachine.ApplyCommandType(CommandType.Leave);
			return true;
		}

		public bool MakeMove(List<Card> cards)
		{
			var index = _history.Last().Index + 1;
			var move = new GameMove()
			{
				Index = index,
				MoveType = MoveType.Drop,
				Cards = cards
			};
			_history.Add(move);
			_cardSet.Extract(cards);
			return true;
		}

		public bool Pass()
		{
			var index = _history.Last().Index + 1;
			var move = new GameMove()
			{
				Index = index,
				MoveType = MoveType.Pass
			};
			_history.Add(move);
			return true;
		}

		public bool Register()
		{
			_stateMachine.ApplyCommandType(CommandType.Register);
			return true;
		}

		public bool StartGame()
		{
			_stateMachine.ApplyCommandType(CommandType.Start);
			_stateMachine.ApplyCommandType(CommandType.JoinGame);
			return true;
		}

		public bool Update()
		{
			return true;
		}

		public List<string> GetPlayers()
		{
			var list = new List<string>();
			for (int i = 0; i < 8; i++)
			{
				list.Add("Player" + i);
			}
			list.Add(this.AuthData.Login);
			return list;
		}

		public StatisticRecord[] GetStatistics()
		{
			var scores = new StatisticRecord[8 + 1];
			var rand = new Random(5);
			for (int i = 0; i < 8; i++)
			{
				scores[i] = new StatisticRecord("Player" + i);
				scores[i].FinishWithDonkey = Convert.ToUInt32(rand.Next(100));
				scores[i].GamesPlayed = Convert.ToUInt32(rand.Next(100));
				scores[i].TotalScore = Convert.ToUInt32(rand.Next(100));
			}
			scores[8] = new StatisticRecord(this.AuthData.Login);
			scores[8].FinishWithDonkey = Convert.ToUInt32(rand.Next(100));
			scores[8].GamesPlayed = Convert.ToUInt32(rand.Next(100));
			scores[8].TotalScore = Convert.ToUInt32(rand.Next(100));
			return scores;
		}

		private void RotatePlayer()
		{
			var counter = 0;
			var players = GetPlayers();
			while (true)
			{
				Thread.Sleep(5000);
				_currentPlayer = players[counter++];
				counter = counter % players.Count;
			}
		}

		public LobbyState GetLobbyState()
		{
			var p1 = new PlayerInLobbyDescription("test_name", PlayerType.Human);
			var p2 = new PlayerInLobbyDescription("not_ready_yet", PlayerType.AI);
			var list = new List<PlayerInLobbyDescription> { p1, p2 };

			var result = new LobbyState(AuthData.Login, list);
			return result;
		}
	}
}
