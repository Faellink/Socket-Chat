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
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                AppendToTexBox("Server started \r\nWaiting for connections... \r\n ");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SERVER Start server: {ex.Message}");
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
                MessageBox.Show($"SERVER AcceptCallBack: {ex.Message}");
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {

            try
            {
                //TODO: work on receive
                //int received = clientSocket.EndReceive(ar);
                //Array.Resize(ref dataBuffer, received);
                //string text = Encoding.ASCII.GetString(dataBuffer);
                //AppendToTexBox(text);
                //Array.Resize(ref dataBuffer, clientSocket.ReceiveBufferSize);
                //clientSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);

                int received = clientSocket.EndReceive(ar);

                if (received == 0)
                {
                    serverSocket.Dispose();
                    //MessageBox.Show("Client Disconnected");
                    AppendToTexBox("Client Disconnected");
                    // close connections
                    //CloseConnections();
                    //serverSocket.Shutdown(SocketShutdown.Both);
                    //serverSocket.Close();
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
                MessageBox.Show($"SERVER ReceiveCallBack: {ex.Message}");
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
            StartServer();
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
                MessageBox.Show($"SERVER SendCallBack: {ex.Message}");
            }
        }

        void CloseConnection(Socket socket)
        {
            //serverSocket.Send();
            //socket.Shutdown(SocketShutdown.Both);
            //socket.Close();
            socket.Dispose();
            socket.Close();
        }

        //private void CloseConnections()
        //{
        //    serverSocket.Shutdown(SocketShutdown.Both);
        //    serverSocket.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), null);

        //    //clientSocket.Close();

        //    //isConnected = false;
        //    //connectButton.Text = "Connect";
        //    //AppendToTexBox("Connection Closed");
        //}

        //private void DisconnectCallBack(IAsyncResult ar)
        //{
        //    try
        //    {
        //        //SocketConnected(clientSocket);
        //        //clientSocket = (Socket)ar.AsyncState;
        //        serverSocket.EndDisconnect(ar);
        //        isConnected = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"SERVER DisconnectCallBack: {ex.Message}");
        //    }
        //}

        // 

        private void UpdateFormLayout(bool isConnected)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
                if (isConnected == true)
                {
                    sendButton.Enabled = true;
                    messageInputTextBox.Enabled = true;
                    connectButton.Text = "Diconnect";
                    Console.WriteLine(isConnected);
                }
                else
                {
                    sendButton.Enabled = false;
                    messageInputTextBox.Enabled = false;
                    connectButton.Text = "Connect";
                    Console.WriteLine(isConnected);
                }
            });

            this.Invoke(invoker);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateFormLayout(isConnected);
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
                //byte[] dataBuffer = Encoding.ASCII.GetBytes(messageInputTextBox.Text);
                clientSocket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallBack), null);
                AppendToTexBox(Encoding.ASCII.GetString(sendBuffer));
                Array.Clear(sendBuffer, 0, sendBuffer.Length);
                messageInputTextBox.Text = "";
            }
            catch (SocketException sex)
            {
                //TODO: close connection
                MessageBox.Show($"SERVER SendClick SocketException: {sex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SERVER SendClick: {ex.Message}");
            }

        }
    }
}
