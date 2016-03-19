using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Winfom_Client
{
    public partial class Form1 : Form
    {
        private bool Flag ;
        private Socket _clientSocket;
        public Form1()
        {
            InitializeComponent();
            textBox2.ReadOnly = true;
            Flag = false;
         //   textBox1.ReadOnly = false;

        }

        private void Connect_click(object sender, EventArgs e)
        {
          
            if (Flag==false)
            {
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try { _clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 1000)); }
               
                catch(SocketException ex)
                {
                   // MessageBox.Show(ex.ToString());
                    MessageBox.Show("Server has refused!");
                   
                }
               
                Flag = true;
            }
            else
            {
                MessageBox.Show("You have entered");
                
            }

            if (_clientSocket.Connected)
                MessageBox.Show("Connected Successful");
                
           
            
            
        }

        private void Disconnect_click(object sender, EventArgs e)
        {
            Flag = false;
            try 
            { 
                _clientSocket.Shutdown(SocketShutdown.Both);
                 _clientSocket.Close();
            }
            catch ( SocketException Ex)
            {
              //  Console.WriteLine(Ex.ToString());
                MessageBox.Show(Ex.ToString());
            }
            catch(NullReferenceException Ex )
            {
                MessageBox.Show("You have no connected");
            }
            catch(ObjectDisposedException EX)
            {

            }

            

        }

        private void Send_click(object sender, EventArgs e)
        {
            byte[] dataSend = Encoding.ASCII.GetBytes(textBox1.Text);
            _clientSocket.Send(dataSend, SocketFlags.None);
            
            // recv data
            byte[] dataRecv = new byte[1024];
            int recv = _clientSocket.Receive(dataRecv, SocketFlags.None);

            String text_recv = Encoding.ASCII.GetString(dataRecv, 0, recv);

            textBox2.Text = text_recv;
        }
    }
}
