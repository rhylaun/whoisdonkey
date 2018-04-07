using System;

namespace Donkey.Common
{
	[AttributeUsage(AttributeTargets.Class)]
    public class ConsoleCommandInfoAttribute : Attribute
    {
        public string ConsoleLine { get; set; }
    }
}
