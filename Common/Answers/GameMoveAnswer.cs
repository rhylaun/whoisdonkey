using System;

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
