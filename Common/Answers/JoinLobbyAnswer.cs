using System;

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
