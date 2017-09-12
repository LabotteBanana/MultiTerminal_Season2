using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Timers;
namespace MultiTerminal
{
    public class UIThread
    { }



    class Global : INotifyPropertyChanged
    {
        public static string globalVar;
        public static string MacroVar;

        public event PropertyChangedEventHandler PropertyChanged;

        public string macroVar
        {
            get { return MacroVar; }
            set
            {
                MacroVar = value;
                // 값 바뀌었을 때 부른당
                OnPropertyChanged("macrovar");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));

            }
        }
    }
    public static class myQueue
    {
        private static List<string> nodes = new List<string>();
        private static int front = 0, rear = 0;
        private static int MAX_QUEUE = 10000;

        public static int Capacity { get { return MAX_QUEUE; } }
        public static int Front { get { return front; } }
        public static int Rear { get { return rear; } }

        private static MainForm MyForm;
        private static RichTextBox Rtb;
        
        public static void Initialize(MainForm myForm, RichTextBox rtb)
        {
            MyForm = myForm;
            Rtb = rtb;

            // 배열 생성 & 메모리 할당
            for (int i = 0; i < MAX_QUEUE + 1; i++)
            {
               nodes.Add(null);
            }
        }

        public static void enqueue(string s)
        {
            int nCount = Count;
            
            if (IsFull)
            {
                if (front < rear) {
                    rear = 1;
                    front = 2;
                }
                else
                {
                    rear++; front++;
                }

            }

            int position = 0;

            // 큐의 후방(rear)이 배열을 벗어났다면
            if (rear == MAX_QUEUE)
            {
                // 후방의 index를 0으로 초기화(순환 큐이므로 계속 돌아온다.)
                rear = 1;
                position = 0;
            }
            else // 그렇지 않다면 그대로 증가
            {
                position = rear;
                rear++;
            }

            // 데이터 삽입
            nodes[position] = s;
        }

        public static string dequeue()
        {
            int nCount = Count;

            if (IsEmpty)
            {
                return null;
            }

            int position = front;

            // 큐의 전방(front)이 배열 끝에 위치해있으면
            if (front == MAX_QUEUE - 1)
                front = 0;
            else
                // 그렇지 않다면 그대로 증가
                front++;

            return nodes[position];
        }
        public static int Count
        {
            get
            {
                // 전방 index가 후방 index보다 앞에 위치해 있다면
                if (front <= rear)
                    return rear - front;
                else
                    return (MAX_QUEUE + 1) - front + rear;
            }
        }
        public static bool IsEmpty
        {
            get
            {
                return front == rear;
            }
        }
        public static bool IsFull
        {
            get
            {
                // 전방 index가 후방 index보다 앞에 위치해 있다면
                if (front < rear)
                    return (rear - front) == MAX_QUEUE;
                else
                    return (rear + 1) == front;
            }
        }
        public static void viewwindow(object obj)
        {
            if (!IsEmpty)
            {
                MyForm.Invoke(new Action(() =>
                {
                    Rtb.AppendText(dequeue());
                    Rtb.SelectionStart = Rtb.Text.Length;
                    Rtb.ScrollToCaret();
                }));
                //Thread.Sleep(20);
            }
        }
    }
}
