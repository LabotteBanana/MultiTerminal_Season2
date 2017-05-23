namespace MultiTerminal
//choi에 분기만들고 커밋해보기
//1212123123123
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.PortListGrid = new System.Windows.Forms.DataGridView();
            this.Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tx = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Rx = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Discon = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.TcpPanel = new System.Windows.Forms.Panel();
            this.ServerCheck = new System.Windows.Forms.CheckBox();
            this.PortNumber = new System.Windows.Forms.ComboBox();
            this.IpNumber = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Tcp_Btn_DisCon = new System.Windows.Forms.Button();
            this.Tcp_Btn_Con = new System.Windows.Forms.Button();
            this.SerialPanel = new System.Windows.Forms.Panel();
            this.Serial_Btn_F5 = new System.Windows.Forms.Button();
            this.Serial_Combo_FlowCon = new System.Windows.Forms.ComboBox();
            this.Serial_Combo_StopBit = new System.Windows.Forms.ComboBox();
            this.Serial_Combo_Parity = new System.Windows.Forms.ComboBox();
            this.Serial_Combo_Data = new System.Windows.Forms.ComboBox();
            this.Serial_Combo_Baud = new System.Windows.Forms.ComboBox();
            this.Serial_Combo_Port = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Label_Se_Port = new System.Windows.Forms.Label();
            this.Serial_Btn_Con = new System.Windows.Forms.Button();
            this.Serial_Btn_DisCon = new System.Windows.Forms.Button();
            this.UdpPanel = new System.Windows.Forms.Panel();
            this.Udp_Btn_DisCon = new System.Windows.Forms.Button();
            this.Udp_Btn_Con = new System.Windows.Forms.Button();
            this.UServerCheck = new System.Windows.Forms.CheckBox();
            this.UPortNumber = new System.Windows.Forms.ComboBox();
            this.UIPNumber = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Btn_Clear = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.Chk_Hexa = new System.Windows.Forms.CheckBox();
            this.LogPanel = new MetroFramework.Controls.MetroPanel();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.ReceiveWindowBox = new System.Windows.Forms.RichTextBox();
            this.UDP_Tile = new MetroFramework.Controls.MetroTile();
            this.TCP_Tile = new MetroFramework.Controls.MetroTile();
            this.UART_Tile = new MetroFramework.Controls.MetroTile();
            this.panel3 = new System.Windows.Forms.Panel();
            this.MacroCheck_4 = new System.Windows.Forms.CheckBox();
            this.MacroCheck_3 = new System.Windows.Forms.CheckBox();
            this.MacroCheck_2 = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.MacroCount = new System.Windows.Forms.TextBox();
            this.MacroCheck_1 = new System.Windows.Forms.CheckBox();
            this.Btn_Send3 = new System.Windows.Forms.Button();
            this.SendBox3 = new System.Windows.Forms.RichTextBox();
            this.Btn_Send4 = new System.Windows.Forms.Button();
            this.Btn_Send2 = new System.Windows.Forms.Button();
            this.SendBox4 = new System.Windows.Forms.RichTextBox();
            this.Btn_Send1 = new System.Windows.Forms.Button();
            this.SendBox2 = new System.Windows.Forms.RichTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.SendBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.asdfasdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.로그저장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.로그불러오기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ddfdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.빈도분석ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.metroPanel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PortListGrid)).BeginInit();
            this.panel4.SuspendLayout();
            this.TcpPanel.SuspendLayout();
            this.SerialPanel.SuspendLayout();
            this.UdpPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.LogPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroPanel1
            // 
            this.metroPanel1.AutoSize = true;
            this.metroPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.metroPanel1.Controls.Add(this.panel5);
            this.metroPanel1.Controls.Add(this.panel4);
            this.metroPanel1.Controls.Add(this.panel2);
            this.metroPanel1.Controls.Add(this.LogPanel);
            this.metroPanel1.Controls.Add(this.UDP_Tile);
            this.metroPanel1.Controls.Add(this.TCP_Tile);
            this.metroPanel1.Controls.Add(this.UART_Tile);
            this.metroPanel1.Controls.Add(this.panel3);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(3, 82);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(720, 671);
            this.metroPanel1.TabIndex = 2;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.PortListGrid);
            this.panel5.Location = new System.Drawing.Point(5, 498);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(707, 114);
            this.panel5.TabIndex = 22;
            // 
            // PortListGrid
            // 
            this.PortListGrid.AllowUserToAddRows = false;
            this.PortListGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PortListGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Num,
            this.Type,
            this.Port,
            this.Time,
            this.Tx,
            this.Rx,
            this.Discon});
            this.PortListGrid.Location = new System.Drawing.Point(37, 3);
            this.PortListGrid.Name = "PortListGrid";
            this.PortListGrid.RowTemplate.Height = 23;
            this.PortListGrid.Size = new System.Drawing.Size(643, 108);
            this.PortListGrid.TabIndex = 0;
            this.PortListGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PortListGrid_CellValue);
            this.PortListGrid.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.PortListGrid_CellMouseUp);
            this.PortListGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PortListGrid_CellValueChanged);
            this.PortListGrid.Click += new System.EventHandler(this.PortListGrid_Click);
            // 
            // Num
            // 
            this.Num.HeaderText = "No";
            this.Num.Name = "Num";
            this.Num.ReadOnly = true;
            this.Num.Width = 50;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // Port
            // 
            this.Port.HeaderText = "연결 이름";
            this.Port.Name = "Port";
            this.Port.ReadOnly = true;
            this.Port.Width = 200;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // Tx
            // 
            this.Tx.HeaderText = "Tx";
            this.Tx.Name = "Tx";
            this.Tx.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Tx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Tx.Width = 50;
            // 
            // Rx
            // 
            this.Rx.HeaderText = "Rx";
            this.Rx.Name = "Rx";
            this.Rx.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Rx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Rx.Width = 50;
            // 
            // Discon
            // 
            this.Discon.DataPropertyName = "Discon";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "X";
            this.Discon.DefaultCellStyle = dataGridViewCellStyle1;
            this.Discon.HeaderText = "X";
            this.Discon.Name = "Discon";
            this.Discon.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Discon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Discon.Text = "X";
            this.Discon.Width = 50;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.TcpPanel);
            this.panel4.Controls.Add(this.SerialPanel);
            this.panel4.Controls.Add(this.UdpPanel);
            this.panel4.Location = new System.Drawing.Point(5, 101);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(159, 284);
            this.panel4.TabIndex = 17;
            // 
            // TcpPanel
            // 
            this.TcpPanel.Controls.Add(this.ServerCheck);
            this.TcpPanel.Controls.Add(this.PortNumber);
            this.TcpPanel.Controls.Add(this.IpNumber);
            this.TcpPanel.Controls.Add(this.label1);
            this.TcpPanel.Controls.Add(this.label7);
            this.TcpPanel.Controls.Add(this.label8);
            this.TcpPanel.Controls.Add(this.Tcp_Btn_DisCon);
            this.TcpPanel.Controls.Add(this.Tcp_Btn_Con);
            this.TcpPanel.Location = new System.Drawing.Point(0, 0);
            this.TcpPanel.Name = "TcpPanel";
            this.TcpPanel.Size = new System.Drawing.Size(150, 276);
            this.TcpPanel.TabIndex = 14;
            this.TcpPanel.Visible = false;
            // 
            // ServerCheck
            // 
            this.ServerCheck.AutoSize = true;
            this.ServerCheck.Location = new System.Drawing.Point(47, 85);
            this.ServerCheck.Name = "ServerCheck";
            this.ServerCheck.Size = new System.Drawing.Size(88, 16);
            this.ServerCheck.TabIndex = 16;
            this.ServerCheck.Text = "서버 활성화";
            this.ServerCheck.UseVisualStyleBackColor = true;
            this.ServerCheck.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // PortNumber
            // 
            this.PortNumber.FormattingEnabled = true;
            this.PortNumber.Location = new System.Drawing.Point(59, 27);
            this.PortNumber.Name = "PortNumber";
            this.PortNumber.Size = new System.Drawing.Size(87, 20);
            this.PortNumber.TabIndex = 13;
            // 
            // IpNumber
            // 
            this.IpNumber.FormattingEnabled = true;
            this.IpNumber.Location = new System.Drawing.Point(50, 59);
            this.IpNumber.Name = "IpNumber";
            this.IpNumber.Size = new System.Drawing.Size(96, 20);
            this.IpNumber.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "포트 :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "IP :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "TCP 설정";
            // 
            // Tcp_Btn_DisCon
            // 
            this.Tcp_Btn_DisCon.Location = new System.Drawing.Point(9, 219);
            this.Tcp_Btn_DisCon.Name = "Tcp_Btn_DisCon";
            this.Tcp_Btn_DisCon.Size = new System.Drawing.Size(62, 23);
            this.Tcp_Btn_DisCon.TabIndex = 1;
            this.Tcp_Btn_DisCon.Text = "연결";
            this.Tcp_Btn_DisCon.UseVisualStyleBackColor = true;
            this.Tcp_Btn_DisCon.Click += new System.EventHandler(this.Tcp_Btn_DisCon_Click);
            // 
            // Tcp_Btn_Con
            // 
            this.Tcp_Btn_Con.Location = new System.Drawing.Point(79, 219);
            this.Tcp_Btn_Con.Name = "Tcp_Btn_Con";
            this.Tcp_Btn_Con.Size = new System.Drawing.Size(62, 23);
            this.Tcp_Btn_Con.TabIndex = 0;
            this.Tcp_Btn_Con.Text = "옵션적용";
            this.Tcp_Btn_Con.UseVisualStyleBackColor = true;
            // 
            // SerialPanel
            // 
            this.SerialPanel.Controls.Add(this.Serial_Btn_F5);
            this.SerialPanel.Controls.Add(this.Serial_Combo_FlowCon);
            this.SerialPanel.Controls.Add(this.Serial_Combo_StopBit);
            this.SerialPanel.Controls.Add(this.Serial_Combo_Parity);
            this.SerialPanel.Controls.Add(this.Serial_Combo_Data);
            this.SerialPanel.Controls.Add(this.Serial_Combo_Baud);
            this.SerialPanel.Controls.Add(this.Serial_Combo_Port);
            this.SerialPanel.Controls.Add(this.label6);
            this.SerialPanel.Controls.Add(this.label5);
            this.SerialPanel.Controls.Add(this.label4);
            this.SerialPanel.Controls.Add(this.label3);
            this.SerialPanel.Controls.Add(this.label2);
            this.SerialPanel.Controls.Add(this.Label_Se_Port);
            this.SerialPanel.Controls.Add(this.Serial_Btn_Con);
            this.SerialPanel.Controls.Add(this.Serial_Btn_DisCon);
            this.SerialPanel.Location = new System.Drawing.Point(3, 3);
            this.SerialPanel.Name = "SerialPanel";
            this.SerialPanel.Size = new System.Drawing.Size(150, 276);
            this.SerialPanel.TabIndex = 7;
            this.SerialPanel.Visible = false;
            // 
            // Serial_Btn_F5
            // 
            this.Serial_Btn_F5.Location = new System.Drawing.Point(10, 248);
            this.Serial_Btn_F5.Name = "Serial_Btn_F5";
            this.Serial_Btn_F5.Size = new System.Drawing.Size(131, 23);
            this.Serial_Btn_F5.TabIndex = 15;
            this.Serial_Btn_F5.Text = "새로고침";
            this.Serial_Btn_F5.UseVisualStyleBackColor = true;
            this.Serial_Btn_F5.Click += new System.EventHandler(this.serial_Refresh_Click);
            // 
            // Serial_Combo_FlowCon
            // 
            this.Serial_Combo_FlowCon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Serial_Combo_FlowCon.FormattingEnabled = true;
            this.Serial_Combo_FlowCon.Location = new System.Drawing.Point(70, 176);
            this.Serial_Combo_FlowCon.Name = "Serial_Combo_FlowCon";
            this.Serial_Combo_FlowCon.Size = new System.Drawing.Size(76, 20);
            this.Serial_Combo_FlowCon.TabIndex = 13;
            this.Serial_Combo_FlowCon.SelectedIndexChanged += new System.EventHandler(this.Serial_Combo_FlowCon_SelectedIndexChanged);
            // 
            // Serial_Combo_StopBit
            // 
            this.Serial_Combo_StopBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Serial_Combo_StopBit.FormattingEnabled = true;
            this.Serial_Combo_StopBit.Location = new System.Drawing.Point(70, 145);
            this.Serial_Combo_StopBit.Name = "Serial_Combo_StopBit";
            this.Serial_Combo_StopBit.Size = new System.Drawing.Size(76, 20);
            this.Serial_Combo_StopBit.TabIndex = 12;
            this.Serial_Combo_StopBit.SelectedIndexChanged += new System.EventHandler(this.Serial_Combo_StopBit_SelectedIndexChanged);
            // 
            // Serial_Combo_Parity
            // 
            this.Serial_Combo_Parity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Serial_Combo_Parity.FormattingEnabled = true;
            this.Serial_Combo_Parity.Location = new System.Drawing.Point(70, 116);
            this.Serial_Combo_Parity.Name = "Serial_Combo_Parity";
            this.Serial_Combo_Parity.Size = new System.Drawing.Size(76, 20);
            this.Serial_Combo_Parity.TabIndex = 11;
            this.Serial_Combo_Parity.SelectedIndexChanged += new System.EventHandler(this.Serial_Combo_Parity_SelectedIndexChanged);
            // 
            // Serial_Combo_Data
            // 
            this.Serial_Combo_Data.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Serial_Combo_Data.FormattingEnabled = true;
            this.Serial_Combo_Data.Location = new System.Drawing.Point(70, 85);
            this.Serial_Combo_Data.Name = "Serial_Combo_Data";
            this.Serial_Combo_Data.Size = new System.Drawing.Size(76, 20);
            this.Serial_Combo_Data.TabIndex = 10;
            this.Serial_Combo_Data.SelectedIndexChanged += new System.EventHandler(this.Serial_Combo_Data_SelectedIndexChanged);
            // 
            // Serial_Combo_Baud
            // 
            this.Serial_Combo_Baud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Serial_Combo_Baud.FormattingEnabled = true;
            this.Serial_Combo_Baud.Location = new System.Drawing.Point(70, 54);
            this.Serial_Combo_Baud.Name = "Serial_Combo_Baud";
            this.Serial_Combo_Baud.Size = new System.Drawing.Size(76, 20);
            this.Serial_Combo_Baud.TabIndex = 9;
            this.Serial_Combo_Baud.SelectedIndexChanged += new System.EventHandler(this.Serial_Combo_Baud_SelectedIndexChanged);
            // 
            // Serial_Combo_Port
            // 
            this.Serial_Combo_Port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Serial_Combo_Port.DropDownWidth = 270;
            this.Serial_Combo_Port.FormattingEnabled = true;
            this.Serial_Combo_Port.IntegralHeight = false;
            this.Serial_Combo_Port.Location = new System.Drawing.Point(70, 22);
            this.Serial_Combo_Port.Name = "Serial_Combo_Port";
            this.Serial_Combo_Port.Size = new System.Drawing.Size(76, 20);
            this.Serial_Combo_Port.TabIndex = 8;
            this.Serial_Combo_Port.SelectedIndexChanged += new System.EventHandler(this.Serial_Combo_Port_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "흐름제어 :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "스탑비트 :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "패리티 :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "데이터 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "속도 :";
            // 
            // Label_Se_Port
            // 
            this.Label_Se_Port.AutoSize = true;
            this.Label_Se_Port.Location = new System.Drawing.Point(32, 27);
            this.Label_Se_Port.Name = "Label_Se_Port";
            this.Label_Se_Port.Size = new System.Drawing.Size(37, 12);
            this.Label_Se_Port.TabIndex = 2;
            this.Label_Se_Port.Text = "포트 :";
            // 
            // Serial_Btn_Con
            // 
            this.Serial_Btn_Con.Location = new System.Drawing.Point(79, 219);
            this.Serial_Btn_Con.Name = "Serial_Btn_Con";
            this.Serial_Btn_Con.Size = new System.Drawing.Size(62, 23);
            this.Serial_Btn_Con.TabIndex = 0;
            this.Serial_Btn_Con.Text = "옵션적용";
            this.Serial_Btn_Con.UseVisualStyleBackColor = true;
            this.Serial_Btn_Con.Click += new System.EventHandler(this.Serial_Btn_OK_Click);
            // 
            // Serial_Btn_DisCon
            // 
            this.Serial_Btn_DisCon.Location = new System.Drawing.Point(10, 219);
            this.Serial_Btn_DisCon.Name = "Serial_Btn_DisCon";
            this.Serial_Btn_DisCon.Size = new System.Drawing.Size(62, 23);
            this.Serial_Btn_DisCon.TabIndex = 14;
            this.Serial_Btn_DisCon.Text = "연결해제";
            this.Serial_Btn_DisCon.UseVisualStyleBackColor = true;
            // 
            // UdpPanel
            // 
            this.UdpPanel.Controls.Add(this.Udp_Btn_DisCon);
            this.UdpPanel.Controls.Add(this.Udp_Btn_Con);
            this.UdpPanel.Controls.Add(this.UServerCheck);
            this.UdpPanel.Controls.Add(this.UPortNumber);
            this.UdpPanel.Controls.Add(this.UIPNumber);
            this.UdpPanel.Controls.Add(this.label9);
            this.UdpPanel.Controls.Add(this.label10);
            this.UdpPanel.Controls.Add(this.label11);
            this.UdpPanel.Controls.Add(this.button4);
            this.UdpPanel.Controls.Add(this.button5);
            this.UdpPanel.Location = new System.Drawing.Point(0, 0);
            this.UdpPanel.Name = "UdpPanel";
            this.UdpPanel.Size = new System.Drawing.Size(150, 276);
            this.UdpPanel.TabIndex = 15;
            this.UdpPanel.Visible = false;
            // 
            // Udp_Btn_DisCon
            // 
            this.Udp_Btn_DisCon.Location = new System.Drawing.Point(11, 222);
            this.Udp_Btn_DisCon.Name = "Udp_Btn_DisCon";
            this.Udp_Btn_DisCon.Size = new System.Drawing.Size(62, 23);
            this.Udp_Btn_DisCon.TabIndex = 19;
            this.Udp_Btn_DisCon.Text = "연결해제";
            this.Udp_Btn_DisCon.UseVisualStyleBackColor = true;
            // 
            // Udp_Btn_Con
            // 
            this.Udp_Btn_Con.Location = new System.Drawing.Point(81, 222);
            this.Udp_Btn_Con.Name = "Udp_Btn_Con";
            this.Udp_Btn_Con.Size = new System.Drawing.Size(62, 23);
            this.Udp_Btn_Con.TabIndex = 18;
            this.Udp_Btn_Con.Text = "옵션적용";
            this.Udp_Btn_Con.UseVisualStyleBackColor = true;
            this.Udp_Btn_Con.Click += new System.EventHandler(this.Udp_Connect_Click);
            // 
            // UServerCheck
            // 
            this.UServerCheck.AutoSize = true;
            this.UServerCheck.Location = new System.Drawing.Point(18, 86);
            this.UServerCheck.Name = "UServerCheck";
            this.UServerCheck.Size = new System.Drawing.Size(88, 16);
            this.UServerCheck.TabIndex = 17;
            this.UServerCheck.Text = "서버 활성화";
            this.UServerCheck.UseVisualStyleBackColor = true;
            this.UServerCheck.CheckedChanged += new System.EventHandler(this.UServerCheck_CheckedChanged);
            // 
            // UPortNumber
            // 
            this.UPortNumber.FormattingEnabled = true;
            this.UPortNumber.Location = new System.Drawing.Point(59, 27);
            this.UPortNumber.Name = "UPortNumber";
            this.UPortNumber.Size = new System.Drawing.Size(87, 20);
            this.UPortNumber.TabIndex = 13;
            // 
            // UIPNumber
            // 
            this.UIPNumber.FormattingEnabled = true;
            this.UIPNumber.Location = new System.Drawing.Point(50, 59);
            this.UIPNumber.Name = "UIPNumber";
            this.UIPNumber.Size = new System.Drawing.Size(96, 20);
            this.UIPNumber.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "포트 :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "IP :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 12);
            this.label11.TabIndex = 5;
            this.label11.Text = "UDP 설정";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(250, 59);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(250, 22);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 0;
            this.button5.Text = "옵션적용";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Btn_Clear);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.Chk_Hexa);
            this.panel2.Location = new System.Drawing.Point(8, 391);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(156, 104);
            this.panel2.TabIndex = 19;
            // 
            // Btn_Clear
            // 
            this.Btn_Clear.Location = new System.Drawing.Point(3, 47);
            this.Btn_Clear.Name = "Btn_Clear";
            this.Btn_Clear.Size = new System.Drawing.Size(63, 23);
            this.Btn_Clear.TabIndex = 24;
            this.Btn_Clear.Text = "Clear";
            this.Btn_Clear.UseVisualStyleBackColor = true;
            this.Btn_Clear.Click += new System.EventHandler(this.Btn_Clear_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 21;
            this.label13.Text = "수신옵션";
            // 
            // Chk_Hexa
            // 
            this.Chk_Hexa.AutoSize = true;
            this.Chk_Hexa.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Chk_Hexa.Location = new System.Drawing.Point(6, 25);
            this.Chk_Hexa.Name = "Chk_Hexa";
            this.Chk_Hexa.Size = new System.Drawing.Size(47, 16);
            this.Chk_Hexa.TabIndex = 8;
            this.Chk_Hexa.Text = "HeX";
            this.Chk_Hexa.UseVisualStyleBackColor = true;
            this.Chk_Hexa.CheckStateChanged += new System.EventHandler(this.Chk_Hexa_CheckedChanged);
            // 
            // LogPanel
            // 
            this.LogPanel.Controls.Add(this.metroLabel7);
            this.LogPanel.Controls.Add(this.ReceiveWindowBox);
            this.LogPanel.HorizontalScrollbarBarColor = true;
            this.LogPanel.HorizontalScrollbarHighlightOnWheel = false;
            this.LogPanel.HorizontalScrollbarSize = 10;
            this.LogPanel.Location = new System.Drawing.Point(170, 186);
            this.LogPanel.Name = "LogPanel";
            this.LogPanel.Size = new System.Drawing.Size(542, 309);
            this.LogPanel.TabIndex = 6;
            this.LogPanel.VerticalScrollbarBarColor = true;
            this.LogPanel.VerticalScrollbarHighlightOnWheel = false;
            this.LogPanel.VerticalScrollbarSize = 10;
            // 
            // metroLabel7
            // 
            this.metroLabel7.AutoSize = true;
            this.metroLabel7.Location = new System.Drawing.Point(5, 2);
            this.metroLabel7.Name = "metroLabel7";
            this.metroLabel7.Size = new System.Drawing.Size(37, 19);
            this.metroLabel7.TabIndex = 16;
            this.metroLabel7.Text = "수신";
            // 
            // ReceiveWindowBox
            // 
            this.ReceiveWindowBox.AcceptsTab = true;
            this.ReceiveWindowBox.AutoWordSelection = true;
            this.ReceiveWindowBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.ReceiveWindowBox.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.ReceiveWindowBox.ImeMode = System.Windows.Forms.ImeMode.On;
            this.ReceiveWindowBox.Location = new System.Drawing.Point(5, 24);
            this.ReceiveWindowBox.Name = "ReceiveWindowBox";
            this.ReceiveWindowBox.ReadOnly = true;
            this.ReceiveWindowBox.Size = new System.Drawing.Size(537, 279);
            this.ReceiveWindowBox.TabIndex = 3;
            this.ReceiveWindowBox.TabStop = false;
            this.ReceiveWindowBox.Text = "";
            // 
            // UDP_Tile
            // 
            this.UDP_Tile.BackColor = System.Drawing.Color.White;
            this.UDP_Tile.ForeColor = System.Drawing.SystemColors.Control;
            this.UDP_Tile.Location = new System.Drawing.Point(5, 70);
            this.UDP_Tile.Name = "UDP_Tile";
            this.UDP_Tile.Size = new System.Drawing.Size(159, 24);
            this.UDP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
            this.UDP_Tile.TabIndex = 9;
            this.UDP_Tile.Text = "UDP";
            this.UDP_Tile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.UDP_Tile.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.UDP_Tile.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.UDP_Tile.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.UDP_Tile.Click += new System.EventHandler(this.UDP_Tile_Click);
            // 
            // TCP_Tile
            // 
            this.TCP_Tile.BackColor = System.Drawing.Color.White;
            this.TCP_Tile.ForeColor = System.Drawing.SystemColors.Control;
            this.TCP_Tile.Location = new System.Drawing.Point(5, 38);
            this.TCP_Tile.Name = "TCP_Tile";
            this.TCP_Tile.Size = new System.Drawing.Size(159, 26);
            this.TCP_Tile.Style = MetroFramework.MetroColorStyle.Silver;
            this.TCP_Tile.TabIndex = 8;
            this.TCP_Tile.Text = "TCP";
            this.TCP_Tile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TCP_Tile.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TCP_Tile.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.TCP_Tile.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.TCP_Tile.Click += new System.EventHandler(this.TCP_Tile_Click);
            // 
            // UART_Tile
            // 
            this.UART_Tile.Location = new System.Drawing.Point(5, 6);
            this.UART_Tile.Name = "UART_Tile";
            this.UART_Tile.Size = new System.Drawing.Size(159, 26);
            this.UART_Tile.Style = MetroFramework.MetroColorStyle.Silver;
            this.UART_Tile.TabIndex = 5;
            this.UART_Tile.Text = "SERIAL";
            this.UART_Tile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.UART_Tile.TileImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.UART_Tile.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.UART_Tile.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.UART_Tile.Click += new System.EventHandler(this.UART_Tile_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.MacroCheck_4);
            this.panel3.Controls.Add(this.MacroCheck_3);
            this.panel3.Controls.Add(this.MacroCheck_2);
            this.panel3.Controls.Add(this.label16);
            this.panel3.Controls.Add(this.MacroCount);
            this.panel3.Controls.Add(this.MacroCheck_1);
            this.panel3.Controls.Add(this.Btn_Send3);
            this.panel3.Controls.Add(this.SendBox3);
            this.panel3.Controls.Add(this.Btn_Send4);
            this.panel3.Controls.Add(this.Btn_Send2);
            this.panel3.Controls.Add(this.SendBox4);
            this.panel3.Controls.Add(this.Btn_Send1);
            this.panel3.Controls.Add(this.SendBox2);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.SendBox1);
            this.panel3.Location = new System.Drawing.Point(170, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(542, 177);
            this.panel3.TabIndex = 21;
            // 
            // MacroCheck_4
            // 
            this.MacroCheck_4.AutoSize = true;
            this.MacroCheck_4.Location = new System.Drawing.Point(512, 132);
            this.MacroCheck_4.Name = "MacroCheck_4";
            this.MacroCheck_4.Size = new System.Drawing.Size(15, 14);
            this.MacroCheck_4.TabIndex = 43;
            this.MacroCheck_4.UseVisualStyleBackColor = true;
            this.MacroCheck_4.CheckedChanged += new System.EventHandler(this.MacroCheck_4_CheckedChanged);
            // 
            // MacroCheck_3
            // 
            this.MacroCheck_3.AutoSize = true;
            this.MacroCheck_3.Location = new System.Drawing.Point(512, 91);
            this.MacroCheck_3.Name = "MacroCheck_3";
            this.MacroCheck_3.Size = new System.Drawing.Size(15, 14);
            this.MacroCheck_3.TabIndex = 42;
            this.MacroCheck_3.UseVisualStyleBackColor = true;
            this.MacroCheck_3.CheckedChanged += new System.EventHandler(this.MacroCheck_3_CheckedChanged);
            // 
            // MacroCheck_2
            // 
            this.MacroCheck_2.AutoSize = true;
            this.MacroCheck_2.Location = new System.Drawing.Point(512, 61);
            this.MacroCheck_2.Name = "MacroCheck_2";
            this.MacroCheck_2.Size = new System.Drawing.Size(15, 14);
            this.MacroCheck_2.TabIndex = 41;
            this.MacroCheck_2.UseVisualStyleBackColor = true;
            this.MacroCheck_2.CheckedChanged += new System.EventHandler(this.MacroCheck_2_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(386, 6);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(51, 12);
            this.label16.TabIndex = 40;
            this.label16.Text = "ms 반복";
            // 
            // MacroCount
            // 
            this.MacroCount.Location = new System.Drawing.Point(444, 2);
            this.MacroCount.Name = "MacroCount";
            this.MacroCount.Size = new System.Drawing.Size(83, 21);
            this.MacroCount.TabIndex = 38;
            // 
            // MacroCheck_1
            // 
            this.MacroCheck_1.AutoSize = true;
            this.MacroCheck_1.Location = new System.Drawing.Point(512, 32);
            this.MacroCheck_1.Name = "MacroCheck_1";
            this.MacroCheck_1.Size = new System.Drawing.Size(15, 14);
            this.MacroCheck_1.TabIndex = 36;
            this.MacroCheck_1.UseVisualStyleBackColor = true;
            this.MacroCheck_1.CheckedChanged += new System.EventHandler(this.MacroCheck_1_CheckedChanged);
            // 
            // Btn_Send3
            // 
            this.Btn_Send3.Location = new System.Drawing.Point(444, 85);
            this.Btn_Send3.Name = "Btn_Send3";
            this.Btn_Send3.Size = new System.Drawing.Size(53, 25);
            this.Btn_Send3.TabIndex = 27;
            this.Btn_Send3.Text = "송신";
            this.Btn_Send3.UseVisualStyleBackColor = true;
            this.Btn_Send3.Click += new System.EventHandler(this.Btn_Send3_Click);
            // 
            // SendBox3
            // 
            this.SendBox3.AcceptsTab = true;
            this.SendBox3.AutoWordSelection = true;
            this.SendBox3.BackColor = System.Drawing.SystemColors.HighlightText;
            this.SendBox3.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.SendBox3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SendBox3.Location = new System.Drawing.Point(6, 86);
            this.SendBox3.Name = "SendBox3";
            this.SendBox3.Size = new System.Drawing.Size(432, 24);
            this.SendBox3.TabIndex = 26;
            this.SendBox3.TabStop = false;
            this.SendBox3.Text = "";
            // 
            // Btn_Send4
            // 
            this.Btn_Send4.Location = new System.Drawing.Point(444, 114);
            this.Btn_Send4.Name = "Btn_Send4";
            this.Btn_Send4.Size = new System.Drawing.Size(53, 48);
            this.Btn_Send4.TabIndex = 25;
            this.Btn_Send4.Text = "송신";
            this.Btn_Send4.UseVisualStyleBackColor = true;
            this.Btn_Send4.Click += new System.EventHandler(this.Btn_Send4_Click);
            // 
            // Btn_Send2
            // 
            this.Btn_Send2.Location = new System.Drawing.Point(444, 55);
            this.Btn_Send2.Name = "Btn_Send2";
            this.Btn_Send2.Size = new System.Drawing.Size(53, 25);
            this.Btn_Send2.TabIndex = 24;
            this.Btn_Send2.Text = "송신";
            this.Btn_Send2.UseVisualStyleBackColor = true;
            this.Btn_Send2.Click += new System.EventHandler(this.Btn_Send2_Click);
            // 
            // SendBox4
            // 
            this.SendBox4.AcceptsTab = true;
            this.SendBox4.AutoWordSelection = true;
            this.SendBox4.BackColor = System.Drawing.SystemColors.HighlightText;
            this.SendBox4.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.SendBox4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SendBox4.Location = new System.Drawing.Point(6, 115);
            this.SendBox4.Name = "SendBox4";
            this.SendBox4.Size = new System.Drawing.Size(432, 47);
            this.SendBox4.TabIndex = 23;
            this.SendBox4.TabStop = false;
            this.SendBox4.Text = "";
            // 
            // Btn_Send1
            // 
            this.Btn_Send1.Location = new System.Drawing.Point(444, 26);
            this.Btn_Send1.Name = "Btn_Send1";
            this.Btn_Send1.Size = new System.Drawing.Size(53, 25);
            this.Btn_Send1.TabIndex = 1;
            this.Btn_Send1.Text = "송신";
            this.Btn_Send1.UseVisualStyleBackColor = true;
            this.Btn_Send1.Click += new System.EventHandler(this.Btn_Send1_Click);
            // 
            // SendBox2
            // 
            this.SendBox2.AcceptsTab = true;
            this.SendBox2.AutoWordSelection = true;
            this.SendBox2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.SendBox2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.SendBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SendBox2.Location = new System.Drawing.Point(6, 56);
            this.SendBox2.Name = "SendBox2";
            this.SendBox2.Size = new System.Drawing.Size(432, 24);
            this.SendBox2.TabIndex = 22;
            this.SendBox2.TabStop = false;
            this.SendBox2.Text = "";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 6);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 12);
            this.label14.TabIndex = 20;
            this.label14.Text = "송신";
            // 
            // SendBox1
            // 
            this.SendBox1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.SendBox1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.SendBox1.Location = new System.Drawing.Point(6, 27);
            this.SendBox1.Name = "SendBox1";
            this.SendBox1.Size = new System.Drawing.Size(432, 24);
            this.SendBox1.TabIndex = 2;
            this.SendBox1.TabStop = false;
            this.SendBox1.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asdfasdfToolStripMenuItem,
            this.ddfdfToolStripMenuItem,
            this.설정ToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 60);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(737, 19);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // asdfasdfToolStripMenuItem
            // 
            this.asdfasdfToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.로그저장ToolStripMenuItem,
            this.로그불러오기ToolStripMenuItem});
            this.asdfasdfToolStripMenuItem.Name = "asdfasdfToolStripMenuItem";
            this.asdfasdfToolStripMenuItem.Size = new System.Drawing.Size(43, 19);
            this.asdfasdfToolStripMenuItem.Text = "메인";
            // 
            // 로그저장ToolStripMenuItem
            // 
            this.로그저장ToolStripMenuItem.Name = "로그저장ToolStripMenuItem";
            this.로그저장ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.로그저장ToolStripMenuItem.Text = "로그 저장";
            this.로그저장ToolStripMenuItem.Click += new System.EventHandler(this.saveLog_Click);
            // 
            // 로그불러오기ToolStripMenuItem
            // 
            this.로그불러오기ToolStripMenuItem.Name = "로그불러오기ToolStripMenuItem";
            this.로그불러오기ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.로그불러오기ToolStripMenuItem.Text = "로그 불러오기";
            this.로그불러오기ToolStripMenuItem.Click += new System.EventHandler(this.openLog_Click);
            // 
            // ddfdfToolStripMenuItem
            // 
            this.ddfdfToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.빈도분석ToolStripMenuItem});
            this.ddfdfToolStripMenuItem.Name = "ddfdfToolStripMenuItem";
            this.ddfdfToolStripMenuItem.Size = new System.Drawing.Size(43, 19);
            this.ddfdfToolStripMenuItem.Text = "분석";
            // 
            // 빈도분석ToolStripMenuItem
            // 
            this.빈도분석ToolStripMenuItem.Name = "빈도분석ToolStripMenuItem";
            this.빈도분석ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.빈도분석ToolStripMenuItem.Text = "빈도 분석";
            this.빈도분석ToolStripMenuItem.Click += new System.EventHandler(this.freq_Click);
            // 
            // 설정ToolStripMenuItem
            // 
            this.설정ToolStripMenuItem.Name = "설정ToolStripMenuItem";
            this.설정ToolStripMenuItem.Size = new System.Drawing.Size(43, 19);
            this.설정ToolStripMenuItem.Text = "설정";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 697);
            this.Controls.Add(this.metroPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(0, 60, 0, 0);
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MultiTerminal";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.metroPanel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PortListGrid)).EndInit();
            this.panel4.ResumeLayout(false);
            this.TcpPanel.ResumeLayout(false);
            this.TcpPanel.PerformLayout();
            this.SerialPanel.ResumeLayout(false);
            this.SerialPanel.PerformLayout();
            this.UdpPanel.ResumeLayout(false);
            this.UdpPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.LogPanel.ResumeLayout(false);
            this.LogPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroTile UART_Tile;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem asdfasdfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ddfdfToolStripMenuItem;
        private MetroFramework.Controls.MetroPanel LogPanel;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        public System.Windows.Forms.RichTextBox ReceiveWindowBox;
        private MetroFramework.Controls.MetroTile TCP_Tile;
        private MetroFramework.Controls.MetroTile UDP_Tile;
        private System.Windows.Forms.RichTextBox SendBox1;
        private System.Windows.Forms.Panel SerialPanel;
        private System.Windows.Forms.ComboBox Serial_Combo_FlowCon;
        private System.Windows.Forms.ComboBox Serial_Combo_StopBit;
        private System.Windows.Forms.ComboBox Serial_Combo_Parity;
        private System.Windows.Forms.ComboBox Serial_Combo_Data;
        private System.Windows.Forms.ComboBox Serial_Combo_Baud;
        private System.Windows.Forms.ComboBox Serial_Combo_Port;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Label_Se_Port;
        private System.Windows.Forms.Button Btn_Send1;
        private System.Windows.Forms.Button Serial_Btn_Con;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.CheckBox Chk_Hexa;
        private System.Windows.Forms.Panel TcpPanel;
        private System.Windows.Forms.Panel UdpPanel;
        private System.Windows.Forms.ComboBox UPortNumber;
        private System.Windows.Forms.ComboBox UIPNumber;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox IpNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button Tcp_Btn_DisCon;
        private System.Windows.Forms.Button Tcp_Btn_Con;
        private System.Windows.Forms.CheckBox ServerCheck;
        private System.Windows.Forms.CheckBox UServerCheck;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RichTextBox SendBox4;
        private System.Windows.Forms.RichTextBox SendBox2;
        private System.Windows.Forms.Button Serial_Btn_DisCon;
        private System.Windows.Forms.Button Btn_Send2;
        private System.Windows.Forms.Button Btn_Send3;
        private System.Windows.Forms.RichTextBox SendBox3;
        private System.Windows.Forms.Button Btn_Send4;
        private System.Windows.Forms.Button Btn_Clear;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox MacroCount;
        private System.Windows.Forms.CheckBox MacroCheck_1;
        private System.Windows.Forms.ComboBox PortNumber;
        private System.Windows.Forms.Button Udp_Btn_DisCon;
        private System.Windows.Forms.Button Udp_Btn_Con;
        private System.Windows.Forms.Button Serial_Btn_F5;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ToolStripMenuItem 로그저장ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 로그불러오기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 빈도분석ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 설정ToolStripMenuItem;
        private System.Windows.Forms.DataGridView PortListGrid;
        private System.Windows.Forms.CheckBox MacroCheck_4;
        private System.Windows.Forms.CheckBox MacroCheck_3;
        private System.Windows.Forms.CheckBox MacroCheck_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Num;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Port;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Tx;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Rx;
        private System.Windows.Forms.DataGridViewButtonColumn Discon;
    }
}

