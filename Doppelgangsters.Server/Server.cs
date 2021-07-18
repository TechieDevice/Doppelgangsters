using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Text;

namespace Doppelgangsters.Server
{
    public class Server
    {
        private static readonly TcpListener tcpListener; // server
        public List<Client> clients = new List<Client>(); // all connections
        public List<Room> rooms; //active game rooms

        static Server()
        {
            tcpListener = new TcpListener(IPAddress.Any, 44276);
        }

        protected internal void AddConnection(Client client)
        {
            if (client != null)
                clients.Add(client);
        }

        protected internal void RemoveConnection(Client client)
        {
            if (client != null)
                clients.Remove(client);
        }

        protected internal void RoomCreate(Client client)
        {
            try
            {
                // create room
                Room room = new Room();
                // remove connection
                room.RoomConnect(client);
            }
            catch
            {
                Console.WriteLine($"{client.username} fail to create room");
                ServerErrorSendMessage("Неверный id комнаты", client);
            }
        }

        protected internal void RoomConnect(Client client, string roomId)
        {
            try
            {
                // get room
                Room room = rooms.FirstOrDefault(r => r.roomId == roomId);
                if (room == null)
                {
                    Console.WriteLine($"{client.username} fail to connect to {roomId}");
                    ServerErrorSendMessage("Неверный id комнаты", client);
                }
                // connect to room
                room.RoomConnect(client);
            }
            catch
            {
                Console.WriteLine($"{client.username} fail to connect to {roomId}");
                ServerErrorSendMessage("Не удалось подключиться к комнате", client);
            }
        }

        protected internal void RoomDisconnect(Client client, Room room)
        {
            try { room.RoomDisconnect(client); }
            finally { Console.WriteLine($"{client.username} disconnect from {room.roomId}"); }

            if (room.clients.Count == 0)
            {
                try { rooms.Remove(room); }
                finally { Console.WriteLine($"{room.roomId} was deleted"); }
            }
        }

        // listen connections
        protected internal void Listen()
        {
            try
            {
                tcpListener.Start();
                Console.WriteLine("Server started");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    Client clientObject = new Client(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // send message to client
        protected internal void ServerErrorSendMessage(string data, Client client)
        {
            byte[] bytes = Encoding.Unicode.GetBytes($"er{data}");
            client.Send(bytes);
        }

        // disconnect all connections
        protected internal void Disconnect(/*DB DateBase*/)
        {
            tcpListener.Stop(); //server stop

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //close user connection
            }

            //DateBase.Close();
            Environment.Exit(0); 
        }
    }
}
