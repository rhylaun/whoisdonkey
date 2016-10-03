using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;

namespace Donkey.Common
{
    public class PlayerStateMachine
    {
        private readonly object _locker = new object();

        private PlayerState _currentState;
        public PlayerState CurrentState
        {
            get
            {
                lock (_locker)
                {
                    return _currentState;

                }
            }
        }

        public PlayerStateMachine()
        {
            _currentState = PlayerState.Offline;
        }

        public void ApplyCommandType(CommandType commandType)
        {
            lock (_locker)
                _currentState = PlayerStateMatrix.Resolve(CurrentState, commandType);
        }

        public bool CanApplyCommandType(CommandType commandType)
        {
            lock (_locker)
            {
                var resolvedState = PlayerStateMatrix.Resolve(CurrentState, commandType);
                return resolvedState != PlayerState.Error;
            }
        }
    }

    public enum PlayerState
    {
        Error,
        Offline,
        Online,
        Lobby,
        Ready,
        Game
    }
}
