using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using Donkey.Common.Commands;
using Donkey.Common.Answers;
using System.IO;

namespace Donkey.Common.ClientServer
{
    public class GameSerializer
    {
        private readonly BinaryFormatter _serializer = new BinaryFormatter();

        public object ToObject(byte[] buffer)
        {
            using (var memStream = new MemoryStream())
            {
                memStream.Write(buffer, 0, buffer.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = _serializer.Deserialize(memStream);
                return obj;
            }
        }

        public byte[] ToBytes(object obj)
        {
            if (!(obj is ServerAnswer || obj is ClientCommand))
                throw new GameServerException("Invalid object type for serialization");

            using (var memStream = new MemoryStream())
            {
                _serializer.Serialize(memStream, obj);
                var buffer = memStream.ToArray();
                return buffer;
            }
        }
    }
}
