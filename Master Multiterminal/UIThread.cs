using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
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
}
