using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server;
namespace DBServerConsole
{
    internal class Program
    {
        static TCP_Server Server;
        static void Main(string[] args)
        {
            Server = new TCP_Server();
            Console.WriteLine("Server is bound to: " + Server.Server.LocalEndPoint.ToString());
            Server.Listen();
            Console.ReadLine();
        }
    }
}
