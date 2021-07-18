using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Doppelgangsters
{
    public class MobileClient
    {
        //server parametrs
        private const string host = "172.31.96.1";
        private const int port = 44276;

        //client parametrs
        private readonly TcpClient client;
        private NetworkStream stream;
        protected internal readonly string username;
        protected internal string roomId;
        protected internal bool isRoomCreator = false;

        //player parametrs        
        private int hp = 2;
        //private Class role;

        private int blocked = 0;
        private bool inPrison = false;
        private int poisoned = 0;
        private int Item = 0;

        private bool goSleep = false;
        private string target;


        public MobileClient(string userName)
        {
            client = new TcpClient();
            username = userName;
        }

        public async Task Connection()
        {
            client.Connect(host, port);
            stream = client.GetStream();

            string data = $"uj{username}";
            Send(data);

            string message = await GetMessage();
            if (message != "erok")
            {
                throw new Exception();
            }
        }

        public void Disconnection()
        {
            string data = $"dc";
            Send(data);

            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }

        public async Task RoomCreate()
        {
            var data = "ra";
            Send(data);

            string message = await GetMessage();
            if (message != "ok")
            {
                throw new Exception();
            }

            isRoomCreator = true;
        }

        public async Task RoomConnect(string roomId)
        {
            var data = $"rc{roomId}";
            Send(data);

            string message = await GetMessage();
            if (message != "ok")
            {
                throw new Exception();
            }
        }

        public async Task RoomDisconnect()
        {
            string data = $"rd";
            Send(data);

            string message = await GetMessage();
            if (message != "ok")
            {
                throw new Exception();
            }
        }

        private void Send(string data)
        {
            byte[] sendData = Encoding.Unicode.GetBytes(data);
            stream.Write(sendData, 0, sendData.Length);
        }

        private async Task<string> GetMessage()
        {
            try
            {
                byte[] resData = new byte[64];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = await stream.ReadAsync(resData, 0, resData.Length);
                    builder.Append(Encoding.Unicode.GetString(resData, 0, bytes));
                }
                while (stream.DataAvailable);

                return builder.ToString();
            }
            catch
            {
                return "dc";
            }
        }
    }
}
