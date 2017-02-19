using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;

namespace Donkey.Common
{
    public static class PlayerStateMatrix
    {
        private static readonly Dictionary<PlayerState, Dictionary<CommandType, PlayerState>> _array =
            new Dictionary<PlayerState, Dictionary<CommandType, PlayerState>>();

        static PlayerStateMatrix()
        {
            foreach (var state in Enum.GetValues(typeof(PlayerState)).Cast<PlayerState>())
            {
                _array[state] = new Dictionary<CommandType, PlayerState>();

                foreach (var cmd in Enum.GetValues(typeof(CommandType)).Cast<CommandType>())
                    _array[state][cmd] = PlayerState.Error;

                _array[state][CommandType.KeepAlive] = state;  //команда keepalive сохраняет состояние клиента
                _array[state][CommandType.GetState] = state;  //команда getstate сохраняет состояние клиента
                _array[state][CommandType.GetLobbies] = state; //команда getlobbies сохраняет состояние клиента
                _array[state][CommandType.GetPlayers] = state; //команда getplayers сохраняет состояние клиента
                _array[state][CommandType.GetGames] = state;   //команда getgames сохраняет состояние клиента
				_array[state][CommandType.GetCurrentPlayer] = state;   //команда getcurrentplayer сохраняет состояние клиента
			}

            _array[PlayerState.Offline][CommandType.Auth] = PlayerState.Online;
            _array[PlayerState.Offline][CommandType.Register] = PlayerState.Offline;

            _array[PlayerState.Online][CommandType.Leave] = PlayerState.Offline;
            _array[PlayerState.Online][CommandType.CreateLobby] = PlayerState.Lobby;
            _array[PlayerState.Online][CommandType.JoinLobby] = PlayerState.Lobby;

            _array[PlayerState.Lobby][CommandType.Leave] = PlayerState.Online;
            _array[PlayerState.Lobby][CommandType.Start] = PlayerState.Ready;

            _array[PlayerState.Ready][CommandType.Leave] = PlayerState.Online;
            _array[PlayerState.Ready][CommandType.JoinGame] = PlayerState.Game;

            _array[PlayerState.Game][CommandType.Leave] = PlayerState.Online;
            _array[PlayerState.Game][CommandType.GameMove] = PlayerState.Game;
            _array[PlayerState.Game][CommandType.GetCardSet] = PlayerState.Game;
            _array[PlayerState.Game][CommandType.GetHistory] = PlayerState.Game;
        }

        public static PlayerState Resolve(PlayerState state, CommandType commandType)
        {
            return _array[state][commandType];
        }


    }
}
