using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Server
{
    public class Lobby
    {
        private readonly object _locker = new object();
        private readonly List<Player> _players = new List<Player>();

        public string Name { get; private set; }
        public bool Ready
        {
            get
            {
                lock (_locker)
                {
                    return _players.All(x => x.State == Common.PlayerState.Ready);
                }
            }
        }

        public Lobby(string name)
        {
            Name = name;
        }

        public void AddPlayer(Player player)
        {
            lock (_locker)
                _players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            lock (_locker)
                _players.Remove(player);
        }

        public IEnumerable<Player> GetPlayers()
        {
            lock (_locker)
                return _players.ToList();
        }
    }
}
