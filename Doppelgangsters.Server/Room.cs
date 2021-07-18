using System;
using System.Collections.Generic;
using System.Text;

namespace Doppelgangsters.Server
{
    enum Class
    {
        Captain = 0,
        Security = 1,
        Engineer = 2,
        Trapper = 3,
        Hacker = 4,
        Psyonic = 5,
        Solder = 6,
        Physicist = 7,
        Agent = 8,
        Biologist = 9,
        Medic = 10,
        Operator = 11
    }
    public class Room
    {
        public string roomId;
        public List<Client> clients = new List<Client>(); // players in room

        //game status
        private int hacking = 0;
        private int sos = 0;
        private bool hasCard = false;

        public Room()
        {
            var rand = new Random();
            for (int ctr = 0; ctr <= 4; ctr++)
                roomId = roomId + rand.Next(0, 10).ToString();
        }


        protected internal void GameStart()
        {















        }


        protected internal void RoomConnect(Client client)
        {
            clients.Add(client);
            client.room = this;
        }

        protected internal void RoomDisconnect(Client client)
        {
            clients.Remove(client);
        }

        // send message to all in room
        protected internal void SendMessageAll(string data)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(data);
                clients[i].Send(bytes);
            }
        }

        // send message to target
        protected internal void SendMessage(string data, string username)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(data);
            clients.Find(u => u.username == username).Send(bytes);
        }

        ~Room() { }
    }
}
