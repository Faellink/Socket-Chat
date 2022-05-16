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
using System.IO;
using System.Threading;

namespace MessengerServer
{
    public partial class Form1 : Form
    {
        //server listening socket
        private Socket serverSocket;
        //client socket
        private Socket clientSocket;
        //data buffer received
        private byte[] dataBuffer;
        // data buffer to send
        private byte[] sendBuffer = new byte[1024];
        //name for debugging 
        private string serverName = "server";
        //
        private bool isConnected = false;
        //private bool isListening = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartServer()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //IPAddress ipAddress = IPAddress.Parse(ipTextBox.Text);
                //int portNumber = int.Parse(portTextBox.Text);
                //IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, portNumber);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 23000);

                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen(0);
                //isListening = true;
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                AppendToTexBox("Server started \r\nWaiting for connections...\n");
                connectButton.Text = "Listening";

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"SERVER Start server: {ex.Message}");
                MessageBox.Show(ex.Message, "SERVER Start server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                clientSocket = serverSocket.EndAccept(ar);
                dataBuffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);
                isConnected = true;
                UpdateFormLayout(isConnected);
                AppendToTexBox("Client connected");
                //serverSocket.BeginAccept(AcceptCallback, null);
            }
            catch (Exception ex)
            {
                //serverSocket.Close();
                //MessageBox.Show($"SERVER AcceptCallBack: {ex.Message}");
                MessageBox.Show(ex.Message, "SERVER AcceptCallBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {

            try
            {
                //TODO: work on receive

                int received = clientSocket.EndReceive(ar);

                if (received == 0)
                {
                    serverSocket.Dispose();
                    AppendToTexBox("Client Disconnected");
                    isConnected = false;
                    UpdateFormLayout(isConnected);
                    return;
                }

                //change number of elements to a specified new size
                Array.Resize(ref dataBuffer, received);

                //encode the bytes array to a string
                string text = Encoding.ASCII.GetString(dataBuffer, 0, received);

                //join text to text box
                AppendToTexBox(text);

                ///change number of elements to a specified new size
                Array.Resize(ref dataBuffer, clientSocket.ReceiveBufferSize);

                //accept new dataBuffer
                clientSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"SERVER ReceiveCallBack: {ex.Message}");
                MessageBox.Show(ex.Message, "SERVER ReceiveCallBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void ConnectClick(object sender, EventArgs e)
        {
            if (isConnected == false)
            {
                StartServer();
            }
            else
            {
                CloseConnection();
            }
        }

        private void SendClick(object sender, EventArgs e)
        {
            SendMessege();
        }

        private void SendCallBack(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"SERVER SendCallBack: {ex.Message}");
                MessageBox.Show(ex.Message, "SERVER SendCallBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseConnection()
        {
            try
            {
                //serverSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Dispose();
                serverSocket.Dispose();
                AppendToTexBox("Server Disconnected");
                isConnected = false;
                UpdateFormLayout(isConnected);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //MessageBox.Show(ex.Message, "Close Connection ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.Message, "SERVER CloseConnection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void CloseConnectionServer(Socket serverSocket)
        {
            //serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Dispose();
        }

        void CloseConnectionClient(Socket clientSocket)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
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
                    //Console.WriteLine(isConnected);
                }
                else
                {
                    sendButton.Enabled = false;
                    messageInputTextBox.Enabled = false;
                    connectButton.Text = "Connect";
                    //Console.WriteLine(isConnected);
                }
            });

            this.Invoke(invoker);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateFormLayout(isConnected);
            //serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void SendKeyPressEnter(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                SendMessege();
            }
        }

        private void SendMessege()
        {

            try
            {
                //serverName = nameTextBox.Text;
                sendBuffer = Encoding.ASCII.GetBytes($"{serverName}: {messageInputTextBox.Text}");
                clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), null);
                AppendToTexBox(Encoding.ASCII.GetString(sendBuffer));
                Array.Clear(sendBuffer, 0, sendBuffer.Length);
                messageInputTextBox.Text = "";
            }
            catch (SocketException sex)
            {
                //TODO: close connection
                //MessageBox.Show($"SERVER SendClick SocketException: {sex.Message}");
                MessageBox.Show(sex.Message, "SERVER SendClick SocketException", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"SERVER SendClick: {ex.Message}");
                MessageBox.Show(ex.Message, "SERVER SendClick", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
