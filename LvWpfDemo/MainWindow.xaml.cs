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
            //var a = new TimePlotSeries("sd", Colors.Lime, 2);
            //var times = new DateTime[] {DateTime.Now,DateTime.Now+TimeSpan.FromMinutes(10), DateTime.Now + TimeSpan.FromMinutes(20),
            //    DateTime.Now+TimeSpan.FromMinutes(30)};
            //var values = new double[] { 10, 5, 10, 20 };

            //a.SetData(times, values,DisplayMode.Normal);
            //plot.Series.Add(a);
            //plot.RefreshDataAndPlot();

            //var b = new TimePlotSeries("sd", Colors.Lime, 2);
            //var times1 = new DateTime[] {DateTime.Now,DateTime.Now+TimeSpan.FromMinutes(10), DateTime.Now + TimeSpan.FromMinutes(20),
            //    DateTime.Now+TimeSpan.FromMinutes(30)};
            //var values2 = new double[] { 10, 5, 10, 20 };
            //b.SetData(times1,values2,DisplayMode.NormalizationAndLogarithm);
            ////b.SetDisplayMode(DisplayMode.NormalizationAndLogarithm);

            //timePlot.Series.Add(b);
            //timePlot.UpdateData();
        }

        private void BtnAddPoint_Click(object sender, RoutedEventArgs e)
        {
            Point[] ps = new Point[2000];
            for (int i = 0; i < 2000; i++)
            {
                ps[i] = new Point(i, i);
            }
            SamplePlotSeries plotseries = new SamplePlotSeries("ssss", Colors.LightBlue, 2);
            plotseries.ImportData(ps);
            plot.Series.Add(plotseries);
            plot.RefreshDataAndPlot();


        }
    }
}
