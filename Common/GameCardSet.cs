using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common
{
    public class GameCardSet
    {
        private readonly PlayerCardSet[] _cardsetArray;
        public readonly DateTime Date = DateTime.UtcNow;

        private AuthData[] _players;

        public GameCardSet(IEnumerable<PlayerCardSet> cardSet)
        {
            _cardsetArray = cardSet.ToArray();
        }

        public PlayerCardSet GetPlayerCardset(int index)
        {
            return _cardsetArray[index];
        }

        public PlayerCardSet GetPlayerCardset(AuthData authData)
        {
            int index = -1;
            for (int i = 0; i < _cardsetArray.Length; i++)
                if (_players[i].Equals(authData))
                {
                    index = i;
                    break;
                }
            return GetPlayerCardset(index);
        }

        public void BindPlayers(AuthData[] players)
        {
            if (players.Length != _cardsetArray.Length)
                throw new ArgumentException("Количество игроков не соответствует количеству колод", "players");

            _players = players;
        }

    }
}
