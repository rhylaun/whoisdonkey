using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Donkey.Common.Commands;
using System.Net;

namespace Donkey.Common.ClientServer
{
    public interface IRequestServer
    {
        void Bind(int port);
        event CommandReceivedEventHandler CommandReceived;
    }

    public delegate void CommandReceivedEventHandler(ClientCommand command, INetworkClient client);
}
