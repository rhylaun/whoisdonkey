using System;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class RemoveAIAnswer : ServerAnswer
	{
		public RemoveAIAnswer(bool success)
			: base(success)
		{
		}
	}
}
