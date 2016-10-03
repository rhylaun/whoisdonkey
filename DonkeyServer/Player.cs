using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Donkey.Common.ClientServer;
using Donkey.Common.Commands;
using Donkey.Common;
using Donkey.Common.Answers;

namespace Donkey.Server
{
    public class Player
    {
        public AuthData AuthData { get; private set; }
        private readonly PlayerStateMachine _stateMachine = new PlayerStateMachine();

        public PlayerState State
        {
            get
            {
                return _stateMachine.CurrentState;
            }
        }

        public Player(AuthData authData)
        {
            AuthData = authData;            
        }

        public void ApplyCommand(ClientCommand command)
        {
            ApplyCommand(command.CommandType);
        }

		public void ApplyCommand(CommandType command)
		{
			_stateMachine.ApplyCommandType(command);
		}

        public bool CanApplyCommand(ClientCommand command)
        {
            return CanApplyCommand(command.CommandType);
        }

		public bool CanApplyCommand(CommandType command)
		{
			return _stateMachine.CanApplyCommandType(command);
		}

        public bool JoinGame()
        {
            if (!_stateMachine.CanApplyCommandType(CommandType.JoinGame))
                return false;

            _stateMachine.ApplyCommandType(CommandType.JoinGame);
            return true;
        }
    }
}
