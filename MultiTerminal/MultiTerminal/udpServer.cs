using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace MultiTerminal
{
    public class udpServer
    {
        MainForm main = null;

        private IPEndPoint EP;
        private IPEndPoint Sender;
        private EndPoint remoteEP;
        public Socket server;
        private bool m_isConnected = false;
        private static Thread th = null;
        public void Connect(MainForm form,int Port)
        {
            try
            {
                main = form;
                EP = new IPEndPoint(IPAddress.Any, Port);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);
                Sender = new IPEndPoint(IPAddress.Any, 0);
                remoteEP = (EndPoint)Sender;
                if (server.IsBound == false)
                {
                    server.Bind(EP);
                }
                //SendMessage("Hello, Client!");

                th = new Thread(new ThreadStart(RecvMessage)); //상대 문자열 수신 쓰레드 가동
                th.Start();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
        public void SendMessage(string sendMsg)
        {
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes(sendMsg);
            server.SendTo(data, remoteEP);
        }
        public void RecvMessage()
        {
            try { 
            byte[] recv = new byte[1024];
            int recvi = server.ReceiveFrom(recv,ref remoteEP);

            string recvMsg = Encoding.Default.GetString(recv);
                ///이부분 문제
                if (main.InvokeRequired)
                {
                    main.Invoke(new Action(() => main.ReceiveWindowBox.Text += "수신 :" + main.GetTimer() +recvMsg + "\n"));


                }
                else
                {
                    main.ReceiveWindowBox.Text += "수신 :" + main.GetTimer() + recvMsg + "\n";
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
        public void DisConnect()
        {
            if(server!=null)
            server.Close();
        }
        public bool isConnected()
        {
            if (server.Connected == true)
                m_isConnected = true;
            else
                m_isConnected = false;
            return m_isConnected;
        }


    }
}
