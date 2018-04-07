using System;

namespace Donkey.Common.Answers
{
	[Serializable]
    public class GetCardSetAnswer : ServerAnswer
    {
        public readonly PlayerCardSet CardSet;

        public GetCardSetAnswer(bool success, PlayerCardSet cardSet)
            : base(success)
        {
            CardSet = cardSet;
        }
    }
}
