using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Multiple_Client
{
    class Client
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            LoopConnect();
            Send_Recv();
            Console.ReadLine();
        }

        private static void Send_Recv()
        {
            string text = string.Empty; 
            while(true)
            {
         //       Thread.Sleep(500);

           Console.WriteLine("Enter Request:");
            // Enter command request 
             text = Console.ReadLine();

        switch(text)
        {
            case "DisconnectServer":
                _clientSocket.Disconnect(true);
                break;
            case "ConnectServer":
                _clientSocket.Connect(IPAddress.Loopback, 1000);
                break;
            default:

                break;

        }
            byte[] dataSend = Encoding.ASCII.GetBytes(text);
            _clientSocket.Send(dataSend, SocketFlags.None);
          // recv data
            byte[] dataRecv= new byte[1024];
            int recv = _clientSocket.Receive(dataRecv, SocketFlags.None);

            String text_recv = Encoding.ASCII.GetString(dataRecv,0,recv);
          
            Console.WriteLine("data received {0}" + text_recv);
            }
        }

        private static void LoopConnect()
        {

            _clientSocket.Connect(IPAddress.Loopback, 1000);

        }
    }
}
