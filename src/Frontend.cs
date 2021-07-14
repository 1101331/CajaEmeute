using System;

namespace CajaEmeute
{
    class Frontend
    {
        public static void StartupScreen()
        {
            Console.WriteLine("-------====|CAJA EMEUTE|====-------");
            Console.Write("\n\n");
        }

        public static void Menu()
        {
            //Display options
            Console.WriteLine("-------Presione un numero-------");
            Console.WriteLine("1.Debug");
            Console.WriteLine("2.Nueva Transaccion");
            Console.WriteLine("3.Cierre del dia");

            char key = Console.ReadKey().KeyChar;
            Console.Write("\b \b");

            switch (key)
            {
                case '1':
                    DebugSubmenu();
                    break;
                case '2':
                    Program.mainSession.CreateTransaction(new Transaction(Console.ReadLine(), "test", 404));
                    Program.mainSession.BufferCleanup();
                    break;
                case '3':
                    CuadreMenu();
                    break;
                default:
                    break;
            }
        }

        public static void DebugSubmenu()
        {
            Console.WriteLine("----------Debug Menu----------");
            Console.WriteLine("1.Print Buffer");
            Console.WriteLine("2.Deque Last On Buffer");
            
            char key = Console.ReadKey().KeyChar;
            Console.Write("\b \b");

            switch (key)
            {
                case '1':
                    Console.WriteLine(Program.mainSession.debugOut());
                    break;
                case '2':
                    Program.mainSession.DebugSingleBuffer();
                    break;
                default:
                    break;
            }
        }

        public static void TransactionSubmenu()
        {
            string patientID;
            patientID = Console.ReadLine();

            //send request to see pending transactions for patient

            //send transaction

        }

        public static void CuadreMenu()
        {

        }
    }
}