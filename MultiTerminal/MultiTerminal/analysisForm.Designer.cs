namespace MultiTerminal
{
    partial class analysisForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.connectedNamePanel = new MetroFramework.Controls.MetroPanel();
            this.connectedNamecheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.analyStartButton = new MetroFramework.Controls.MetroButton();
            this.connectedNamePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectedNamePanel
            // 
            this.connectedNamePanel.Controls.Add(this.connectedNamecheckedListBox);
            this.connectedNamePanel.Controls.Add(this.analyStartButton);
            this.connectedNamePanel.HorizontalScrollbarBarColor = true;
            this.connectedNamePanel.HorizontalScrollbarHighlightOnWheel = false;
            this.connectedNamePanel.HorizontalScrollbarSize = 10;
            this.connectedNamePanel.Location = new System.Drawing.Point(23, 63);
            this.connectedNamePanel.Name = "connectedNamePanel";
            this.connectedNamePanel.Size = new System.Drawing.Size(238, 234);
            this.connectedNamePanel.TabIndex = 0;
            this.connectedNamePanel.VerticalScrollbarBarColor = true;
            this.connectedNamePanel.VerticalScrollbarHighlightOnWheel = false;
            this.connectedNamePanel.VerticalScrollbarSize = 10;
            // 
            // connectedNamecheckedListBox
            // 
            this.connectedNamecheckedListBox.FormattingEnabled = true;
            this.connectedNamecheckedListBox.Location = new System.Drawing.Point(0, 0);
            this.connectedNamecheckedListBox.Name = "connectedNamecheckedListBox";
            this.connectedNamecheckedListBox.Size = new System.Drawing.Size(238, 196);
            this.connectedNamecheckedListBox.TabIndex = 4;
            this.connectedNamecheckedListBox.SelectedIndexChanged += new System.EventHandler(this.connectedNamecheckedListBox_SelectedIndexChanged);
            // 
            // analyStartButton
            // 
            this.analyStartButton.Location = new System.Drawing.Point(82, 202);
            this.analyStartButton.Name = "analyStartButton";
            this.analyStartButton.Size = new System.Drawing.Size(75, 23);
            this.analyStartButton.TabIndex = 3;
            this.analyStartButton.Text = "분석 시작";
            this.analyStartButton.Click += new System.EventHandler(this.analyStart_Click);
            // 
            // analysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 469);
            this.Controls.Add(this.connectedNamePanel);
            this.Name = "analysisForm";
            this.Text = "빈도 분석";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.analysisForm_Closing);
            this.Load += new System.EventHandler(this.analysisForm_Load);
            this.connectedNamePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel connectedNamePanel;
        private System.Windows.Forms.CheckedListBox connectedNamecheckedListBox;
        private MetroFramework.Controls.MetroButton analyStartButton;
    }
}