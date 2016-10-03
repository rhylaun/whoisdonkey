﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.Answers
{
    [Serializable]
    public class StartGameAnswer : ServerAnswer
    {
        public StartGameAnswer(bool success)
            : base(success)
        {
        }
    }
}
