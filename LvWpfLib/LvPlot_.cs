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
using System.ComponentModel;

namespace LvWpfLib
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:LvWpfLib"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:LvWpfLib;assembly=LvWpfLib"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class LvPlot_ : Control,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string plotTitle="Plot";

        //huitu 参数
        private double plotRegionLeft = 20;
        private double plotRegionTop = 40;
        private double plotRegionRight = 20;
        private double plotRegionBottom = 40;
        private double plotRegionWidth =100;
        private double plotRegionHeight =100;

        public string PlotTitle { get => plotTitle; set { plotTitle = value;PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlotTitle")); } }

        public double PlotRegionHeight { get { return this.Height-this.PlotRegionTop-this.PlotRegionBottom; } set => plotRegionHeight = value; }

        public double PlotRegionTop { get { return plotRegionTop; } set => plotRegionTop = value; }
        public double PlotRegionWidth { get { return this.Width - this.PlotRegionLeft - this.PlotRegionRight; } set => plotRegionWidth = value; }
        public double PlotRegionLeft { get => plotRegionLeft; set => plotRegionLeft = value; }
        public double PlotRegionRight { get => plotRegionRight; set => plotRegionRight = value; }
        public double PlotRegionBottom { get => plotRegionBottom; set => plotRegionBottom = value; }

        static LvPlot_()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LvPlot_), new FrameworkPropertyMetadata(typeof(LvPlot_)));

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        private void DrawTitle()
        {
            //this.
        }
    }
}
