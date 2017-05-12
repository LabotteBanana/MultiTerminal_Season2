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

        private IPEndPoint EP;
        public EndPoint clientEP;
        private Dictionary<int,EndPoint> m_clinetEPList = new Dictionary<int, EndPoint>();
        public Socket server;
        private bool m_isConnected = false;
        //private static Thread th = null;
        private IAsyncResult asyncResult;
        byte[] recv = new byte[1024];
        byte[] send = new byte[1024];
        public void Connect(MainForm form,int Port)
        {
            try
            {
                main = form;
                EP = new IPEndPoint(IPAddress.Any, Port);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
               IPEndPoint client;

                client = new IPEndPoint(IPAddress.Any, 0);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontRoute, 1);
                clientEP = (EndPoint)client;
                if (server.IsBound == false)
                {
                    server.Bind(EP);
                }
                server.BeginSendTo(send, 0, send.Length, SocketFlags.None, clientEP, new AsyncCallback(SendMessage), clientEP);
                server.BeginReceiveFrom(recv, 0, recv.Length, SocketFlags.None, ref clientEP, new AsyncCallback(RecvMessage), null);
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
                server.SendTo(data, data.Length, SocketFlags.None, EP);
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
            try {

                server.EndReceiveFrom(asyncResult,ref clientEP);
                server.BeginReceiveFrom(recv,0,recv.Length,SocketFlags.None,ref clientEP, new AsyncCallback(RecvMessage),null);

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


    }
}
