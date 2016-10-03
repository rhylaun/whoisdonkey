using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
