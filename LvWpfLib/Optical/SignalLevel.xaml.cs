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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ncer.UI
{
    /// <summary>
    /// SignalLevel.xaml 的交互逻辑
    /// </summary>
    public partial class SignalLevel : UserControl
    {
        public SignalLevel()
        {
            InitializeComponent();
        }



        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(SignalLevel), new PropertyMetadata(0d,OnValueChanged));



        private static void OnValueChanged(DependencyObject dpobj, DependencyPropertyChangedEventArgs e)
        {
            double tmp = Math.Min(1, Math.Max(0, (double)e.NewValue));
            SignalLevel signalLevel = dpobj as SignalLevel;
            signalLevel.PART_Indicator.Width = signalLevel.PART_Track.ActualWidth * tmp;
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double tmp = Math.Min(1, Math.Max(0, (double)Value));
            PART_Indicator.Width = PART_Track.ActualWidth * tmp;
        }
    }
}
