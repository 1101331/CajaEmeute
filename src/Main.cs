using System;
using log4net;
using Microsoft.Extensions.Configuration.Xml;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace CajaEmeute
{

    class Program
    {
        
        public static Session mainSession;
       
        static void Main(string[] args)
        {
            
            string username, password;
            // Console.Write("Username: ");
            // username = Console.ReadLine();
            // Console.Write("Password: ");
            // password = Console.ReadLine();
            Program.mainSession = new Session("username", "password", "localhost:2222");
            Frontend.StartupScreen();
            while (true)
            {
                Frontend.Menu();
            }
        }

        static void testInterface()
        {

            // mainSession.CreateTransaction(new Transaction("3231", "Checkeo", 432));
            // mainSession.CreateTransaction(new Transaction("NIGGY", "Checkeo", 432));
            
            // Console.Write(mainSession.debugOut());

            // mainSession.BufferCleanup();

            // Console.Write(mainSession.debugOut());

        }
    }
}
