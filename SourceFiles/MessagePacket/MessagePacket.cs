using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace MessagePacket
{
    [Serializable()]
    public enum MessageType : byte
    {
        Select, InsertUpdate, Delete, OK, Error
    }

    [Serializable()]
    public enum DBType : byte
    {
        Books, Authors
    }

    [Serializable()]
    public class MessageQueryPacket
    {
        public byte[] Content;
        public MessageType Type;
        public DBType DBType;
        public static byte[] ToByte(MessageQueryPacket mqp)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, mqp);
                return ms.ToArray();
            }
        }

        public static MessageQueryPacket ToMessagePacket(byte[] buff)
        {
            using (var ms = new MemoryStream(buff))
            {
                return (MessageQueryPacket)new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}
