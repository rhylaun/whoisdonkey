using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
    [Serializable]
    public class GameMoveAnswer : ServerAnswer
    {
        public GameMoveAnswer(bool success)
            : base(success)
        {
        }
    }
}
