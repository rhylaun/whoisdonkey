using System;

namespace Donkey.Common.Answers
{
	[Serializable]
    public class LeaveAnswer : ServerAnswer
    {
        public LeaveAnswer(bool success)
            : base(success)
        {
        }
    }
}
