using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// ColormapBar.xaml 的交互逻辑
    /// </summary>
    public partial class ColormapBar : UserControl
    {
        private int indicatorNum = 3;
        private ColormapType colormapType;
        private double minValue = 0;
        private double maxValue = 100;
        private bool showNumericalIndicator = true;
        private NumMapType numMapType;
        private Brush indicatorBackground;
        private string indicatorStringFormat = "{0:F2}";

        public ColormapType ColormapType
        {
            get => colormapType; set
            {
                colormapType = value;
                OnTypeSet(value);
            }
        }

        public NumMapType NumMapType
        {
            get => numMapType; set
            {
                numMapType = value;
                OnDataChanged();
            }
        }
        public Brush IndicatorBackground
        {
            get => indicatorBackground; set
            {
                indicatorBackground = value;
                CreateIndicator();
            }
        }

        public double IndicatorHeight { get; set; } = 20;
        public string IndicatorStringFormat
        {
            get => indicatorStringFormat; set
            {
                indicatorStringFormat = value;
                CreateIndicator();
            }
        }
        public bool ShowNumericalIndicator
        {
            get => showNumericalIndicator; set
            {
                showNumericalIndicator = value;
            }
        }
        public double MaxValue
        {
            get => maxValue; set
            {
                maxValue = value;
                OnDataChanged();
            }
        }
        public double MinValue
        {
            get => minValue; set
            {
                minValue = value;
                OnDataChanged();

            }
        }
        public int IndicatorNum
        {
            get => indicatorNum; set
            {
                indicatorNum = value;
                CreateIndicator();
            }
        }

        public ColormapBar()
        {
            this.DataContext = this;
            InitializeComponent();
            this.Loaded += ColormapBar_Loaded;
            this.SizeChanged += ColormapBar_SizeChanged;
        }

        private void ColormapBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.CreateIndicator();
            this.OnDataChanged();
        }

        private void ColormapBar_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void OnTypeSet(ColormapType colormapType)
        {
            switch (colormapType)
            {
                case ColormapType.Gray:
                    this.gridMain.Background = (Brush)this.Resources["colormapGray"];

                    break;
                case ColormapType.Jet:
                    this.gridMain.Background = (Brush)this.Resources["colormapJet"];

                    break;
                default:
                    break;
            }
        }

        public void OnDataChanged()
        {
            if (IndicatorNum < 2) return;
            if (gridIndicator.Children.Count != indicatorNum)
            {
                this.CreateIndicator();
            }
            double[] indicatorValues = new double[indicatorNum];
            indicatorValues[0] = maxValue;
            indicatorValues[indicatorValues.Length - 1] = minValue;
            switch (this.NumMapType)
            {
                case NumMapType.Linear:
                    for (int i = 1; i < indicatorValues.Length - 1; i++)
                    {
                        indicatorValues[i] = maxValue - (maxValue - minValue) * i / (indicatorValues.Length - 1);
                    }
                    break;
                case NumMapType.Log:
                    double logMax = Math.Log(maxValue + 1);
                    double logMin = Math.Log(minValue + 1);
                    for (int i = 1; i < indicatorValues.Length - 1; i++)
                    {
                        indicatorValues[i] = Math.Pow(10, logMax - (logMax - logMin) * i / (indicatorValues.Length - 1)) - 1;
                    }
                    break;
                case NumMapType.Log10:
                    double log10Max = Math.Log10(maxValue + 1);
                    double log10Min = Math.Log10(minValue + 1);
                    for (int i = 1; i < indicatorValues.Length - 1; i++)
                    {
                        indicatorValues[i] = Math.Pow(10, log10Max - (log10Max - log10Min) * i / (indicatorValues.Length - 1)) - 1;
                    }
                    break;
                default:
                    break;
            }
            for (int i = 0; i < indicatorValues.Length; i++)
            {
                Label label = (Label)gridIndicator.Children[i];
                label.Content = string.Format(this.IndicatorStringFormat, indicatorValues[i]);
            }

        }

        /// <summary>
        /// 创建指示器 
        /// </summary>
        public void CreateIndicator()
        {
            gridIndicator.Children.Clear();
            var inter = (this.Height-this.IndicatorHeight) / (this.IndicatorNum - 1);
            main.Margin = new Thickness(0, IndicatorHeight / 2, 0, IndicatorHeight / 2);
            for (int i = 0; i < this.IndicatorNum; i++)
            {
                Label temp = new Label();
                temp.Foreground = this.Foreground;
                temp.Background = this.IndicatorBackground;
                temp.ContentStringFormat = this.ContentStringFormat;
                temp.Name = $"indicator{i}";
                temp.VerticalAlignment = VerticalAlignment.Top;
                temp.HorizontalAlignment = HorizontalAlignment.Left;
                temp.VerticalContentAlignment = VerticalAlignment.Center;
                temp.Height = IndicatorHeight;
                temp.Padding = new Thickness(2);
                temp.Background = this.IndicatorBackground;
                temp.Margin = new Thickness(0,i * inter, 0, 0);
                gridIndicator.Children.Add(temp);
            }

        }

    }
    public enum ColormapType
    {
        Gray = 0,
        Jet = 1,
    }

    public enum NumMapType
    {
        Linear,
        Log,
        Log10,
    }
}
