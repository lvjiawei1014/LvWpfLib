using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Point = System.Windows.Point;
using System.Globalization;
using LvWpfLib.LvGeometry;

namespace LvWpfLib.LvImageView
{
    public abstract class ImageViewElement:IComparable<ImageViewElement>
    {
        #region 事件
        public delegate void ElementChange(object sender);
        public event ElementChange OnElementChangeEvent;

        public delegate void ElementChangeDone(object sender);
        public event ElementChangeDone OnElementChangeDoneEvent;

        public delegate void ElementDelete(object sender);
        public event ElementDelete OnElementDelete;
        #endregion


        private object item;
        private string name="";
        private string tag="";
        private Coordinate coordinate=new Coordinate();
        private Coordinate globalCoordinate;

        private ImageViewElement parent;
        private List<ImageViewElement> childrens = new List<ImageViewElement>();

        private bool visible = true;
        private bool selected = false;
        private bool isComplete = true;
        private bool showTag = true;

        private Cursor elementCursor = Cursors.Arrow;
        private Color color = Colors.Lime;



        //private Font font;

        public Coordinate Coordinate { get => coordinate; set => coordinate = value; }
        public string Name { get => name; set => name = value; }
        public string Tag { get => tag; set => tag = value; }
        public ImageViewElement Parent { get => parent; set => parent = value; }
        public List<ImageViewElement> Childrens { get => childrens; set => childrens = value; }
        public bool Visible { get => visible; set => visible = value; }

        #region 属性
        public virtual double X { get { return coordinate.X; } set { coordinate.X = value; } }
        public virtual double Y { get { return coordinate.Y; } set { coordinate.Y = value; } }
        public virtual double Z { get { return coordinate.Z; } set { coordinate.Z = value; } }
        public virtual double Width { get { return coordinate.Width; } set { coordinate.Width = value; } }
        public virtual double Height { get { return coordinate.Height; } set { coordinate.Height = value; } }
        public double Scale { get { return coordinate.Scale; } set { coordinate.Scale = value; } }

        public bool Selected { get => selected; set => selected = value; }
        public Coordinate GlobalCoordinate { get => globalCoordinate; set => globalCoordinate = value; }
        public Cursor ElementCursor { get => elementCursor; set => elementCursor = value; }
        public bool IsComplete { get => isComplete; set => isComplete = value; }
        public Color Color { get => color; set => color = value; }
        public bool ShowTag { get => showTag; set => showTag = value; }
        public object Item { get => item; set => item = value; }

        #endregion
        public ImageViewElement()
        {
            //this.OnElementChangeEvent+=this.
        }

        public virtual void Move(double x,double y)
        {
            this.coordinate.X = x;
            this.coordinate.Y = y;
        }

        public abstract void Drawing(DrawingContext drawingContext);
        public virtual void OnElementChange(ImageViewElement element)
        {
            this.OnElementChangeEvent?.Invoke(this);
        }

        public abstract void BeSelect(bool b);

        public abstract bool IsIn(double x, double y);

        public int CompareTo(ImageViewElement other)
        {
            return this.Z == other.Z ? 0 : (this.Z > other.Z ? 1 : -1);
        }

        public void Delete()
        {
            this.OnElementDelete?.Invoke(this);
        }

        public void ChangeDone()
        {
            this.OnElementChangeDoneEvent?.Invoke(this);
            Parent?.ChangeDone();
        }
    }


    public class ImageElement : ImageViewElement
    {
        private ImageSource image;

        public ImageSource Image
        {
            get { return image; }

            set { this.image = value; this.Width = (Image == null) ? 0 : image.Width; this.Height = (Image == null) ? 0 : image.Height; }
        }
        public ImageElement() : base()
        {

        }
        public void FitToWindow(double w, double h)
        {
            this.Scale = Math.Min(h / image.Height, w / image.Width);
            this.X = Math.Max(0, (w - image.Width * this.Scale) / 2);
            this.Y = Math.Max(0, (h - image.Height * this.Scale) / 2);
        }

        public void ScaleImage(System.Windows.Point anchor, double scale)
        {
            Point imageAnchor = Coordinate.CoordinateTransport(anchor, Coordinate.BaseCoornidate, this.Coordinate);
            this.X = anchor.X - scale * imageAnchor.X;
            this.Y = anchor.Y - scale * imageAnchor.Y;
            this.Scale = scale;
        }

        public override void Drawing(DrawingContext drawingContext)
        {
            //base.Drawing(drawingContext);
            if (this.image != null)
            {

                drawingContext.DrawImage(this.image, new Rect(this.X, this.Y, image.Width * this.Scale, image.Height * this.Scale));
            }
            
        }

        public override void OnElementChange(ImageViewElement element)
        {
            throw new NotImplementedException();
        }

        public override void BeSelect(bool b)
        {
            throw new NotImplementedException();
        }

        public override bool IsIn(double x, double y)
        {
            throw new NotImplementedException();
        }
    }

    public class TractionPoint : ImageViewElement
    {

        private Point previousLocation = new Point(0, 0);
        public double Size { get; set; }
        /// <summary>
        /// 点位拖拽事件
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public delegate void TractionEvent(TractionPoint element, double x, double y);
        public event TractionEvent OnTractionEventHandler;

        public TractionPoint(ImageViewElement parent)
        {
            this.Parent = parent;
            this.Size = 6;
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="parent"></param>
        public TractionPoint(double x, double y, ImageViewElement parent) : base()
        {
            this.Size = 6;
            this.X = x;
            this.Y = y;
            this.Parent = parent;
            this.Z = parent.Z + 0.01f;
        }

        public Point GetPoint()
        {
            return new Point(X, Y);
        }

        public override void OnElementChange(ImageViewElement element)
        {
            throw new NotImplementedException();
        }

        public override void BeSelect(bool b)
        {

        }
        public override void Move(double x, double y)
        {
            Traction(x, y);
        }



        public void MoveBack()
        {
            this.X = previousLocation.X;
            this.Y = previousLocation.Y;
            if (OnTractionEventHandler != null)
            {
                OnTractionEventHandler(this, this.X, this.Y);
            }
        }
        /// <summary>
        /// 拖拽移动点位
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Traction(double x, double y)
        {
            previousLocation.X = this.X;
            previousLocation.Y = this.Y;
            this.X = x;
            this.Y = y;
            if (OnTractionEventHandler != null)
            {
                OnTractionEventHandler(this, x, y);
            }
        }

        public void Traction(Point location)
        {
            Traction(location.X, location.Y);
        }

        public override bool IsIn(double x, double y)
        {
            if (GlobalCoordinate == null) return false;
            if (this is TractionPoint)
            {
                bool b = Math.Abs(this.X - x) * GlobalCoordinate.Scale < this.Size / 2 && Math.Abs(this.Y - y) * GlobalCoordinate.Scale < this.Size / 2;
                return b && this.Visible;
            }
            return false;
        }

        public override void Drawing(DrawingContext drawingContext)
        {
            Point loca = Coordinate.CoordinateTransport(new Point(this.X, this.Y), this.Parent.GlobalCoordinate, Coordinate.BaseCoornidate);
            drawingContext.DrawRectangle(System.Windows.Media.Brushes.Black, new Pen(Brushes.White, 1), new Rect(loca.X - this.Size / 2, loca.Y - this.Size / 2, this.Size, this.Size));

        }
    }

    public class KeyPoint
    {


        public delegate void KeyPointChangeEvent(KeyPoint kp, double x, double y);
        public event KeyPointChangeEvent KeyPointChangeEventHandler;
        private TractionPoint tractionPoint;
        public ImageViewElement Parent { get; set; }
        public Point Point { get { return TractionPoint.GetPoint(); } }
        public double X { get { return TractionPoint.X; } set { TractionPoint.X = value; } }
        public double Y { get { return TractionPoint.Y; } set { TractionPoint.Y = value; } }

        public TractionPoint TractionPoint { get => tractionPoint; set => tractionPoint = value; }

        public KeyPoint(double x, double y, ImageViewElement parent)
        {
            this.Parent = parent;
            this.TractionPoint = new TractionPoint(x, y, parent);
            TractionPoint.GlobalCoordinate = parent.GlobalCoordinate;
            TractionPoint.Parent = parent;
            //tractionPoint.ElementCursor = Cursors.SizeAll;
            TractionPoint.Z = TractionPoint.Parent.Z - 0.01f;
            TractionPoint.Visible = true;

            this.TractionPoint.OnTractionEventHandler += OnTraction;

        }

        public void OnTraction(TractionPoint elemant, double x, double y)
        {
            if (KeyPointChangeEventHandler != null)
            {
                KeyPointChangeEventHandler(this, X, Y);
            }
        }
    }

    [Serializable()]
    public abstract class KeyPointElement : ImageViewElement
    {
        private int keyPointAmount;

        private int keyPointCount;

        public List<KeyPoint> keyPointList = new List<KeyPoint>();

        public int KeyPointAmount { get => keyPointAmount; set => keyPointAmount = value; }
        public int KeyPointCount { get => keyPointCount; set => keyPointCount = value; }

        public abstract bool AddKeyPoint(Point p);
        public abstract void AdjustNextKeyPoint(Point point);

        public List<Point> GetPoints()
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < keyPointList.Count; i++)
            {
                points.Add(new Point(keyPointList[i].X, keyPointList[i].Y));
            }
            return points;
        }
        public KeyPointElement()
            : base()
        { }
    }
    [Serializable()]
    public class RectElement : KeyPointElement
    {
        //public static Font DefaultFont = new Font("微软雅黑", 12f, FontStyle.Bold);
        public static Cursor ElementDefaultCursor = Cursors.Hand;
        public const float RECTANGLE_DEFAULT_Z = 2f;
        public KeyPoint keyPoint1, keyPoint2;
        public TractionPoint leftTopPoint, leftBottomPoint, rightTopPoint, rightBottomPoint;
        public new Coordinate GlobalCoordinate
        {
            get
            {
                return base.GlobalCoordinate;
            }
            set
            {
                base.GlobalCoordinate = value;
                leftTopPoint.GlobalCoordinate = value;
                leftBottomPoint.GlobalCoordinate = value;
                rightBottomPoint.GlobalCoordinate = value;
                rightTopPoint.GlobalCoordinate = value;
            }
        }
        public override double X
        {
            get
            {
                return leftTopPoint.X;
            }
            set
            {

                leftTopPoint.X = value;
                leftBottomPoint.X = value;
                //this.OnElementChange(this);
            }
        }
        public override double Y
        {
            get
            {
                return leftTopPoint.Y;
            }
            set
            {
                leftTopPoint.Y = value;
                rightTopPoint.Y = value;
                //OnElementChange(this);
            }
        }

        public override double Width
        {
            get
            {
                return rightBottomPoint.X - leftTopPoint.X;
            }
            set
            {
                rightTopPoint.X = value + leftTopPoint.X;
                rightBottomPoint.X = rightTopPoint.X;
                //OnElementChange(this);
            }
        }

        public override double Height
        {
            get
            {
                return rightBottomPoint.Y - leftTopPoint.Y;
            }
            set
            {
                leftBottomPoint.Y = value + leftTopPoint.Y;
                rightBottomPoint.Y = leftBottomPoint.Y;
                //OnElementChange(this);
            }
        }

        public RectElement() : this(0, 0, 1, 1) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public RectElement(double x, double y, double width, double height) : base()
        {
            KeyPointAmount = 2;
            this.ElementCursor = RectElement.ElementDefaultCursor;
            this.leftTopPoint = new TractionPoint(this);
            this.leftBottomPoint = new TractionPoint(this);
            this.rightTopPoint = new TractionPoint(this);
            this.rightBottomPoint = new TractionPoint(this);
            keyPoint1 = new KeyPoint(0, 0, this);
            keyPoint2 = new KeyPoint(0, 0, this);
            keyPoint1.TractionPoint = leftTopPoint;
            keyPoint2.TractionPoint = rightBottomPoint;
            keyPointList.Add(keyPoint1);
            keyPointList.Add(keyPoint2);

            this.leftTopPoint.ElementCursor = Cursors.SizeNWSE;
            this.leftBottomPoint.ElementCursor = Cursors.SizeNESW;
            this.rightBottomPoint.ElementCursor = Cursors.SizeNWSE;
            this.rightTopPoint.ElementCursor = Cursors.SizeNESW;

            this.X = x;
            Y = y;
            Z = RECTANGLE_DEFAULT_Z;
            this.Width = width;
            this.Height = height;

            this.leftTopPoint.OnTractionEventHandler += OnLeftTopPointTraction;
            this.leftBottomPoint.OnTractionEventHandler += OnLeftBottomPointTraction;
            this.rightTopPoint.OnTractionEventHandler += OnRightTopPointTraction;
            this.rightBottomPoint.OnTractionEventHandler += OnRightBottomPointTraction;
            Visible = true;
            Parent = null;
        }

        public void FlipVertical()
        {

            this.leftTopPoint.OnTractionEventHandler -= OnLeftTopPointTraction;
            this.leftBottomPoint.OnTractionEventHandler -= OnLeftBottomPointTraction;
            this.rightTopPoint.OnTractionEventHandler -= OnRightTopPointTraction;
            this.rightBottomPoint.OnTractionEventHandler -= OnRightBottomPointTraction;
            System.Console.WriteLine("vFlip");
            TractionPoint tmp = leftTopPoint;
            leftTopPoint = leftBottomPoint;
            leftBottomPoint = tmp;
            tmp = rightTopPoint;
            rightTopPoint = rightBottomPoint;
            rightBottomPoint = tmp;
            this.leftTopPoint.OnTractionEventHandler += OnLeftTopPointTraction;
            this.leftBottomPoint.OnTractionEventHandler += OnLeftBottomPointTraction;
            this.rightTopPoint.OnTractionEventHandler += OnRightTopPointTraction;
            this.rightBottomPoint.OnTractionEventHandler += OnRightBottomPointTraction;
            this.leftTopPoint.ElementCursor = Cursors.SizeNWSE;
            this.leftBottomPoint.ElementCursor = Cursors.SizeNESW;
            this.rightBottomPoint.ElementCursor = Cursors.SizeNWSE;
            this.rightTopPoint.ElementCursor = Cursors.SizeNESW;
            System.Console.WriteLine("lt==lb:" + object.ReferenceEquals(leftTopPoint, leftBottomPoint));
        }
        public void FlipHorizontal()
        {
            this.leftTopPoint.OnTractionEventHandler -= OnLeftTopPointTraction;
            this.leftBottomPoint.OnTractionEventHandler -= OnLeftBottomPointTraction;
            this.rightTopPoint.OnTractionEventHandler -= OnRightTopPointTraction;
            this.rightBottomPoint.OnTractionEventHandler -= OnRightBottomPointTraction;
            System.Console.WriteLine("hFlip");
            TractionPoint tmp = leftTopPoint;
            leftTopPoint = rightTopPoint;
            rightTopPoint = tmp;
            tmp = leftBottomPoint;
            leftBottomPoint = rightBottomPoint;
            rightBottomPoint = tmp;

            this.leftTopPoint.OnTractionEventHandler += OnLeftTopPointTraction;
            this.leftBottomPoint.OnTractionEventHandler += OnLeftBottomPointTraction;
            this.rightTopPoint.OnTractionEventHandler += OnRightTopPointTraction;
            this.rightBottomPoint.OnTractionEventHandler += OnRightBottomPointTraction;
            this.leftTopPoint.ElementCursor = Cursors.SizeNWSE;
            this.leftBottomPoint.ElementCursor = Cursors.SizeNESW;
            this.rightBottomPoint.ElementCursor = Cursors.SizeNWSE;
            this.rightTopPoint.ElementCursor = Cursors.SizeNESW;
        }
        public void OnLeftTopPointTraction(TractionPoint element, double x, double y)
        {
            leftBottomPoint.X = x;
            rightTopPoint.Y = y;
            if (this.Height < 0)
            {
                FlipVertical();
            }
            if (this.Width < 0)
            {
                FlipHorizontal();
            }
            OnElementChange(this);

        }
        public void OnLeftBottomPointTraction(TractionPoint element, double x, double y)
        {
            leftTopPoint.X = x;
            rightBottomPoint.Y = y;
            if (this.Height < 0)
            {
                FlipVertical();
            }
            if (this.Width < 0)
            {
                FlipHorizontal();
            }
            OnElementChange(this);
        }
        public void OnRightTopPointTraction(TractionPoint element, double x, double y)
        {
            leftTopPoint.Y = y;
            rightBottomPoint.X = x;
            if (this.Width < 0)
            {
                FlipHorizontal();
            }
            if (this.Height < 0)
            {
                FlipVertical();
            }
            OnElementChange(this);
        }
        public void OnRightBottomPointTraction(TractionPoint element, double x, double y)
        {
            leftBottomPoint.Y = y;
            rightTopPoint.X = x;
            if (this.Width < 0)
            {
                FlipHorizontal();
            }
            if (this.Height < 0)
            {
                FlipVertical();
            }
            OnElementChange(this);
        }
        public override void BeSelect(bool b)
        {
            this.rightBottomPoint.Visible = b;
            this.rightTopPoint.Visible = b;
            this.leftBottomPoint.Visible = b;
            this.leftTopPoint.Visible = b;
        }
        public override void OnElementChange(ImageViewElement element)
        {
            base.OnElementChange(element);
            System.Console.WriteLine("leftTop:" + leftTopPoint.X + " " + leftTopPoint.Y);
            System.Console.WriteLine("leftBottom:" + leftBottomPoint.X + " " + leftBottomPoint.Y);
            System.Console.WriteLine("rightTop:" + rightTopPoint.X + " " + rightTopPoint.Y);
            System.Console.WriteLine("rightBottom:" + rightBottomPoint.X + " " + rightBottomPoint.Y);
        }

        public override bool IsIn(double x, double y)
        {
            //return (x < (X + Width) && x > X && Math.Abs(y - Y) < 6 / ParentCoordinate.Scale)
            //    || (x < (X + Width) && x > X && Math.Abs(y - (Y + Height)) < 6 / ParentCoordinate.Scale)
            //    || (y > Y && y < (Y + Height) && Math.Abs(x - X) < 6 / ParentCoordinate.Scale)
            //    || (y > Y && y < (Y + Height) && Math.Abs(x - (X + Width)) < 6 / ParentCoordinate.Scale);
            return x <= (X + Width) && x >= X && y <= Y + Height && y > Y; 
        }
        public override void Move(double x, double y)
        {
            double h = this.Height;
            double w = this.Width;
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }
        public override bool AddKeyPoint(Point point)
        {
            if (!this.IsComplete)
            {
                switch (KeyPointCount)
                {
                    case 0:
                        this.Move(point.X, point.Y);
                        KeyPointCount++;
                        break;
                    case 1:
                        KeyPointCount++;
                        break;
                    default:
                        break;
                }
                if (KeyPointCount == KeyPointAmount)
                {
                    IsComplete = true;
                }
            }
            return IsComplete;
        }
        public override void AdjustNextKeyPoint(Point point)
        {
            switch (KeyPointCount)
            {
                case 1:
                    //this.Width = point.X - this.X;
                    //this.Height = point.Y - this.Y;
                    this.keyPoint2.TractionPoint.Traction(point);
                    break;
            }
        }

        public override void Drawing(DrawingContext drawingContext)
        {
            Point loca = Coordinate.CoordinateTransport(new Point(X,Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
            //g.DrawRectangle(p, loca.X, loca.Y, this.Width * ParentCoordinate.Scale, this.Height * ParentCoordinate.Scale);
            //g.DrawString(this.info, RectElement.DefaultFont, Brushes.Blue, loca.X + 10, loca.Y + 10);
            Brush br = new SolidColorBrush(this.Color);

            drawingContext.DrawRectangle(null, new Pen(br,1),new Rect(loca.X,loca.Y,Width*Parent.Coordinate.Scale,Height*Parent.Coordinate.Scale));
            if (ShowTag && Tag != null && Tag != "")
            {
                drawingContext.DrawText(new FormattedText(this.Tag, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Normal,
                FontStretches.Normal), 16, br), new Point(loca.X + 10, loca.Y + 10));
            }
            
            if (this.Selected)
            {
                this.leftBottomPoint.Drawing(drawingContext);
                this.leftTopPoint.Drawing(drawingContext);
                this.rightBottomPoint.Drawing(drawingContext);
                this.rightTopPoint.Drawing(drawingContext);
            }
        }
    }

    /// <summary>
    /// 多边形
    /// </summary>
    public class PolygonElement : KeyPointElement
    {

        private KeyPoint tmpPoint;//绘图时的临时关键点


        public override double X
        {
            get
            {
                if (keyPointList.Count > 0)
                {
                    return keyPointList[0].X;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (keyPointList.Count > 0)
                {
                    keyPointList[0].X = value;
                }
            }
        }

        public override double Y
        {
            get
            {
                if (keyPointList.Count > 0)
                {
                    return keyPointList[0].Y;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (keyPointList.Count > 0)
                {
                    keyPointList[0].Y = value;
                }
            }
        }

        public PolygonElement()
        {
            tmpPoint = new KeyPoint(0, 0, this);
            this.ElementCursor = Cursors.Hand;
        }
        public bool Check()
        {
            if (this.keyPointList.Count < 3)
            {
                return false;
            }
            int i, j,m,n;
            int x = keyPointList.Count;
            for (i = keyPointList.Count-1, j = 0; j < keyPointList.Count; i=j++)
            {
                for (m = keyPointList.Count - 1, n = 0; n < keyPointList.Count ; m = n++)
                {
                    if (m == i|| i==n||j==m) continue;
                    if (GeometryCal.SegmentIntersect(keyPointList[i].Point, keyPointList[j].Point, keyPointList[m].Point, keyPointList[n].Point))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 移动到目标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void Move(double x, double y)
        {
            var dx = x - this.X;
            var dy = y - this.Y;

            for (int i = 0; i < this.keyPointList.Count; i++)
            {
                keyPointList[i].X += dx;
                keyPointList[i].Y += dy;
            }
            OnElementChange(this);
        }

        /// <summary>
        /// 添加关键点
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool AddKeyPoint(Point p)
        {
            if (this.keyPointList.Count == 0)
            {
                this.tmpPoint.X = p.X;
                this.tmpPoint.Y = p.Y;

            }
            KeyPoint kp = new KeyPoint(p.X, p.Y, this);
            kp.TractionPoint.ElementCursor = Cursors.SizeAll;
            kp.KeyPointChangeEventHandler += Kp_KeyPointChangeEventHandler;
            this.keyPointList.Add(kp);
            return false;

        }

        private void Kp_KeyPointChangeEventHandler(KeyPoint kp, double x, double y)
        {
            if (!Check())
            {
                kp.TractionPoint.MoveBack();
            }
            OnElementChange(this);
        }

        public override void AdjustNextKeyPoint(Point point)
        {
            tmpPoint.X = point.X;
            tmpPoint.Y = point.Y;
        }

        public override void BeSelect(bool b)
        {
            for (int i = 0; i < keyPointList.Count; i++)
            {
                keyPointList[i].TractionPoint.Visible = b;
            }
        }

        public override void Drawing(DrawingContext drawingContext)
        {
            Point loca = Coordinate.CoordinateTransport(new Point(X, Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
            Brush br = new SolidColorBrush(this.Color);
            Pen pen = new Pen(br, 1);

            for (int i = 0; i < keyPointList.Count - 1; i++)
            {
                Point p1 = Coordinate.CoordinateTransport(new Point(keyPointList[i].X,keyPointList[i].Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                Point p2 = Coordinate.CoordinateTransport(new Point(keyPointList[i+1].X, keyPointList[i+1].Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                drawingContext.DrawLine(pen, p1, p2);
            }
            //未完成
            if (tmpPoint != null && !this.IsComplete && keyPointList.Count > 0)
            {
                Point p1 = Coordinate.CoordinateTransport(new Point(keyPointList.Last().X, keyPointList.Last().Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                Point p2 = Coordinate.CoordinateTransport(new Point(tmpPoint.X, tmpPoint.Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                drawingContext.DrawLine(pen, p1, p2);
            }
            //已完成
            if (this.IsComplete && keyPointList.Count > 0)
            {
                Point p1 = Coordinate.CoordinateTransport(new Point(keyPointList.Last().X, keyPointList.Last().Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                Point p2 = Coordinate.CoordinateTransport(new Point(keyPointList.First().X, keyPointList.First().Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                drawingContext.DrawLine(pen, p1, p2);
            }
            //文字
            if (ShowTag && Tag != null && Tag != "")
            {
                drawingContext.DrawText(new FormattedText(this.Tag, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Normal,
                FontStretches.Normal), 16, br), new Point(loca.X + 10, loca.Y + 10));
            }

            if (this.Selected)
            {
                for (int i = 0; i < keyPointList.Count; i++)
                {
                    keyPointList[i].TractionPoint.Drawing(drawingContext);
                }
            }
        }

        public override bool IsIn(double x, double y)
        {
            return GeometryCal.PointInPolygon(x,y,this.GetPoints());
        }

        /// <summary>
        /// 完成该多边形
        /// </summary>
        /// <returns></returns>
        public bool Complete()
        {
            if (Check())
            {
                this.IsComplete = true;
                return true;
            }
            else
            {
                return false;
            }
        }

    }



    /// <summary>
    /// 坐标系和尺寸
    /// </summary>
    [Serializable()]
    public class Coordinate
    {

        /// <summary>
        /// 基本几何参数
        /// </summary>
        private double x, y, z, width, height, scale;
        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public double Z { get { return z; } set { z = value; } }
        public double Width { get { return width; } set { width = value; } }
        public double Height { get { return height; } set { height = value; } }

        public double Scale { get { return scale; } set { scale = value; } }
        public Coordinate(double x, double y, double scale)
        {
            this.X = x;
            this.Y = y;
            Scale = scale;
        }

        public Coordinate()
        {
            
            Scale = 1f;
        }

        public static Coordinate BaseCoornidate = new Coordinate(0f, 0f, 1f);

        public static Point CoordinateTransport(Point p, Coordinate src, Coordinate dst)
        {
            Point point = new Point
            {
                X = (src.X + p.X * src.Scale - dst.X) / dst.Scale,
                Y = (src.Y + p.Y * src.Scale - dst.Y) / dst.Scale
            };
            return point;
        }
    }

    public enum ImageViewState
    {
        Normal = 0,
        Edit = 1,
        Draw = 2,
    }

    public enum MouseState
    {
        Idle = 0,
        Operating = 1,
    }

    public enum ElementType
    {
        Image = 0,
        Point = 1,
        Line = 2,
        Rectangle = 3,
        Polygon = 4,
        Ellipse = 5,
    }
}
