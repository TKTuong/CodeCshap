using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Css
{
    
    class Program
    {
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Socket> _clientSocket = new List<Socket>();
        private static byte[] _buffer = new byte[1024];
        private static void SetupServer()
        {
            Console.WriteLine("Setting up Server");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 1000));
            _serverSocket.Listen(2);
            _serverSocket.BeginAccept(new AsyncCallback(Accepcallback), null);

        }

        private static void Accepcallback(IAsyncResult AR)
        {
            Console.WriteLine("Connect Successful");
            Socket socket = _serverSocket.EndAccept(AR);
            Console.WriteLine("Server is connectting to :" + socket.RemoteEndPoint.ToString());
            _clientSocket.Add(socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None,new AsyncCallback(Receivecalback), socket);
            _serverSocket.BeginAccept(new AsyncCallback(Accepcallback), null);
        }
        private static void Close_Server()
        {
           
        }
        private static void Receivecalback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
      
            int received;
            try { 
                 received = socket.EndReceive(AR); }
            
            catch(SocketException e)      
            {
                socket.Close();
                
                _clientSocket.Remove(socket);
                Console.WriteLine(e.ToString());
                Console.WriteLine("Client forcility disconnected");
                return;
            }

            byte[] dataBuf = new byte[received];
            Array.Copy(_buffer, dataBuf, received);
            string text = Encoding.ASCII.GetString(dataBuf,0,received);
            Console.WriteLine("Text recv :" + text);

            string response = string.Empty;

            if (text.ToLower() !="get time")
            {
                response = "Invalid Request!";
            }
            else 
            {
                response = DateTime.Now.ToLongTimeString();

            }
           
            byte[] data = new byte[1024];
            data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data,0,data.Length,SocketFlags.None,new AsyncCallback(Sendcallback),socket);
           // notice add  if  it will not  run 
            try {
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(Receivecalback), socket);
            }
            catch(SocketException e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Client shutdown and disconnect");

            }
            
        }

        private static void Sendcallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);

        }
        static void Main(string[] args)
        {
            SetupServer();
            Console.ReadLine();

            
        }
    }
}
