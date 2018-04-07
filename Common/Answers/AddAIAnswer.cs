using System;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class AddAIAnswer : ServerAnswer
	{
		public AddAIAnswer(bool success)
			: base(success)
		{
		}
	}
}
