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


        private DateTime maxTime;
        private DateTime minTime;
        private DateTime[] graduateTime;
        private TimeSpan timeSpan;
        private TimeSpanLevel timeSpanLevel;

        private double[] graduateX;
        private double[] graduateY;

        private bool autoFitX = true;
        private bool autoFitY = true;


        private PlotType plotType = PlotType.Plane;

        double canvasWidth;
        double canvasHeight;

        private List<Series> series = new List<Series>();

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
        public List<Series> Series { get => series; set => series = value; }
        public double[] GraduateX { get => graduateX; set => graduateX = value; }
        public double[] GraduateY { get => graduateY; set => graduateY = value; }
        public double CanvasWidth { get => canvasWidth; set => canvasWidth = value; }
        public double CanvasHeight { get => canvasHeight; set => canvasHeight = value; }
        public PlotType PlotType { get => plotType; set => plotType = value; }
        public DateTime MaxTime { get => maxTime; set => maxTime = value; }
        public DateTime MinTime { get => minTime; set => minTime = value; }
        public DateTime[] GraduateTime { get => graduateTime; set => graduateTime = value; }
        public TimeSpan TimeSpan { get => timeSpan; set => timeSpan = value; }
        public TimeSpanLevel TimeSpanLevel { get => timeSpanLevel; set => timeSpanLevel = value; }

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


        public void DrawAxis(DrawingContext drawingContext)
        {
            Pen AxisPen = new Pen(this.Foreground, 1);
            drawingContext.DrawLine(AxisPen, new Point(this.leftSpace, this.topSpace), new Point(this.leftSpace, this.ActualHeight - this.bottomSpace));
            drawingContext.DrawLine(AxisPen, new Point(this.leftSpace, this.ActualHeight - this.bottomSpace), new Point(this.ActualWidth - this.rightSpace, this.ActualHeight - this.bottomSpace));
            drawingContext.DrawLine(AxisPen, new Point(this.leftSpace, this.topSpace), new Point(this.leftSpace - 4, this.topSpace + 7));
            drawingContext.DrawLine(AxisPen, new Point(this.leftSpace, this.topSpace), new Point(this.leftSpace + 4, this.topSpace + 7));
            drawingContext.DrawLine(AxisPen, new Point(this.ActualWidth - this.rightSpace - 7, this.ActualHeight - this.bottomSpace - 4), new Point(this.ActualWidth - this.rightSpace, this.ActualHeight - this.bottomSpace));
            drawingContext.DrawLine(AxisPen, new Point(this.ActualWidth - this.rightSpace - 7, this.ActualHeight - this.bottomSpace + 4), new Point(this.ActualWidth - this.rightSpace, this.ActualHeight - this.bottomSpace));
            double x, y;
            switch (this.PlotType)
            {
                case PlotType.Plane:
                    for (int i = 0; i < graduateX.Length; i++)
                    {
                        x = leftSpace + CanvasWidth * (graduateX[i] - minX) / (maxX - minX);
                        drawingContext.DrawLine(AxisPen, new Point(x, topSpace + CanvasHeight), new Point(x, topSpace + CanvasHeight - 4));
                        FormattedText graduateText = new FormattedText(graduateX[i].ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("宋体"), this.FontSize, this.Foreground);
                        drawingContext.DrawText(graduateText, new Point(x - graduateText.Width / 2, topSpace + CanvasHeight + 10));
                    }
                    break;
                case PlotType.TimePlot:

                    switch (this.TimeSpanLevel)
                    {
                        case TimeSpanLevel.Minute:

                            break;
                        case TimeSpanLevel.Hour:
                            break;
                        case TimeSpanLevel.day:
                            break;
                        case TimeSpanLevel.Month:
                            break;
                        case TimeSpanLevel.Year:
                            break;
                        default:
                            break;
                    }

                    break;
                default:
                    break;
            }

            

            for (int i = 0; i < graduateY.Length; i++)
            {
                y = topSpace + CanvasHeight - (CanvasHeight * (graduateY[i] - minY) / (maxY - minY));
                drawingContext.DrawLine(AxisPen, new Point(leftSpace, y), new Point(leftSpace + 4, y));
                FormattedText graduateText = new FormattedText(graduateY[i].ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("宋体"), this.FontSize, this.Foreground);
                drawingContext.DrawText(graduateText, new Point(leftSpace - 8 - graduateText.Width, y - graduateText.Height / 2));
            }
        }

        /// <summary>
        /// 绘制图线
        /// </summary>
        /// <param name="drawingContext"></param>
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
        /// <summary>
        /// 更新数据
        /// </summary>
        public void RefreshData()
        {
            CanvasWidth = this.ActualWidth - this.leftSpace - this.rightSpace;
            CanvasHeight = this.ActualHeight - this.topSpace - this.bottomSpace;
            //y轴自适应
            if (AutoFitY && this.Series.Count > 0 && this.HasData())
            {

                foreach (var item in series)
                {
                    if (item.points != null && item.points.Length > 0)
                    {
                        this.MinY = item.Points[0].Y;
                        this.MaxY = this.MinY;
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
                if (this.minY == this.maxY)
                {
                    this.maxY += 1;
                }
            }
            //x轴自适应
            if(AutoFitY && this.Series.Count > 0 && this.HasData())
            {
                switch (this.PlotType)
                {
                    case PlotType.Plane:
                        foreach (var item in series)
                        {
                            if (item.points != null && item.points.Length > 0 && item is PlotSeries)
                            {
                                this.MinX = item.Points[0].X;
                                this.MaxX = this.MinX;
                            }
                        }
                        foreach (var item in Series)
                        {
                            if (item.points != null && item is PlotSeries)
                            {
                                for (int i = 0; i < item.Points.Length; i++)
                                {

                                    this.minX = Math.Min(minX, item.points[i].X);
                                    this.maxX = Math.Max(maxX, item.points[i].X);
                                }

                            }
                        }
                        if (this.minX == this.maxX)
                        {
                            this.maxX += 1;
                        }

                        break;
                    case PlotType.TimePlot:
                        foreach (var item in series)
                        {
                            if (item.points != null && item.points.Length > 0 && item is TimePlotSeries)
                            {
                                this.minTime = (item as TimePlotSeries).times[0];
                                this.MaxY = this.MinY;
                            }
                        }
                        foreach (var item in Series)
                        {
                            if (item.points != null && item is TimePlotSeries)
                            {
                                var series = (item as TimePlotSeries);
                                for (int i = 0; i < item.Points.Length; i++)
                                {

                                    this.minTime = minTime <series.times[i]?minTime : series.times[i];
                                    this.maxTime = maxTime > series.times[i] ? maxTime : series.times[i];
                                }

                            }
                        }
                        if (this.minTime == this.maxTime)
                        {
                            maxTime = maxTime + TimeSpan.FromSeconds(1000);
                        }


                        break;
                    default:
                        break;
                }
            }

            foreach (var item in this.Series)
            {
                //PointTransform(item);
                item.PointTransform(this);
            }
            //计算刻度 数值
            double tx = 50 * (maxX - minX) / CanvasWidth;
            double ty = 50 * (maxY - minY) / CanvasHeight;
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
            //时间刻度
            long x = (this.maxTime - this.minTime).Ticks * 80 / (long)(this.canvasWidth);
            long interval;
            if (x < TimeSpan.FromMinutes(1).Ticks)
            {
                this.timeSpanLevel = TimeSpanLevel.Minute;
                interval = TimeSpan.FromMinutes(1).Ticks;

            }
            else if (x < TimeSpan.FromMinutes(60).Ticks)
            {
                this.timeSpanLevel = TimeSpanLevel.Hour;
                interval = TimeSpan.FromMinutes(60).Ticks;
            }
            else if (x < TimeSpan.FromHours(24).Ticks)
            {
                this.timeSpanLevel = TimeSpanLevel.day;
                interval = TimeSpan.FromDays(1).Ticks;
            }
            else if (x < TimeSpan.FromDays(30).Ticks)
            {
                this.timeSpanLevel = TimeSpanLevel.Month;
                interval = TimeSpan.FromDays(30).Ticks;
            }
            else 
            {
                this.timeSpanLevel = TimeSpanLevel.Year;
                int y = (int)Math.Log10(x / TimeSpan.FromDays(365).Ticks);
                interval = (x / (TimeSpan.FromDays(365).Ticks * ((long)Math.Pow(10, y)))) * TimeSpan.FromDays(365).Ticks * ((long)Math.Pow(10, y));
                interval = Math.Max(interval, TimeSpan.FromDays(365).Ticks);
            }

            var start = (minTime.Ticks / interval + 1)*interval;
            int count = (int)((maxTime.Ticks - start) / interval) + 1;
            graduateTime = new DateTime[count];
            for (int i = 0; i < count; i++)
            {
                graduateTime[i] = new DateTime(start + interval * i);
            }
            
        }


        /// <summary>
        /// 更新数据和图形
        /// </summary>
        public void RefreshDataAndPlot()
        {
            this.RefreshData();
            this.InvalidateVisual();
        }

        /// <summary>
        /// 计算图形数据的实际坐标
        /// 
        /// </summary>
        /// <param name="plotSeries"></param>
        public void PointTransform(PlotSeries plotSeries)
        {
            if (plotSeries.points != null)
            {
                if (plotSeries.transportedPoints == null || plotSeries.transportedPoints.Length != plotSeries.points.Length)
                {
                    plotSeries.transportedPoints = new Point[plotSeries.points.Length];
                }
                for (int i = 0; i < plotSeries.points.Length; i++)
                {
                    var x = this.leftSpace + CanvasWidth * (plotSeries.points[i].X - minX) / (this.maxX - this.minX);
                    var y = this.ActualHeight - bottomSpace - (CanvasHeight * (plotSeries.points[i].Y - minY) / (this.maxY - this.minY));
                    plotSeries.transportedPoints[i] = new Point(x, y);
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RefreshDataAndPlot();
        }
    }


    public enum PlotType
    {
        Plane=0,
        TimePlot=1,
    }

    public enum TimeSpanLevel
    {
       Minute=1,
       Hour=2,
       day=3,
       Month=4,
       Year=5,
    }



    public abstract class Series
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

        /// <summary>
        /// 数据在当前坐标系中的实际坐标
        /// </summary>
        public Point[] TransportedPoints { get => transportedPoints; set => transportedPoints = value; }
        public abstract void PointTransform(NcPlot plot);

    }

    public class PlotSeries:Series
    {
        
        

        public PlotSeries(string name, Color color, double tickness)
        {
            this.Tickness = tickness;
            this.Color = color;
            this.Name = name;
        }


        public override void PointTransform(NcPlot plot)
        {
            if (this.points != null)
            {
                if (this.transportedPoints == null || this.transportedPoints.Length != this.points.Length)
                {
                    this.transportedPoints = new Point[this.points.Length];
                }
                for (int i = 0; i < this.points.Length; i++)
                {
                    var x = plot.LeftSpace + plot.CanvasWidth * (this.points[i].X - plot.MinX) / (plot.MaxX - plot.MinX);
                    var y = plot.ActualHeight - plot.BottomSpace - (plot.CanvasHeight * (this.points[i].Y - plot.MinY) / (plot.MaxY - plot.MinY));
                    this.transportedPoints[i] = new Point(x, y);
                }
            }
            else
            {
                this.transportedPoints = null;
            }
        }


    }

    public class TimePlotSeries : Series
    {
        public double[] values;

        public DateTime[] times;

        public TimePlotSeries(string name, Color color, double tickness)
        {
            this.Tickness = tickness;
            this.Color = color;
            this.Name = name;
        }


        public override void PointTransform(NcPlot plot)
        {

            if (this.values == null || this.times == null || this.times.Length != this.values.Length ||plot.PlotType!=PlotType.TimePlot)
            {
                this.transportedPoints = null;
            }
            if (this.transportedPoints == null || this.transportedPoints.Length != this.values.Length)
            {
                this.transportedPoints = new Point[this.values.Length];
            }


            for (int i = 0; i < this.points.Length; i++)
            {
                var x = plot.LeftSpace + plot.CanvasWidth * ((double)(this.times[i].Ticks - plot.MinTime.Ticks)) / (plot.MaxTime.Ticks - plot.MinTime.Ticks);
                var y = plot.ActualHeight - plot.BottomSpace - (plot.CanvasHeight * (this.points[i].Y - plot.MinY) / (plot.MaxY - plot.MinY));
                this.transportedPoints[i] = new Point(x, y);
            }

        }
    }
}
