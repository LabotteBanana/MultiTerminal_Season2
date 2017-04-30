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

namespace MultiTerminal
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        // 연결 타입 정의 ^0^/
        enum TYPE { SERIAL=0, TCP, UDP };

        public bool isServ = false;
        private TYPE connectType;
        public Tserv tserv = null;
        public Tserv tcla = null;
        public udpServer userv = new udpServer();
        public udpClient ucla = new udpClient();
        public static Thread macroThread;
        public static Thread SendThread;
        public static Thread RecvThread;
        public delegate void TRecvCallBack();

        // 체크박스 부분
        static public int Chk_Hexa_Flag = 0;

        // 시리얼 부분.
        public Serial[] serial = new Serial[99];    // 동적 배열 객체로 바꾸어야 한다... ㅎㅎㅎㅎ;
        public int Sport_Count = 0;
        public int[] Serial_Send_Arr = new int[8];       // 시리얼 선택적 송신 체크 옵션
        public int[] Serial_Receive_Arr = new int[8];    // 시리얼 선택적 수신 체크 옵션
        private string[] SerialOpt = new string[6];


        public System.Timers.Timer timer = null;
        Stopwatch sw = new Stopwatch();
        public static System.Timers.Timer mactimer = null;
        public System.Timers.Timer aftertimer = null;
        private DateTime nowTime;

        // 다중 연결 리스트 부분
        private GridView[] gridview = new GridView[99];
        private List<string> connetName = new List<string>();

        //리스트를 이용한 다중연결 관리
        private List<Serial> SerialList = new List<Serial>();
        private List<GridView> GridList = new List<GridView>();
        


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
            if (connectType == TYPE.UDP)
            {
                if (userv != null)
                {
                    if (userv.server != null)
                        if (userv.server.IsBound == true)
                            userv.RecvMessage();
                }
                if (ucla != null)
                {
                    if (ucla.client != null)
                        if (ucla.m_isConnected == true)
                            ucla.RecvMessage();
                }
            }
        }

        private void OnMacro(Object soruce, System.Timers.ElapsedEventArgs e)
        {
            if (connectType == TYPE.SERIAL)
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
                    //MessageBox.Show(ex.Message);
                }
            }
            if (connectType == TYPE.TCP)
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
            if (connectType == TYPE.UDP)
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
                                    userv.SendMessage(SendBox1.Text);

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
            OptionSelect(0);
            this.UART_Tile.Style = MetroFramework.MetroColorStyle.Pink; // 클릭시 박스 색 변경
            this.TCP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
            this.UDP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
            this.Serial_Combo_Port.DropDownWidth = GetLargestTextEntent();
        }
        private void TCP_Tile_Click(object sender, EventArgs e)
        {
            OptionSelect(1);
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
            OptionSelect(2);
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
                case 0:
                    {
                        connectType = TYPE.SERIAL;
                        SerialPanel.Location = Loc;
                        SerialPanel.Visible = true;    // 시리얼 패널 보이기

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
                case 1:
                    {
                        connectType = TYPE.TCP;
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
                case 2:
                    {
                        connectType = TYPE.UDP;
                        UdpPanel.Location = Loc;
                        UdpPanel.Visible = true;

                        SerialPanel.Visible = false;
                        TcpPanel.Visible = false;
                        if (tserv != null)
                            tserv.ServerStop();
                        if (tcla != null)
                            tcla.DisConnect();
                    }
                    break;
                default:
                    {
                        SerialPanel.Visible = false;
                        TcpPanel.Visible = false;
                        UdpPanel.Visible = false;
                        if (tserv != null)
                            tserv.ServerStop();
                        if (tcla != null)
                            tcla.DisConnect();
                        if (userv != null)
                            userv.DisConnect();
                        if (ucla != null)
                            ucla.DisConnect();

                    }break;
            }
        }

        private void DisConBtn_Click(object sender, EventArgs e)    // 연결 해제 버튼
        {
            if (connectType == TYPE.SERIAL) //시리얼
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
                
                
                
            }   
            catch(Exception E)
            {
                MessageBox.Show(E.ToString());
            }
            
            if (serial[Sport_Count].IsOpen())
            {
                int size = GridList.Count;  
                string portname = Serial_Combo_Port.Items[Serial_Combo_Port.SelectedIndex].ToString();  // 연결에 성공한 시리얼 객체의 포트네임 가져옴


                gridview[GridList.Count] = new GridView(GridList.Count, portname, "SERIAL", Sport_Count);  // 그리드뷰 객체에 적용,   타입형태(시리얼,UDP..), 타입의 순번도 그리드 객체로 슝들어감.    
                DrawGrid(gridview[GridList.Count].MyNum, gridview[GridList.Count].Portname, gridview[GridList.Count].Time);

                Sport_Count++;
                GridList.Add(gridview[GridList.Count]);
                //DrawGrid(gridview[Grid_Count].MyNum, gridview[Grid_Count].Portname, gridview[Grid_Count].Time); // 그리드뷰 객체를 UI에 적용

                //GridList.Add(gridview[Grid_Count]);                
                //Grid_Count++;
                // 최대 그리드뷰는 귀찮;; 걍 만들다 넘치면 알아서 프로그램 뻗겠지 ^오^
                //if (Grid_Count < 14) { Grid_Count++; } else { MessageBox.Show("최대 그리드 초과, 연결을 삭제해주세요."); }             

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
                    this.ReceiveWindowBox.Text += Global.globalVar;
                    this.ReceiveWindowBox.ScrollToCaret();
                }));
            }));
            thread.Start();
        }

        #endregion

        #region TCP UI
        private void button3_Click(object sender, EventArgs e)
        {
            //comboBox5 -> IP, comboBox6 -> Port
            if (ServerCheck.Checked == true)
            {
                int port = Int32.Parse(PortNumber.Text);
                tserv = new Tserv(this, port);
                tserv.ServerStart();

            }
            else
            {
                int port = Int32.Parse(PortNumber.Text);
                string ip = IpNumber.Text;
                tcla = new Tserv(this, ip, port);
                tcla.Connect();
            }
        }
        #region TCP서버여부
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ServerCheck.Checked == true)
            {
                IpNumber.Enabled = false;
                isServ = true;

            }
            else
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

            DataGridViewCellStyle headerstyle = new DataGridViewCellStyle();
            headerstyle.BackColor = Color.Beige;
            headerstyle.Font = new Font("verdana", 10, FontStyle.Bold);
            PortListGrid.ColumnHeadersDefaultCellStyle = headerstyle;


        }
        #endregion
        #region 보내기 버튼 묶음

        private void Btn_Send1_Click(object sender, EventArgs e)    
        {
            try
            {
                if (connectType == TYPE.SERIAL)
                {
                    //serial[0].SerialSend(SendBox1.Text);
                    // 우선 버튼 1에만 멀티 전송 구현
                    Sport_Num_Select_Send(serial, SendBox1.Text);   // 시리얼 객체의 수신여부 상태 확인하고 전송하는 기능 


                    ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox1.Text + "\n");
                    ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
                    ReceiveWindowBox.ScrollToCaret();
                }


                if (connectType == TYPE.TCP)
                {
                    if (isServ == true && tserv.client.Connected == true)
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
                if (connectType == TYPE.UDP)
                {
                    if (isServ == true)
                    {
                        SendThread = new Thread(new ThreadStart(delegate ()
                          {
                              this.Invoke(new Action(() =>
                              {
                                  userv.SendMessage(SendBox1.Text);

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
                //MessageBox.Show(ex.Message);
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

        private void Sport_Num_Select_Send(Serial[] Serial, string msg)   // 시리얼 선택적 전송 기능 함수
        {
            int gridcount = PortListGrid.Rows.Count;    // 현재 그리드뷰 리스트의 갯수 가져옴

            for(int i = 0; i <= gridcount; i++) //그리드뷰 리스트 처음부터 순회
            {
                if( gridview[i].Type == "SERIAL" && gridview[i].TxCheckedState == true )    // 그리드뷰리스트의 타입이 시리얼, 그리고 수신 체크박스 상태가 체크되어있다면
                {
                    serial[gridview[i].Typenum].SerialSend(msg);    // serial [그리드뷰 객체에 저장된 시리얼 타입 객체의 순번]
                }
            }

        }

        // ☆★ 요거 TCP 선택전송 위한것~! ☆★
        private void TCP_Num_Select_Send(Serial[] Serial, string msg)   // TCP 선택적 전송 기능 함수
        {
            int gridcount = PortListGrid.Rows.Count;    // 현재 그리드뷰 리스트의 갯수 가져옴

            for (int i = 0; i <= gridcount; i++) //그리드뷰 리스트 처음부터 순회
            {
                if (gridview[i].Type == "SERIAL" && gridview[i].TxCheckedState == true)    // 그리드뷰리스트의 타입이 시리얼, 그리고 수신 체크박스 상태가 체크되어있다면
                {                   
                    tserv.SendMsg(SendBox1.Text);
                }
            }

        }
        

        private void Sport_Num_Select_Receive(Serial[] Serial,int[] arr, string msg)
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
                userv.Connect(this,port);

            }
            else
            {
                int port = Int32.Parse(UPortNumber.Text);
                string ip = UIPNumber.Text;
                ucla.Connect(this, ip, port);
            }

        }

        private void saveLog_Click(object sender, EventArgs e) {
            SaveFileDialog saveLog = new SaveFileDialog();

            saveLog.InitialDirectory = @"C:\"; //기본 경로 설정
            saveLog.Title = "로그 저장";//파일 저장 다이얼로그 제목
            saveLog.Filter = "로그 파일(*.log)|*.log|모든 파일|*.*";//파일 형식 필터
            saveLog.DefaultExt = "log";
            saveLog.AddExtension = true;

            if (saveLog.ShowDialog() == DialogResult.OK) {
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
        private void openLog_Click(object sender, EventArgs e) {
            OpenFileDialog openLog = new OpenFileDialog();
            openLog.Title = "로그 열기";
            openLog.Filter = "로그 파일(*.log)|*.log|모든 파일|*.*";
            if (openLog.ShowDialog() == DialogResult.OK) {
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
        private void receiveWindowBoxClear_Click(object sender, EventArgs e) {
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


        // gridview 체크박스 관련 ^-^
        #region
        private void PortListGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) // 그리드뷰 체크박스 클릭 이벤트
        {
            if (e.ColumnIndex == 4 && e.RowIndex != -1) // Tx부분 체크박스 속성,  열이 3번째이고, 행이 1개 이상 있을때 조건 발생!
            {
                if ( gridview[e.RowIndex].TxCheckedState == false)  // 그리드뷰의 현재 클릭 행, 번째의 그리드뷰 클래스안에 체크박스 속성 건드려버리기~
                { gridview[e.RowIndex].TxCheckedState = true; }
                else
                { gridview[e.RowIndex].TxCheckedState = false; }           
            }

            if (e.ColumnIndex == 5 && e.RowIndex != -1) // Rx부분 체크박스 속성,   열이 4번째이고, 행이 1개 이상 있을때 조건 발생!
            {
                if (gridview[e.RowIndex].RxCheckedState == false)
                {
                    gridview[e.RowIndex].RxCheckedState = true;
                    serial[gridview[e.RowIndex].Typenum].RxState = true;
                }
                else
                {
                    gridview[e.RowIndex].RxCheckedState = false;
                    serial[gridview[e.RowIndex].Typenum].RxState = false;
                }
            }

        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            bool fuck1 = gridview[0].TxCheckedState;
            bool fuck2 = gridview[0].RxCheckedState;
            MessageBox.Show(fuck1.ToString() +" "+ fuck2.ToString());
        }

        private void PortListGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex != -1)
            {
                PortListGrid.EndEdit();
            }
            if (e.ColumnIndex == 5 && e.RowIndex != -1)
            {
                PortListGrid.EndEdit();
            }
        }

        // 그리드뷰 연결 취소 버튼 클릭 이벤트 ^0^
        private void PortListGrid_CellValue(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                int Selected_Grid_Num = e.RowIndex;
                string Selected_Grid_Type = gridview[Selected_Grid_Num].Type;

                // 이곳이 바로 취소버튼을 누르면, 타입별로 연결 해제 하는 부분 ^오^
                switch (Selected_Grid_Type)
                {
                    case "SERIAL":
                        {
                            try
                            {
                                serial[gridview[Selected_Grid_Num].Typenum].DisConSerial();   // 시리얼 연결 해제 ^-^
                                MessageBox.Show(gridview[Selected_Grid_Num].Portname + "가 연결해제 되었습니다."); // ex: 0번 시리얼이 연결 해제되었습니다.

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                        }
                        break;

                    case "TCP":
                        {

                        }
                        break;

                    case "UDP":
                        {

                        }
                        break;
                }
                if (GridList.Count > Selected_Grid_Num + 1 )    // 이 부분이 바로 바뀐 순서번호, 고치는 부분~~~!
                {
                    for (int i = Selected_Grid_Num + 1; i <= GridList.Count - 1; i++)
                    {
                        gridview[i].MyNum = gridview[i].MyNum - 1;
                        PortListGrid.Rows[i].Cells[0].Value = gridview[i].MyNum.ToString(); // 다이렉트로 순번을 수정함!
                    }
                }
                GridList.RemoveAt(Selected_Grid_Num);           // 자체 그리드 객체에서의 리스트 삭제
                PortListGrid.Rows.RemoveAt(Selected_Grid_Num);  // UI 그리드에서의 리스트 삭제
                MessageBox.Show(Selected_Grid_Num.ToString()); //TODO - Button Clicked - Execute Code Here

                PortListGrid.Update();              
                PortListGrid.Refresh();
                

            }
        }

        private void DrawGrid(int num, string name, string time)    // 그리드에 열 추가 ~~
        {
            string[] row = new string[] { num.ToString(), name, time };
            PortListGrid.Rows.Add(row);
        }
       


        #endregion

        
    }

}
