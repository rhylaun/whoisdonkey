using System;

namespace Donkey.Common
{
	public class GameServerException : Exception
    {
        public GameServerException(string message)
            : base(message)
        {
        }
    }
}
