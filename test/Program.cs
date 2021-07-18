using System;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            MobileClient client = new MobileClient("vasya");
            client.Connection();
            Console.ReadKey();
        }
    }
}
