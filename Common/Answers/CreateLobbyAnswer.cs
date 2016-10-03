using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
    [Serializable]
    public class CreateLobbyAnswer : ServerAnswer
    {
        public CreateLobbyAnswer(bool success)
            : base(success)
        {
        }
    }
}
