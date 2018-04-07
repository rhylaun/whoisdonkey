using System;

namespace Donkey.Common.Commands
{
	[Serializable]
	[RegisterClientCommand(CommandType = CommandType.GetServerInfo, NeedAuth = true)]
	public class GetServerInfoCommand : ClientCommand
	{
		public GetServerInfoCommand(AuthData authdata)
			: base(authdata, CommandType.GetServerInfo)
		{
		}
	}
}
