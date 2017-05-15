using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Collections;
namespace MultiTerminal
{
    public class udpServer
    {
        MainForm main = null;
        public struct User
        {
            public EndPoint m_UserEP;
            public string m_UserName;
        }
        public ArrayList m_ClientList = new ArrayList();
        public byte[] datatStream = new byte[1024];
        private delegate void UpdateStatusDelegate(string status);
        private UpdateStatusDelegate updateStatusDelegate = null;

        //private IPEndPoint EP;
        public Socket server;
        private bool m_isConnected = false;
        //private static Thread th = null;
        byte[] data = new byte[1024];
        //byte[] recv = new byte[1024];
        //byte[] send = new byte[1024];
        public void Connect(MainForm form,int Port)
        {
            try
            {
               main = form;
               IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, Port);
               server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                if (server.IsBound == false)
                {
                    server.Bind(serverEP);
                }
                //this.updateStatusDelegate = new UpdateStatusDelegate(this.UpdateStatus);

                IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
                EndPoint clientEP = (EndPoint)client;

                //server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                //server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                //server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);

                datatStream = new byte[] { 0xdd, 0xf1 };
                server.BeginReceiveFrom(datatStream, 0, datatStream.Length, SocketFlags.None, ref clientEP, new AsyncCallback(RecvMessage), clientEP);
                //server.BeginSendTo(send, 0, send.Length, SocketFlags.None, clientEP, new AsyncCallback(SendMessage), clientEP);
                //server.BeginReceiveFrom(recv, 0, recv.Length, SocketFlags.None, ref clientEP, new AsyncCallback(RecvMessage), null);
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

                data = Encoding.Default.GetBytes(sendMsg);
                //server.SendTo(data, data.Length, SocketFlags.None, EP);
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

        public void SendMessage(IAsyncResult asyncResult)
        {
            try
            {
                server.EndSend(asyncResult);
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
                server.EndReceiveFrom(asyncResult,endPoint);

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

                IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
                EndPoint clientEP = (EndPoint)clients;
                SendData.ChatName = ReceiveData.ChatName;


                // Populate client object
                User client = new User();
                client.m_UserEP =clientEP;
                client.m_UserName = ReceiveData.ChatName;

                // Add client to list
                this.m_ClientList.Add(client);

                SendData.ChatMessage = string.Format("-- {0} is online --", ReceiveData.ChatName);

                    //case DataIdentifier.Logout:
                    //    // Remove current client from list
                    //    foreach (User c in this.m_ClientList)
                    //    {
                    //        if (c.m_UserEP.Equals(clientEP))
                    //        {
                    //            this.m_ClientList.Remove(c);
                    //            break;
                    //        }
                    //    }


                data = SendData.GetDataStream();

                foreach (User c in this.m_ClientList)
                {
                    if (c.m_UserEP != clientEP)
                    {
                        // Broadcast to all logged on users
                        server.BeginSendTo(data, 0, data.Length, SocketFlags.None, c.m_UserEP, new AsyncCallback(this.SendMessage), c.m_UserEP);
                    }
                }

                   server.BeginReceiveFrom(this.datatStream, 0, this.datatStream.Length, SocketFlags.None, ref clientEP, new AsyncCallback(RecvMessage), clientEP);
                //if (main.InvokeRequired)
                //    main.Invoke(this.updateStatusDelegate, new object[] { SendData.ChatMessage });

                //if (data.Length == 0) return;
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

        private void UpdateStatus(string status)
        {
            string recvMsg = Encoding.Default.GetString(data);

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
            Array.Clear(data, 0, data.Length);
        }

    }
}
