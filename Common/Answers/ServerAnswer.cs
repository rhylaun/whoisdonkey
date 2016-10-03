using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
	[Serializable]
	public class ServerAnswer
	{
		public readonly bool Success;

		public ServerAnswer(bool success)
		{
			Success = success;
		}
	}
}
