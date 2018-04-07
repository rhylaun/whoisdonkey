using System;
using Donkey.Common.Commands;

namespace Donkey.Server
{
	[AttributeUsage(AttributeTargets.Class)]
    public class CommandProcessorInfoAttribute : Attribute
    {
        public CommandType CommandType { get; set; }
    }
}
