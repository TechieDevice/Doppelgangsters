using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Doppelgangsters.Server
{
    public class Client
    {
        protected internal string username;
        private TcpClient client;
        private NetworkStream stream;
        private Server server;
        private bool connected = true;
        protected internal Room room;
        private bool roomCreator = false;

        //player parametrs        
        private int hp = 2;
        private Class role;

        private int blocked = 0;
        private bool inPrison = false;
        private int poisoned = 0;
        private int Item = 0;

        private bool goSleep = false;
        private string target;


        public Client(TcpClient tcpClient, Server serverObject)
        {
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }


        protected internal void DoAction()
        {











        }

        private void ChangeStats(string message)
        {







        }


        protected internal void Process()
        {
            try
            {
                stream = client.GetStream();

                string startmessage = GetMessage();
                string startcode = startmessage.Substring(0, 2);
                if (startcode != "dc")
                {
                    username = startmessage.Substring(2);
                    server.ServerErrorSendMessage("ok", this);
                    Console.WriteLine($"{this.username} connected");

                    while (connected)
                    {
                        string message = GetMessage();
                        string code;
                        if (message.Length > 2)
                        {
                            code = message.Substring(0, 2);
                        }
                        else
                        {
                            code = message;
                        }
                        switch (code)
                        {
                            case "cs":  //Change Stats
                                {
                                    message = message.Substring(2);
                                    ChangeStats(message);
                                    break;
                                }
                            case "rc":  //Room Connect
                                {
                                    message = message.Substring(2);
                                    server.RoomConnect(this, message);
                                    room.SendMessage($"ok{room.clients}", username);
                                    break;
                                }
                            case "ra":  //Room Add
                                {
                                    server.RoomCreate(this);
                                    roomCreator = true;
                                    room.SendMessage("ok", username);
                                    break;
                                }
                            case "rd":  //Room Disconnect
                                {
                                    server.RoomDisconnect(this, room);
                                    room.SendMessage("ok", username);
                                    break;
                                }
                            case "gs":  //Game Start
                                {
                                    room.GameStart();
                                    room.SendMessageAll("ok");
                                    break;
                                }
                            case "sm":  //Server Message
                                {

                                    break;
                                }
                            case "dc":  //DisConnect
                                {
                                    connected = false;
                                    break;
                                }
                        }
                    }
                }
                Console.WriteLine($"{this.username} leave");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                server.RemoveConnection(this);
                Close();
            }
        }

        protected internal void Send(byte[] data)
        {
            stream.Write(data, 0, data.Length);
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

                string message = builder.ToString();

                if (message == "") return "dc";
                return message;
            }
            catch (Exception)
            {
                return "dc";
            }
        }

        protected internal void Close()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
