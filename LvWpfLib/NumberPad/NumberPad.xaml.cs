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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ncer.UI
{
    /// <summary>
    /// NumberPad.xaml 的交互逻辑
    /// </summary>
    public partial class NumberPad : UserControl
    {
        private NumberPadViewModel ViewModel { get; set; }
        public NumberPad()
        {
            InitializeComponent();
            this.ViewModel = base.DataContext as NumberPadViewModel;
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            txtNumber.Text += 1;
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            if (txtNumber.Text.Length > 1)
            {
                txtNumber.Text = txtNumber.Text.Substring(0, txtNumber.Text.Length - 1);
            }
            else
            {
                ViewModel.NumberValue = 0;
            }
        }

        private void btnAC_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NumberValue = 0;
        }

        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NumberValue = -ViewModel.NumberValue;
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            txtNumber.Text += 2;

        }

        private void btnDot_Click(object sender, RoutedEventArgs e)
        {
            txtNumber.Text += ".";
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            txtNumber.Text += 4;

        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            txtNumber.Text += 5;

        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            txtNumber.Text += 6;

        }
    }
}
