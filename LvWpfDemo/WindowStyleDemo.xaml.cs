using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LvWpfDemo
{
    /// <summary>
    /// WindowStyleDemo.xaml 的交互逻辑
    /// </summary>
    public partial class WindowStyleDemo : Window,INotifyPropertyChanged
    {
        private double signal = 0.1;
        public WindowStyleDemo()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        public double Signal
        {
            get => signal; set
            {
                signal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Signal)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sl.States.Add(new Ncer.UI.SignalState() { MaxValue = 0.9, MinValue = 0.6 });
            sl.States.Add(new Ncer.UI.SignalState() { MaxValue = 0.6, MinValue = 0,Fill =new SolidColorBrush(Colors.Red) });
            sl.States.Add(new Ncer.UI.SignalState() { MaxValue = 1, MinValue = 0.9,Fill =new SolidColorBrush(Colors.Yellow) });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Signal = 0.95;
        }
    }
}
