using System;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class GetStateAnswer : ServerAnswer
	{
		public readonly PlayerState State;

		public GetStateAnswer(bool success, PlayerState state)
			: base(success)
		{
			State = state;
		}
	}
}
