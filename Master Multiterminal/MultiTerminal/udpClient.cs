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
    public class udpClient
    {
        MainForm main = null;

        public Socket client;
        private IPEndPoint serverEP;
        private IPEndPoint Sender;
        private EndPoint remoteEP;
        public bool m_isConnected = false;
        private static Thread Recvth = null;
        public bool bSend = false;
        public int bRecv = 0;
        public void Connect(MainForm form, string IP, int port)
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
                bSend = true;
                SendMessage("서버로 접속");
                m_isConnected = true;

                bSend = false;
                Recvth = new Thread(new ThreadStart(RecvMessage)); //상대 문자열 수신 쓰레드 가동
                m_isConnected = true;
                Recvth.Start();
                if (main.InvokeRequired)
                {
                    // 그리드뷰 객체에 적용,   타입형태(시리얼,UDP..), 타입의 순번도 그리드 객체로 슝들어감.    

                    main.Invoke(new Action(() => main.gridview[main.GridList.Count] = new GridView(main.GridList.Count, port.ToString(), "UDPServer", main.GridList.Count)));
                    main.Invoke(new Action(() => main.DrawGrid(main.gridview[main.GridList.Count].MyNum, main.gridview[main.GridList.Count].Type, main.gridview[main.GridList.Count].Portname, main.gridview[main.GridList.Count].Time)));
                    main.Invoke(new Action(() => main.GridList.Add(main.gridview[main.GridList.Count])));
                }
                else
                {
                    main.gridview[main.GridList.Count] = new GridView(main.GridList.Count, port.ToString(), "UDPServer", main.GridList.Count);
                    main.DrawGrid(main.gridview[main.GridList.Count].MyNum, main.gridview[main.GridList.Count].Type, main.gridview[main.GridList.Count].Portname, main.gridview[main.GridList.Count].Time);
                    main.GridList.Add(main.gridview[main.GridList.Count]);
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
                        byte[] data = new byte[1024];
                        if (client != null)
                        {
                        if (bRecv == 1)
                        {
                            if (main.RowIndex >= 0)
                            {
                                    int recvi = client.ReceiveFrom(data, data.Length, SocketFlags.None, ref remoteEP);
                                    string recvMsg = Encoding.Default.GetString(data);
                                    if (recvi > 0)
                                    {
                                        m_isConnected = true;
                                    }
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
                        else
                        {
                            return;
                        }

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
        public void SendMessage(string sendMsg)
        {
            try
            {
                if (bSend == true)
                {
                    byte[] data = new byte[1024];
                    data = Encoding.Default.GetBytes(sendMsg);

                    if (client != null)
                        client.SendTo(data, data.Length, SocketFlags.None, serverEP);
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
        public void DisConnect()
        {
            if (client != null)
            {
                m_isConnected = false;
                //client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
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