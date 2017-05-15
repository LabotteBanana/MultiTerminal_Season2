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

        private Dictionary<int, EndPoint> m_ClientEP = new Dictionary<int, EndPoint>();
        private int m_ClientCount;
        public Socket server;
        private IPEndPoint EP;
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
                if (server.IsBound == false)
                {
                    server.Bind(EP);
                }

                th = new Thread(new ThreadStart(RecvMessage)); //상대 문자열 수신 쓰레드 가동
                th.Start();
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
            }
        }
        public void SendMessage(string sendMsg)
        {
            try
            {
                byte[] data = new byte[1024];
                data = Encoding.UTF8.GetBytes(sendMsg);
                for (int i = 0; i < m_ClientCount; i++)
                {
                    server.SendTo(data, m_ClientEP[i]);
                }
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
            }

        }
        public void RecvMessage()
        {
            try
            {
                while (true)
                {
                    byte[] recv = new byte[1024];
                    IPEndPoint Sender = new IPEndPoint(IPAddress.Any, 0);
                    EndPoint remoteEP = Sender;
                    int recvi = server.ReceiveFrom(recv, ref remoteEP);

                    for (int i = 0; i < m_ClientEP.Count; i++)
                    {
                        if (m_ClientEP[i].ToString() == remoteEP.ToString())
                        {
                            m_ClientEP.Remove(i);
                            m_ClientCount--;
                        }
                    }
                    if(recvi >0 ) //메시지 도달
                    {
                        m_ClientEP.Add(m_ClientCount++, remoteEP);
                    }
                    string recvMsg = Encoding.Default.GetString(recv);
                    ///이부분 문제
                    if (main.InvokeRequired)
                    {
                        main.Invoke(new Action(() => main.ReceiveWindowBox.Text += "수신 :" + main.GetTimer() + recvMsg + "\n"));
                        main.Invoke(new Action(() => main.ReceiveWindowBox.Text += "" + Environment.NewLine));
                        main.Invoke(new Action(() => main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length));
                        main.Invoke(new Action(() => main.ReceiveWindowBox.ScrollToCaret()));

                    }
                    else
                    {
                        main.ReceiveWindowBox.Text += "수신 :" + main.GetTimer() + recvMsg + "\n";
                        main.ReceiveWindowBox.Text += "" + Environment.NewLine;
                        main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                        main.ReceiveWindowBox.ScrollToCaret();
                    }
                }
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
            }
        }
        public void DisConnect()
        {
            if (server != null)
            {
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
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
