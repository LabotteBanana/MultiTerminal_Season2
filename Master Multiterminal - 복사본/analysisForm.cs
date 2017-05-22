using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MultiTerminal
{
    public partial class analysisForm : MetroFramework.Forms.MetroForm
    {
        private RichTextBox ReceiveWindowBox;
        private List<string> connectedName = new List<string>();
        private bool[] selectState = null;
        private frequency fre = null;
        private int indexer=0;
        private bool ggomsoo = false;
        public analysisForm(RichTextBox ReceiveWindowBox)
        {
            this.ReceiveWindowBox = ReceiveWindowBox;
            this.BringToFront();
            InitializeComponent();
        }
        private void analysisForm_Load(object sender, EventArgs e)
        {
            fre = new frequency(ReceiveWindowBox);
            connectedName = fre.getConnection(); //로그 내에서 모든 포트 이름 가져오기
            int size = connectedName.Count;
            for (int i = 0; i < size; i++)
            {
                connectedNamecheckedListBox.Items.Add(connectedName[i]);
            }

            selectState = new bool[connectedName.Count];
            for (int i = 0; i < connectedName.Count; i++) selectState[i] = false;
            analyChart.Series.Clear();
        }
        //checkedlistbox 클릭 이벤트
        private void connectedNamecheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = connectedNamecheckedListBox.SelectedIndex;
            this.indexer = index;
        }
        //checkedlistbox itemcheck 이벤트
        private void colnnectedNamecheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ggomsoo == false)
            {
                if (connectedNamecheckedListBox.GetItemCheckState(indexer) != CheckState.Checked)
                {
                    selectState[indexer] = true;
                }
                else
                {
                    selectState[indexer] = false;
                }
            }
        }

        //분석시작 클릭 이벤트
        private void analyStart_Click(object obj, EventArgs e)
        {
            int num = 0;
            for (int i = 0; i < connectedName.Count; i++) {
                if (selectState != null)
                {
                    if (selectState[i] == true)
                        num++;
                }
            }
            if (selectState == null || num == 0)
                MessageBox.Show("분석할 장치를 선택해 주세요.");
            else {
                int graphMaxTime = fre.getgraphTime();
                int analyMax = 0;
                int analySize = 0;
                int[,] freTable = new int[connectedName.Count, graphMaxTime+1];
                freTable = fre.getDivision(connectedName,selectState);
                for (int i = 0; i < connectedName.Count; i++)
                {
                    for (int j = 0; j <= graphMaxTime; j++)
                    {
                        if (freTable[i,j] > analyMax)
                            analyMax = freTable[i, j];
                        analySize++;
                    }
                }
                //차트 그리는 부분
                analyChart.Series.Clear();
                fre.drawingChart(freTable, graphMaxTime, analyMax, analySize, connectedName, analyChart,selectState);
                Controls.Add(analyChart);
            }
        }
        private void allSelect_btn_Click(object sender, EventArgs e)
        {
            ggomsoo = true;
            int itemNum = connectedNamecheckedListBox.Items.Count;
            int allCheck = 0;
            for (int i = 0; i < itemNum; i++)
                if (connectedNamecheckedListBox.GetItemCheckState(i) == CheckState.Checked)
                    allCheck++;
            if (allCheck == itemNum)
                for (int i = 0; i < itemNum; i++)
                {
                    connectedNamecheckedListBox.SetItemChecked(i, false);
                    selectState[i] = false;
                }
            else
                for (int i = 0; i < itemNum; i++)
                {
                    connectedNamecheckedListBox.SetItemChecked(i, true);
                    selectState[i] = true;
                }
            ggomsoo = false;
        }

        private void analysisForm_Closing(object sender, FormClosingEventArgs e)  // 폼 닫혔을 때 
        {
            this.Dispose();
        }
    }
}