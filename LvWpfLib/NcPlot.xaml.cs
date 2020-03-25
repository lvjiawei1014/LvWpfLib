using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Ncer.UI
{
    /// <summary>
    /// NcPlot.xaml 的交互逻辑
    /// </summary>
    public partial class NcPlot : UserControl
    {
        #region 布局与显示相关
        private string plotTitle = "Plot";
        private string axisNameY = "Axis Y";
        private string axisNameX = "Axis X";

        private double topSpace = 40;
        private double leftSpace = 40;
        private double bottomSpace = 40;
        private double rightSpace = 40;


        private double scale = 1.0;

        private double maxY = 1000;
        private double minY = 0;
        private double maxX = 1000;
        private double minX = 0;

        private double[] graduateX;
        private double[] graduateY;

        private bool autoFitX = true;
        private bool autoFitY = true;

        StreamGeometry streamGeometry;

        #endregion

        double canvasWidth;
        double canvasHeight;
        #region 数据与内容

        private List<SamplePlotSeries> series = new List<SamplePlotSeries>();
        private List<PlotItem> items = new List<PlotItem>();


        #endregion
        #region 属性
        /// <summary>
        /// 图线显示模式
        /// </summary>
        private DisplayMode DisplayMode { get; set; } = DisplayMode.Normal;

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
        /// <summary>
        /// 图线系列
        /// </summary>
        public List<SamplePlotSeries> Series { get => series; set => series = value; }
        public double[] GraduateX { get => graduateX; set => graduateX = value; }
        public double[] GraduateY { get => graduateY; set => graduateY = value; }
        public double CanvasWidth { get => canvasWidth; set => canvasWidth = value; }
        public double CanvasHeight { get => canvasHeight; set => canvasHeight = value; }


        /// <summary>
        /// 附加项目
        /// </summary>
        public List<PlotItem> Items { get => items; set => items = value; }
        public string PlotTitle { get => plotTitle; set => plotTitle = value; }
        public string AxisNameY { get => axisNameY; set => axisNameY = value; }


        #endregion

        public NcPlot()
        {
            InitializeComponent();
            this.DataContext = this;
            RefreshData();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            base.OnRender(drawingContext);
            FormattedText title = new FormattedText(this.PlotTitle, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("黑体"), this.FontSize + 4, this.Foreground);
            drawingContext.DrawText(title, new Point(this.ActualWidth / 2 - title.Width / 2, this.topSpace / 2 - title.Height / 2));
            this.DrawAxis(drawingContext);
            this.DrawSeries(drawingContext);
            stopwatch.Stop();
            System.Console.WriteLine("render time:" + stopwatch.Elapsed.TotalMilliseconds.ToString("0.000") + "ms");
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

                    for (int i = 0; i < graduateX.Length; i++)
                    {
                        x = leftSpace + CanvasWidth * (graduateX[i] - minX) / (maxX - minX);
                        drawingContext.DrawLine(AxisPen, new Point(x, topSpace + CanvasHeight), new Point(x, topSpace + CanvasHeight - 4));
                        FormattedText graduateText = new FormattedText(graduateX[i].ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("宋体"), this.FontSize, this.Foreground);
                        drawingContext.DrawText(graduateText, new Point(x - graduateText.Width / 2, topSpace + CanvasHeight + 10));
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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var item in this.Series)
            {
                if (item.HasData() && item.Enable &&item.Geometry!=null)
                {
                    Pen seriesPen = new Pen(new SolidColorBrush(item.Color), item.Tickness);
                    //drawingContext.DrawGeometry(null, seriesPen, item.Geometry);
                    //plotCanvas.
                    //for (int i = 0; i < item.TransportedPoints.Length - 1; i++)
                    //{
                    //    drawingContext.DrawLine(seriesPen, item.TransportedPoints[i], item.TransportedPoints[i + 1]);
                    //}


                    //using (StreamGeometryContext ctx = streamGeometry.Open())
                    //{
                    //    ctx.BeginFigure(item.transportedPoints[0], false, false);
                    //    for (int i = 1; i < item.transportedPoints.Length; i++)
                    //    {
                    //        ctx.LineTo(item.transportedPoints[i], false, false);
                    //    }
                    //}

                }
            }


            sw.Stop();
            System.Console.WriteLine("draw time:" + sw.Elapsed.TotalMilliseconds.ToString("0.0000") + "ms");

        }


        
        public bool HasData()
        {
            foreach (var item in series)
            {
                if (item.HasData())
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
                    if (item.HasData())
                    {
                        this.MinY = item.MinValue();
                        this.MaxY = this.MinY;
                    }
                }
                foreach (var item in Series)
                {
                    if (item.HasData())
                    {
                        this.MinY = Math.Min(MinY, item.MinValue());
                        this.MaxY = Math.Max(MaxY, item.MaxValue());

                    }
                }
                if (this.minY == this.maxY)
                {
                    this.maxY += 1;
                }
            }
            //x轴自适应
            if (AutoFitY && this.Series.Count > 0 && this.HasData())
            {

                        foreach (var item in series)
                        {
                            if (item.Points != null && item.Points.Count > 0 && item is PlotSeries)
                            {
                                this.MinX = item.Points[0].X;
                                this.MaxX = this.MinX;
                            }
                        }
                        foreach (var item in Series)
                        {
                            if (item.Points != null && item is PlotSeries)
                            {
                                for (int i = 0; i < item.Points.Count; i++)
                                {

                                    this.minX = Math.Min(minX, item.Points[i].X);
                                    this.maxX = Math.Max(maxX, item.Points[i].X);
                                }

                            }
                        }
                        if (this.minX == this.maxX)
                        {
                            this.maxX += 1;
                        }


                
            }

            foreach (var item in this.Series)
            {
                //PointTransform(item);
                item.RefreshCoordinate(this);
            }
            //this.CreateGeomotry();
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


        }


        /// <summary>
        /// 更新数据和图形
        /// </summary>
        public void RefreshDataAndPlot()
        {
            this.RefreshData();
            this.InvalidateVisual();
        }

        ///// <summary>
        ///// 计算图形数据的实际坐标
        ///// 
        ///// </summary>
        ///// <param name="plotSeries"></param>
        //public void PointTransform(PlotSeries plotSeries)
        //{
        //    if (plotSeries.points != null)
        //    {
        //        if (plotSeries.transportedPoints == null || plotSeries.transportedPoints.Length != plotSeries.points.Length)
        //        {
        //            plotSeries.transportedPoints = new Point[plotSeries.points.Length];
        //        }
        //        for (int i = 0; i < plotSeries.points.Length; i++)
        //        {
        //            var x = this.leftSpace + CanvasWidth * (plotSeries.points[i].X - minX) / (this.maxX - this.minX);
        //            var y = this.ActualHeight - bottomSpace - (CanvasHeight * (plotSeries.points[i].Y - minY) / (this.maxY - this.minY));
        //            plotSeries.transportedPoints[i] = new Point(x, y);
        //        }
        //    }
        //}

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RefreshDataAndPlot();
        }
    }

    public class SamplePlotSeries
    {


        private string name;
        private Color color = Colors.Lime;
        private bool enable = true;
        private double tickness = 1;
        private List<Point> points = new List<Point>();
        private List<Point> coordinatePoints=null;

        private StreamGeometry streamGeometry;
        private NcPlot plot;

        public StreamGeometry Geometry { get => streamGeometry; set => streamGeometry = value; }
        public List<Point> Points { get => points; set => points = value; }
        public NcPlot Plot { get => plot; set => plot = value; }
        public string Name { get => name; set => name = value; }
        public Color Color { get => color; set => color = value; }
        public bool Enable { get => enable; set => enable = value; }
        public double Tickness { get => tickness; set => tickness = value; }

        public SamplePlotSeries(string name,Color lineColor,double tickness)
        {
            this.name = name;
            this.color = color;
            this.tickness = tickness;
        }

        #region data operate
        public double MaxValue()
        {
            double max = this.points[0].Y;
            for (int i = 0; i < this.points.Count; i++)
            {
                max = Math.Max(max, this.points[i].Y);
            }
            return max;
        }

        public double MinValue()
        {
            double min = this.points[0].Y;
            for (int i = 0; i < this.points.Count; i++)
            {
                min = Math.Min(min, this.points[i].Y);
            }
            return min;
        }


        public bool ImportData(double[] xs,double[] ys)
        {
            if(xs==null || ys==null) { return false; }
            if (xs.Length != ys.Length) { return false; }

            return true;
        }

        public void ImportData(Point[] points)
        {
            this.Points.Clear();
            this.Points.AddRange(points);
        }

        public void AppendData(Point[] points)
        {
            this.Points.AddRange(points);
        }

        public void Append(Point point)
        {
            this.Points.Add(point);
        }

        public bool HasData()
        {
            return Points.Count > 0;
        }
        #endregion

        public void RefreshCoordinate(NcPlot plot)
        {
            if(!this.HasData()) {
                this.streamGeometry = null;
                return; }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.coordinatePoints = new List<Point>();

            for (int i = 0; i < this.points.Count; i++)
            {
                var x = plot.LeftSpace + plot.CanvasWidth * (this.points[i].X - plot.MinX) / (plot.MaxX - plot.MinX);
                var y = plot.ActualHeight - plot.BottomSpace - (plot.CanvasHeight * (this.points[i].Y - plot.MinY) / (plot.MaxY - plot.MinY));
                this.coordinatePoints.Add(new Point(x, y));
            }
            this.CreateGeomotry();
            sw.Stop();
            System.Console.WriteLine("RefreshCoordinate time:" + sw.Elapsed.TotalMilliseconds.ToString("0.0000") + "ms");


        }

        private void CreateGeomotry()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            streamGeometry = new StreamGeometry();
            streamGeometry.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext ctx = streamGeometry.Open())
            {
                ctx.BeginFigure(coordinatePoints[0], false, false);
                for (int i = 1; i < coordinatePoints.Count; i++)
                {
                    ctx.LineTo(coordinatePoints[i], true, false);
                }
            }


            streamGeometry.Freeze();

            sw.Stop();
            System.Console.WriteLine("CreateGeomotry time:" + sw.Elapsed.TotalMilliseconds.ToString("0.0000") + "ms");

        }

    }



    #region old for time plot 

    public enum PlotType
    {
        Plane = 0,
        TimePlot = 1,
    }

    public enum TimeSpanLevel
    {
        Minute = 1,
        Hour = 2,
        day = 3,
        Month = 4,
        Year = 5,
    }
    /// <summary>
    /// 图线显示模式
    /// </summary>
    public enum DisplayMode
    {
        /// <summary>
        /// 常规
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 归一化
        /// </summary>
        Normalization = 1,
        /// <summary>
        /// 对数处理ln
        /// </summary>
        NormalizationAndLogarithm = 2,
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

        public abstract bool HasDate();
        public abstract double MaxValue();
        public abstract double MinValue();

    }

    public class PlotSeries : Series
    {
        private double[] x;
        private double[] y;

        public PlotSeries(string name, Color color, double tickness)
        {
            this.Tickness = tickness;
            this.Color = color;
            this.Name = name;
        }

        public override bool HasDate()
        {
            return this.points != null && this.points.Length > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double MaxValue()
        {
            double max = this.points[0].Y;
            for (int i = 0; i < this.points.Length; i++)
            {
                max = Math.Max(max, this.points[i].Y);
            }
            return max;
        }

        public override double MinValue()
        {
            double min = this.points[0].Y;
            for (int i = 0; i < this.points.Length; i++)
            {
                min = Math.Min(min, this.points[i].Y);
            }
            return min;
        }

        
        public override void PointTransform(NcPlot plot)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
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
            sw.Stop();
            System.Console.WriteLine("trans time:" + sw.Elapsed.TotalMilliseconds.ToString("0.0000") + "ms");
        }

    }

    public class TimePlotSeries : Series
    {
        public DisplayMode DisplayMode { get; protected set; } = DisplayMode.Normal;

        private double[] tmpValues;
        public double[] rawValues;
        public DateTime[] times;

        public TimePlotSeries(string name, Color color, double tickness)
        {
            this.Tickness = tickness;
            this.Color = color;
            this.Name = name;
        }

        public override bool HasDate()
        {
            return this.tmpValues != null && this.times != null && this.tmpValues.Length != 0 && this.tmpValues.Length == this.times.Length;
        }

        public double MaxRawValue()
        {
            double max = rawValues[0];
            for (int i = 0; i < rawValues.Length; i++)
            {
                max = Math.Max(max, rawValues[i]);
            }
            return max;
        }

        public double MinRawValue()
        {
            double min = this.rawValues[0];
            for (int i = 0; i < this.rawValues.Length; i++)
            {
                min = Math.Min(min, this.rawValues[i]);
            }
            return min;
        }

        public override double MaxValue()
        {
            double max = tmpValues[0];
            for (int i = 0; i < tmpValues.Length; i++)
            {
                max = Math.Max(max, tmpValues[i]);
            }
            return max;
        }

        public override double MinValue()
        {
            double min = this.tmpValues[0];
            for (int i = 0; i < this.tmpValues.Length; i++)
            {
                min = Math.Min(min, this.tmpValues[i]);
            }
            return min;
        }

        public void PointTransform(NcTimePlot plot)
        {
            if (this.tmpValues == null || this.times == null || this.times.Length != this.tmpValues.Length)
            {
                this.transportedPoints = null;
                return;
            }
            if (this.transportedPoints == null || this.transportedPoints.Length != this.tmpValues.Length)
            {
                this.transportedPoints = new Point[this.tmpValues.Length];
            }


            for (int i = 0; i < this.tmpValues.Length; i++)
            {
                var x = plot.LeftSpace + plot.CanvasWidth * ((double)(this.times[i].Ticks - plot.MinTime.Ticks)) / (plot.MaxTime.Ticks - plot.MinTime.Ticks);
                var y = plot.ActualHeight - plot.BottomSpace - (plot.CanvasHeight * (this.tmpValues[i] - plot.MinY) / (plot.MaxY - plot.MinY));
                this.transportedPoints[i] = new Point(x, y);
            }
        }
        /// <summary>
        /// 设定显示模式
        /// </summary>
        /// <param name="displayMode"></param>
        public void SetDisplayMode(DisplayMode displayMode)
        {
            this.DisplayMode = displayMode;
            ProcessData();
        }
        /// <summary>
        /// 并生成中间数据
        /// </summary>
        public void ProcessData()
        {
            if (rawValues == null ||rawValues.Length==0) return;

            if (tmpValues == null || tmpValues.Length != rawValues.Length)
            {
                this.tmpValues = new double[rawValues.Length];
            }
            var max = this.MaxRawValue();
            var min = this.MinRawValue();
            var tmp = Math.Max(Math.Abs(max), Math.Abs(min));

            switch (this.DisplayMode)
            {
                case DisplayMode.Normal:
                    //this.tmpValues = rawValues;
                    for (int i = 0; i < rawValues.Length; i++)
                    {
                        tmpValues[i] = rawValues[i];
                    }
                    break;
                case DisplayMode.Normalization:

                    for (int i = 0; i < rawValues.Length; i++)
                    {
                        tmpValues[i] = rawValues[i] / tmp;
                    }

                    break;
                case DisplayMode.NormalizationAndLogarithm:
                    var l = Math.Log(tmp);
                    for (int i = 0; i < rawValues.Length; i++)
                    {
                        var factor = rawValues[i] > 0 ? 1.0 : -1.0;
                        tmpValues[i] = factor * Math.Log(factor * rawValues[i]+1) / l;
                    }

                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="time"></param>
        /// <param name="values"></param>
        public void SetData(DateTime[] time, double[] values,DisplayMode displayMode)
        {
            this.rawValues = (double[])values.Clone();
            this.times = (DateTime[])time.Clone();
            this.DisplayMode = displayMode;
            this.ProcessData();
        }

        public override void PointTransform(NcPlot plot)
        {
            throw new NotImplementedException();
        }
    }


    public enum AnchorDirection
    {
        AnchorX = 0,
        AnchorY = 1,
        AnchorZ = 2,
    }

    public enum RegionDirection
    {
        RegionX = 0,
        RegionY = 1,
        RegionZ = 2,
    }

    public enum Axis
    {
        X = 0,
        Y = 1,
        Z = 1,
    }

    public abstract class PlotItem
    {
        private string name = "";

        public string Name { get => name; set => name = value; }
    }

    public class Anchor : PlotItem
    {
        private AnchorDirection anchorType = AnchorDirection.AnchorX;
        public AnchorDirection AnchorType { get => anchorType; set => anchorType = value; }
    }

    public class TimeAnchor : Anchor
    {
        private Color color;

        private DateTime value;


        public DateTime Value { get => value; set => this.value = value; }
        public Color Color { get => color; set => color = value; }

        public TimeAnchor()
        {
            Color = Colors.Black;
            Value = DateTime.Now;
        }
        public TimeAnchor(DateTime time)
        {
            Color = Colors.Black;
            Value = time;
        }
    }
    /// <summary>
    /// 区间
    /// </summary>
    public class Region : PlotItem
    {
        private RegionDirection regionType = RegionDirection.RegionX;
    }

    public class TimeRegion : Region
    {
        private TimeAnchor anchorStart;
        private TimeAnchor anchorEnd;
        Color Color;
        public TimeRegion(DateTime start, DateTime end)
        {
            anchorEnd = new TimeAnchor(end);
            anchorStart = new TimeAnchor(start);
        }
    }

    #endregion




}
