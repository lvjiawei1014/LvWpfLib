using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LvWpfLib;
using LvWpfLib.LvImageView;


namespace LvWpfDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.plot.PlotTitle = "wierqp";
            //this.plot.UpdateLayout();
            BitmapImage image = new BitmapImage(new Uri(@"J:\Project\LvWpfLib\LvWpfDemo\1.jpg"));
            
            imageView.AutoFit = true;
            imageView.Image = image;
        }

        private void btnImage2_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage image = new BitmapImage(new Uri(@"H:\Project\LvWpfLib\LvWpfDemo\2.jpg"));

            imageView.AutoFit = true;
            imageView.Image = image;
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            imageView.AddElement(new RectElement(0, 0, 200, 200));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
           imageView.ChangeMode(ImageViewState.Edit);
        }

        private void BtnDrawRect_Click(object sender, RoutedEventArgs e)
        {
            imageView.CreateElement(ElementType.Rectangle);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var s = new PlotSeries("",Colors.Yellow,2);
            s.points = new Point[] { new Point(0, 100), new Point(100, 200), new Point(200, 666), new Point(800, 200) };
            plot.Series.Add(s);
            plot.RefreshDataAndPlot();
        }

        private void BtnAddPoint_Click(object sender, RoutedEventArgs e)
        {
            var s = new PlotSeries("",Colors.Lime,2);
            s.points = new Point[] { new Point(0, 100), new Point(100, 200), new Point(200, 666), new Point(800, 200) };
            plot.Series.Add(s);
            plot.RefreshDataAndPlot();
        }
    }
}
