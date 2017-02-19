using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class CurrentPlayerAnswer : ServerAnswer
	{
		public readonly string CurrentPlayerName;

		public CurrentPlayerAnswer(bool success, string currentPlayerName) : base(success)
		{
			CurrentPlayerName = currentPlayerName;
		}
	}
}
