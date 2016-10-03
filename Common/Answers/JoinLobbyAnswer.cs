using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
    [Serializable]
    public class JoinLobbyAnswer : ServerAnswer
    {
        public JoinLobbyAnswer(bool success)
            : base(success)
        {
        }
    }
}
