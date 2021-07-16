using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace CajaEmeute
{
    class connection
    {
        static byte[] bytes;
                
        public connection(string x) //pls parametrizar el ip y el puerto
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            
            bytes = new byte[1024];
            connectionTest(host, ipAddress, remoteEP, x);
        }

        public static byte[] test(string x)
        {
            string name;
            string pass;
            Console.WriteLine("Ingrese su nombre de usuario: ");
            name = Console.ReadLine();
            Console.WriteLine("Ingrese su contraseña: ");
            pass = Console.ReadLine();
            byte[] ToEncode = Encoding.ASCII.GetBytes(x + "|" + name + "|" + pass + "|" + "\0");
            return ToEncode;
        }

        public static void connectionTest(IPHostEntry host, IPAddress ipAddress, IPEndPoint remoteEP, string x) //IPHostEntry host, IPAddress ipAddress, IPEndPoint remoteEP
        {
            //IPHostEntry host = Dns.GetHostEntry("localhost");
            //IPAddress ipAddress = host.AddressList[0];
            //IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            
            Socket sender = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miPC = new IPEndPoint(ipAddress, 11005);
            
            sender.Bind(miPC);
            try
            {
                // conectándose al end point remoto  
                sender.Connect(remoteEP);

                Console.WriteLine("Socket conectado a la IP: {0}",
                    sender.RemoteEndPoint.ToString());

                //byte[] ToEncode = Encoding.ASCII.GetBytes("Conn\0");//encoding the message

                int bytesSent = sender.Send(test(x)); //send the message

                //receive the answer of the server  
                int bytesRec = sender.Receive(bytes);
                Console.WriteLine( bytesRec + "Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (Exception e)
            {
                //Console.WriteLine("Unexpected exception : {0}", e.ToString());
                Session.log.Error("Unexpected exception : {0}", e);
            }
        }

        public string SendTransaction(Transaction t) //returns server answer
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            Socket sender = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
            byte[] received = new byte[1024];

            bytes = Encoding.ASCII.GetBytes(t.debugOut() + "\0");

            try
            {
                sender.Connect(remoteEP);
                sender.Send(bytes);
                sender.Receive(received);
            }
            catch (Exception)
            {
                throw;
            }
            return Encoding.ASCII.GetString(received);
        }
    }
}