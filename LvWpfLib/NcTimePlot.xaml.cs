using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ncer.UI
{
    /// <summary>
    /// NcTimePlot.xaml 的交互逻辑
    /// </summary>
    public partial class NcTimePlot : UserControl
    {
        public NcTimePlot()
        {
            InitializeComponent();
            timePlot.DataContext = this;
            UpdateDisplay();
        }

        private string plotTitle = "Plot";
        private string axisNameY = "Axis Y";
        private string axisNameX = "Axis X";
        private double topSpace = 40;
        private double leftSpace = 40;
        private double bottomSpace = 40;
        private double rightSpace = 40;

        private double maxY = 1000;
        private double minY = 0;

        private double scale = 1.0;
        private double defaultMaxY = 1000;
        private double defaultMinY = 0;
        private DateTime defaultMaxTime;
        private DateTime defaultMinTime;


        private double axisGraduateX = 60;

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

        private List<TimePlotSeries> series = new List<TimePlotSeries>();
        private List<PlotItem> items = new List<PlotItem>();

        #region 属性
        /// <summary>
        /// 图线显示模式
        /// </summary>
        private DisplayMode DisplayMode { get; set; } = DisplayMode.Normal;
        /// <summary>
        /// 图形标题
        /// </summary>
        public string PlotTitle
        {
            get => plotTitle; set
            {
                plotTitle = value;
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

        public bool AutoFitX { get => autoFitX; set => autoFitX = value; }
        public bool AutoFitY { get => autoFitY; set => autoFitY = value; }
        /// <summary>
        /// 图线系列
        /// </summary>
        public List<TimePlotSeries> Series { get => series; set => series = value; }
        public double[] GraduateX { get => graduateX; set => graduateX = value; }
        public double[] GraduateY { get => graduateY; set => graduateY = value; }
        public double CanvasWidth { get => canvasWidth; set => canvasWidth = value; }
        public double CanvasHeight { get => canvasHeight; set => canvasHeight = value; }
        /// <summary>
        /// 图表类型
        /// </summary>
        public PlotType PlotType { get => plotType; set => plotType = value; }
        /// <summary>
        /// 时间范围最大值
        /// </summary>
        public DateTime MaxTime { get => maxTime; set => maxTime = value; }
        /// <summary>
        /// 时间范围最小值
        /// </summary>
        public DateTime MinTime { get => minTime; set => minTime = value; }
        public DateTime[] GraduateTime { get => graduateTime; set => graduateTime = value; }
        public TimeSpan TimeSpan { get => timeSpan; set => timeSpan = value; }
        public TimeSpanLevel TimeSpanLevel { get => timeSpanLevel; set => timeSpanLevel = value; }
        /// <summary>
        /// 附加项目
        /// </summary>
        public List<PlotItem> Items { get => items; set => items = value; }
        public double MaxY { get => maxY; set => maxY = value; }
        public double MinY { get => minY; set => minY = value; }
        /// <summary>
        /// X轴坐标间隔距离
        /// </summary>
        public double AxisGraduateX { get => axisGraduateX; set => axisGraduateX = value; }
        public DateTime DefaultMaxTime { get => defaultMaxTime; set => defaultMaxTime = value; }
        public DateTime DefaultMinTime { get => defaultMinTime; set => defaultMinTime = value; }
        #endregion
        #region 绘图

        protected override void OnRender(DrawingContext drawingContext)
        {

            base.OnRender(drawingContext);

            //DrawingBrush tileBack = (DrawingBrush)this.Resources["TileBack"];
            SolidColorBrush colorBrush = (SolidColorBrush)this.Resources["ColorBack"];
            drawingContext.DrawRectangle(colorBrush, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

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
            //时间轴
            for (int i = 0; i < graduateTime.Length; i++)
            {
                x = leftSpace + CanvasWidth * ((double)(graduateTime[i].Ticks - minTime.Ticks) / (maxTime.Ticks - minTime.Ticks));
                drawingContext.DrawLine(AxisPen, new Point(x, topSpace + CanvasHeight), new Point(x, topSpace + CanvasHeight - 4));
                FormattedText graduateText = new FormattedText(Utils.GetTimeLabel(graduateTime[i], this.timeSpanLevel), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("宋体"), this.FontSize, this.Foreground);
                drawingContext.DrawText(graduateText, new Point(x - graduateText.Width / 2, topSpace + CanvasHeight + 10));
            }
            //y轴
            for (int i = 0; i < graduateY.Length; i++)
            {
                y = topSpace + CanvasHeight - (CanvasHeight * (graduateY[i] - MinY) / (MaxY - MinY));
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
                if (item.HasDate() && item.Enable)
                {
                    Pen seriesPen = new Pen(new SolidColorBrush(item.Color), item.Tickness);
                    for (int i = 0; i < item.TransportedPoints.Length - 1; i++)
                    {
                        drawingContext.DrawLine(seriesPen, item.TransportedPoints[i], item.TransportedPoints[i + 1]);
                    }
                }
            }
        }

        #endregion
        public bool HasData()
        {
            foreach (var item in series)
            {
                if (item.HasDate())
                {
                    return true;
                }
            }
            return false;
        }
        
        #region 交互操作
        /// <summary>
        /// Zoom
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="v"></param>
        public void Zoom(double x,double y,int v)
        {
            CanvasWidth = this.ActualWidth - this.leftSpace - this.rightSpace;
            CanvasHeight = this.ActualHeight - this.topSpace - this.bottomSpace;
            var factor = v > 0 ? 1.25 : 0.8;
            this.scale *= factor;
            var rx = x - leftSpace;
            var ry = this.ActualHeight - bottomSpace - y;

            var vy = minY + (ry / CanvasHeight) * (maxY - minY);
            minY = vy - (vy - minY) * factor;
            maxY = vy + (maxY - vy) * factor;

            var vx =minTime.Ticks + (long)((rx / canvasWidth) * (maxTime.Ticks - minTime.Ticks));
            minTime = new DateTime(Math.Max(0,vx - (long)((vx - minTime.Ticks) * factor)));
            maxTime = new DateTime(Math.Max(0,vx + (long)((maxTime.Ticks - vx) * factor)));


        }
        /// <summary>
        /// 移动图线
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        public void Move(double sx,double sy,double ex,double ey)
        {
            CanvasWidth = this.ActualWidth - this.leftSpace - this.rightSpace;
            CanvasHeight = this.ActualHeight - this.topSpace - this.bottomSpace;

            var dy = (maxY - minY) * (ey - sy) / canvasHeight;
            var dx = (long)((maxTime.Ticks - minTime.Ticks) * (ex - sx) / canvasWidth);
            this.minY += dy;
            this.maxY+=dy;
            this.minTime = new DateTime(Math.Max(0,(minTime.Ticks - dx)));
            this.maxTime = new DateTime(Math.Max(0, (maxTime.Ticks - dx)));
        }

        #endregion
        #region 交互事件处理
        Point interactionStartPoint;
        Point interactionPoint;
        InteractionState interactionState=InteractionState.Idle;
        public enum InteractionState
        {
            Idle=0,
            Operating=1,
        }



        #endregion

        #region 更新相关

        /// <summary>
        /// 更改数据内容后调用此方法更新
        /// </summary>
        public void UpdateData()
        {
            this.scale = 1;
            CanvasWidth = this.ActualWidth - this.leftSpace - this.rightSpace;
            CanvasHeight = this.ActualHeight - this.topSpace - this.bottomSpace;

            //y轴自适应
            if (AutoFitY && this.Series.Count > 0 && this.HasData())
            {
                foreach (var item in series)
                {
                    if (item.HasDate())
                    {
                        this.MinY = item.MinValue();
                        this.MaxY = this.MinY;
                    }
                }
                foreach (var item in Series)
                {
                    if (item.HasDate())
                    {
                        this.MinY = Math.Min(MinY, item.MinValue());
                        this.MaxY = Math.Max(MaxY, item.MaxValue());

                    }
                }
                if (this.MinY == this.MaxY)
                {
                    this.MaxY += 1;
                }
                this.defaultMaxY = maxY;
                this.defaultMinY = minY;
            }



            //x轴自适应
            foreach (var item in series)
            {
                if (item is TimePlotSeries)
                {
                    var s = item as TimePlotSeries;
                    if (s.HasDate())
                    {
                        this.minTime = s.times[0];
                        this.maxTime = minTime;
                    }
                }
            }
            foreach (var item in Series)
            {
                if (item is TimePlotSeries && item.HasDate())
                {
                    var series = (item as TimePlotSeries);
                    for (int i = 0; i < series.times.Length; i++)
                    {
                        this.minTime = minTime < series.times[i] ? minTime : series.times[i];
                        this.maxTime = maxTime > series.times[i] ? maxTime : series.times[i];
                    }

                }
            }
            if (this.minTime == this.maxTime)
            {
                maxTime = maxTime + TimeSpan.FromSeconds(1000);
            }

            this.DefaultMinTime = MinTime;
            this.DefaultMaxTime = MaxTime;

            this.UpdateDisplay();
        }
        /// <summary>
        /// 更新数据展现
        /// </summary>
        public void UpdateDisplay()
        {
            CanvasWidth = this.ActualWidth - this.leftSpace - this.rightSpace;
            CanvasHeight = this.ActualHeight - this.topSpace - this.bottomSpace;

            foreach (var item in this.Series)
            {
                item.PointTransform(this);
            }
            //计算刻度 数值
            double ty = 50 * (MaxY - MinY) / CanvasHeight;
            double jgy = ((int)((ty) / Math.Pow(10, ((int)Math.Log10(ty) - 1))) + 1) * Math.Pow(10, ((int)Math.Log10(ty) - 1));
            int sy = (int)(MinY / jgy) + 1;
            int ey = (int)(MaxY / jgy);

            graduateY = new double[ey - sy + 1];
            for (int i = sy; i < ey + 1; i++)
            {
                graduateY[i - sy] = jgy * i;
            }
            //时间刻度
            long x = (this.maxTime - this.minTime).Ticks * (long)(this.AxisGraduateX) / (long)(this.canvasWidth);
            long interval;
            if (x > TimeSpan.FromDays(365).Ticks)
            {
                this.timeSpanLevel = TimeSpanLevel.Year;
                int y = (int)Math.Log10(x / TimeSpan.FromDays(365).Ticks);
                interval = (x / (TimeSpan.FromDays(365).Ticks * ((long)Math.Pow(10, y)))) * TimeSpan.FromDays(365).Ticks * ((long)Math.Pow(10, y));
                interval = Math.Max(interval, TimeSpan.FromDays(365).Ticks);
            }
            else if (x > TimeSpan.FromDays(30).Ticks)
            {
                this.timeSpanLevel = TimeSpanLevel.Month;
                interval = TimeSpan.FromDays(30).Ticks;
            }
            else if (x > TimeSpan.FromDays(1).Ticks)
            {
                this.timeSpanLevel = TimeSpanLevel.day;
                interval = TimeSpan.FromDays(1).Ticks;
            }
            else if (x > TimeSpan.FromHours(1).Ticks)
            {
                this.timeSpanLevel = TimeSpanLevel.Hour;
                interval = TimeSpan.FromMinutes(60).Ticks;
            }
            else //if (x > TimeSpan.FromMinutes(1).Ticks)
            {
                this.timeSpanLevel = TimeSpanLevel.Minute;
                interval = TimeSpan.FromMinutes(1).Ticks;
            }

            var start = Utils.DateTimeRound(minTime, this.timeSpanLevel).Ticks;
            var lastPositionX = -this.AxisGraduateX;
            List<DateTime> graduateTimeX = new List<DateTime>();
            while (start < maxTime.Ticks)
            {
                var positionX = canvasWidth * (start - minTime.Ticks) / (maxTime.Ticks - minTime.Ticks);
                if (start > minTime.Ticks && (positionX - lastPositionX) > this.AxisGraduateX)
                {
                    graduateTimeX.Add(new DateTime(start));
                    lastPositionX = positionX;
                }
                start += interval;
            }

            this.graduateTime = graduateTimeX.ToArray();
            this.InvalidateVisual();
        }

        #endregion

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateDisplay();
        }

        private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }

        private void UserControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var p = e.GetPosition(this);
            switch (interactionState)
            {
                case InteractionState.Idle:
                    break;
                case InteractionState.Operating:
                    this.Move(interactionPoint.X, interactionPoint.Y, p.X, p.Y);
                    this.interactionPoint=p;
                    this.UpdateDisplay();
                    break;
                default:
                    break;
            }
            
        }

        private void UserControl_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            this.interactionStartPoint = e.GetPosition(this);
            this.Zoom(interactionStartPoint.X, interactionStartPoint.Y, e.Delta);
            this.UpdateDisplay();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateDisplay();
        }

        private void timePlot_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            interactionStartPoint = e.GetPosition(this);
            interactionPoint = interactionStartPoint;
            interactionState = InteractionState.Operating;
        }



        private void timePlot_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.interactionState = InteractionState.Idle;
        }
    }
}
