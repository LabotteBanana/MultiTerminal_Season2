﻿using System;
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
        private string nowPortSt;
        private Dictionary<int, EndPoint> m_ClientEP = new Dictionary<int, EndPoint>();
        //private int m_ClientCount;
        public Socket server;
        private IPEndPoint EP;
        public int port;
        public bool m_isConnected = false;
        private static Thread th = null;
        private List<string> m_PortList = new List<string>();
        //public Dictionary<int, bool> m_bSendList = new Dictionary<int, bool>();
        //public bool m_bRecv = true;
        private GridView gridview = null;
        private List<GridView> gridList = null;

        public void Connect(MainForm form, int Port, GridView Gridview, List<GridView> GridList)
        {
            try
            {
                main = form;
                EP = new IPEndPoint(IPAddress.Any, Port);
                port = Port;
                gridList = GridList;
                gridview = Gridview;
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
                    for (int i = 0; i < gridList.Count; i++)
                    {
                        //if (m_bSendList[i] == true)
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
                        bool connectMsg = recvMsg.StartsWith("서버로 접속");
                        bool disconnectMsg = recvMsg.StartsWith("서버로 해제");
                        if (recvi > 0) //메시지 도달
                        {
                            m_isConnected = true;
                            String ClientInfo = remoteEP.ToString();
                            int StrStart = ClientInfo.IndexOf(':');
                            String PortStr = ClientInfo.Substring(StrStart + 1); //접속해온 곳의 Port번호 확인
                            nowPortSt = PortStr;

                            if (connectMsg == true && recvi == 11)
                            {
                                ///추가
                                //System.Windows.Forms.DataGridViewCheckBoxColumn udpRx = main.Rx;
                                m_ClientEP.Add(gridList.Count, remoteEP);
                                //m_bSendList.Add(gridList.Count, true);
                                if (main.InvokeRequired)
                                {
                                    // 그리드뷰 객체에 적용,   타입형태(시리얼,UDP..), 타입의 순번도 그리드 객체로 슝들어감.
                                    main.Invoke(new Action(() => gridview = new GridView(gridList.Count, PortStr.ToString(), "UDP Client", gridList.Count)));
                                    main.Invoke(new Action(() => main.DrawGrid(gridview.MyNum, gridview.Type, gridview.Portname, gridview.Time)));
                                    main.Invoke(new Action(() => gridList.Add(gridview)));
                                    m_PortList.Add(PortStr.ToString());
                                    //main.Invoke(new Action(() => udpRx.ReadOnly = true));
                                }
                                else
                                {
                                    gridview = new GridView(gridList.Count, PortStr.ToString(), "UDP Client", gridList.Count);
                                    main.DrawGrid(gridview.MyNum, gridview.Type, gridview.Portname, gridview.Time);
                                    gridList.Add(gridview);
                                    m_PortList.Add(PortStr.ToString());

                                    //udpRx.ReadOnly = true;
                                }
                            }
                            if (disconnectMsg == true && recvi == 11)
                            {
                                for (int i = 0; i < m_PortList.Count; i++)
                                {
                                    if (PortStr == m_PortList[i])
                                    {
                                        m_ClientEP.Remove(i);
                                        main.RemoveGridforIP(m_PortList[i]);
                                    }
                                }
                            }
                            if (main.RowIndex >= 0)
                            {
                                {
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
                                //else
                                //continue;
                            }
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                //클라이언트의 접속종료를 알게되는 시점 (보내봐야안다.)
                if (ex.ErrorCode == 10054)
                {
                    System.Windows.Forms.MessageBox.Show(nowPortSt + "포트 클라이언트에서 연결을 종료했습니다.");

                    //System.Windows.Forms.MessageBox.Show(m_DisConnectNum + "번 클라이언트에서 연결을 종료했습니다.");
                    //m_ClientEP.Remove(m_DisConnectNum);
                    //gridList.RemoveAt(m_DisConnectNum);

                    //m_bSendList.Remove(m_DisConnectNum);
                    //main.PortListGrid.Rows.RemoveAt(m_DisConnectNum);
                    //main.PortListGrid.Update();
                    //main.PortListGrid.Refresh();

                    ///
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
        public void DisConnect(int i)
        {
            if (server != null)
            {
                IPEndPoint Sender = new IPEndPoint(IPAddress.Any, 0);
                EndPoint remoteEP = (EndPoint)Sender;
                String ClientInfo = remoteEP.ToString();
                int StrStart = ClientInfo.IndexOf(':');
                String PortStr = ClientInfo.Substring(StrStart + 1); //접속해온 곳의 Port번호 확인
                if (PortStr == m_PortList[i])
                {
                    m_ClientEP.Remove(i);
                    main.RemoveGridforIP(m_PortList[i]);
                }
            }
        }

        public void DisConnect()
        {
            if (server != null)
            {
                //int row;
                //m_isConnected = false;
                //string udpclient = "UDPClient";
                ////그리드 리스트의 n번째 인덱스
                //if (main.PortListGrid.Columns[2].ToString() == udpclient)
                //{
                //    row = main.PortListGrid.CurrentCell.RowIndex;
                //    main.GridList.RemoveAt(row);
                //    main.PortListGrid.Rows.RemoveAt(row);
                //}
                //main.PortListGrid.Update();
                //main.PortListGrid.Refresh();

                //main.RemoveGridforIP(Ip);
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