using System;
using System.Collections.Generic;
//comment
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
            buffer.SendTransactions();
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
            //add log here
        }

        public void AddTransactionToBuffer(Transaction t)
        {
            data.Enqueue(t);
            //add log here
        }

        public void SendTransactions()
        {
            data.Dequeue();
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