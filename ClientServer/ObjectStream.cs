using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Donkey.ClientServer
{
    public class ObjectStream : IDisposable
    {
        private readonly NetworkStream _netStream;
        private readonly BinaryFormatter _serializer = new BinaryFormatter();

        public ObjectStream(Socket socket)
        {
            _netStream = new NetworkStream(socket, true);
        }

        public bool Send(object obj)
        {
            try
            {
                using(var memStream = new MemoryStream())
                {
                    _serializer.Serialize(memStream, obj);
                    _netStream.Write(BitConverter.GetBytes(memStream.Length), 0, 4);
                    memStream.WriteTo(_netStream);
                }
            }
            catch (SocketException)
            {
                return false;
            }
            return true;
        }

        public object Receive()
        {
            var buffer = new byte[4];
            _netStream.Read(buffer, 0, 4);
            var length = BitConverter.ToInt32(buffer, 0);
            buffer = new byte[length];
            int readed = 0;
            while (readed < length)
                readed += _netStream.Read(buffer, readed, length - readed);
            using (var memStream = new MemoryStream(buffer))
                return _serializer.Deserialize(memStream);
        }

        public void Dispose()
        {
            _netStream.Close();
            _netStream.Dispose();
        }
    }
}
