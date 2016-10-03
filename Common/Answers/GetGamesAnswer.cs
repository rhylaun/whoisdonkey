using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
    [Serializable]
    public class GetGamesAnswer : ServerAnswer
    {
        public string[] Games;

        public GetGamesAnswer(bool success)
            : base(success)
        {
        }
    }
}
