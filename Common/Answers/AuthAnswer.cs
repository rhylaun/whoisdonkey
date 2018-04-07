using System;

namespace Donkey.Common.Answers
{
	[Serializable]
    public class AuthAnswer : ServerAnswer
    {
        public AuthAnswer(bool success)
            : base(success)
        {
        }
    }
}
