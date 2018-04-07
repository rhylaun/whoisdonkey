using System;

namespace Donkey.Common.Answers
{
	[Serializable]
    public class GetGamesAnswer : ServerAnswer
    {
        public string[] Games;

        public GetGamesAnswer(bool success)
            : base(success)
        {
        }
    }
}
