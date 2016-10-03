using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;
using Donkey.Common;

namespace ConsoleClient
{
	internal class Options
	{
		[Option('l', "login", Required=false, HelpText="User login")]
		public string Login { get; set; }

		[Option('p', "password", Required=false, HelpText="User password")]
		public string Password { get; set; }

		[Option('a', "address", Required=false, HelpText="Server address", DefaultValue="127.0.0.1")]
		public string Address { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			var usage = new StringBuilder();
			usage.AppendLine("Who is donkey. Console client.");
			usage.AppendLine("Read user manual for usage instructions...");
			return usage.ToString();
		}

		public AuthData GetAuthData()
		{
			return new AuthData(Login, Password);
		}
	}
}
