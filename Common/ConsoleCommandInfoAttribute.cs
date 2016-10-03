using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConsoleCommandInfoAttribute : Attribute
    {
        public string ConsoleLine { get; set; }
    }
}
