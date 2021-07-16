using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
//this is a comment
namespace CajaEmeute
{
    class Session
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string username;
        private string password;
        private string connexion;
        private TransactionBuffer buffer;
        connection apiConn;

        private DateTime sessionStart;

        public Session(string _username, string _password, string _connexion)
        {
            buffer = new TransactionBuffer(20);
            username = _username;
            password = _password; //Storing password in plain text !!TEMP MEASURE!!!
            connexion = _connexion;
            //add log here for conn status
            //log.Info();//conn status
            sessionStart = DateTime.Now;

            buffer.loadTextBuffer();
            //apiConn = new connection();
            BufferCleanup();
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
            while(true)
            {
                try
                {
                    buffer.SendTransaction(apiConn);
                }
                catch (System.InvalidOperationException)
                {
                    log.Info("Transactions sens, buffer succesfully emptied");
                    break;
                }
                catch(System.Exception)
                {
                    log.Error("Could not send a transaction. Falling back on buffer");
                    break;
                }
            }
        }

        public void DebugSingleBuffer() //Tries to send transaction
        {
            try
            {
                buffer.SendTransaction(apiConn);
            }
            catch (System.InvalidOperationException)
            {
                log.Error("Empty buffer");
            }
            catch(System.Exception)
            {
                log.Error("Could not send a transaction. Falling back on buffer");
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
            string path = "transact.buffer";
            try
            {
                bufferFile = File.Open(path, FileMode.Open);
                bufferFile.Close();
            }
            catch (FileNotFoundException)
            {
                Session.log.Info("No offline buffer found. Continuing with an empty buffer");
                return;
            }

            List<string> tempData = new List<string>(File.ReadAllLines(path, Encoding.Unicode));
            foreach (string line in tempData)
            {
                string[] tempLine = line.Split('|');
                data.Enqueue(new Transaction(DateTime.Parse(tempLine[0]),tempLine[1], tempLine[2], int.Parse(tempLine[3])));
            }
            Session.log.Info("Offline text buffer loaded, trying to send and clear buffer");
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
                    bufferFile.Write(Encoding.Unicode.GetBytes(t.debugOut() + "\n"), 0, Encoding.Unicode.GetByteCount(t.debugOut() + "\n"));
                }
                catch (System.Exception)
                {
                    Session.log.Error("Could not create offline buffer file. Offline transactions will be lost when closing this program");
                    return false;
                }
            }
            
            bufferFile.Write(Encoding.Unicode.GetBytes(t.debugOut() + "\n"), 0, Encoding.Unicode.GetByteCount(t.debugOut() + "\n"));
            bufferFile.Close();

            return true;
        }

        public bool RemoveFromTextBuffer() //removes last entry in the buffer file
        {
            FileStream bufferFile;
            List<string> bufferFileLines;
            string path = "transact.buffer";
            try
            {
                bufferFile = File.Open(path, FileMode.Append);
            }
            catch (System.Exception)
            {
                Session.log.Error("Could not find offline buffer file. Offline transactions will be lost when closing this program");
                return false;
            }

            bufferFile.Close();
            bufferFileLines = new List<string>(File.ReadAllLines(path, Encoding.Unicode));
            bufferFileLines.RemoveAt(0);
            if (bufferFileLines.Count != 0)
            {
                File.WriteAllLines(path, bufferFileLines, Encoding.Unicode);
            }
            else
            {
                File.Create(path);
            }
            

            return true;
        }

        public void AddTransactionToBuffer(Transaction t)
        {
            data.Enqueue(t);
            Session.log.Info("Transaction added to buffer"); //to check
        }

        public void SendTransaction(connection apiConn) //throws exception to parent function if buffer is empty or connection has failed
        {
            try
            {
                Console.WriteLine("INFO: Received from server: {0}", apiConn.SendTransaction(data.Peek()));
                data.Dequeue();
                //code to encode and send to socket
            }
            catch (System.InvalidOperationException)
            {
                Session.log.Error("Error sending transaction"); //check this
                throw;
            }
            catch(System.Exception)
            {
                Session.log.Error("Could not send transaction"); //check this
                throw;
            }
            RemoveFromTextBuffer();
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

        public Transaction(DateTime _issueDate, String _pacientID, String _operationType, int _monetaryAmount) //overload for loading from text file
        {
            issueDate = _issueDate;
            pacientID = _pacientID;
            operationType = _operationType;
            monetaryAmount = _monetaryAmount;
        }

        public string debugOut()
        {
            return issueDate.ToString() + "|" + pacientID + "|" + operationType + "|" + monetaryAmount.ToString();
        }
    }
}