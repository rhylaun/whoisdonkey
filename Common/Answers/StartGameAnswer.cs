using System;

namespace Donkey.Common.Answers
{
	[Serializable]
    public class StartGameAnswer : ServerAnswer
    {
        public StartGameAnswer(bool success)
            : base(success)
        {
        }
    }
}
