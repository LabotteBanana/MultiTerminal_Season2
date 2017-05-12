using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MultiTerminal
{
    class frequency
    {
        private RichTextBox ReceiveWindowbox;
        private int graphTime = 0;
        private string preTime = null;
        public frequency(RichTextBox ReceiveWindowBox)
        {
            this.ReceiveWindowbox = ReceiveWindowBox;
        }

        //이름만 가져와서 선택 체크박스로 선택 가능하게.
        public List<string> getConnection()
        {
            string[] wow = ReceiveWindowbox.Text.Split('\n');
            List<string> connectedName = new List<string>();
            int i = 0;
            while (true)
            {
                try
                {
                    if (wow[i].Contains("수신"))
                    {
                        int end = wow[i].IndexOf(')');
                        string buf = wow[i].Substring(3,end-3);
                        if (connectedName.Contains(buf) == false)
                        {
                            connectedName.Add(buf);
                        }
                        graphTime += getAllTime(wow[i]);
                    }
                    i++;
                }
                catch (Exception e) {
                    //MessageBox.Show(e.ToString());
                    break;
                }
            }
            return connectedName;
        }
        //분석할 때 시간 계산할 메소드
        private int getAllTime(string wow)
        {
            try
            {
                int timestart = wow.IndexOf('[');
                int timeend = wow.IndexOf(']');
                string subTime = wow.Substring(timestart + 1, timeend - timestart);
                int alltime = 0;
                if (preTime != null)
                {
                    string[] computeTime = subTime.Split(':');
                    string[] computePreTime = preTime.Split(':');
                    int hour = Int32.Parse(computeTime[0]);
                    int minute = Int32.Parse(computeTime[1]);
                    int sec = Int32.Parse(computeTime[2]);
                    int prehour = Int32.Parse(computePreTime[0]);
                    int preminute = Int32.Parse(computePreTime[1]);
                    int presec = Int32.Parse(computePreTime[2]);
                    if (hour < prehour) alltime = (hour + 24 - prehour) * 3600;
                    else alltime = (hour - prehour) * 3600;
                    if (minute < preminute) alltime = (minute + 60 - prehour) * 60;
                    else alltime = (minute - preminute) * 60;
                    if (sec < presec) alltime = (sec + 60 - presec);
                    else alltime = (sec - presec);
                }
                preTime = subTime;
                return alltime;
            }
            catch (Exception e) {
                return 0;
            }
        }

        //2차원 배열정의 및 구현 필요
        public int[,] getDivision(List<string> connectedName,bool[] selectState)
        {
            preTime = null;
            int allTime = 0;
            int[,] freTable = new int[connectedName.Count,graphTime + 1];
            for (int i = 0; i < connectedName.Count; i++) {
                for (int j = 0; j < graphTime + 1; j++)
                    freTable[i, j] = 0;
            }
            string[] frewow = ReceiveWindowbox.Text.Split('\n');
            int ii = 0;
            while (true)
            {
                try
                {
                    if (frewow[ii].Contains("수신"))
                    {
                        int end = frewow[ii].IndexOf(')');
                        string buf = frewow[ii].Substring(3, end - 3);
                        int row = connectedName.IndexOf(buf);
                        allTime += getAllTime(frewow[ii]);
                        if (selectState[connectedName.IndexOf(buf)] != true)
                        {
                            ii++;
                            continue;
                        }
                        freTable[row, allTime]++;
                    }
                    ii++;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    break;
                }
            }

            return freTable;
        }
        public void drawingChart(int[,] freTable, int graphMaxTime, int analyMax, int analySize, List<string> connectedName, Chart analyChart, bool[] selectState)
        {
            int freMax = analyMax;
            int timeMax = graphMaxTime;
            int stateSize = 0;
            for (int i = 0; i < selectState.Length; i++)
            {
                if (selectState[i] == true)
                    stateSize++;
            }
            //선택한 것만 가져오기 위해서 하는 처리
            int[] clientSeleted = new int[stateSize];
            stateSize = 0;
            for (int i = 0; i < selectState.Length; i++)
            {
                if (selectState[i] == true)
                    clientSeleted[stateSize++] = i;
            }

            Series[] analyGraph = new Series[clientSeleted.Length];
            Random r = new Random();
            for (int i = 0; i < clientSeleted.Length; i++)
            {
                analyGraph[i] = analyChart.Series.Add(connectedName[clientSeleted[i]]);
                analyGraph[i].ChartType = SeriesChartType.Line;
                analyGraph[i].BorderWidth = 2;
                analyGraph[i].Color = Color.FromArgb(r.Next(128, 256),r.Next(128,256),r.Next(128,256));
                for (int j = 0; j <= graphMaxTime; j++)
                {
                    analyGraph[i].Points.AddXY(j, freTable[clientSeleted[i], j]);
                }
            }

            chartSetting(analyChart, graphMaxTime, analyMax);
        }
        private void chartSetting(Chart analyChart, int graphMaxTime, int analyMax)
        {
            analyChart.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            analyChart.ChartAreas["ChartArea1"].AxisX.Maximum = graphMaxTime;
            analyChart.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            analyChart.ChartAreas["ChartArea1"].AxisY.Maximum = analyMax;
            analyChart.ChartAreas["ChartArea1"].AxisY.Interval = 1;
        }

        public int getgraphTime() {
            return this.graphTime;
        }
    }
}
