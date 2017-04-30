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
namespace MultiTerminal
{
    public class Tcla
    {
        MainForm main = null;
        public Socket client = null;
        private string ip;
        private IPEndPoint ipep;
        private int port;
        public NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        private NetworkStream m_ns;
        private StreamReader m_sr;
        private StreamWriter m_sw;
        public Dictionary<int, string> m_NetInfo = new Dictionary<int, string>();

        public Tcla(MainForm Main, string IP, int Port) //클라로 만들때
        {
            main = Main;
            port = Port;
            ip = IP;
        }

        #region Client
        //채팅 서버와 연결 시도
        public bool Connect()
        {
            try
            {

                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ipep);
                client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                m_ns = new NetworkStream(client);
                m_sr = new StreamReader(m_ns);
                m_sw = new StreamWriter(m_ns);

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
                System.Windows.Forms.MessageBox.Show("기타에러" + lineNum + "에서 발생" + ex.Message);
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

                        if (m_ns != null) m_ns.Close();
                        if (m_sw != null) m_sw.Close();
                        if (m_sr != null) m_sr.Close();
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
                System.Windows.Forms.MessageBox.Show("기타에러" + lineNum + "에서 발생" + ex.Message);
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
                        m_sw.WriteLine(msg);
                        m_sw.Flush();
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
                ///Client의 Recv
                if (client != null)
                {
                    ///???
                    if (client.Connected == false)
                    {
                        client.Disconnect(true);
                    }
                    else
                    {
                        while (client.Connected)
                        {
                            string msg = m_sr.ReadLine();
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
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);

                if (ex.TargetSite.DeclaringType.Name == "NetworkStream")
                {
                        if (client.Connected == false)
                        {
                        client.Disconnect(true);
                        }

                }
            }
            catch (Exception ex)
            {
                if (ex.TargetSite.DeclaringType.Name == "NetworkStream")
                {

                    if (client.Connected == false)
                    {
                        client.Disconnect(true);
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

    }
}
