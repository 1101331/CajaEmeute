using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CajaEmeute
{
    class Session
    {
        private string username;
        private string password;
        private string connexion;
        private TransactionBuffer buffer;

        private DateTime sessionStart;

        public Session(string _username, string _password, string _connexion)
        {
            buffer = new TransactionBuffer(20);
            username = _username;
            password = _password; //Storing password in plain text !!TEMP MEASURE!!!
            connexion = _connexion;
            //add log here for conn status
            sessionStart = DateTime.Now;
        }

        public void CreateTransaction(Transaction t)
        {
            buffer.AddTransactionToBuffer(t);
            buffer.AddToTextBuffer(t);
        }

        public bool CheckConnection()
        {
            bool conn = true; //replace by function to check connection to api!!!!
            if (conn)
                return true;
            return false;
        }

        private bool logIn()
        {
            //Add code to send username and password
            return true;
        }

        public void BufferCleanup() //Tries to send transaction
        {
            while (true)
            {
                try
                {
                    buffer.SendTransaction();        
                }
                catch (System.InvalidOperationException)
                {
                    //add log to signify that the buffer is clear    
                    break;
                }
            }
        }

        public Transaction DebugPeek()
        {
            return buffer.DebugPeek();
        }
        public string debugOut()
        {
            return buffer.debugOut();
        }
    }

    class TransactionBuffer
    {
        private Queue<Transaction> data; 
        public TransactionBuffer(int bufferLength)
        {
            data = new Queue<Transaction>();
        }

        public void loadTextBuffer() //Loads buffered transactions save to text
        {
            FileStream bufferFile;

        }

        public bool AddToTextBuffer(Transaction t) //returns false if could not create file
        {
            FileStream bufferFile;
            string path = "transact.buffer";
            try
            {
                bufferFile = File.Open(path, FileMode.Append);
            }
            catch (FileNotFoundException)
            {
                try
                {
                    bufferFile = File.Open(path, FileMode.Create);
                }
                catch (System.Exception)
                {
                    Console.WriteLine("ERROR: Could not create offline buffer file. Offline transactions will be lost when closing this program"); //replace with log4net
                    return false;
                }
            }
            
            bufferFile.Write(Encoding.Unicode.GetBytes(t.debugOut()), 0, Encoding.Unicode.GetByteCount(t.debugOut()));
            bufferFile.Close();

            return true;
        }

        public void RemoveFromTextBuffer()
        {
            
        }

        public void AddTransactionToBuffer(Transaction t)
        {
            data.Enqueue(t);
            //add log here
        }

        public void SendTransaction() //throws exception to parent function if buffer is empty
        {
            try
            {
                data.Dequeue();
                //code to encode and send to socket
            }
            catch (System.InvalidOperationException)
            {
                Console.WriteLine("INFO: Buffer is clear"); //replace with log4net
                throw;
            }
        }

        public Transaction DebugPeek()
        {
            return data.Peek();
        }

        public string debugOut()
        {
            string allTransacOut = "BUFFER INFO:\n";
            foreach (Transaction t in data)
            {
                allTransacOut = allTransacOut + t.debugOut();
                allTransacOut = allTransacOut + "\n";
            }
            return allTransacOut;
        }
    }

    class Transaction //placeholder members !!change according to core!!
    {
        private DateTime issueDate;
        private String pacientID;
        private String operationType;
        private int monetaryAmount;

        public Transaction(String _pacientID, String _operationType, int _monetaryAmount)
        {
            issueDate = DateTime.Now;
            pacientID = _pacientID;
            operationType = _operationType;
            monetaryAmount = _monetaryAmount;
        }

        public string debugOut()
        {
            return issueDate.ToString() + " | " + pacientID + " | " + operationType + " | " + monetaryAmount.ToString();
        }
    }
}