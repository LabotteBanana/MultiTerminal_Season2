using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiTerminal
{
    class GridView
    {
        private int num;
        private string type;
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

        public int Num
        {
            get
            {
                return num;
            }

            set
            {
                num = value;
            }
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

        public GridView(int num , string portname)
        {
            this.num = num;
            this.portname = portname;
            this.time = System.DateTime.Now.ToString("HH-mm-ss");
            this.txCheckedState = false;
            this.rxCheckedState = false;
        }

        public void Grid_DisCon_Button()
        {

        }

        
    }


}