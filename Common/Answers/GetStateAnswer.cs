using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
