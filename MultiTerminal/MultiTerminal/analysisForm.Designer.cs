using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms.PropertyGridInternal;
using System.Windows.Forms.ComponentModel;
using System.Windows;
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.connectedNamePanel = new MetroFramework.Controls.MetroPanel();
            this.connectedNamecheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.analyStartButton = new MetroFramework.Controls.MetroButton();
            this.analyChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.connectedNamePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.analyChart)).BeginInit();
            this.SuspendLayout();
            // 
            // connectedNamePanel
            // 
            this.connectedNamePanel.Controls.Add(this.metroButton1);
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
            this.connectedNamecheckedListBox.HorizontalScrollbar = true;
            this.connectedNamecheckedListBox.Location = new System.Drawing.Point(0, 0);
            this.connectedNamecheckedListBox.Name = "connectedNamecheckedListBox";
            this.connectedNamecheckedListBox.Size = new System.Drawing.Size(238, 196);
            this.connectedNamecheckedListBox.TabIndex = 4;
            this.connectedNamecheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.colnnectedNamecheckedListBox_ItemCheck);
            this.connectedNamecheckedListBox.SelectedIndexChanged += new System.EventHandler(this.connectedNamecheckedListBox_SelectedIndexChanged);
            // 
            // analyStartButton
            // 
            this.analyStartButton.Location = new System.Drawing.Point(140, 202);
            this.analyStartButton.Name = "analyStartButton";
            this.analyStartButton.Size = new System.Drawing.Size(75, 23);
            this.analyStartButton.TabIndex = 3;
            this.analyStartButton.Text = "분석 시작";
            this.analyStartButton.Click += new System.EventHandler(this.analyStart_Click);
            // 
            // analyChart
            // 
            chartArea3.Name = "ChartArea1";
            this.analyChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.analyChart.Legends.Add(legend3);
            this.analyChart.Location = new System.Drawing.Point(277, 63);
            this.analyChart.Name = "analyChart";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.analyChart.Series.Add(series3);
            this.analyChart.Size = new System.Drawing.Size(659, 300);
            this.analyChart.TabIndex = 1;
            this.analyChart.Text = "analyChart";
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(28, 202);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(75, 23);
            this.metroButton1.TabIndex = 5;
            this.metroButton1.Text = "전체선택";
            this.metroButton1.Click += new System.EventHandler(this.allSelect_btn_Click);
            // 
            // analysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 469);
            this.Controls.Add(this.analyChart);
            this.Controls.Add(this.connectedNamePanel);
            this.Name = "analysisForm";
            this.Text = "빈도 분석";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.analysisForm_Closing);
            this.Load += new System.EventHandler(this.analysisForm_Load);
            this.connectedNamePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.analyChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroPanel connectedNamePanel;
        private System.Windows.Forms.CheckedListBox connectedNamecheckedListBox;
        private MetroFramework.Controls.MetroButton analyStartButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart analyChart;
        private MetroFramework.Controls.MetroButton metroButton1;
    }
}