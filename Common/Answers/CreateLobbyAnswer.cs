using System;

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
