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
        public struct User
        {
            public EndPoint m_UserEP;
            public string m_UserName;
        }

        public Socket client;
        private EndPoint remoteEP;
        public bool m_isConnected = false;
        public byte[] datatStream = new byte[1024];
        public byte[] data = new byte[1024];
        private static Thread Recvth = null;
        public void Connect(MainForm form,string IP,int port)
        {
            try
            {
                main = form;
                client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPEndPoint  server = new IPEndPoint(IPAddress.Parse(IP), port);
                remoteEP = (EndPoint)server;

                //client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                //client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                //client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);

                client.Bind(remoteEP);
                datatStream = new byte[] { 0xa1,0xb2 };
                client.BeginSendTo(this.datatStream, 0, this.datatStream.Length, SocketFlags.None, remoteEP, new AsyncCallback(this.SendMessage), null);
                //Recvth = new Thread(new ThreadStart(RecvMessage)); //상대 문자열 수신 쓰레드 가동
                m_isConnected = true;
                //Recvth.Start();
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message, "ConnectError");
            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message, "ConnectError");
            }

        }
        //public void RecvMessage()
        //{
        //    try
        //    {
        //        byte[] data = new byte[1024];
        //        client.ReceiveFrom(data, data.Length, SocketFlags.None, ref remoteEP);
        //        if (data.Length == 0) return;
        //        string recvMsg = Encoding.Default.GetString(data);
        //        if (main.InvokeRequired)
        //        {
        //            main.Invoke(new Action(() => main.ReceiveWindowBox.AppendText("수신 :" + main.GetTimer() + recvMsg)));
        //            main.Invoke(new Action(() => main.ReceiveWindowBox.AppendText("" + Environment.NewLine)));
        //            main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
        //            main.ReceiveWindowBox.ScrollToCaret();

        //        }
        //        else
        //        {
        //            main.ReceiveWindowBox.AppendText("수신 : " + main.GetTimer() + recvMsg + "\n");
        //            main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
        //            main.ReceiveWindowBox.ScrollToCaret();
        //        }
        //    }
        //    catch (SocketException ex)
        //    {
        //        int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
        //        System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message, "RecvError");
        //    }

        //    catch (Exception ex)
        //    {
        //        int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
        //        System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message, "RecvError");
        //    }
        //}
        //public void SendMessage(string sendMsg)
        //{
        //    try
        //    {
        //        byte[] data = new byte[1024];

        //        data = Encoding.Default.GetBytes(sendMsg);
        //        //client.SendTo(data, data.Length, SocketFlags.None, serverEP);
        //    }
        //    catch (SocketException ex)
        //    {
        //        int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
        //        System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message,"SendError");
        //    }

        //    catch (Exception ex)
        //    {
        //        int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
        //        System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message, "SendError");
        //    }

        //}
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
        public void SendMessage(IAsyncResult asyncResult)
        {
            try
            {
                client.EndSend(asyncResult);
                client.BeginReceiveFrom(this.datatStream, 0, this.datatStream.Length, SocketFlags.None, ref remoteEP, new AsyncCallback(this.RecvMessage), null);
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

        public void RecvMessage(IAsyncResult asyncResult)
        {
            try
            {
                //byte[] data;
                this.client.EndReceive(asyncResult);

                 Packet ReceiveData = new Packet(this.datatStream);
                if (main.InvokeRequired)
                {
                    main.Invoke(new Action(delegate ()
                    {
                        data = Encoding.Default.GetBytes(main.SendBox1.Text);
                    }));
                }
                else
                {
                    data = Encoding.Default.GetBytes(main.SendBox1.Text);
                }
                Packet SendData = new Packet(data);

                client.BeginSendTo(this.datatStream, 0, this.datatStream.Length, SocketFlags.None, remoteEP, new AsyncCallback(this.SendMessage), null);

                string recvMsg = Encoding.Default.GetString(data);
                ///이부분 문제
                if (main.InvokeRequired)
                {
                    if (main.InvokeRequired)
                    {
                        main.Invoke(new Action(() => main.ReceiveWindowBox.AppendText("수신 :" + main.GetTimer() + recvMsg)));
                        main.Invoke(new Action(() => main.ReceiveWindowBox.AppendText("" + Environment.NewLine)));
                        main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                        main.ReceiveWindowBox.ScrollToCaret();

                    }
                    else
                    {
                        main.ReceiveWindowBox.AppendText("수신 : " + main.GetTimer() + recvMsg + "\n");
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


    }
}
