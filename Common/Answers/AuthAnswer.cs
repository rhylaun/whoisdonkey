using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
    [Serializable]
    public class AuthAnswer : ServerAnswer
    {
        public AuthAnswer(bool success)
            : base(success)
        {
        }
    }
}
