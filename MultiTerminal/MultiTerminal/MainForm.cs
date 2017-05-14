using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Management;
using System.IO;
using System.Text;
namespace MultiTerminal
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        public bool isServ = false;
        public int connectType = 1;
        public Tserv tserv = null;
        public Tserv tcla = null;
        public udpServer userv = new udpServer();
        public udpClient ucla = new udpClient();
        public static Thread macroThread;
        public static Thread SendThread;
        public static Thread AcceptThread;
        public static Thread RecvThread;
        public delegate void TRecvCallBack();

        // 체크박스 부분
        static public int Chk_Hexa_Flag = 0;

        public Serial[] serial = new Serial[9];
        public int Sport_Count = 0;
        public int[] Serial_Send_Arr = new int[8];       // 시리얼 선택적 송신 체크 옵션
        public int[] Serial_Receive_Arr = new int[8];    // 시리얼 선택적 수신 체크 옵션
        private string[] SerialOpt = new string[6];


        public System.Timers.Timer timer = null;
        Stopwatch sw = new Stopwatch();
        public static System.Timers.Timer mactimer = null;
        public System.Timers.Timer aftertimer = null;
        private DateTime nowTime;

        private List<string> connetName = new List<string>();

        public MainForm()
        {

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)  // 폼 열렸을 때
        {
            this.Style = MetroFramework.MetroColorStyle.Yellow;

            UI_Init();
        }

        #region Timer(타임스탬프)
        private void OnTimeEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            nowTime = e.SignalTime; //현재시분초
        }

        private void RecvEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (connectType == 6)
            {
                if (userv != null)
                {
                    //if (userv.server != null)
                    //    if (userv.server.IsBound == true)
                    //        userv.RecvMessage();
                }
                if (ucla != null)
                {
                    if (ucla.client != null)
                        if (ucla.m_isConnected == true)
                            ucla.RecvMessage();
                }
            }
        }
        private void WaitAccept(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (connectType == 5)
            {
                try
                {
                    if (tserv != null)
                    {
                        if (isServ == true)
                        {
                            tserv.ServerWait();
                            //AcceptThread = new Thread(new ThreadStart(delegate ()
                            //{
                            //    this.Invoke(new Action(() =>
                            //    {

                            //    }));
                            //}));
                            //AcceptThread.Start();

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            if (connectType == 6)
            {
                try
                {
                    if (userv != null)
                    {
                        if (isServ == true)
                        {

                            //udp서버 대기
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void OnMacro(Object soruce, System.Timers.ElapsedEventArgs e)
        {
            if (connectType == 2)
            {
                ///여기에 시리얼 센드부분
                try
                {
                    /*
                    if (Flag_AEAS[0] == 0)
                    {
                    */
                    serial[0].SerialSend(this.SendBox1.Text);

                    /*
                    else if (Flag_AEAS[0] == 1)
                    {
                        serial.SerialSend(SendBox1.Text.Insert(SendBox1.Text.Length, "\n"));
                    }
                    else
                    {
                        serial.SerialSend(SendBox1.Text.Insert(SendBox1.Text.Length, " "));
                    }
                    */
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (connectType == 5)
            {
                try
                {
                    if (tserv != null)
                    {
                        if (isServ == true && tserv.client.Connected == true)
                        {
                            SendThread = new Thread(new ThreadStart(delegate ()
                            {
                                this.Invoke(new Action(() =>
                                {
                                    tserv.SendMsg(SendBox1.Text);
                                    ReceiveWindowBox.Text += "송신 : " + GetTimer() + SendBox1.Text + "\n";
                                }));
                            }));
                            SendThread.Start();

                        }
                    }
                    if (tcla != null)
                    {
                        if (isServ == false && tcla.client.Connected == true)
                        {
                            SendThread = new Thread(new ThreadStart(delegate ()
                            {
                                this.Invoke(new Action(() =>
                                {
                                    tcla.SendMsg(SendBox1.Text);

                                    ReceiveWindowBox.Text += "송신 : " + GetTimer() + SendBox1.Text + "\n";
                                }));
                            }));
                            SendThread.Start();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            if (connectType == 6)
            {
                try
                {
                    if (userv != null)
                    {
                        if (isServ == true)
                        {
                            SendThread = new Thread(new ThreadStart(delegate ()
                            {
                                this.Invoke(new Action(() =>
                                {
                                    //userv.SendMessage(SendBox1.Text);
                                    byte[] send = Encoding.UTF8.GetBytes(SendBox1.Text);

                                    //userv.server.BeginSendTo(send, 0, send.Length, System.Net.Sockets.SocketFlags.None, userv.senderEP, new AsyncCallback(userv.SendMessage), null);
                                    //userv.SendMessage(SendBox1.Text);
                                    ReceiveWindowBox.Text += "송신 : " + GetTimer() + SendBox1.Text + "\n";
                                }));
                            }));
                            SendThread.Start();

                        }
                    }
                    if (ucla != null)
                    {
                        if (isServ == false)
                        {
                            SendThread = new Thread(new ThreadStart(delegate ()
                            {
                                this.Invoke(new Action(() =>
                                {
                                    ucla.SendMessage(SendBox1.Text);

                                    ReceiveWindowBox.Text += "송신 : " + GetTimer() + SendBox1.Text + "\n";
                                }));
                            }));
                            SendThread.Start();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        public string GetTimer()
        {

            string now = null;
            now = "[ " + nowTime.Hour + "::" + nowTime.Minute + "::" + nowTime.Second + "::" + nowTime.Millisecond + "]";
            return now;
        }


        public void SetMacroTime(int count)
        {
            // 초당 10번이면 100/1000
            // 초당 5번 이면 50/1000
            mactimer.Enabled = true;
            mactimer.Interval = count;

        }

        //public void AfterTime(double perSec)
        //{
        //    // 초당 10번이면 100/1000
        //    // 초당 5번 이면 50/1000
        //    aftertimer.Interval = perSec * 1000;
        //    aftertimer.Enabled = true;

        //}

        #endregion

        private void MainForm_Closed(object sender, FormClosedEventArgs e)  // 메인폼 닫혔을 때 
        {

            //serial.DisConSerial();
            tserv.ServerStop();
            tcla.DisConnect();

        }


        #region 버튼부분 입니당 ^-^         

        // 연결 방법 선택 1 ~ 6 및 박스 색깔 변경 //
        private void UART_Tile_Click(object sender, EventArgs e)
        {
            OptionSelect(2);
            this.UART_Tile.Style = MetroFramework.MetroColorStyle.Pink; // 클릭시 박스 색 변경
            this.TCP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
            this.UDP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
            this.Serial_Combo_Port.DropDownWidth = GetLargestTextEntent();
        }
        private void TCP_Tile_Click(object sender, EventArgs e)
        {
            OptionSelect(5);
            isServ = false;

            this.UART_Tile.Style = MetroFramework.MetroColorStyle.Silver; // 클릭시 박스 색 변경
            this.TCP_Tile.Style = MetroFramework.MetroColorStyle.Pink;
            this.UDP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
        }
        private void CheckMacro()
        {
        }
        private void UDP_Tile_Click(object sender, EventArgs e)
        {
            OptionSelect(6);
            this.UART_Tile.Style = MetroFramework.MetroColorStyle.Silver; // 클릭시 박스 색 변경
            this.TCP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
            this.UDP_Tile.Style = MetroFramework.MetroColorStyle.Pink;
        }

        // 연결 번호에 따른 각기 다른 옵션패널 띄우는 함수 //
        private void OptionSelect(int OptionNumber)  // 연결 버튼
        {

            Point Loc = new Point(0, 3);

            switch (OptionNumber)
            {
                case 1:
                    {
                        connectType = 1;
                        this.SerialPanel.Visible = false;
                        if (tserv != null)
                            tserv.ServerStop();
                        if (tcla != null)
                            tcla.DisConnect();
                        if (userv != null)
                            userv.DisConnect();
                        if (ucla != null)
                            ucla.DisConnect();

                        break;
                    }
                case 3:
                    {
                        connectType = 3;
                        if (tserv != null)
                            tserv.ServerStop();
                        if (tcla != null)
                            tcla.DisConnect();
                        if (userv != null)
                            userv.DisConnect();
                        if (ucla != null)
                            ucla.DisConnect();

                        break;
                    }
                case 2:
                    {
                        connectType = 2;
                        SerialPanel.Location = Loc;
                        this.SerialPanel.Visible = true;    // 시리얼 패널 보이기

                        TcpPanel.Visible = false;
                        UdpPanel.Visible = false;
                        Serial_Combo_Init();
                        if (tserv != null)
                            tserv.ServerStop();
                        if (tcla != null)
                            tcla.DisConnect();
                        if (userv != null)
                            userv.DisConnect();
                        if (ucla != null)
                            ucla.DisConnect();


                    }
                    break;
                case 5:
                    {
                        connectType = 5;
                        TcpPanel.Location = Loc;
                        TcpPanel.Visible = true;
                        SerialPanel.Visible = false;
                        UdpPanel.Visible = false;
                        if (userv != null)
                            userv.DisConnect();
                        if (ucla != null)
                            ucla.DisConnect();

                    }
                    break;
                case 6:
                    {
                        connectType = 6;
                        UdpPanel.Location = Loc;
                        SerialPanel.Visible = false;
                        TcpPanel.Visible = false;
                        UdpPanel.Visible = true;
                        if (tserv != null)
                            tserv.ServerStop();
                        if (tcla != null)
                            tcla.DisConnect();
                    }
                    break;
            }
        }

        private void DisConBtn_Click(object sender, EventArgs e)    // 연결 해제 버튼
        {
            if (connectType == 2) //시리얼
            {
                //serial.DisConSerial();
            }
        }
        #endregion

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();
        }

        #region 시리얼 설정 부분~!
        // 시리얼 설정 부분 선택지    
        private void Serial_Combo_Init()
        {

            // 시리얼 옵션 콤보박스 초기화

            this.Serial_Combo_Port.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Serial_Combo_Baud.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Serial_Combo_Data.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Serial_Combo_FlowCon.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Serial_Combo_Parity.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Serial_Combo_StopBit.DropDownStyle = ComboBoxStyle.DropDownList;


            // 이 부분에서 포트 없는 상태에서 불러올때마다 에러생기는 듯. 조건식 필요~!
            List<string> data = new List<string>();
            foreach (string s in SerialPort.GetPortNames())
            {
                data.Add(s);
            }
            Serial_Combo_Port.Items.AddRange(data.Cast<object>().ToArray());

            using (var searcher = new ManagementObjectSearcher
               ("SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                //상세한 이름 가져오기
                var tList = (from n in portnames
                             join p in ports on n equals p["DeviceID"].ToString()
                             select " - " + p["Caption"]).ToList();
                //지원하는 포트 이름만 가져오기(비교해서 위에 있는 놈 붙여넣으려고)
                var cmpList = (from n in portnames
                               join p in ports on n equals p["DeviceID"].ToString()
                               select n).ToList();
                //usb이름 가져오는 녀석
                foreach (string s in cmpList)
                {
                    for (int i = 0; i < Serial_Combo_Port.Items.Count; i++)
                    {
                        try
                        {
                            int a = Serial_Combo_Port.Items.IndexOf(s);
                            Serial_Combo_Port.Items[a] += tList[i];
                        }
                        catch (ArgumentException e)
                        {

                        }
                    }
                }
                //comport 이름 가져오는 녀석
                foreach (COMPortInfo comPort in COMPortInfo.GetCOMPortsInfo())
                {
                    string[] comportName = comPort.Name.Split('-');
                    int a = Serial_Combo_Port.Items.IndexOf(comportName[0]);
                    Serial_Combo_Port.Items[a] += " - " + comPort.Description;
                }
            }
            if (Serial_Combo_Port.Items.Count != 0)
                Serial_Combo_Port.SelectedIndex = 0;

            //로그 분석할거양
            for (int i = 0; i < Serial_Combo_Port.Items.Count; i++)
            {
                connetName.Add(Serial_Combo_Port.Items[i].ToString());
            }

            List<string> data2 = new List<string>();
            string[] Baud = { "4800", "9600", "14400", "19200" };
            foreach (string s in Baud)
            {
                data2.Add(s);
            }
            Serial_Combo_Baud.Items.AddRange(data2.Cast<object>().ToArray());
            Serial_Combo_Baud.SelectedIndex = 0;

            List<string> data3 = new List<string>();
            string[] Data = { "7", "8" };
            foreach (string s in Data)
            {
                data3.Add(s);
            }
            Serial_Combo_Data.Items.AddRange(data3.Cast<object>().ToArray());
            Serial_Combo_Data.SelectedIndex = 1;

            List<string> data4 = new List<string>();
            string[] Parity = { "none", "odd", "even", "mark", "space" };
            foreach (string s in Parity)
            {
                data4.Add(s);
            }
            Serial_Combo_Parity.Items.AddRange(data4.Cast<object>().ToArray());
            Serial_Combo_Parity.SelectedIndex = 0;

            List<string> data5 = new List<string>();
            string[] Stopbit = { "none", "1 bit", "2 bit", "1.5 bit" };
            foreach (string s in Stopbit)
            {
                data5.Add(s);
            }
            Serial_Combo_StopBit.Items.AddRange(data5.Cast<object>().ToArray());
            Serial_Combo_StopBit.SelectedIndex = 1;

            List<string> data6 = new List<string>();
            string[] FlowCon = { "none", "Xon/Xoff", "hardware" };
            foreach (string s in FlowCon)
            {
                data6.Add(s);
            }
            Serial_Combo_FlowCon.Items.AddRange(data6.Cast<object>().ToArray());
            Serial_Combo_FlowCon.SelectedIndex = 0;
        }

        // 선택시 이벤트
        private void Serial_Combo_Port_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] portName = Serial_Combo_Port.Text.Split(' ');
            SerialOpt[0] = portName[0];
        }

        private void Serial_Combo_Baud_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialOpt[1] = Serial_Combo_Baud.Text;
        }

        private void Serial_Combo_Data_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialOpt[2] = Serial_Combo_Data.Text;
        }

        private void Serial_Combo_Parity_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialOpt[3] = Serial_Combo_Parity.Text;
        }

        private void Serial_Combo_StopBit_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialOpt[4] = Serial_Combo_StopBit.Text;
        }

        private void Serial_Combo_FlowCon_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialOpt[5] = Serial_Combo_FlowCon.Text;
        }

        private void Serial_Btn_OK_Click(object sender, EventArgs e)    // 시리얼 오~픈~!!
        {
            try
            {
                serial[Sport_Count] = new Serial();
                serial[Sport_Count].SerialOpen(SerialOpt[0], SerialOpt[1], SerialOpt[2], SerialOpt[3], SerialOpt[4], "500", "500");
                serial[Sport_Count].sPort.DataReceived += new SerialDataReceivedEventHandler(UpdateWindowText);
                if (serial[Sport_Count].IsOpen())
                {
                    Sport_Count++;
                    switch (Sport_Count)
                    {
                        case 1: Sport_label1.Visible = true; Serial_select_CHK1.Visible = true; Serial_select_CHK11.Visible = true; break;
                        case 2: Sport_label2.Visible = true; Serial_select_CHK2.Visible = true; Serial_select_CHK22.Visible = true; break;
                        case 3: Sport_label3.Visible = true; Serial_select_CHK3.Visible = true; Serial_select_CHK33.Visible = true; break;
                        case 4: Sport_label4.Visible = true; Serial_select_CHK4.Visible = true; Serial_select_CHK44.Visible = true; break;
                        case 5: Sport_label5.Visible = true; Serial_select_CHK5.Visible = true; Serial_select_CHK55.Visible = true; break;
                        case 6: Sport_label6.Visible = true; Serial_select_CHK6.Visible = true; Serial_select_CHK66.Visible = true; break;
                        case 7: Sport_label7.Visible = true; Serial_select_CHK7.Visible = true; Serial_select_CHK77.Visible = true; break;
                        case 8: Sport_label8.Visible = true; Serial_select_CHK8.Visible = true; Serial_select_CHK88.Visible = true; break;
                    }
                }

            }
            catch (Exception E)
            {
                MessageBox.Show(E.ToString());
            }



        }
        #endregion


        #region 리치 텍스트 박스

        // 수신 텍스트박스 업데이트 이벤트
        public void UpdateWindowText(object sender, SerialDataReceivedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                this.Invoke(new Action(() =>
                {
                    this.ReceiveWindowBox.Text = Global.globalVar;
                    this.ReceiveWindowBox.ScrollToCaret();
                }));
            }));
            thread.Start();
        }

        #endregion

        #region TCP UI
        private void button3_Click(object sender, EventArgs e)
        {

        }
        #region TCP서버여부
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isServ == false)
            {
                IpNumber.Enabled = false;
                isServ = true;
                mactimer.Elapsed += WaitAccept;


            }
            else if (ServerCheck.Checked == false)
            {
                IpNumber.Enabled = true;
                isServ = false;
            }


        }
        #endregion

        #region TCP 로그
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (isServ == true && tserv.client.Connected == true)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        tserv.SendMsg(SendBox1.Text);

                        ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox1.Text + "\n");
                        ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
                        ReceiveWindowBox.ScrollToCaret();

                    }
                }
                else if (isServ == false && tcla.client.Connected == true)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        tcla.SendMsg(SendBox1.Text);

                        ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox1.Text + "\n");
                        ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
                        ReceiveWindowBox.ScrollToCaret();

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #endregion

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            int count = Int32.Parse(MacroCount.Text);

            if (MacroCheck.CheckState == CheckState.Checked)
            {
                macroThread = new Thread(() => SetMacroTime(count));

                mactimer.Elapsed += OnMacro;
                mactimer.Enabled = false;
                macroThread.Start();
            }


            else
            {
                MacroCheck.CheckState = CheckState.Unchecked;
                sw.Stop();
                mactimer.Enabled = false;
                mactimer.Elapsed -= OnMacro;
                mactimer.Enabled = false;
                SendThread.Abort();
                macroThread.Abort();
            }
        }

        #region UI 초기화
        private void UI_Init()
        {
            TcpPanel.Visible = false;
            UdpPanel.Visible = false;
            SerialPanel.Visible = false;


            TcpPanel.Visible = false;

            timer = new System.Timers.Timer();
            mactimer = new System.Timers.Timer();
            aftertimer = new System.Timers.Timer();
            timer.Interval = 0.0001; // 1000==>1초 0.0001==>1000만분의1
            timer.Enabled = true;
            mactimer.Enabled = true;
            timer.AutoReset = true;
            mactimer.AutoReset = true;

            aftertimer.Enabled = true;
            aftertimer.AutoReset = true;
            timer.Elapsed += OnTimeEvent;
            timer.Elapsed += RecvEvent;
            timer.Elapsed += WaitAccept;
        }
        #endregion
        #region 보내기 버튼 묶음

        private void Btn_Send1_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectType == 2)
                {
                    //serial[0].SerialSend(SendBox1.Text);
                    // 우선 버튼 1에만 멀티 전송 구현
                    Sport_Num_Select_Send(serial, Serial_Send_Arr, SendBox1.Text);
                    ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox1.Text + "\n");
                    ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
                    ReceiveWindowBox.ScrollToCaret();
                }
                if (connectType == 5)
                {
                    if (isServ == true && tserv.m_clientCount > 0)
                    {
                        tserv.SendMsg(SendBox1.Text);
                        ReceiveWindowBox.Text += "송신 : " + GetTimer() + SendBox1.Text + "\n";
                    }
                    if (isServ == false && tcla.client.Connected == true)
                    {
                        tcla.SendMsg(SendBox1.Text);
                        ReceiveWindowBox.Text += "송신 : " + GetTimer() + SendBox1.Text + "\n";
                    }
                }
                if (connectType == 6)
                {
                    if (isServ == true)
                    {
                        SendThread = new Thread(new ThreadStart(delegate ()
                          {
                              this.Invoke(new Action(() =>
                              {
                                  byte[] send = Encoding.UTF8.GetBytes(SendBox1.Text);

                                  //userv.SendMessage(SendBox1.Text);
                                  ReceiveWindowBox.Text += "송신 : " + GetTimer() + SendBox1.Text + "\n";
                              }));
                          }));
                        SendThread.Start();
                    }
                    if (isServ == false)
                    {
                        SendThread = new Thread(new ThreadStart(delegate ()
                         {
                             this.Invoke(new Action(() =>
                             {
                                 ucla.SendMessage(SendBox1.Text);
                                 ReceiveWindowBox.Text += "송신 : " + GetTimer() + SendBox1.Text + "\n";
                             }));
                         }));
                        SendThread.Start();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void Btn_Send2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (connectType == 2)
        //        {
        //            serial[0].SerialSend(SendBox2.Text);
        //            ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox2.Text + "\n");
        //            ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //            ReceiveWindowBox.ScrollToCaret();
        //        }
        //        if (connectType == 5)
        //        {
        //            if (isServ == true && tserv.client.Connected == true)
        //            {
        //                tserv.SendMsg(SendBox2.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox2.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }
        //            if (isServ == false && tcla.client.Connected == true)
        //            {
        //                tcla.SendMsg(SendBox2.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox2.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }
        //        }
        //        if (connectType == 6)
        //        {
        //            if (isServ == true && userv.client.Connected == true)
        //            {
        //                userv.SendMsg(SendBox2.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox2.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }
        //            if (isServ == false && ucla.client.Connected == true)
        //            {
        //                ucla.SendMsg(SendBox2.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox2.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message);
        //    }
        //}


        //private void Btn_Send3_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (connectType == 2)
        //        {
        //            serial[0].SerialSend(SendBox3.Text);
        //            ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox3.Text + "\n");
        //            ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //            ReceiveWindowBox.ScrollToCaret();
        //        }
        //        if (connectType == 5)
        //        {
        //            if (isServ == true && tserv.client.Connected == true)
        //            {
        //                tserv.SendMsg(SendBox3.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox3.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }
        //            if (isServ == false && tcla.client.Connected == true)
        //            {
        //                tcla.SendMsg(SendBox3.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox3.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }
        //        }
        //        if (connectType == 6)
        //        {
        //            if (isServ == true && userv.client.Connected == true)
        //            {
        //                userv.SendMsg(SendBox3.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox3.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }
        //            if (isServ == false && ucla.client.Connected == true)
        //            {
        //                ucla.SendMsg(SendBox3.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox3.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message);
        //    }


        //}

        private void Sport_Num_Select_Send(Serial[] Serial, int[] arr, string msg)   // 시리얼포트, 체크박스변수, 보낼메시지
        {
            int count = Serial.Length;  // 먼저 현재 시리얼 포트 살아있는 것 갯수부터
            for (int i = 0; i <= count; i++)    // 살아있는 시리얼 포트 만큼 순회
            {
                if (arr[i] == 1)   // 체크박스 송신 체크되있으면 전송하긔
                {
                    serial[i].SerialSend(msg);
                }
            }

        }

        private void Sport_Num_Select_Receive(Serial[] Serial, int[] arr, string msg)
        {

        }

        //private void Btn_Send4_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (connectType == 2)
        //        {
        //            serial[0].SerialSend(SendBox4.Text);
        //            ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox4.Text + "\n");
        //            ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //            ReceiveWindowBox.ScrollToCaret();
        //        }
        //        if (connectType == 5)
        //        {
        //            if (isServ == true && tserv.client.Connected == true)
        //            {
        //                tserv.SendMsg(SendBox4.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox4.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }
        //            if (isServ == false && tcla.client.Connected == true)
        //            {
        //                tcla.SendMsg(SendBox4.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox4.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }
        //        }
        //        if (connectType == 6)
        //        {
        //            if (isServ == true && userv.client.Connected == true)
        //            {
        //                userv.SendMsg(SendBox4.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox4.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }
        //            if (isServ == false && ucla.client.Connected == true)
        //            {
        //                ucla.SendMsg(SendBox4.Text);
        //                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox4.Text + "\n");
        //                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
        //                ReceiveWindowBox.ScrollToCaret();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message);
        //    }
        //}

        #endregion

        #region 시리얼 송수신 옵션

        // 시리얼 옵션 체크박스 
        private void Serial_select_CHK1_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }

        private void Serial_select_CHK2_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK3_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK4_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK11_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK22_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK33_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK44_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK5_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK55_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK6_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK7_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK8_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK66_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK77_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        private void Serial_select_CHK88_CheckedChanged(object sender, EventArgs e)
        {
            Update_Serial_Opt();
        }
        // 체크박스 체크했을 시 변수값 변경...
        void Update_Serial_Opt()
        {
            if (Serial_select_CHK1.Checked) { Serial_Send_Arr[0] = 1; }
            else { Serial_Send_Arr[0] = 0; }
            if (Serial_select_CHK2.Checked) { Serial_Send_Arr[1] = 1; }
            else { Serial_Send_Arr[1] = 0; }
            if (Serial_select_CHK3.Checked) { Serial_Send_Arr[2] = 1; }
            else { Serial_Send_Arr[2] = 0; }
            if (Serial_select_CHK4.Checked) { Serial_Send_Arr[3] = 1; }
            else { Serial_Send_Arr[3] = 0; }
            if (Serial_select_CHK4.Checked) { Serial_Send_Arr[4] = 1; }
            else { Serial_Send_Arr[4] = 0; }
            if (Serial_select_CHK4.Checked) { Serial_Send_Arr[5] = 1; }
            else { Serial_Send_Arr[5] = 0; }
            if (Serial_select_CHK4.Checked) { Serial_Send_Arr[6] = 1; }
            else { Serial_Send_Arr[6] = 0; }
            if (Serial_select_CHK4.Checked) { Serial_Send_Arr[7] = 1; }
            else { Serial_Send_Arr[7] = 0; }

            if (Serial_select_CHK11.Checked) { Serial_Receive_Arr[0] = 1; }
            else { Serial_Receive_Arr[0] = 0; }
            if (Serial_select_CHK22.Checked) { Serial_Receive_Arr[1] = 1; }
            else { Serial_Receive_Arr[1] = 0; }
            if (Serial_select_CHK33.Checked) { Serial_Receive_Arr[2] = 1; }
            else { Serial_Receive_Arr[2] = 0; }
            if (Serial_select_CHK44.Checked) { Serial_Receive_Arr[3] = 1; }
            else { Serial_Receive_Arr[3] = 0; }
            if (Serial_select_CHK55.Checked) { Serial_Receive_Arr[4] = 1; }
            else { Serial_Receive_Arr[4] = 0; }
            if (Serial_select_CHK66.Checked) { Serial_Receive_Arr[5] = 1; }
            else { Serial_Receive_Arr[5] = 0; }
            if (Serial_select_CHK77.Checked) { Serial_Receive_Arr[6] = 1; }
            else { Serial_Receive_Arr[6] = 0; }
            if (Serial_select_CHK88.Checked) { Serial_Receive_Arr[7] = 1; }
            else { Serial_Receive_Arr[7] = 0; }
        }

        #endregion

        #region 수신 옵션들 묶음
        private void Btn_Clear_Click(object sender, EventArgs e)
        {

        }

        private void Chk_Hexa_CheckedChanged(object sender, EventArgs e)
        {
            if (Chk_Hexa.CheckState == CheckState.Checked)
                Chk_Hexa_Flag = 1;
            else
                Chk_Hexa_Flag = 0;

        }
        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tserv != null)
                tserv.ServerStop();
            if (tcla != null)
                tcla.DisConnect();
            if (userv != null)
                userv.DisConnect();
            if (ucla != null)
                ucla.DisConnect();
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.Kill();

        }

        private void UServerCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (UServerCheck.Checked == true)
            {
                UIPNumber.Enabled = false;
                isServ = true;

            }
            else
            {
                UIPNumber.Enabled = true;
                isServ = false;
            }

        }

        private void Udp_Connect_Click(object sender, EventArgs e)
        {
            if (UServerCheck.Checked == true)
            {
                int port = Int32.Parse(UPortNumber.Text);
                userv.Connect(this, port);

            }
            else
            {
                int port = Int32.Parse(UPortNumber.Text);
                string ip = UIPNumber.Text;
                ucla.Connect(this, ip, port);
            }

        }

        private void saveLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveLog = new SaveFileDialog();

            saveLog.InitialDirectory = @"C:\"; //기본 경로 설정
            saveLog.Title = "로그 저장";//파일 저장 다이얼로그 제목
            saveLog.Filter = "로그 파일(*.log)|*.log|모든 파일|*.*";//파일 형식 필터
            saveLog.DefaultExt = "log";
            saveLog.AddExtension = true;

            if (saveLog.ShowDialog() == DialogResult.OK)
            {
                //saveLog.Filename에서 경로를 가져온다.
                FileStream filestream = new FileStream(saveLog.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter streamwriter = new StreamWriter(filestream);
                streamwriter.WriteLine(ReceiveWindowBox.Text);//텍박에 있는 거 저장

                //lastIndexOf 지정된 문자열이 마지막으로 발견되는 위치 값을 받는다.
                int position = saveLog.FileName.LastIndexOf("\\");

                //Substring에 위치 값을 하나만 넣어주면 그 위치 부터 문자열의 끝까지 출력한다.
                string textboxname = saveLog.FileName.Substring(position + 1);
                streamwriter.Close();
            }
        }
        private void openLog_Click(object sender, EventArgs e)
        {
            OpenFileDialog openLog = new OpenFileDialog();
            openLog.Title = "로그 열기";
            openLog.Filter = "로그 파일(*.log)|*.log|모든 파일|*.*";
            if (openLog.ShowDialog() == DialogResult.OK)
            {
                //열기 대상 파일 경로
                string openfileposition = openLog.FileName;

                //lastIndexOf 지정된 문자열이 마지막으로 발견되는 위치 값을 받는다.
                int openPosition = openLog.FileName.LastIndexOf("\\");

                //Substring에 위치 값을 하나만 넣어주면 그 위치부터 문자열의 끝까지 출력한다.
                string logfileName = openLog.FileName.Substring(openPosition + 1);

                StreamReader streamreader = new StreamReader(openfileposition);

                //Text를 Null로 초기화 후 읽어들인 문자를 Text에 넣어준다.
                ReceiveWindowBox.Text = null;
                ReceiveWindowBox.Text = streamreader.ReadToEnd();

                //Close
                streamreader.Close();
            }
        }
        private void receiveWindowBoxClear_Click(object sender, EventArgs e)
        {
            ReceiveWindowBox.Text = null;
        }

        //시리얼 포트 콤보박스에서 제일 긴 녀석 넓이 가져오기
        private int GetLargestTextEntent()
        {
            ComboBox cb = this.Serial_Combo_Port;
            int maxLen = -1;
            if (cb.Items.Count > -1)
            {
                using (Graphics g = cb.CreateGraphics())
                {
                    int vertScrollBarWidth = 0;
                    if (cb.Items.Count > cb.MaxDropDownItems)
                    {
                        vertScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
                    }
                    for (int nLoopCnt = 0; nLoopCnt < cb.Items.Count; nLoopCnt++)
                    {
                        int newWidth = (int)g.MeasureString(cb.Items[nLoopCnt].ToString(), cb.Font).Width + vertScrollBarWidth;
                        if (newWidth > maxLen)
                        {
                            maxLen = newWidth;
                        }
                    }
                }
            }
            return maxLen;
        }

        private void Tcp_Btn_DisCon_Click(object sender, EventArgs e)
        {
            //comboBox5 -> IP, comboBox6 -> Port
            if (Tcp_Btn_DisCon.Text == "연결")
            {
                if (ServerCheck.Checked == true)
                {
                    int port = Int32.Parse(PortNumber.Text);
                    tserv = new Tserv(this, port);
                    tserv.ServerStart();
                    //AcceptThread = new Thread(() => tserv.ServerWait());
                    //AcceptThread.Start();


                }
                else
                {
                    int port = Int32.Parse(PortNumber.Text);
                    string ip = IpNumber.Text;
                    tcla = new Tserv(this, ip, port);
                    tcla.Connect();
                }
                Tcp_Btn_DisCon.Text = "연결해제";
                return;
            }
            else if (Tcp_Btn_DisCon.Text == "연결해제")
            {
                if (tserv != null && isServ == true)
                    tserv.ServerStop();
                else if (tcla != null && isServ == false)
                    tcla.DisConnect();
            }
            Tcp_Btn_DisCon.Text = "연결";
            return;

        }
    }
}
