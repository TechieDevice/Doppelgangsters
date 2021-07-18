using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Doppelgangsters.Server
{
    class Program
    {
        private static Server server;
        static Thread listenThread;
        //private static DB DateBase;

        static void Main(string[] args)
        {
            try
            {
                //DateBase = new DB();
                //DateBase.Connect();

                server = new Server();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //start
            }
            catch (Exception ex)
            {
                server.Disconnect(/*DateBase*/);
                Console.WriteLine(ex.Message);
            }
        }


    }
}
