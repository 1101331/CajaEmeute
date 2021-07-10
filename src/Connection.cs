using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace CajaEmeute
{
    class connection
    {
        public static DateTime issueDate;
        public static String pacientID;
        public static String operationType;
        public static int monetaryAmount;

        static byte[] bytes = new byte[1024];
                
        public connection(string _pacientID, string _operationType, int _monetaryAmount)
        {
            issueDate = DateTime.Now;
            pacientID = _pacientID;
            operationType = _operationType;
            monetaryAmount = _monetaryAmount;
        }

        public static void connectionTest()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            
            Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

             try
                {
                    // conectándose al end point remoto  
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket conectado a la IP: {0}",
                        sender.RemoteEndPoint.ToString());

                    byte[] ToEncode = Encoding.ASCII.GetBytes("Suck my dick.. \0");//encoding the message

                    int bytesSent = sender.Send(ToEncode); //send the message

                    //receive the answer of the server  
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                       
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }


        }
    }
}