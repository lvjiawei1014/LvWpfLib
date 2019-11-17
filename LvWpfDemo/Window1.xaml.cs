using LvWpfLib;
using System;
using System.Collections.Generic;
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
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var a = new TimePlotSeries("sd", Colors.Lime, 2);
            a.times = new DateTime[] {DateTime.Now,DateTime.Now+TimeSpan.FromMinutes(10), DateTime.Now + TimeSpan.FromMinutes(20),
                DateTime.Now+TimeSpan.FromMinutes(30)};
            a.rawValues = new double[] { 10, 5, 10, 20 };
            plot.Series.Add(a);
        }
    }
}
