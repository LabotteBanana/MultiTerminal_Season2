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
        public Socket server =null;
        public Socket client = null;
        private IPEndPoint ipep;
        private Dictionary<int, string> m_ipList = new Dictionary<int, string>();
        private int port;
        private Thread AcceptTh;
        public NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        private Dictionary<int,NetworkStream> m_ns = new Dictionary<int, NetworkStream>();
        private Dictionary<int, StreamReader> m_sr = new Dictionary<int, StreamReader>();
        private Dictionary<int, StreamWriter> m_sw = new Dictionary<int, StreamWriter>();
        public Dictionary<int, string> m_NetInfo = new Dictionary<int, string>();
        public Dictionary<int, Socket> m_ClientList = new Dictionary<int, Socket>();
        public Dictionary<int, Thread> m_ClientThread = new Dictionary<int, Thread>();
        public int m_clientCount = 0;

        public Tserv(MainForm Main,int Port) //서버로 만들때
        {
            main = Main;
            port = Port;
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
                        server.Listen(30);
                    }
                    Socket newclient = server.Accept();
                    newclient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    ///동일한 접속에 대한 예외처리가 필요하다.
                    if (newclient != null)
                    {
                        IPEndPoint claIP = (IPEndPoint)newclient.RemoteEndPoint;
                        string client_ip = claIP.Address.ToString();

                        if (newclient.Connected == true)
                        {
                            Thread th = new Thread(new ThreadStart(RecvMsg)); //상대 문자열 수신 쓰레드 가동
                            th.Start();

                            for (int i = 0; i < m_clientCount; i++)
                            {
                                if (m_ipList[i] == client_ip)
                                {
                                    th.Abort();
                                    return;
                                }
                            }
                            m_clientCount++;

                            m_ClientThread.Add(m_clientCount-1, th);
                            m_ClientList.Add(m_clientCount-1, newclient);
                            m_ipList.Add(m_clientCount-1, client_ip);
                            NetworkStream ns = new NetworkStream(newclient);
                            StreamReader sr = new StreamReader(ns);
                            StreamWriter sw = new StreamWriter(ns);
                            m_ns.Add(m_clientCount-1, ns);
                            m_sr.Add(m_clientCount-1, sr);
                            m_sw.Add(m_clientCount-1, sw);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show(lineNum + "에서 발생" + ex.Message);
            }
        }
        #region Server
        public void ServerStart()
        {
            try
            {
                ipep = new IPEndPoint(IPAddress.Any, port);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(ipep);
                server.Listen(30);//30초대기
                server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress , true);
                AcceptTh = new Thread(new ThreadStart(ServerWait)); //상대 문자열 수신 쓰레드 가동
                AcceptTh.Start();

                //       DisplayNetworkInfo();
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러" + lineNum + "에서 발생" + ex.Message);
            }
        }

        //채팅 서버 프로그램 중지
        public void ServerStop()
        {
            try
            {
                for (int i = 0; i < m_clientCount; i++)
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
                        m_ClientList.Remove(i);
                    }
                    if (m_ClientThread[i] != null)
                    {
                        m_ClientThread[i].Abort();
                        m_ClientThread.Remove(i);
                    }
                }
                //AcceptTh = null;
                server.Shutdown(SocketShutdown.Both);
                server.Disconnect(true);
                server.Close();
            }
            catch (SocketException ex)
            {

                if (ex.ErrorCode == 10057)
                {
                    LingerOption linger = new LingerOption(false,3);
                    server.LingerState = linger;
                }
            }
            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show(lineNum+"에서 발생" +ex.Message);
            }
        }

        public void GetClaInfo()
        {
            for(int i = 0; i<m_clientCount;i++)
            {
                if(m_ClientList[i].Connected==false)
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

       


        #region SendMsg,RecvMsg
        public void SendMsg(string msg)
        {
            try
            {
                ///Client의 Send

                if (server != null && m_clientCount > 0)
                {
                        for (int i = 0; i < m_clientCount; i++)
                        {
                            if (m_ClientList[i].Connected == true)
                            {
                                m_sw[i].WriteLine(msg);
                                m_sw[i].Flush();
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
                System.Windows.Forms.MessageBox.Show("기타에러" + lineNum + "에서 발생" + ex.Message);
            }
        }
        public void RecvMsg()
        {
            try
            {
                ///server의 Recv
                if (server != null)
                {
                    for (int i = 0; i < m_clientCount; i++)
                    {
                        while (m_ClientList[i].Connected)
                        {
                            string msg = m_sr[i].ReadLine();

                            if (main.InvokeRequired)
                            {
                                main.Invoke(new Action(() => main.ReceiveWindowBox.Text += "수신 : " + main.GetTimer() + msg + "\n"));
                            }
                            else
                            {
                                main.ReceiveWindowBox.Text += "수신 : " + main.GetTimer() + msg + "\n";
                            }
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                ///ex.int
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
                            m_ClientList[i].Disconnect(true);
                            m_ClientList.Remove(i);
                            m_clientCount--;
                        }
                    }
                }
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
                            m_ClientList[i].Disconnect(true);
                            m_ClientList.Remove(i);
                            m_clientCount--;
                        }
                    }
                }
                else
                {
                    int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                    System.Windows.Forms.MessageBox.Show(lineNum + "에서 발생" + ex.Message);
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


                netInfo += "네트워크 카드 : "+adapter.Description+"\n";   //하드웨어 타입
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
