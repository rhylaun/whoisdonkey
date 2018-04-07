using System;

namespace Donkey.Common.Answers
{
	[Serializable]
    public class KeepAliveAnswer : ServerAnswer
    {
        public KeepAliveAnswer() : base(true)
        {
        }
    }
}
