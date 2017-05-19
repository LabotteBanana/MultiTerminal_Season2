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
        private int m_DisConnectNum;
        public Socket server;
        private IPEndPoint EP;
        public int port;
        public bool m_isConnected = false;
        private static Thread th = null;
        public Dictionary<int, bool> m_bSendList = new Dictionary<int, bool>();
        public Dictionary<int, bool> m_bRecvList = new Dictionary<int, bool>();
        public void Connect(MainForm form, int Port)
        {
            try
            {
                main = form;
                EP = new IPEndPoint(IPAddress.Any, Port);
                port = Port;
                server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);
                if (server.IsBound == false)
                {
                    server.Bind(EP);
                }
                m_isConnected = true;

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

                if (server != null)
                {
                    for (int i = 0; i < m_ClientCount; i++)
                    {
                        if (m_bSendList[i] == true)
                        {
                            server.SendTo(data, m_ClientEP[i]);
                        }
                    }
                }
                else
                {
                    return;
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
                while (m_isConnected)
                {
                    byte[] recv = new byte[1024];
                    IPEndPoint Sender = new IPEndPoint(IPAddress.Any, 0);
                    EndPoint remoteEP = Sender;

                    if (server != null)
                    {
                        int recvi = server.ReceiveFrom(recv, ref remoteEP);
                        //어느 클라이언트에서 보냈는지 판단 가능
                        string recvMsg = Encoding.Default.GetString(recv);
                        bool aaaa = recvMsg.StartsWith("서버로 접속");
                        if (recvi > 0) //메시지 도달
                        {
                            m_isConnected = true;
                            String ClientInfo = remoteEP.ToString();
                            int StrStart = ClientInfo.IndexOf(':');
                            String PortStr = ClientInfo.Substring(StrStart + 1); //접속해온 곳의 Port번호 확인


                            if (aaaa == true && recvi == 11)
                            {
                                m_ClientEP.Add(m_ClientCount++, remoteEP);
                                if (main.InvokeRequired)
                                {
                                    // 그리드뷰 객체에 적용,   타입형태(시리얼,UDP..), 타입의 순번도 그리드 객체로 슝들어감.    

                                    main.Invoke(new Action(() => main.gridview[main.GridList.Count] = new GridView(main.GridList.Count, PortStr.ToString(), "UDPClient", m_ClientCount)));
                                    main.Invoke(new Action(() => main.DrawGrid(main.gridview[main.GridList.Count].MyNum, main.gridview[main.GridList.Count].Type, main.gridview[main.GridList.Count].Portname, main.gridview[main.GridList.Count].Time)));
                                    main.Invoke(new Action(() => main.GridList.Add(main.gridview[main.GridList.Count])));
                    
                                }
                                else
                                {
                                    main.gridview[main.GridList.Count] = new GridView(main.GridList.Count, PortStr.ToString(), "UDPClient", m_ClientCount);
                                    main.DrawGrid(main.gridview[main.GridList.Count].MyNum, main.gridview[main.GridList.Count].Type, main.gridview[main.GridList.Count].Portname, main.gridview[main.GridList.Count].Time);
                                    main.GridList.Add(main.gridview[main.GridList.Count]);
                                }
                            }
                            if (main.RowIndex >= 0 && main.GridList[main.RowIndex].RxCheckedState == true)
                            {
                                for (int j = 0; j < m_ClientCount; j++)
                                {
                                    String Info = m_ClientEP[j].ToString();
                                    int Start = Info.IndexOf(':');
                                    String Port = Info.Substring(Start + 1); //접속해온 곳의 Port번호 확인

                                    m_DisConnectNum = j;
                                    if (m_bRecvList[j] == true && String.Compare(Port, PortStr) == 0)
                                    {
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
                                    else
                                        continue;
                                }
                            }
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                //클라이언트가 접속종료를 알게되는 시점 (보내봐야안다.)
                if (ex.ErrorCode == 10054)
                {
                    System.Windows.Forms.MessageBox.Show(m_DisConnectNum + "번 클라이언트에서 연결을 종료했습니다.");
                    m_ClientEP.Remove(m_DisConnectNum);
                    m_bRecvList.Remove(m_DisConnectNum);
                    m_bSendList.Remove(m_DisConnectNum);
                    main.GridList.Remove(main.gridview[m_DisConnectNum]);
                    ///
                    m_ClientCount--;
                }
                else
                {
                    int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                    System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
                }
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
                m_isConnected = false;

                //server.Shutdown(SocketShutdown.Both);
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