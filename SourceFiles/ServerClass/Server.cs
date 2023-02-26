using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Data.OleDb;
using System.Runtime.Serialization.Formatters.Binary;
using BookContext = Context.Context;
using Models;
using System.Data;
using MessagePacket;
using System.Data.Entity.Migrations;
using ModelsDTO;
using System.Windows;

namespace Server
{
    public class TCP_Server : IDisposable
    {
        public readonly Socket Server;
        private readonly EndPoint LocalPoint;
        private readonly Mutex mutex;
        public TCP_Server()
        {
            if (Mutex.TryOpenExisting("DBAdminMutex", out Mutex m) == true)
            {
                throw new Exception("You can't deploy server while the server is working! Turn it off and try again");
            }
            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            LocalPoint = new IPEndPoint(IPAddress.Parse(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString()), 45000);
            try
            {
                Server.Bind(LocalPoint);
                Server.Listen(0);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Server.Dispose();
                return;
            }
            mutex = new Mutex(false, "DBServerMutex");
            Server.ReceiveBufferSize = 2_000_000_000;
            Server.SendBufferSize = 2_000_000_000;
        }

        public async void Listen()
        {
            try
            {
                while (true)
                {
                    var client = await Server.AcceptAsync();
                    Console.WriteLine($"User connected: {client.RemoteEndPoint}");
                    Task.Run(() => Serve(client));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void Serve(Socket client)
        {
            MessageQueryPacket mqp = null;
            try
            {
                while (true)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        while (client.Available == 0)
                        { }
                        do
                        {
                            byte[] buff = new byte[1024];
                            var q = await client.ReceiveAsync(new ArraySegment<byte>(buff), SocketFlags.None);
                            ms.Write(buff, 0, q);
                        } while (client.Available > 0);
                        mqp = MessageQueryPacket.ToMessagePacket(ms.ToArray());
                    }
                    using (var context = new BookContext())
                    {
                        Console.WriteLine($"{Enum.GetName(typeof(MessageType),mqp.Type)} request to {Enum.GetName(typeof(DBType), mqp.DBType)} database from {client.RemoteEndPoint}");
                        switch (mqp.Type)
                        {
                            case MessageType.InsertUpdate:
                                switch (mqp.DBType)
                                {
                                    case DBType.Books:
                                        var books = (List<BookDTO>)new BinaryFormatter().Deserialize(new MemoryStream(mqp.Content));
                                        foreach (var item in books)
                                        {
                                            var book = Book.FromDTO(item);
                                            List<Author> auths = new List<Author>();
                                            foreach (var item1 in book.Authors)
                                            {
                                                var temp = context.Authors.Where(a => a.Name == item1.Name).FirstOrDefault();
                                                if (temp != null)
                                                    auths.Add(temp);
                                            }
                                            book.Authors = auths;
                                            context.Books.AddOrUpdate(book);
                                        }
                                        break;
                                    case DBType.Authors:
                                        var authors = (List<AuthorDTO>)new BinaryFormatter().Deserialize(new MemoryStream(mqp.Content));
                                        foreach (var item in authors)
                                        {
                                            var auth = Author.FromDTO(item);
                                            List<Book> books1 = new List<Book>();
                                            foreach (var item1 in auth.Books)
                                            {
                                                var temp = context.Books.Where(b => b.Title == item1.Title).FirstOrDefault();
                                                if (temp != null)
                                                    books1.Add(temp);
                                            }
                                            auth.Books = books1;
                                            context.Authors.AddOrUpdate(auth);
                                        }
                                        break;
                                }
                                await context.SaveChangesAsync();
                                await client.SendAsync(new ArraySegment<byte>(MessageQueryPacket.ToByte(new MessageQueryPacket() { DBType = mqp.DBType, Type = MessageType.OK })), SocketFlags.None);
                                break;
                            case MessageType.Delete:
                                switch (mqp.DBType)
                                {
                                    case DBType.Books:
                                        var books = (List<BookDTO>)new BinaryFormatter().Deserialize(new MemoryStream(mqp.Content));
                                        books.ForEach(x => context.Books.Remove(context.Books.Find(Book.FromDTO(x).Id)));
                                        break;
                                    case DBType.Authors:
                                        var authors = (List<AuthorDTO>)new BinaryFormatter().Deserialize(new MemoryStream(mqp.Content));
                                        authors.ForEach(x => context.Authors.Remove(context.Authors.Find(Author.FromDTO(x).Id)));
                                        break;
                                }
                                await context.SaveChangesAsync();
                                await client.SendAsync(new ArraySegment<byte>(MessageQueryPacket.ToByte(new MessageQueryPacket() { DBType = mqp.DBType, Type = MessageType.OK })), SocketFlags.None);
                                break;
                            case MessageType.Select:
                                switch (mqp.DBType)
                                {
                                    case DBType.Books:
                                        using (var ms = new MemoryStream())
                                        {
                                            List<BookDTO> temp = new List<BookDTO>();
                                            foreach (var item in context.Books)
                                            {
                                                temp.Add(Book.ToDTO(item));
                                            }
                                            new BinaryFormatter().Serialize(ms, temp);
                                            await client.SendAsync(new ArraySegment<byte>(MessageQueryPacket.ToByte(new MessageQueryPacket() { Content = ms.ToArray(), Type = mqp.Type, DBType = mqp.DBType })), SocketFlags.None);
                                        }
                                        break;
                                    case DBType.Authors:
                                        using (var ms = new MemoryStream())
                                        {
                                            List<AuthorDTO> temp = new List<AuthorDTO>();
                                            foreach (var item in context.Authors)
                                            {
                                                temp.Add(Author.ToDTO(item));
                                            }
                                            new BinaryFormatter().Serialize(ms, temp);
                                            await client.SendAsync(new ArraySegment<byte>(MessageQueryPacket.ToByte(new MessageQueryPacket() { Content = ms.ToArray(), Type = mqp.Type, DBType = mqp.DBType })), SocketFlags.None);
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var packet = new MessageQueryPacket() { Content = Encoding.UTF8.GetBytes(ex.Message), Type = MessageType.Error };
                if(mqp!=null)
                {
                    packet.DBType = mqp.DBType;
                }
                await client.SendAsync(new ArraySegment<byte>(MessageQueryPacket.ToByte(packet)), SocketFlags.None);
            }
        }


        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
