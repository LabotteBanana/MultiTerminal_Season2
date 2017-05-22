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
        public Socket server =null; //listening socket
        public Socket client = null;
        public GridView gridview = null;
        public List<GridView> gridlist = null;
        private string ip;
        private bool m_isConncted = false;
        private IPEndPoint ipep;
        private List< string> m_ipList = new List< string>();
        private int port;
        private Thread th;
        public NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        private List<NetworkStream> m_ns = new List< NetworkStream>();
        private List< StreamReader> m_sr = new List< StreamReader>();
        private List< StreamWriter> m_sw = new List< StreamWriter>();
        public List< string> m_NetInfo = new List<string>();
        public List< Socket> m_ClientList = new List< Socket>();
        public bool exitClickedState = false;

        public Tserv(MainForm Main,int Port, GridView GridView, List<GridView> GridList) //서버로 만들때
        {
            main = Main;
            port = Port;
            gridview = GridView;
            gridlist = GridList;
        }

        public Tserv(MainForm Main, string IP, int Port, GridView GridView, List<GridView> GridList) //클라로 만들때
        {
            main = Main;
            port = Port;
            ip = IP;
            gridview = GridView;
            gridlist = GridList;
        }

        public void ServerWait()
        {
            while (true)
            {
                try
                {
                    if (server != null)
                    {
                        if (server.IsBound == false)
                        {
                            server.Bind(ipep);

                            server.Listen(10);
                            m_isConncted = true;
                        }
                        Socket newclient = server.Accept();
                        //wsa blocking call
                        if (newclient != null)
                        {
                            IPEndPoint claIP = (IPEndPoint)newclient.RemoteEndPoint;
                            string client_ip = claIP.Address.ToString();
                            if (newclient.Connected == true)
                            {
                                m_ClientList.Add(newclient);
                                m_ipList.Add(client_ip);
                                NetworkStream ns = new NetworkStream(newclient);
                                StreamReader sr = new StreamReader(ns);
                                StreamWriter sw = new StreamWriter(ns);
                                m_ns.Add(ns);
                                m_sr.Add(sr);
                                m_sw.Add(sw);
                                Thread th = new Thread(new ThreadStart(RecvMsg)); //상대 문자열 수신 쓰레드 가동
                                th.Start();
                                //그리드뷰에 등록
                                string client_IP = client_ip;  // client ip 가져오기
                                
                                main.Invoke(new Action(() =>
                                {
                                    gridview = new GridView(gridlist.Count, client_IP, "TCP Client", m_ClientList.Count-1);  // 그리드뷰 객체에 적용,   타입형태(시리얼,UDP..), 타입의 순번도 그리드 객체로 슝들어감.    
                                    main.DrawGrid(gridview.MyNum, gridview.Type, gridview.Portname, gridview.Time);
                                    gridlist.Add(gridview);
                                }));
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

                    if (ex.ErrorCode == 10004)
                    {
                        System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생 \n Interrupted 펑션 콜");
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
                m_isConncted = true;
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
        public void ServerStop(int selectClient)
        {
            try
            {
                if (m_ClientList[selectClient].Connected == true)
                {
                     if (m_ns[selectClient] != null)
                    {
                        m_ns[selectClient].Close();
                        m_ns.RemoveAt(selectClient);
                    }
                    if (m_sr[selectClient] != null)
                    {
                        m_sr[selectClient].Close();
                        m_sr.RemoveAt(selectClient);
                    }
                    if (m_sw[selectClient] != null)
                    {
                        m_sw[selectClient].Close();
                        m_sw.RemoveAt(selectClient);
                    }
                    if (m_ClientList[selectClient] != null)
                    {
                        decreaseTypeNum(selectClient);
                        m_ClientList[selectClient].Shutdown(SocketShutdown.Both);
                        m_ClientList[selectClient].Disconnect(true);
                        m_ClientList[selectClient].Close();
                        m_ClientList.RemoveAt(selectClient);
                    }
                }
                m_isConncted = false;
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                if (ex.ErrorCode == 10057) //연결이 된 소켓이 없다.
                {
                    //클라이언트의 접속종료
                    if (server == null)
                    {
                        System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생 \n연결된 소켓이 없다.");
                        server.Close();
                        return;
                    }
                    if (client == null)
                    {
                        System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생 \n연결된 소켓이 없다.");
                        server.Close();
                        return;
                    }
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
                //수정 필요(for문에서 사용하는 m_clientCount. 0부터 순차적으로 사용되지 않는 경우가 있을 수 있음.
                for (int i = 0; i < gridlist.Count; i++)
                {
                    if (gridlist[i].Type == "TCP Client")
                    {
                        if (m_ClientList[0].Connected == true)
                        {
                            if (m_ns[0] != null)
                            {
                                m_ns[0].Close();
                                m_ns.RemoveAt(0);
                            }
                            if (m_sr[0] != null)
                            {
                                m_sr[0].Close();
                                m_sr.RemoveAt(0);
                            }
                            if (m_sw[0] != null)
                            {
                                m_sw[0].Close();
                                m_sw.RemoveAt(0);
                            }
                            if (m_ClientList[0] != null)
                            {
                                decreaseTypeNum(0);
                                m_ClientList[0].Shutdown(SocketShutdown.Both);
                                m_ClientList[0].Disconnect(true);
                                m_ClientList[0].Close();
                                m_ClientList.RemoveAt(0);
                            }
                        }
                    }
                }
                m_isConncted = false;
                server.Shutdown(SocketShutdown.Both);
                server.Disconnect(true);
                server.Close();
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                if (ex.ErrorCode == 10057) //연결이 된 소켓이 없다.
                {
                    //클라이언트의 접속종료
                    if (server == null)
                    {
                        System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생 \n연결된 소켓이 없다.");
                        server.Close();
                        return;
                    }
                    if (client == null)
                    {
                        System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생 \n연결된 소켓이 없다.");
                        server.Close();
                        return;
                    }
                }
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
            }

            catch (Exception ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
            }
        }
        #endregion
        private void decreaseTypeNum(int selectedNum)
        {
            for (int i = selectedNum+1; i < gridlist.Count; i++)
            {
                if(gridlist[i].Type == "TCP Server" || gridlist[i].Type == "TCP Client")
                    gridlist[i].Typenum--;
            }
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
                NetworkStream ns = new NetworkStream(client);
                StreamReader sr = new StreamReader(ns);
                StreamWriter sw = new StreamWriter(ns);

                Thread th = new Thread(new ThreadStart(RecvMsg)); //상대 문자열 수신 쓰레드 가동
                th.Start();
                m_isConncted = true;
                //     DisplayNetworkInfo();
                //그리드뷰에 등록
                int size = gridlist.Count;
                string server_IP = ip;  // client ip 가져오기

                main.Invoke(new Action(() =>
                {
                    gridview = new GridView(gridlist.Count, server_IP, "TCP Server", m_ClientList.Count);  // 그리드뷰 객체에 적용,   타입형태(시리얼,UDP..), 타입의 순번도 그리드 객체로 슝들어감.    
                    main.DrawGrid(gridview.MyNum, gridview.Type, gridview.Portname, gridview.Time);
                    gridlist.Add(gridview);
                }));
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
                    m_isConncted = false;
                    for (int i = 0; i < gridlist.Count; i++)
                    {
                        if (gridlist[i].Type == "TCP Server")
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
                m_isConncted = true;
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
                else if (server!=null)
                {
                    for (int i = 0; i < m_ClientList.Count; i++)
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
        public void setExitClickedState(bool exitClickedState) {
            this.exitClickedState = exitClickedState;
        }
        public void RecvMsg()
        {
            try
            {
                m_isConncted = true;
                ///Client의 Recv
                if (client != null)
                {
                    if(client.Connected ==false)
                    {
                        client.Shutdown(SocketShutdown.Both);
                        client.Disconnect(true);
                        m_isConncted = false;
                    }
                    while (client.Connected)
                    {
                        NetworkStream ns = new NetworkStream(client);
                        StreamReader sr = new StreamReader(ns);
                        StreamWriter sw = new StreamWriter(ns);
                        string msg = sr.ReadLine();
                        //임시조치.. 서버에서 disconnect하면 client로 null값이 계속 도는데 그거 처리하는 부분
                        if (msg == null)
                        {
                            if (exitClickedState)
                            {
                                exitClickedState = false;
                                break;
                            }
                            main.RemoveGridforIP(ip);
                            break;
                        }
                        if (main.InvokeRequired)
                        {
                            main.Invoke(new Action(() =>
                            {
                                main.ReceiveWindowBox.AppendText("수신{" + ip + "}" + main.GetTimer() + msg + "\n");
                                main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                                main.ReceiveWindowBox.ScrollToCaret();
                            }));
                        }
                        else
                        {
                            main.ReceiveWindowBox.AppendText("수신{" + ip + "}" + main.GetTimer() + msg + "\n");
                            main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                            main.ReceiveWindowBox.ScrollToCaret();
                        }
                    }
                }
                //Server의 receive
                else if(server!=null)
                {
                    int uniqueClientNum = m_ClientList.Count - 1;

                    m_isConncted = true;
                    while (m_ClientList[uniqueClientNum].Connected)
                    {
                        string msg = m_sr[uniqueClientNum].ReadLine();

                        //임시조치..
                        if (msg == null)
                        {
                            if (exitClickedState)
                            {
                                exitClickedState = false;
                                break;
                            }
                            main.RemoveGridforIP(m_ipList[uniqueClientNum]);
                            break;
                        }
                        if (main.InvokeRequired)
                        {
                            ///비정상 종료시 계속 되는이유
                            main.Invoke(new Action(() =>
                            {
                                main.ReceiveWindowBox.AppendText("수신{" + m_ipList[uniqueClientNum] + "}" + main.GetTimer() + msg + "\n");
                                main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                                main.ReceiveWindowBox.ScrollToCaret();
                            }));
                        }
                        else
                        {
                            main.ReceiveWindowBox.AppendText("수신{" + m_ipList[uniqueClientNum] + "}" + main.GetTimer() + msg + "\n");
                            main.ReceiveWindowBox.SelectionStart = main.ReceiveWindowBox.Text.Length;
                            main.ReceiveWindowBox.ScrollToCaret();
                        }
                    }/*
                    //클라이언트 종료감지
                    if (m_ClientList[uniqueClientNum].Connected == false)
                    {
                        m_ns.Remove(uniqueClientNum);
                        m_sr.Remove(uniqueClientNum);
                        m_sw.Remove(uniqueClientNum);
                        m_ipList.Remove(uniqueClientNum);
                        m_ClientList[uniqueClientNum].Shutdown(SocketShutdown.Both);
                        m_ClientList[uniqueClientNum].Disconnect(true);
                        m_ClientList.Remove(uniqueClientNum);
                    }*/
                }
            }
            catch (SocketException ex)
            {
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("소켓에러 " + lineNum + "에서 발생" + ex.Message);
            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                if(server !=null)
                {
                    //클라이언트 종료감지
                    if (ex.TargetSite.DeclaringType.Name == "NetworkStream")
                    {
                        for (int i = 0; i < m_ClientList.Count; i++)
                        {

                            if (m_ClientList[i].Connected == false)
                            {
                                m_ns.RemoveAt(i);
                                m_sr.RemoveAt(i);
                                m_sw.RemoveAt(i);
                                m_ipList.RemoveAt(i);
                                m_ClientList[i].Shutdown(SocketShutdown.Both);
                                m_ClientList[i].Disconnect(true);
                                m_ClientList.RemoveAt(i);
                            }
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
                m_NetInfo.Add(netInfo);
                netInfo = "";
                System.Windows.Forms.MessageBox.Show(m_NetInfo[i]);
                i++;
            }

        }

        #endregion
    }
}
