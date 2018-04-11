using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donkey.Common.AI
{
	[AttributeUsage(AttributeTargets.Class)]
	public class AIModuleAttribute : Attribute
	{
		public string Name { get; set; }
	}
}
