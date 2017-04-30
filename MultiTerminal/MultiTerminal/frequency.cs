using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiTerminal
{
    class frequency
    {
        private RichTextBox ReceiveWindowbox;
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
                        int end = wow[i].IndexOf(']');
                        string buf = wow[i].Substring(3,end-3);
                        if (connectedName.Contains(buf) == false)
                        {
                            connectedName.Add(buf);
                        }
                    }
                    i++;
                }
                catch (Exception e) {
                    break;
                }
            }
            return connectedName;
        }
        //분석할 때 시간 계산할 메소드
        private void getAllTime()
        {

        }
        //빈도를 증가시킬 메소드
        private void addFrequency()
        {

        }
        //그래프를 그릴 메소드
        //2차원 배열정의 및 구현 필요
        private void graphAnalysis()
        {
            
        }
    }
}
