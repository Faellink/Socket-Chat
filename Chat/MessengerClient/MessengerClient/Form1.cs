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
using System.Threading;

namespace MessengerClient
{
    public partial class Form1 : Form
    {

        private Socket clientSocket;
        private byte[] dataBuffer;
        private byte[] sendBuffer = new byte[1024];
        private string clientName = "client";
        private bool isConnected = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectClick(object sender, EventArgs e)
        {
            if (isConnected == false)
            {
                try
                {
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //IPAddress ipAddress = IPAddress.Parse(ipTextBox.Text);
                    //int portNumber = int.Parse(portTextBox.Text);
                    //IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, portNumber);
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 23000);
                    clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(ConnectCallBack), null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                CloseConnections();
                AppendToTexBox("Connection Closed");
            }
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);
                dataBuffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
                AppendToTexBox("Connected to server");
                //sendButton.Enabled = true;
                isConnected = true;
                UpdateFormLayout(isConnected);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CLIENT ConnectCallBack: {ex.Message}");
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                ////TODO: work on receive
                
                //end receive returns bytes received
                int received = clientSocket.EndReceive(ar);

                if (received == 0)
                {
                    CloseConnections();
                    return;
                }

                //change number of elements to a specified new size
                Array.Resize(ref dataBuffer, received);

                //encode the bytes array to a string
                string text = Encoding.ASCII.GetString(dataBuffer,0,received);

                //join text to text box
                AppendToTexBox(text);

                ///change number of elements to a specified new size
                Array.Resize(ref dataBuffer, clientSocket.ReceiveBufferSize);

                //accept new dataBuffer
                clientSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
            }
            catch (Exception ex)
            {
                CloseConnections();
                //MessageBox.Show($"CLIENT ReceiveCallBack: {ex.Message}");
            }
        }

        private void AppendToTexBox(string text)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
                chatLogTextBox.AppendText($"{text} \r\n");
            });

            this.Invoke(invoker);
        }


        private void SendCallBack(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CLIENT SendCallBack: {ex.Message}");
            }
        }

        private void SendClick(object sender, EventArgs e)
        {
            SendMessege();
        }

        //TODO: close connections
        private void CloseConnections()
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), null);

            isConnected = false;
            UpdateFormLayout(isConnected);
            //AppendToTexBox("Connection Closed");
        }

        private void DisconnectCallBack(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndDisconnect(ar);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"CLIENT DisconnectCallBack: {ex.Message}");
            }
        }

        private void SendKeyPressEnter(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                SendMessege();
            }
        }

        void SendMessege()
        {
            try
            {
                //clientName = nameTextBox.Text;
                sendBuffer = Encoding.ASCII.GetBytes($"{clientName}: {messageInputTextBox.Text}");
                clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), null);
                AppendToTexBox(Encoding.ASCII.GetString(sendBuffer));
                Array.Clear(sendBuffer, 0, sendBuffer.Length);
                messageInputTextBox.Text = "";
            }
            catch (SocketException)
            {
                //TODO: close connection
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CLIENT SendClick: {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateFormLayout(isConnected);
        }

        private void UpdateFormLayout(bool isConnected)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
                if (isConnected == true)
                {
                    sendButton.Enabled = true;
                    messageInputTextBox.Enabled = true;
                    connectButton.Text = "Diconnect";
                }
                else
                {
                    sendButton.Enabled = false;
                    messageInputTextBox.Enabled = false;
                    connectButton.Text = "Connect";
                }
            });
            this.Invoke(invoker);
        }
    }
}
