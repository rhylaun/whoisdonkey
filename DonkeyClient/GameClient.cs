using System;
using System.Collections.Generic;
using Donkey.Common;
using System.Net;
using Donkey.Common.ClientServer;
using Donkey.Common.Commands;
using Donkey.Common.Answers;
using System.Threading;

namespace Donkey.Client
{
	public class GameClient<TNetworkClient> : IGameClient where TNetworkClient : INetworkClient, new()
	{
		private readonly object _checkLocker = new object();

		private readonly IPEndPoint _endPoint;
		private CommandSender<TNetworkClient> _commandSender;
		private readonly GameHistory _history = new GameHistory();

		private readonly Thread _checkStateThread;

		private PlayerCardSet _cardSet;
		private GameState _currentGameState;

		public string LobbyName { get; private set; }

		public AuthData AuthData { get; private set; }
		private PlayerState _state;
		public PlayerState State
		{
			get
			{
				lock (_checkLocker)
					return _state;
			}
			private set
			{
				lock (_checkLocker)
					_state = value;
			}
		}

		public int CurrentGameStep
		{
			get
			{
				if (State != PlayerState.Game)
					return -1;
				if (_history.Count == 0)
					return -1;
				return _history.Last().Index;
			}
		}

		public bool IsMyTurn
		{
			get
			{
				lock (_checkLocker)
				{
					return AuthData.Login.Equals(_currentGameState.ActivePlayerName, StringComparison.InvariantCulture);
				}
			}
		}

		public GameState CurrentGameState
		{
			get
			{
				lock (_checkLocker)
				{
					return _currentGameState;
				}
			}
		}

		public GameClient(AuthData authData, string serverAddress, int port = Defaults.Port)
		{
			AuthData = authData;
			_endPoint = new IPEndPoint(IPAddress.Parse(serverAddress), port);
			State = PlayerState.Offline;
			_commandSender = new CommandSender<TNetworkClient>(_endPoint);
			InitGameState();
			_checkStateThread = new Thread(CheckStateRoutine)
			{
				IsBackground = true
			};
			_checkStateThread.Start();

		}

		private void InitGameState()
		{
			_history.Clear();
			_cardSet = new PlayerCardSet();
		}

		private void CheckStateRoutine()
		{
			while (true)
			{
				try
				{
					Thread.Sleep(1000);
					if (!CheckState())
						continue;

					if (State != PlayerState.Game)
						continue;

					CheckCurrentPlayer();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error in checking thread: " + ex.Message);
				}
			}
		}

		private ServerAnswer SendCommand(ClientCommand command)
		{
			lock (_checkLocker)
			{
				var result = _commandSender.Request(command);
				return result;
			}
		}

		private bool ExecuteAndCheckState(ClientCommand command)
		{
			var result = SendCommand(command).Success;
			if (!result) return false;
			return CheckState();
		}

		private bool CheckState()
		{
			var stateBeforeCheck = State;
			var command = new GetStateCommand(AuthData);
			var answer = SendCommand(command);
			if (answer.Success)
				State = ((GetStateAnswer)answer).State;

			var shouldInitGameState = stateBeforeCheck != PlayerState.Game && State == PlayerState.Game;
			if (shouldInitGameState)
				InitGameState();

			return answer.Success;
		}

		private bool CheckCurrentPlayer()
		{
			var command = new GetCurrentGameStateCommand(AuthData);
			var answer = SendCommand(command);
			if (answer.Success)
				_currentGameState = ((CurrentGameStateAnswer)answer).GameState;
			return answer.Success;
		}

		public List<Card> GetCards()
		{
			if (_cardSet == null)
				return new List<Card>();

			return _cardSet.ToList();
		}

		public void Dispose()
		{
			_checkStateThread.Interrupt();
		}

		public bool Update()
		{
			if (!CheckState())
				return false;

			if (State != PlayerState.Game)
				return false;

			var getHistoryCommand = new GetHistoryCommand(AuthData, CurrentGameStep);
			var historyResult = SendCommand(getHistoryCommand);
			if (!historyResult.Success)
				return false;
			var moveArray = ((GetHistoryAnswer)historyResult).History;
			foreach (var move in moveArray)
			{
				if (move.Index <= CurrentGameStep)
					continue;

				_history.Add(move);
				ProcessMove(move);
			}
			return true;
		}

		private void ProcessMove(GameMove move)
		{
			if (move.PlayerName != null && !move.PlayerName.Equals(AuthData))
				return;

			if (move.MoveType == MoveType.Drop)
				_cardSet.Extract(move.Cards);

			if (move.MoveType == MoveType.Take)
				_cardSet.Add(move.Cards);
		}

		public GameMove[] GetHistory(int fromIndex)
		{
			if (_history.Count - 1 <= fromIndex)
				Update();
			return _history.ToArray(fromIndex);
		}

		public bool Register()
		{
			var command = new RegisterCommand(AuthData);
			return SendCommand(command).Success;
		}

		public bool Auth()
		{
			var command = new AuthCommand(AuthData);
			return ExecuteAndCheckState(command);
		}

		public bool JoinLobby(string lobbyName)
		{
			var command = new JoinLobbyCommand(AuthData, lobbyName);
			var result = ExecuteAndCheckState(command);
			if (result)
				LobbyName = lobbyName;
			return result;
		}

		public bool CreateLobby(string lobbyName)
		{
			var command = new CreateLobbyCommand(AuthData, lobbyName);
			var result = ExecuteAndCheckState(command);
			if (result)
				LobbyName = lobbyName;
			return result;
		}

		public List<string> GetLobbies()
		{
			var command = new GetLobbiesCommand(AuthData);
			var answer = SendCommand(command);
			if (answer.Success)
				return new List<string>(((GetLobbiesAnswer)answer).Lobbies);

			return new List<string>();
		}

		public bool Leave()
		{
			var command = new LeaveCommand(AuthData);
			return ExecuteAndCheckState(command);
		}

		public bool StartGame()
		{
			var command = new StartGameCommand(AuthData);
			return ExecuteAndCheckState(command);
		}

		public bool Pass()
		{
			var command = new GameMoveCommand(AuthData, MoveType.Pass, new List<Card>());
			return SendCommand(command).Success;
		}

		public bool MakeMove(List<Card> cards)
		{
			var command = new GameMoveCommand(AuthData, MoveType.Drop, cards);
			return SendCommand(command).Success;
		}

		public GameMove[] GetHistory()
		{
			return GetHistory(CurrentGameStep);
		}

		public List<string> GetPlayers()
		{
			var command = new GetPlayersCommand(AuthData);
			var result = SendCommand(command);
			if (result.Success)
				return new List<string>(((GetPlayersAnswer)result).Players);
			return new List<string>();
		}

		public StatisticRecord[] GetStatistics()
		{
			var command = new GetStatisticCommand(AuthData);
			var result = SendCommand(command);
			if (result.Success)
				return ((GetStatisticAnswer)result).Statistic;
			return new StatisticRecord[0];
		}

		public LobbyState GetLobbyState()
		{
			if (string.IsNullOrEmpty(LobbyName))
				return new LobbyState("", null);

			var command = new GetLobbyStateCommand(AuthData, LobbyName);
			var result = SendCommand(command);
			if (result.Success)
				return (((GetLobbyStateAnswer)result).State);
			return new LobbyState("", null);
		}

		public bool AddAI(string botName)
		{
			if (string.IsNullOrEmpty(LobbyName))
				throw new InvalidOperationException("Cannot add AI while not in lobby");

			var command = new AddAICommand(AuthData, LobbyName, botName);
			return SendCommand(command).Success;
		}

		public bool RemoveAI(string botName)
		{
			if (string.IsNullOrEmpty(LobbyName))
				throw new InvalidOperationException("Cannot remove AI while not in lobby");

			var command = new RemoveAICommand(AuthData, LobbyName, botName);
			return SendCommand(command).Success;
		}

		public List<string> GetServerInfo()
		{
			var command = new GetServerInfoCommand(AuthData);
			var result = SendCommand(command);

			if (result.Success)
				return ((GetServerInfoAnswer)result).BotNames;
			return new List<string>();
		}
	}
}
