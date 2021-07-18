using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace test
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

        public void Connection()
        {
            client.Connect(host, port);
            stream = client.GetStream();

            string data = $"uj{username}";
            byte[] sendData = Encoding.Unicode.GetBytes(data);
            stream.Write(sendData, 0, sendData.Length);

            string message = GetMessage();
            if (message != "erok")
            {
                throw new Exception();
            }
        }

        public void Disconnection()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }

        private string GetMessage()
        {
            try
            {
                byte[] resData = new byte[64];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(resData, 0, resData.Length);
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
