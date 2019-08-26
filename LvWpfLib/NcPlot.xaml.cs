using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LvWpfLib
{
    /// <summary>
    /// NcPlot.xaml 的交互逻辑
    /// </summary>
    public partial class NcPlot : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string plotTitle = "Plot";
        private string axisNameY = "Axis Y";
        private string axisNameX = "Axis X";
        private double topSpace = 40;
        private double leftSpace = 40;
        private double bottomSpace = 40;
        private double rightSpace = 40;

        private double maxY = 1000;
        private double minY = 0;
        private double maxX = 1000;
        private double minX = 0;

        private double[] graduateX;
        private double[] graduateY;

        private bool autoFitX = true;
        private bool autoFitY = true;

        double canvasWidth;
        double canvasHeight;

        private List<PlotSeries> series = new List<PlotSeries>();

        /// <summary>
        /// 图形标题
        /// </summary>
        public string PlotTitle
        {
            get => plotTitle; set
            {
                plotTitle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlotTitle"));
            }
        }
        /// <summary>
        /// Y轴名称
        /// </summary>
        public string AxisNameY
        {
            get => axisNameY; set
            {
                axisNameY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AxisNameY"));
            }
        }
        /// <summary>
        /// X轴名称
        /// </summary>
        public string AxisNameX { get => axisNameX; set => axisNameX = value; }
        public double TopSpace { get => topSpace; set => topSpace = value; }
        public double LeftSpace { get => leftSpace; set => leftSpace = value; }
        public double BottomSpace { get => bottomSpace; set => bottomSpace = value; }
        public double RightSpace { get => rightSpace; set => rightSpace = value; }

        public double PlotRegionHeight { get { return this.Height - topSpace - bottomSpace; } }
        public double PlotRegionWidth { get { return this.Width - leftSpace - rightSpace; } }

        public double MaxY { get => maxY; set => maxY = value; }
        public double MinY { get => minY; set => minY = value; }
        public double MaxX { get => maxX; set => maxX = value; }
        public double MinX { get => minX; set => minX = value; }
        public bool AutoFitX { get => autoFitX; set => autoFitX = value; }
        public bool AutoFitY { get => autoFitY; set => autoFitY = value; }
        public List<PlotSeries> Series { get => series; set => series = value; }
        public double[] GraduateX { get => graduateX; set => graduateX = value; }
        public double[] GraduateY { get => graduateY; set => graduateY = value; }

        public NcPlot()
        {
            InitializeComponent();
            this.DataContext = this;
            RefreshData();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {

            base.OnRender(drawingContext);
            FormattedText title = new FormattedText(this.PlotTitle, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("黑体"), this.FontSize + 4, this.Foreground);
            drawingContext.DrawText(title, new Point(this.ActualWidth / 2 - title.Width / 2, this.topSpace / 2 - title.Height / 2));
            this.DrawAxis(drawingContext);
            this.DrawSeries(drawingContext);

        }


        private void DrawAxis(DrawingContext drawingContext)
        {
            Pen AxisPen = new Pen(this.Foreground, 1);
            drawingContext.DrawLine(AxisPen, new Point(this.leftSpace, this.topSpace), new Point(this.leftSpace, this.ActualHeight - this.bottomSpace));
            drawingContext.DrawLine(AxisPen, new Point(this.leftSpace, this.ActualHeight - this.bottomSpace), new Point(this.ActualWidth - this.rightSpace, this.ActualHeight - this.bottomSpace));
            drawingContext.DrawLine(AxisPen, new Point(this.leftSpace, this.topSpace), new Point(this.leftSpace - 4, this.topSpace + 7));
            drawingContext.DrawLine(AxisPen, new Point(this.leftSpace, this.topSpace), new Point(this.leftSpace + 4, this.topSpace + 7));
            drawingContext.DrawLine(AxisPen, new Point(this.ActualWidth - this.rightSpace - 7, this.ActualHeight - this.bottomSpace - 4), new Point(this.ActualWidth - this.rightSpace, this.ActualHeight - this.bottomSpace));
            drawingContext.DrawLine(AxisPen, new Point(this.ActualWidth - this.rightSpace - 7, this.ActualHeight - this.bottomSpace + 4), new Point(this.ActualWidth - this.rightSpace, this.ActualHeight - this.bottomSpace));
            double x, y;
            for (int i = 0; i < graduateX.Length; i++)
            {
                x = leftSpace + canvasWidth * (graduateX[i] - minX) / (maxX - minX);
                drawingContext.DrawLine(AxisPen, new Point(x, topSpace + canvasHeight), new Point(x, topSpace + canvasHeight - 4));
                FormattedText graduateText = new FormattedText(graduateX[i].ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("宋体"), this.FontSize, this.Foreground);
                drawingContext.DrawText(graduateText, new Point(x - graduateText.Width / 2, topSpace + canvasHeight + 10));
            }

            for (int i = 0; i < graduateY.Length; i++)
            {
                y = topSpace + canvasHeight - (canvasHeight * (graduateY[i] - minY) / (maxY - minY));
                drawingContext.DrawLine(AxisPen, new Point(leftSpace, y), new Point(leftSpace + 4, y));
                FormattedText graduateText = new FormattedText(graduateY[i].ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("宋体"), this.FontSize, this.Foreground);
                drawingContext.DrawText(graduateText, new Point(leftSpace - 8 - graduateText.Width, y - graduateText.Height / 2));
            }
        }

        private void DrawSeries(DrawingContext drawingContext)
        {
            foreach (var item in this.Series)
            {
                if (item.points != null && item.Enable)
                {
                    Pen seriesPen = new Pen(new SolidColorBrush(item.Color), item.Tickness);
                    for (int i = 0; i < item.Points.Length - 1; i++)
                    {
                        drawingContext.DrawLine(seriesPen, item.TransportedPoints[i], item.TransportedPoints[i + 1]);
                    }
                }
            }
        }
        public bool HasData()
        {
            foreach (var item in series)
            {
                if (item.points != null && item.points.Length > 0) 
                {
                    return true;
                }
            }
            return false;
        }

        public void RefreshData()
        {
            canvasWidth = this.ActualWidth - this.leftSpace - this.rightSpace;
            canvasHeight = this.ActualHeight - this.topSpace - this.bottomSpace;

            if (AutoFitY && this.Series.Count > 0 && this.HasData())
            {

                foreach (var item in series)
                {
                    if (item.points != null && item.points.Length > 0)
                    {
                        this.MinY = item.Points[0].Y;
                        this.MaxY = this.MinY + 1;
                    }
                }
                foreach (var item in Series)
                {
                    if (item.points != null)
                    {
                        for (int i = 0; i < item.Points.Length; i++)
                        {

                            this.minY = Math.Min(minY, item.points[i].Y);
                            this.maxY = Math.Max(maxY, item.points[i].Y);
                        }

                    }
                }
            }


            foreach (var item in this.Series)
            {
                PointTransform(item);
            }

            double tx = 50 * (maxX - minX) / canvasWidth;
            double ty = 50 * (maxY - minY) / canvasHeight;
            double jgx = ((int)((tx) / Math.Pow(10, ((int)Math.Log10(tx) - 1))) + 1) * Math.Pow(10, ((int)Math.Log10(tx) - 1));
            int sx = (int)(minX / jgx) + 1;
            int ex = (int)(maxX / jgx);
            double jgy = ((int)((ty) / Math.Pow(10, ((int)Math.Log10(ty) - 1))) + 1) * Math.Pow(10, ((int)Math.Log10(ty) - 1));
            int sy = (int)(minY / jgy) + 1;
            int ey = (int)(maxY / jgy);
            graduateX = new double[ex - sx + 1];
            for (int i = sx; i < ex + 1; i++)
            {
                graduateX[i - sx] = jgx * i;
            }
            graduateY = new double[ey - sy + 1];
            for (int i = sy; i < ey + 1; i++)
            {
                graduateY[i - sy] = jgy * i;
            }

        }



        public void RefreshDataAndPlot()
        {
            this.RefreshData();
            this.InvalidateVisual();
        }


        private void PointTransform(PlotSeries plotSeries)
        {
            if (plotSeries.points != null)
            {
                if (plotSeries.transportedPoints == null || plotSeries.transportedPoints.Length != plotSeries.points.Length)
                {
                    plotSeries.transportedPoints = new Point[plotSeries.points.Length];
                }
                for (int i = 0; i < plotSeries.points.Length; i++)
                {
                    var x = this.leftSpace + canvasWidth * (plotSeries.points[i].X - minX) / (this.maxX - this.minX);
                    var y = this.ActualHeight - bottomSpace - (canvasHeight * (plotSeries.points[i].Y - minY) / (this.maxY - this.minY));
                    plotSeries.transportedPoints[i] = new Point(x, y);
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RefreshDataAndPlot();
        }
    }

    public class PlotSeries
    {
        private string name;
        private Color color = Colors.Lime;
        private bool enable = true;
        private double tickness = 1;

        public Point[] points;
        public Point[] transportedPoints;

        public string Name { get => name; set => name = value; }
        public Color Color { get => color; set => color = value; }
        public bool Enable { get => enable; set => enable = value; }
        public Point[] Points { get => points; set => points = value; }
        public double Tickness { get => tickness; set => tickness = value; }
        public Point[] TransportedPoints { get => transportedPoints; set => transportedPoints = value; }

        public PlotSeries(string name, Color color, double tickness)
        {
            this.tickness = tickness;
            this.color = color;
            this.name = name;
        }
    }
}
