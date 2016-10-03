using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;

namespace Donkey.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterClientCommandAttribute : Attribute
    {
        public CommandType CommandType { get; set; }
        public bool NeedAuth { get; set; }
    }
}
