using Ncer.UI;
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
        private MainViewModel mainViewModel;
        private CircleElement circle = new CircleElement(100, 100, 100);
        public WindowStyleDemo()
        {
            //this.DataContext = this;
            InitializeComponent();
            this.mainViewModel = base.DataContext as MainViewModel;
        }

        public double Signal
        {
            get => signal; set
            {
                signal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Signal)));
            }
        }

        public MainViewModel MainViewModel { get => mainViewModel; set => mainViewModel = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sl.States.Add(new Ncer.UI.SignalState() { MaxValue = 0.9, MinValue = 0.6 });
            sl.States.Add(new Ncer.UI.SignalState() { MaxValue = 0.6, MinValue = 0,Fill =new SolidColorBrush(Colors.Red) });
            sl.States.Add(new Ncer.UI.SignalState() { MaxValue = 1, MinValue = 0.9,Fill =new SolidColorBrush(Colors.Yellow) });
            sl.SetBinding(SignalLevel.ValueProperty, new Binding("Signal") { Source = MainViewModel, Mode = BindingMode.TwoWay });
            initView();
        }

        public void initView()
        {
            circle.Radius = 20;
            ivMain.AddCircle(circle);
            //ivMain.AddCircle(new CircleElement(100, 100, 40));
            ivMain.InvalidateVisual();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Signal = 0.95;
            MainViewModel.Signal = 0.8;
        }

        private void btnCircle_Click(object sender, RoutedEventArgs e)
        {
            ivMain.CreateElement(ElementType.Circle);
        }
    }
}
