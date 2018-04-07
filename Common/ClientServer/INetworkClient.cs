using System.Net;

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
