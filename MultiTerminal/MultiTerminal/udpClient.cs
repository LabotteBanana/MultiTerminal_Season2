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
    public class udpClient
    {
        MainForm main = null;

        public Socket client;
        private IPEndPoint serverEP;
        private IPEndPoint Sender;
        private EndPoint remoteEP;
        public bool m_isConnected = false;
        private static Thread Recvth = null;
        public void Connect(MainForm form,string IP,int port)
        {
            try
            {
                main = form;
                client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                serverEP = new IPEndPoint(IPAddress.Parse(IP), port);
                Sender = new IPEndPoint(IPAddress.Any, 0);
                remoteEP = (EndPoint)Sender;
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);

                client.Bind(Sender);
                Recvth = new Thread(new ThreadStart(RecvMessage)); //상대 문자열 수신 쓰레드 가동
                m_isConnected = true;
                Recvth.Start();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
                
        }
        public void RecvMessage()
        {
            try
            {
                byte[] data = new byte[1024];
                client.ReceiveFrom(data, data.Length, SocketFlags.None, ref remoteEP);
                if (data.Length == 0) return;
                string recvMsg = Encoding.Default.GetString(data);
                if (main.InvokeRequired)
                {
                    main.Invoke(new Action(() => main.ReceiveWindowBox.Text += "수신 :" + main.GetTimer() + recvMsg + "\n"));


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
        public void SendMessage(string sendMsg)
        {
            byte[] data = new byte[1024];

            data = Encoding.Default.GetBytes(sendMsg);
            client.SendTo(data,data.Length,SocketFlags.None,serverEP);
        }
        public void DisConnect()
        {
            if(client !=null)
            client.Close();
            m_isConnected = false;
        }
        public bool isConnected()
        {
            if (client.Connected == true)
                m_isConnected = true;
            else
                m_isConnected = false;
            return m_isConnected;
        }


    }
}
