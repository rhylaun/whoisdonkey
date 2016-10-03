using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
    [Serializable]
    public class KeepAliveAnswer : ServerAnswer
    {
        public KeepAliveAnswer() : base(true)
        {
        }
    }
}
