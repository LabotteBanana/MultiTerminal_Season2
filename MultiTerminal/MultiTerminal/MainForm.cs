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
        // 연결 타입 정의 ^0^/
        enum TYPE { SERIAL = 0, TCP, UDP };


        public bool isServ = false;
        private TYPE connectType;
        public Tserv tserv = null;
        public Tserv tcla = null;
        public udpServer userv = new udpServer();
        public udpClient ucla = new udpClient();
        public static Thread macroThread;
        public static Thread SendThread;
        public static Thread AcceptThread;
        public static Thread RecvThread;
        public delegate void TRecvCallBack();
        public int RowIndex = 0, ColumnIndex = 0;
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
        private GridView gridview = null;
        private List<string> connetName = new List<string>();

        //리스트를 이용한 다중연결 관리
        private List<Serial> SerialList = new List<Serial>();
        private List<GridView> GridList = new List<GridView>();

        public string prePortName = null;

        System.Threading.Timer viewTimer;

        public MainForm()
        {
            InitializeComponent();

        }


        private void MainForm_Load(object sender, EventArgs e)  // 폼 열렸을 때
        {
            this.Style = MetroFramework.MetroColorStyle.Yellow;
            UI_Init();
            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                this.Invoke(new Action(() =>
                {
                    Serial_Combo_Init();
                    myQueue.Initialize(this,ReceiveWindowBox);
                }));
            }));
            //큐를 1초 후에 20ms마다 실행한다.
            viewTimer = new System.Threading.Timer(myQueue.viewwindow, null, 1000, 20);

            thread.Start();
        }

        #region 매크로 기능


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
                    
                }
                if (ucla != null)
                {
                    if (ucla.client != null)
                        if (ucla.m_isConnected == true)
                            ucla.RecvMessage();
                }
            }
        }

        private void OnMacro_1(Object soruce, System.Timers.ElapsedEventArgs e)
        {
            BattleGround_PlayMore(SendBox1.Text);
        }
        private void OnMacro_2(Object soruce, System.Timers.ElapsedEventArgs e)
        {
            BattleGround_PlayMore(SendBox2.Text);
        }
        private void OnMacro_3(Object soruce, System.Timers.ElapsedEventArgs e)
        {
            BattleGround_PlayMore(SendBox3.Text);
        }
        private void OnMacro_4(Object soruce, System.Timers.ElapsedEventArgs e)
        {
            BattleGround_PlayMore(SendBox4.Text);
        }

        public string GetTimer()
        {

            string now = null;
            now = "[" + nowTime.Hour + ":" + nowTime.Minute + ":" + nowTime.Second + ":" + nowTime.Millisecond + "]";
            return now;
        }


        public void SetMacroTime(int count)
        {
            // 초당 10번이면 100/1000
            // 초당 5번 이면 50/1000
            mactimer.Enabled = true;
            mactimer.Interval = count;

        }
        private void MacroCheck_1_CheckedChanged(object sender, EventArgs e)
        {
            if(MacroCount.Text != null)
            { 
                if (MacroCheck_2.CheckState != CheckState.Checked &&
                      MacroCheck_3.CheckState != CheckState.Checked &&
                        MacroCheck_4.CheckState != CheckState.Checked)
                {
                    int count = Int32.Parse(MacroCount.Text);

                    if (MacroCheck_1.CheckState == CheckState.Checked)
                    {
                        macroThread = new Thread(() => SetMacroTime(count));

                        mactimer.Elapsed += OnMacro_1;
                        mactimer.Enabled = false;
                        macroThread.Start();
                    }


                    else
                    {
                        MacroCheck_1.CheckState = CheckState.Unchecked;
                        sw.Stop();
                        mactimer.Enabled = false;
                        mactimer.Elapsed -= OnMacro_1;
                        mactimer.Enabled = false;

                        //SendThread.Abort();
                        macroThread.Abort();
                    }
                }
                else
                {
                    if(MacroCheck_2.Checked == true) MacroCheck_2.Checked = false;
                    if (MacroCheck_3.Checked == true) MacroCheck_3.Checked = false;
                    if (MacroCheck_4.Checked == true) MacroCheck_4.Checked = false;
                }
            }
            else
            {
                MessageBox.Show("반복주기 미입력 에러!!! -> ms 반복 주기를 입력해주세요 ^0^");
            }
        }

        private void MacroCheck_2_CheckedChanged(object sender, EventArgs e)
        {
            if (MacroCheck_1.CheckState != CheckState.Checked &&
                  MacroCheck_3.CheckState != CheckState.Checked &&
                    MacroCheck_4.CheckState != CheckState.Checked)
            {
                    int count = Int32.Parse(MacroCount.Text);

                if (MacroCheck_2.CheckState == CheckState.Checked)
                {
                    macroThread = new Thread(() => SetMacroTime(count));

                    mactimer.Elapsed += OnMacro_2;
                    mactimer.Enabled = false;
                    macroThread.Start();
                }


                else
                {
                    MacroCheck_2.CheckState = CheckState.Unchecked;
                    sw.Stop();
                    mactimer.Enabled = false;
                    mactimer.Elapsed -= OnMacro_2;
                    mactimer.Enabled = false;

                    //SendThread.Abort();
                macroThread.Abort();
                }
            }
            else
            {
                if (MacroCheck_1.Checked == true) MacroCheck_1.Checked = false;
                if (MacroCheck_3.Checked == true) MacroCheck_3.Checked = false;
                if (MacroCheck_4.Checked == true) MacroCheck_4.Checked = false;
            }
        }

        private void MacroCheck_3_CheckedChanged(object sender, EventArgs e)
        {
            if (MacroCheck_1.CheckState != CheckState.Checked &&
                  MacroCheck_2.CheckState != CheckState.Checked &&
                    MacroCheck_4.CheckState != CheckState.Checked)
            {
                int count = Int32.Parse(MacroCount.Text);

                if (MacroCheck_3.CheckState == CheckState.Checked)
                {
                    macroThread = new Thread(() => SetMacroTime(count));

                    mactimer.Elapsed += OnMacro_3;
                    mactimer.Enabled = false;
                    macroThread.Start();
                }


                else
                {
                    MacroCheck_3.CheckState = CheckState.Unchecked;
                    sw.Stop();
                    mactimer.Enabled = false;
                    mactimer.Elapsed -= OnMacro_3;
                    mactimer.Enabled = false;

                    //SendThread.Abort();
                    macroThread.Abort();
                }
            }
            else
            {
                if (MacroCheck_1.Checked == true) MacroCheck_1.Checked = false;
                if (MacroCheck_2.Checked == true) MacroCheck_2.Checked = false;
                if (MacroCheck_4.Checked == true) MacroCheck_4.Checked = false;
            }
        }

        private void MacroCheck_4_CheckedChanged(object sender, EventArgs e)
        {
            if (MacroCheck_1.CheckState != CheckState.Checked &&
                  MacroCheck_2.CheckState != CheckState.Checked &&
                    MacroCheck_3.CheckState != CheckState.Checked)
            {
                int count = Int32.Parse(MacroCount.Text);

                if (MacroCheck_4.CheckState == CheckState.Checked)
                {
                    macroThread = new Thread(() => SetMacroTime(count));

                    mactimer.Elapsed += OnMacro_4;
                    mactimer.Enabled = false;
                    macroThread.Start();
                }


                else
                {
                    MacroCheck_4.CheckState = CheckState.Unchecked;
                    sw.Stop();
                    mactimer.Enabled = false;
                    mactimer.Elapsed -= OnMacro_3;
                    mactimer.Enabled = false;

                    //SendThread.Abort();
                    macroThread.Abort();
                }
            }
            else
            {
                if (MacroCheck_1.Checked == true) MacroCheck_1.Checked = false;
                if (MacroCheck_2.Checked == true) MacroCheck_2.Checked = false;
                if (MacroCheck_3.Checked == true) MacroCheck_3.Checked = false;
            }
        }

        private void PortListGrid_Click(object sender, EventArgs e)
        {
            if (PortListGrid.CurrentCell != null)
            {
                RowIndex = PortListGrid.CurrentCell.RowIndex;
                ColumnIndex = PortListGrid.CurrentCell.ColumnIndex;
            }
        }

     
        private void BattleGround_PlayMore(string SendBox)  // Macro 보내기 전, 통신 방식 확인하고 보내기 위해서 >ㅁ<
        {
            int gridcount = PortListGrid.Rows.Count;    // 현재 그리드뷰 리스트의 갯수 가져옴

            for (int i = 0; i < gridcount; i++) //그리드뷰 리스트 처음부터 순회
            {
                if (GridList[i].Type == "SERIAL" && GridList[i].TxCheckedState == true)    // 그리드뷰리스트의 타입이 시리얼, 그리고 송신 체크박스 상태가 체크되어있다면
                {
                    serial[GridList[i].Typenum].SerialSend(SendBox);    // serial [그리드뷰 객체에 저장된 시리얼 타입 객체의 순번]
                }
                if (GridList[i].Type == "TCP Client" && GridList[i].TxCheckedState == true
                    && isServ == true && tserv.m_ClientList.Count > 0)    // 서버 -> 클라이언트
                {
                    SendThread = new Thread(new ThreadStart(delegate ()
                    {
                        this.Invoke(new Action(() =>
                        {
                            tserv.SendMsg(SendBox);
                        }));
                    }));
                    SendThread.Start();
                }
                if (GridList[i].Type == "TCP Server" && GridList[i].TxCheckedState == true
                    && isServ == false && tcla.client.Connected == true) // 클라이언트 -> 서버
                {
                    SendThread = new Thread(new ThreadStart(delegate ()
                    {
                        this.Invoke(new Action(() =>
                        {
                            tcla.SendMsg(SendBox);
                        }));
                    }));
                    SendThread.Start();
                }
            }
            this.Invoke(new Action(() =>
            {
                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox + "\n");
                ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
                ReceiveWindowBox.ScrollToCaret();
            }));
        }


        #endregion

        #region 버튼부분 입니당 ^-^         

        // 연결 방법 선택 1 ~ 6 및 박스 색깔 변경 //
        private void UART_Tile_Click(object sender, EventArgs e)
        {
            OptionSelect(0);
            this.UART_Tile.Style = MetroFramework.MetroColorStyle.Pink; // 클릭시 박스 색 변경
            this.TCP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
            this.UDP_Tile.Style = MetroFramework.MetroColorStyle.Silver;

            if (Serial_Combo_Port.Items.Count != 0)  // 만약 아무 포트도 없을 경우 그냥 지나치는 예외처리 ^-^
                this.Serial_Combo_Port.DropDownWidth = GetLargestTextEntent();
        }
        private void TCP_Tile_Click(object sender, EventArgs e)
        {
            OptionSelect(1);

            this.UART_Tile.Style = MetroFramework.MetroColorStyle.Silver; // 클릭시 박스 색 변경
            this.TCP_Tile.Style = MetroFramework.MetroColorStyle.Pink;
            this.UDP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
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
                    }
                    break;
                case 1:
                    {
                        connectType = TYPE.TCP;
                        TcpPanel.Location = Loc;
                        TcpPanel.Visible = true;

                        SerialPanel.Visible = false;
                        UdpPanel.Visible = false;
                    }
                    break;
                case 2:
                    {
                        connectType = TYPE.UDP;
                        UdpPanel.Location = Loc;
                        UdpPanel.Visible = true;

                        SerialPanel.Visible = false;
                        TcpPanel.Visible = false;
                    }
                    break;
            }
        }


        #endregion

        #region 시리얼 설정 부분~!
        //시리얼 포트 초기화
        private void serial_port_Init()
        {
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
                    try
                    {
                        string[] comportName = comPort.Name.Split('-');
                        int a = Serial_Combo_Port.Items.IndexOf(comportName[0]);
                        Serial_Combo_Port.Items[a] += " - " + comPort.Description;
                    }
                    catch (Exception e) { }
                }
            }
            if (Serial_Combo_Port.Items.Count != 0)
                Serial_Combo_Port.SelectedIndex = 0;
        }

        //시리얼 새로고침 버튼
        private void serial_Refresh_Click(object sender, EventArgs e)
        {
            Serial_Combo_Port.Items.Clear();
            serial_port_Init();
        }

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

            serial_port_Init();

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
                serial[Sport_Count] = new Serial(this, Serial_Combo_Port.Items[Serial_Combo_Port.SelectedIndex].ToString());
                serial[Sport_Count].SerialOpen(SerialOpt[0], SerialOpt[1], SerialOpt[2], SerialOpt[3], SerialOpt[4], "500", "500");
                //serial[Sport_Count].sPort.DataReceived += new SerialDataReceivedEventHandler(UpdateWindowText);
            }
            catch (Exception E)
            {
                int lineNum = Convert.ToInt32(E.StackTrace.Substring(E.StackTrace.LastIndexOf(' ')));
                MessageBox.Show("기타에러 " + lineNum + "에서 발생" + E.Message);
            }

            if (serial[Sport_Count].IsOpen())
            {
                int size = GridList.Count;
                string portname = Serial_Combo_Port.Items[Serial_Combo_Port.SelectedIndex].ToString();  // 연결에 성공한 시리얼 객체의 포트네임 가져옴
                
                gridview = new GridView(GridList.Count, portname, "SERIAL", Sport_Count);  // 그리드뷰 객체에 적용,   타입형태(시리얼,UDP..), 타입의 순번도 그리드 객체로 슝들어감.    
                DrawGrid(gridview.MyNum, gridview.Type, gridview.Portname, gridview.Time);

                Sport_Count++;
                GridList.Add(gridview);
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
            //Thread thread = new Thread(new ThreadStart(delegate ()
            //{
            //    this.Invoke(new Action(() =>
            //    {
            //this.ReceiveWindowBox.AppendText("수신(" + gridview[0].Portname + ") : " + GetTimer() + Global.globalVar + "\n");
            //this.ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
            //this.ReceiveWindowBox.ScrollToCaret();
            //    }));
            //}));
            //thread.Start();
        }

        #endregion

        #region TCP UI

        #endregion


        #region TCP서버여부
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isServ == false)
            {
                IpNumber.Enabled = false;
                isServ = true;
                // mactimer.Elapsed += WaitAccept;


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
                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                System.Windows.Forms.MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);

            }
        }
        #endregion

        
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
        
        private void RainBowSixSiege(string SendBox)
        {
            int gridcount = PortListGrid.Rows.Count;    // 현재 그리드뷰 리스트의 갯수 가져옴

            for (int i = 0; i < gridcount; i++) //그리드뷰 리스트 처음부터 순회
            {
                if (GridList[i].Type == "SERIAL" && GridList[i].TxCheckedState == true)    // 그리드뷰리스트의 타입이 시리얼, 그리고 송신 체크박스 상태가 체크되어있다면
                {
                    serial[GridList[i].Typenum].SerialSend(SendBox);    // serial [그리드뷰 객체에 저장된 시리얼 타입 객체의 순번]
                }
                if (GridList[i].Type == "TCP Client" && GridList[i].TxCheckedState == true
                    && isServ == true && tserv.m_ClientList.Count > 0)    // 서버 -> 클라이언트
                {
                    tserv.SendMsg(SendBox);
                }
                if (GridList[i].Type == "TCP Server" && GridList[i].TxCheckedState == true
                    && isServ == false && tcla.client.Connected == true) // 클라이언트 -> 서버
                {
                    tcla.SendMsg(SendBox);
                }

                if (GridList[i].Type == "UDP" && GridList[i].TxCheckedState == true)    // 그리드뷰리스트의 타입이 시리얼, 그리고 송신 체크박스 상태가 체크되어있다면
                {
                    if (isServ == true)
                    {
                        SendThread = new Thread(new ThreadStart(delegate ()
                        {
                            //this.Invoke(new Action(() =>
                            //{
                                userv.SendMessage(SendBox);
                            //}));
                        }));
                        SendThread.Start();
                    }
                    if (isServ == false)
                    {
                        SendThread = new Thread(new ThreadStart(delegate ()
                        {
                            //this.Invoke(new Action(() =>
                            //{
                                ucla.SendMessage(SendBox);
                            //}));
                        }));
                        SendThread.Start();

                    }
                }
            }
            this.Invoke(new Action(() =>
            {
                ReceiveWindowBox.AppendText("송신 : " + GetTimer() + SendBox + "\n");
                this.ReceiveWindowBox.SelectionStart = ReceiveWindowBox.Text.Length;
                this.ReceiveWindowBox.ScrollToCaret();
            }));
        }
        private void Btn_Send1_Click(object sender, EventArgs e)
        {
            try
            {
                RainBowSixSiege(SendBox1.Text);     // 송신 함수 ^-^             
            }
            catch (Exception ex)
            {
            }
        }
        private void Btn_Send2_Click(object sender, EventArgs e)
        {
            try
            {
                RainBowSixSiege(SendBox2.Text);     // 송신 함수 ^-^             
            }
            catch (Exception ex)
            {
            }
        }
        private void Btn_Send3_Click(object sender, EventArgs e)
        {
            try
            {
                RainBowSixSiege(SendBox3.Text);     // 송신 함수 ^-^             
            }
            catch (Exception ex)
            {
            }
        }
        private void Btn_Send4_Click(object sender, EventArgs e)
        {
            try
            {
                RainBowSixSiege(SendBox4.Text);     // 송신 함수 ^-^             
            }
            catch (Exception ex)
            {
            }
        }



        #endregion


        #region 수신 옵션들 묶음
        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            ReceiveWindowBox.Text = null;
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
            if (tcla != null)
                tcla.setExitClickedState(true);
            if (tserv != null)
                tserv.setExitClickedState(true);
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
            if (Udp_Btn_Con.Text == "연결")
            {

                if (UServerCheck.Checked == true)
                {
                    int port = Int32.Parse(UPortNumber.Text);
                    userv.Connect(this, port, gridview, GridList);

                }
                else
                {
                    int port = Int32.Parse(UPortNumber.Text);
                    string ip = UIPNumber.Text;
                    ucla.Connect(this, ip, port, gridview, GridList);
                }
                Udp_Btn_Con.Text = "연결해제";
                return;
            }
            else if (Udp_Btn_Con.Text == "연결해제")
            {
                if (UServerCheck.Checked == true)
                {
                    userv.DisConnect();

                }
                else
                {
                    ucla.DisConnect();
                }
                Udp_Btn_Con.Text = "연결";
                return;

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
        #region 그리드뷰 체크박스
        private void PortListGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) // 그리드뷰 체크박스 클릭 이벤트
        {

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
            if (tcla != null)
                tcla.setExitClickedState(true);
            if (tserv != null)
                tserv.setExitClickedState(true);
            var senderGrid = (DataGridView)sender;
            
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                int Selected_Grid_Num = e.RowIndex;
                string Selected_Grid_Type = GridList[Selected_Grid_Num].Type;

                // 이곳이 바로 취소버튼을 누르면, 타입별로 연결 해제 하는 부분 ^오^
                switch (Selected_Grid_Type)
                {
                    case "SERIAL":
                        {
                            try
                            {
                                serial[GridList[Selected_Grid_Num].Typenum].DisConSerial();   // 시리얼 연결 해제 ^-^
                            }
                            catch (Exception ex)
                            {
                                int lineNum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
                                MessageBox.Show("기타에러 " + lineNum + "에서 발생" + ex.Message);
                            }
                        }
                        break;
                    //server side
                    case "TCP Client":
                        {
                            tserv.ServerStop(GridList[Selected_Grid_Num].Typenum);
                        }   
                        break;
                    //client side
                    case "TCP Server":
                        {
                            tcla.DisConnect();
                            Tcp_Btn_DisCon.Text = "연결";
                        }
                        break;
                    case "UDP":
                        {

                        }
                        break;
                }
                fixGridListSequence(Selected_Grid_Num);
                MessageBox.Show(prePortName + "가 연결해제 되었습니다."); // ex: 0번 시리얼이 연결 해제되었습니다.
            }
        }
        //그리드 리스트 수정하는 부분
        public void fixGridListSequence(int Selected_Grid_Num)
        {
            if (GridList.Count > Selected_Grid_Num + 1)    // 이 부분이 바로 바뀐 순서번호, 고치는 부분~~~!
            {
                for (int i = Selected_Grid_Num + 1; i <= GridList.Count - 1; i++)
                {
                    GridList[i].MyNum--;
                    PortListGrid.Rows[i].Cells[0].Value = GridList[i].MyNum.ToString(); // 다이렉트로 순번을 수정함!
                }
            }
            prePortName = GridList[Selected_Grid_Num].Portname;
            GridList.RemoveAt(Selected_Grid_Num);           // 자체 그리드 객체에서의 리스트 삭제
            PortListGrid.Rows.RemoveAt(Selected_Grid_Num);  // UI 그리드에서의 리스트 삭제

            PortListGrid.Update();
            PortListGrid.Refresh();
        }

        public void DrawGrid(int num, string type, string name, string time)    // 그리드에 열 추가 ~~
        {
            string[] row = new string[] { num.ToString(), type, name, time };
            PortListGrid.Rows.Add(row); //row 가로 column 세로

            // 디폴트로 true로
            PortListGrid.Rows[num ].Cells[4].Value = true;
            PortListGrid.Rows[num ].Cells[5].Value = true;
        }
        //tcp에서 ip찾아 그리드뷰 수정하는 부분, 클라이언트 종료->서버 종료나 서버 종료->클라이언트 종료일 때 실행
        public void RemoveGridforIP(string ip) {
            int rowIndex = 0;
            foreach (DataGridViewRow row in PortListGrid.Rows)
            {
                if (row.Cells[2].Value.ToString().Equals(ip))
                {
                    rowIndex = row.Index;
                    break;
                }
            }
            if (tserv != null)
            {
                tserv.ServerStop(GridList[rowIndex].Typenum);
            }
            if (tcla != null)
                tcla.DisConnect();
            this.Invoke(new MethodInvoker(delegate()
            {
                if(tcla != null)
                    Tcp_Btn_DisCon.Text = "연결";
                fixGridListSequence(rowIndex);
            }));
            MessageBox.Show(prePortName + "가 연결해제 되었습니다."); // ex: 0번 시리얼이 연결 해제되었습니다.
        }

        #endregion
        private void Tcp_Btn_DisCon_Click(object sender, EventArgs e)
        {
            if (tcla != null)
                tcla.setExitClickedState(true);
            if (tserv != null)
                tserv.setExitClickedState(true);
            //comboBox5 -> IP, comboBox6 -> Port
            if (Tcp_Btn_DisCon.Text == "연결")
            {
                if (ServerCheck.Checked == true)
                {
                    int port = Int32.Parse(PortNumber.Text);
                    tserv = new Tserv(this, port, gridview, GridList);
                    tserv.ServerStart();
                }
                else
                {
                    int port = Int32.Parse(PortNumber.Text);
                    string ip = IpNumber.Text;
                    tcla = new Tserv(this, ip, port, gridview, GridList);
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
                {
                    tcla.DisConnect();
                    for (int i = 0; i < GridList.Count; i++)
                    {
                        if(GridList[i].Type =="TCP Server")
                            fixGridListSequence(i);
                    }
                }
            }
            Tcp_Btn_DisCon.Text = "연결";
        }

        //분석 폼 열기
        private void freq_Click(object obj, EventArgs e)
        {
            this.SendToBack();
            analysisForm aF = new analysisForm(ReceiveWindowBox);
            aF.Show();
        }
    }
}