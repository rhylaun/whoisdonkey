using System.Net;
using Donkey.Common.Commands;
using Donkey.Common.Answers;

namespace Donkey.Common.ClientServer
{
    public interface INetworkClient
    {
        bool Connect(IPEndPoint endPoint);
        void Close();
        void Send(object obj);
        object Receive();
    }
}
