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
//
namespace MultiTerminal
{
    public class Tserv
    {
        MainForm main = null;
        public Socket server =null;
        public Socket client = null;
        private Thread th= null; //Recv가능 쓰레드
        private string ip;
        private string client_ip;
        private int port;
        public NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        private NetworkStream ns = null;
        private StreamReader sr = null;
        private StreamWriter sw = null;
            

        public Tserv(MainForm Main,int Port) //서버로 만들때
        {
            main = Main;
            port = Port;
        }

        public Tserv(MainForm Main,string IP,int Port) //클라로 만들때
        {
            main = Main;
            port = Port;
            ip = IP;
        }

        #region Server
        public void ServerStart()
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                server.Bind(ipep);
                server.Listen(30);//30초대기

                client = server.Accept();

                IPEndPoint claIP = (IPEndPoint)client.RemoteEndPoint;
                client_ip = claIP.Address.ToString();

                ns = new NetworkStream(client);
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);

                th = new Thread(new ThreadStart(RecvMsg)); //상대 문자열 수신 쓰레드 가동
                th.Start();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
        //채팅 서버 프로그램 중지
        public void ServerStop()
        {
            try
            {
                if (ns != null) ns.Close();
                if (sw != null) sw.Close();
                if (sr != null) sr.Close();
                if (client != null) client.Close();
                if ((th != null) && (th.IsAlive)) th.Abort();

                server.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
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
                client_ip = ip;

                ns = new NetworkStream(client);
                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);

                th = new Thread(new ThreadStart(RecvMsg));
                th.Start();

                return true;
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
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
                        if (ns != null) ns.Close();
                        if (sw != null) sw.Close();
                        if (sr != null) sr.Close();
                        client.Close();

                        if (th.IsAlive)
                            th.Abort();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        #endregion


        #region SendMsg,RecvMsg
        public void SendMsg(string msg)
        {
            try
            {
                if (client.Connected)
                {
                    sw.WriteLine(msg);
                    sw.Flush();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("전송 실패");
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());

            }
        }
        public void RecvMsg()
        {
            try
            {
                while (client.Connected)
                {

                    string msg = sr.ReadLine();
                    main.ReceiveWindowBox.Text += "수신 : " + main.GetTimer() + msg + "\n";
                    main.ReceiveWindowBox.ScrollToCaret();
                    /*
                    string msg = sr.ReadLine();
                    Global.MacroVar = "수신 : " + main.GetTimer() + msg + "\n";
                   */

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
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
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                GatewayIPAddressInformationCollection Gatewayaddress = adapterProperties.GatewayAddresses;
                IPAddressCollection dhcpServers = adapterProperties.DhcpServerAddresses;
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;

             
                main.ReceiveWindowBox.Text += "네트워크 카드 : "+adapter.Description;   //하드웨어 타입
                main.ReceiveWindowBox.Text += "Physical Address : " + adapter.GetPhysicalAddress(); //피지컬 주소
                main.ReceiveWindowBox.Text += "IP Address : " + Get_MyIP(); // 내 IP주소
              

                if (Gatewayaddress.Count > 0)
                {
                    foreach (GatewayIPAddressInformation address in Gatewayaddress)
                    {
                        main.ReceiveWindowBox.Text += "GateWay Address :" + address.Address.ToString(); //게이트웨이 주소
                    }
                }
                if (dhcpServers.Count > 0)
                {
                    foreach (IPAddress dhcp in dhcpServers)
                    {
                        main.ReceiveWindowBox.Text += "DHCP Servers : " + dhcp.ToString(); //DHCP 주소
                    }
                }
                if (dnsServers.Count > 0)
                {
                    foreach (IPAddress dns in dnsServers)
                    {
                        main.ReceiveWindowBox.Text += "DNS Servers : " + dns.ToString(); //DNS 주소
                    }
                }

            }
        }

        #endregion
    }
}
