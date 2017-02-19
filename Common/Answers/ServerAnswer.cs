using System;

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
