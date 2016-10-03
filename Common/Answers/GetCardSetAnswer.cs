using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
