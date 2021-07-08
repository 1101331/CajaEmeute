using System;

namespace CajaEmeute
{
    class Program
    {
        static void Main(string[] args)
        {
            testInterface();
        }

        static void testInterface()
        {
            string username, password;
            Session mainSession;

            Console.Write("Username: ");
            username = Console.ReadLine();
            Console.Write("Password: ");
            password = Console.ReadLine();

            mainSession = new Session(username, password, "localhost:2222");
            mainSession.CreateTransaction(new Transaction("3231", "Checkeo", 432));
            mainSession.CreateTransaction(new Transaction("3231", "Checkeo", 432));
            Console.Write(mainSession.debugOut());


        }
    }
}
