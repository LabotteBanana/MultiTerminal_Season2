using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Collections;
//
namespace MultiTerminal
{
    public class Tserv
    {
        MainForm main = null;
        public Socket server = null; //listening socket
        public Socket client = null;
        private string ip;
        private IPEndPoint ipep;
        private Dictionary<int, string> m_ipList = new Dictionary<int, string>();
        private int port;
        private Thread th;
        public NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        private Dictionary<int, NetworkStream> m_ns = new Dictionary<int, NetworkStream>();
        private Dictionary<int, StreamReader> m_sr = new Dictionary<int, StreamReader>();
        private Dictionary<int, StreamWriter> m_sw = new Dictionary<int, StreamWriter>();
        public Dictionary<int, string> m_NetInfo = new Dictionary<int, string>();
        public Dictionary<int, Socket> m_ClientList = new Dictionary<int, Socket>();
        public Dictionary<int, Thread> m_ClientThread = new Dictionary<int, Thread>();
        public int m_clientCount = 0;

        public Tserv(MainForm Main, int Port) //서버로 만들때
        {
            main = Main;
            port = Port;
        }

        public Tserv(MainForm Main, string IP, int Port) //클라로 만들때
        {
            main = Main;
            port = Port;
            ip = IP;
        }

        public void ServerWait()
        {
            try
            {
                if (server != null)
                {
                    if (server.IsBound == false)
                    {
                        server.Bind(ipep);
                        server.Listen(10);
                    }
                    Socket newclient = server.Accept();
                    if (newclient != null)
                    {

                        IPEndPoint claIP = (IPEndPoint)newclient.RemoteEndPoint;
                        string client_ip = claIP.Address.ToString();
                        //for(int i = 0;  i<m_clientCount;i++)
                        //{
                        //    if (m_ipList[i] == client_ip)
                        //    {
                        //        m_clientCount--;
                        //        return;
                        //    }
                        //}
                        if (newclient.Connected == true)
                        {
                            m_clientCount++;
                            Thread th = new Thread(new ThreadStart(RecvMsg)); //상대 문자열 수신 쓰레드 가동
                            th.Start();
                            m_ClientThread.Add(m_clientCount - 1, th);
                            m_ClientList.Add(m_clientCount - 1, newclient);
                            m_ipList.Add(m_clientCount - 1, client_ip);
                            NetworkStream ns = new NetworkStream(newclient);
                            StreamReader sr = new StreamReader(ns);
                            StreamWriter sw = new StreamWriter(ns);
                            m_ns.Add(m_clientCount - 1, ns);
                            m_sr.Add(m_clientCount - 1, sr);
                            m_sw.Add(m_clientCount - 1, sw);
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                if (ex.ErrorCode == 10048)
                {
                    System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생 \n 동일한 ip나 port가 사용되고있다.");
                    return;
                }
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
            }

        }
        #region Server
        public void ServerStart()
        {
            try
            {
                ipep = new IPEndPoint(IPAddress.Any, port);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (server.IsBound == false)
                {
                    server.Bind(ipep);
                    server.Listen(10);
                }
                th = new Thread(new ThreadStart(ServerWait)); //상대 문자열 수신 쓰레드 가동
                th.Start();

                //       DisplayNetworkInfo();
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));

                if (ex.ErrorCode == 10048)
                {
                    System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생 \n 동일한 ip나 port가 사용되고있다.");
                    return;
                }
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);

            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
            }

        }
        //채팅 서버 프로그램 중지
        public void ServerStop()
        {
            try
            {

                for (int i = 0; i < m_clientCount; i++)
                {
                    if (m_ClientList[i].Connected == true)
                    {
                        if (m_ns[i] != null)
                        {
                            m_ns[i].Close();
                            m_ns.Remove(i);
                        }
                        if (m_sr[i] != null)
                        {
                            m_sr[i].Close();
                            m_sr.Remove(i);
                        }
                        if (m_sw[i] != null)
                        {
                            m_sw[i].Close();
                            m_sw.Remove(i);
                        }
                        if (m_ClientList[i] != null)
                        {
                            m_ClientList[i].Disconnect(true);
                            m_ClientList[i].Close();
                            m_ClientList.Remove(i);
                        }
                        if (m_ClientThread[i] != null)
                        {
                            if (m_ClientThread[i].ThreadState == ThreadState.Running)
                            {
                                m_ClientThread[i].Abort();
                            }
                            m_ClientThread.Remove(i);
                        }
                    }
                }
                if (server.IsBound == true)
                {
                    server.Shutdown(SocketShutdown.Both);
                    server.Disconnect(true);
                    server.Close();
                }
                else
                {
                    server.Shutdown(SocketShutdown.Both);
                    server.Disconnect(true);
                    server.Close();

                }
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                if (ex.ErrorCode == 10057) //연결이 된 소켓이 없다.
                {
                    System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생 \n연결된 소켓이 없다.");
                    server.Close();
                    return;
                }
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
            }
        }

        public void GetClaInfo()
        {
            for (int i = 0; i < m_clientCount; i++)
            {
                if (m_ClientList[i].Connected == false)
                {
                    m_ns[i].Close();
                    m_sw[i].Close();
                    m_sr[i].Close();
                    m_ClientList.Remove(i);
                    m_clientCount--;
                }
            }
        }
        #endregion 

        #region Client
        //채팅 서버와 연결 시도
        public bool Connect()
        {
            try
            {

                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ipep);
                NetworkStream ns = new NetworkStream(client);
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                Thread th = new Thread(new ThreadStart(RecvMsg)); //상대 문자열 수신 쓰레드 가동
                th.Start();
                //     DisplayNetworkInfo();
                return true;
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
                return false;
            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
                return false;
            }
        }
        //채팅 서버와의 연결 종료
        public void DisConnect()
        {
            try
            {
                if (client != null)
                {
                    if (client.Connected)
                    {
                        NetworkStream ns = new NetworkStream(client);
                        StreamReader sr = new StreamReader(ns);
                        StreamWriter sw = new StreamWriter(ns);

                        if (ns != null) ns.Close();
                        if (sw != null) sw.Close();
                        if (sr != null) sr.Close();
                        client.Shutdown(SocketShutdown.Both);
                        client.Disconnect(true);
                        client.Close();
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
        #endregion


        #region SendMsg,RecvMsg
        public void SendMsg(string msg)
        {
            try
            {
                ///Client의 Send

                if (client != null)
                {
                    if (client.Connected)
                    {
                        NetworkStream ns = new NetworkStream(client);
                        StreamReader sr = new StreamReader(ns);
                        StreamWriter sw = new StreamWriter(ns);
                        sw.WriteLine(msg);
                        sw.Flush();
                    }
                }
                else if (server != null)
                {
                    for (int i = 0; i < m_clientCount; i++)
                    {
                        m_sw[i].WriteLine(msg);
                        m_sw[i].Flush();

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
        public void RecvMsg()
        {
            try
            {
                ///Client의 Recv
                if (client != null)
                {
                    if (client.Connected == false)
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Disconnect(true);
                    }
                    while (client.Connected)
                    {
                        NetworkStream ns = new NetworkStream(client);
                        StreamReader sr = new StreamReader(ns);
                        StreamWriter sw = new StreamWriter(ns);
                        string msg = sr.ReadLine();
                        if (main.InvokeRequired)
                        {
                            main.Invoke(new Action(() => main.ReceiveWindowBox.AppendText("수신 : " + main.GetTimer() + msg + "\n");
                            main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                            main.ReceiveWindowBox.ScrollToCaret();
                    ));
                        }
                        else
                        {
                            main.ReceiveWindowBox.AppendText("수신 : " + main.GetTimer() + msg + "\n");
                            main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                            main.ReceiveWindowBox.ScrollToCaret();
                    
                        }
                    }
                }
                else if (server != null)
                {
                    for (int i = 0; i < m_clientCount; i++)
                    {

                        while (m_ClientList[i].Connected)
                        {
                            string msg = m_sr[i].ReadLine();

                            if (main.InvokeRequired)
                            {
                                main.Invoke(new Action(() => main.ReceiveWindowBox.AppendText("수신 : " + main.GetTimer() + msg + "\n");
                                main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                                main.ReceiveWindowBox.ScrollToCaret();));
                            }
                            else
                            {
                    main.ReceiveWindowBox.AppendText("수신 : " + main.GetTimer() + msg + "\n");
                    main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                    main.ReceiveWindowBox.ScrollToCaret();
                }
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
                if (ex.TargetSite.DeclaringType.Name == "NetworkStream")
                {
                    for (int i = 0; i < m_clientCount; i++)
                    {

                        if (m_ClientList[i].Connected == false)
                        {
                            m_ns.Remove(i);
                            m_sr.Remove(i);
                            m_sw.Remove(i);
                            m_ipList.Remove(i);
                            m_ClientList[i].Shutdown(SocketShutdown.Both);
                            m_ClientList[i].Disconnect(true);
                            m_ClientList.Remove(i);
                            m_clientCount--;
                        }
                    }
                }
                else
                {
                    int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                    System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
                }
            }
        }
        #endregion

        #region Socket State
        public void GetState()
        {

        }
        public void SetState()
        {

        }
        public void SetDelay(int timer)
        {
            this.server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, timer);
        }
        public string Get_MyIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string myip = host.AddressList[0].ToString();
            return myip;
        }
        public void DisplayNetworkInfo()
        {
            string netInfo = null;
            int i = 0;
            foreach (NetworkInterface adapter in adapters)
            {

                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                GatewayIPAddressInformationCollection Gatewayaddress = adapterProperties.GatewayAddresses;
                IPAddressCollection dhcpServers = adapterProperties.DhcpServerAddresses;
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;


                netInfo += "네트워크 카드 : " + adapter.Description + "\n";   //하드웨어 타입
                netInfo += "Physical Address : " + adapter.GetPhysicalAddress() + "\n"; //피지컬 주소
                netInfo += "IP Address : " + Get_MyIP() + "\n"; // 내 IP주소


                if (Gatewayaddress.Count > 0)
                {
                    foreach (GatewayIPAddressInformation address in Gatewayaddress)
                    {
                        netInfo += "GateWay Address :" + address.Address.ToString() + "\n"; //게이트웨이 주소
                    }
                }
                if (dhcpServers.Count > 0)
                {
                    foreach (IPAddress dhcp in dhcpServers)
                    {
                        netInfo += "DHCP Servers : " + dhcp.ToString() + "\n"; //DHCP 주소
                    }
                }
                if (dnsServers.Count > 0)
                {
                    foreach (IPAddress dns in dnsServers)
                    {
                        netInfo += "DNS Servers : " + dns.ToString() + "\n"; //DNS 주소
                    }
                }
                m_NetInfo.Add(i, netInfo);
                netInfo = "";
                System.Windows.Forms.MessageBox.Show(m_NetInfo[i]);
                i++;
            }

        }

        #endregion
    }
}
