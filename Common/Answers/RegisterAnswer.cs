using System;

namespace Donkey.Common.Answers
{
	[Serializable]
    public class RegisterAnswer : ServerAnswer
    {
        public RegisterAnswer(bool success)
            : base(success)
        {
        }
    }
}
