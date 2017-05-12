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
        public analysisForm(RichTextBox ReceiveWindowBox)
        {
            this.ReceiveWindowBox = ReceiveWindowBox;
            this.BringToFront();
            InitializeComponent();
            
        }
        private void analysisForm_Load(object sender, EventArgs e)
        {
            fre = new frequency(ReceiveWindowBox);
            connectedName = fre.getConnection();
            int size = connectedName.Count;
            for (int i = 0; i < size; i++)
            {
                connectedNamecheckedListBox.Items.Add(connectedName[i]);
            }

            selectState = new bool[connectedName.Count];
            for (int i = 0; i < connectedName.Count; i++) selectState[i] = false;
        }
        private void connectedNamecheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = connectedNamecheckedListBox.SelectedIndex;
            this.indexer = index;
        }
        private void colnnectedNamecheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
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
                Console.WriteLine("graphTime : " + graphMaxTime);
                int[,] freTable = new int[connectedName.Count, graphMaxTime+1];
                freTable = fre.getDivision(connectedName,selectState);
                for (int i = 0; i < connectedName.Count; i++)
                {
                    for (int j = 0; j <= graphMaxTime; j++)
                    {
                        Console.Write(freTable[i, j]);
                        if (freTable[i,j] > analyMax)
                            analyMax = freTable[i, j];
                        analySize++;
                    }
                    Console.Write("\n-----\n");
                }
                //차트 그리는 부분
                analyChart.Series.Clear();
                fre.drawingChart(freTable, graphMaxTime, analyMax, analySize, connectedName, analyChart,selectState);
                Controls.Add(analyChart);
            }
        }

        private void analysisForm_Closing(object sender, FormClosingEventArgs e)  // 폼 닫혔을 때 
        {
            this.Dispose();
        }

        private void connectedNamecheckedListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (connectedNamecheckedListBox.GetItemCheckState(indexer) == CheckState.Checked)
            {
                selectState[indexer] = true;
            }
            else
            {
                selectState[indexer] = false;
            }
        }
    }
}
