using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MultiTerminal
{
    public partial class analysisForm : MetroFramework.Forms.MetroForm
    {
        private RichTextBox ReceiveWindowBox;
        private List<string> connectedName = new List<string>();
        public analysisForm(RichTextBox ReceiveWindowBox)
        {
            this.ReceiveWindowBox = ReceiveWindowBox;
            InitializeComponent();
            
        }
        private void analysisForm_Load(object sender, EventArgs e)
        {
            frequency fre = new frequency(ReceiveWindowBox);
            connectedName = fre.getConnection();
            int size = connectedName.Count;
            for (int i = 0; i < size; i++)
            {
                connectedNamecheckedListBox.Items.Add(connectedName[i]);
            }
        }
        private void connectedNamecheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool[] selectState = new bool[connectedName.Count];
            for (int i = 0; i < connectedName.Count; i++) selectState[i] = false;
            int index = connectedNamecheckedListBox.SelectedIndex;
            selectState[index] = !selectState[index];
        }

        private void analyStart_Click(object obj, EventArgs e)
        {

        }

        private void analysisForm_Closing(object sender, FormClosingEventArgs e)  // 폼 닫혔을 때 
        {
            this.Dispose();
        }
    }
}
