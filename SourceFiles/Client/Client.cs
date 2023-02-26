using MessagePacket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client
{
    public class Client : IDisposable
    {
        private readonly Socket _Client;
        public Client(IPEndPoint server_location)
        {
            _Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                _Client.Connect(server_location);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            _Client.SendBufferSize = 2_000_000_000;
            _Client.ReceiveBufferSize = 2_000_000_000;
        }

        public void Dispose()
        {
            _Client.Disconnect(false);
            _Client.Dispose();
        }

        public async Task<MessageQueryPacket> SendAndReceiveAsync(MessageQueryPacket mqp)
        {
            try
            {
                await _Client.SendAsync(new ArraySegment<byte>(MessageQueryPacket.ToByte(mqp)), SocketFlags.None);
                using (var ms = new MemoryStream())
                {
                    while (_Client.Available == 0)
                    {

                    }
                    do
                    {
                        byte[] buff = new byte[1024];
                        var count = await _Client.ReceiveAsync(new ArraySegment<byte>(buff), SocketFlags.None);
                        ms.Write(buff, 0, count);
                    }
                    while (_Client.Available > 0);
                    return MessageQueryPacket.ToMessagePacket(ms.ToArray());
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
