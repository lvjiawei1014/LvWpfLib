using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public static Brush DefaultFill = new SolidColorBrush(Colors.Lime);

        public List<SignalState> States { get; } = new List<SignalState>();

        public SignalState SignalState { get; private set; }
        public SignalLevel()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(SignalLevel), new FrameworkPropertyMetadata(0d,FrameworkPropertyMetadataOptions.None,  new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject dpobj, DependencyPropertyChangedEventArgs e)
        {
            
            double tmp = Math.Min(1, Math.Max(0, (double)e.NewValue));
            SignalLevel signalLevel = dpobj as SignalLevel;
            bool isStateMatch = false;
            foreach (var item in signalLevel.States)
            {
                if (item.IsValueIn(signalLevel.Value))
                {
                    isStateMatch = true;
                    signalLevel.SignalState = item;
                    signalLevel.ToolTip = item.Name;
                    signalLevel.Fill = item.Fill;
                }
            }
            if (!isStateMatch)
            {
                signalLevel.SignalState = null;
                signalLevel.ToolTip = string.Empty;
                signalLevel.Fill = SignalLevel.DefaultFill;
            }
            signalLevel.PART_Indicator.Width = signalLevel.PART_Track.ActualWidth * tmp;
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(SignalLevel), new PropertyMetadata(DefaultFill));


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double tmp = Math.Min(1, Math.Max(0, (double)Value));
            PART_Indicator.Width = PART_Track.ActualWidth * tmp;
        }


    }

    public interface ISignalState 
    {
        Brush GetFill(double value);
        string GetState(double value);
    }


    public class SignalState
    {
        public Brush Fill { get; set; } = SignalLevel.DefaultFill;

        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 1;


        public bool ContainMin { get; set; } = true;
        public bool ContainMax { get; set; } = true;

        public string Name { get; set; }
        public bool IsValueIn(double value)
        {
            return ((value > MinValue) && (value < MaxValue) || (ContainMin && value == MinValue) || (ContainMax && value == MaxValue));
        }

    }

}
