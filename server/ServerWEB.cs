using System;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webChatSERVER
{
    class ServerWEB
    {
        static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<Socket> clients = new List<Socket>();
        static byte[] buffRec = new byte[1024];
        static string receiveMesssage;
        static myExcel excelAnswer = new myExcel(@"C:\Daniel\test3.xlsx", 1);

        static void Main()
        {
            ConnectSocket();

            while (true)
            {
                Console.ReadKey(); // jest in the first time it swallowed first char
            }
        }

       static void ConnectSocket()
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8080));
            serverSocket.Listen(10);
            serverSocket.BeginAccept(Accept_client, null);
            Console.WriteLine("server on air...");
            Console.WriteLine("////////////////////////////////////////////////////////\n");
        }

        static void Accept_client(IAsyncResult AR)
        {
            Socket newClient= serverSocket.EndAccept(AR);
            clients.Add(newClient);
            Console.WriteLine("new client connect...");
            Console.WriteLine("////////////////////////////////////////////////////////\n");

            newClient.BeginReceive(buffRec,0, buffRec.Length, SocketFlags.None, Receive_message, newClient);

            serverSocket.BeginAccept(new AsyncCallback(Accept_client), null); // rec-loop for get clients

        }

        static void Receive_message(IAsyncResult AR)
        {
            Socket client = (Socket)AR.AsyncState;

            int rec;
            try
            {
                rec = client.EndReceive(AR); // ??
            }
            catch
            {
                Console.WriteLine("client disconnect...");
                Console.WriteLine("////////////////////////////////////////////////////////\n");
                client.Close();
                clients.Remove(client);
                return;

            }
            byte[] data = new byte[rec];


            Array.Copy(buffRec, data, rec);
            receiveMesssage = Encoding.ASCII.GetString(data);

            Console.WriteLine("receive message: " +receiveMesssage);
            string massageToSend = Make_message();
            Console.WriteLine("server send: " + massageToSend);
            Console.WriteLine("////////////////////////////////////////////////////////\n");

            byte[] toSend = Encoding.ASCII.GetBytes(massageToSend);
            client.Send(toSend);

            try
            {
            client.BeginReceive(buffRec, 0, buffRec.Length, SocketFlags.None, Receive_message, client); // to listen loop
            }
            catch
            {
                Console.WriteLine("client disconnect...");
                Console.WriteLine("////////////////////////////////////////////////////////\n");
                client.Close();
                clients.Remove(client);
                return;
            }
        }

        static string Make_message()
        {
            int rowAnswer = excelAnswer.Search_row_question(receiveMesssage);
            string answer;
            if (rowAnswer != -1)
            {
                answer = excelAnswer.WorkSheet.Cells[rowAnswer, 1].value2;
            }
            else
            {
                answer = "Sorry, I didn't understand your question. Try another question :)";
            }

            return answer;
        }
       
    }
}
