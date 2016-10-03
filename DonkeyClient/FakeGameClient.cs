using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common;
using Donkey.Common.Commands;

namespace Donkey.Client
{
    public class FakeGameClient : IGameClient
    {
        private readonly PlayerStateMachine _stateMachine = new PlayerStateMachine();
        private readonly List<string> _lobbies = new List<string>();

        public FakeGameClient()
        {
            Auth();
            _lobbies.Add("test_lobby");

            _history.Add(new GameMove() { MoveType = MoveType.Clear, Index = 0, });
            _history.Add(new GameMove() { MoveType = MoveType.Drop, Index = 1, Player = new AuthData("test2", ""), Cards = new List<Card> { Card.One, Card.One, Card.One } });
            _history.Add(new GameMove() { MoveType = MoveType.Pass, Index = 2, Player = new AuthData("test3", ""), });
            _history.Add(new GameMove() { MoveType = MoveType.Clear, Index = 3, });

            _cardSet = new PlayerCardSet();
            _cardSet.Add(new List<Card>() { Card.Donkey, Card.One, Card.Nine, Card.Nine, Card.Five });
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
                return _stateMachine.CurrentState;
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
            if (!_lobbies.Contains(lobbyName)) return false;

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
			return new List<string>() { "p1", "p2", "p3" };
		}
	}
}
