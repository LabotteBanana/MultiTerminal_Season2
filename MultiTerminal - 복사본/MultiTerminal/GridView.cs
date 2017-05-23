using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiTerminal
{
    public class GridView
    {
        public int MyNum;   // 그리드 자신의 순번
        private string type;    // 들어온 타입의 형태, 스트링 유디피~
        private int typenum; // 들어온 타입의 순번
        private string portname;
        private string time;
        private bool txCheckedState;
        private bool rxCheckedState;

        public bool TxCheckedState
        {
            get { return txCheckedState; }
            set { txCheckedState = value; }
        }

        public bool RxCheckedState
        {
            get { return rxCheckedState; }
            set { rxCheckedState = value; }
        }

   

        public string Portname
        {
            get
            {
                return portname;
            }

            set
            {
                portname = value;
            }
        }

        public string Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public int Typenum
        {
            get
            {
                return typenum;
            }

            set
            {
                typenum = value;
            }
        }

        public GridView(int num , string portname, string type, int typenum)
        {
            this.MyNum = num;
            this.type = type;
            this.Typenum = typenum;
            this.portname = portname;
            this.time = System.DateTime.Now.ToString("HH:mm:ss");
            this.txCheckedState = false;
            this.rxCheckedState = false;
        }

        public void Grid_DisCon_Button()
        {

        }
    }
}