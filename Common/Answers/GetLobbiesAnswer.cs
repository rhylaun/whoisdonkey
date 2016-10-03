using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class GetLobbiesAnswer : ServerAnswer
	{
		public string[] Lobbies;

		public GetLobbiesAnswer(bool success)
			: base(success)
		{
		}
	}
}
