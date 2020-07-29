using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ncer.UI
{
    /// <summary>
    /// 椭圆
    /// </summary>
    public class EllipseElement : KeyPointElement
    {
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
            }
        }

        public Point Center
        {
            get
            {
                return new Point(X + Width / 2,Y+Height/2);
            }

            set
            {
                X = value.X - Width / 2;
                Y = value.Y - Height / 2;
            }
        }

        public double RadiusX
        {
            get
            {
                return Width / 2;
            }
            set
            {
                Width = value * 2;
            }
        }

        public double RadiusY
        {
            get
            {
                return Height / 2;
            }
            set
            {
                Height = value * 2;
            }
        }

        public EllipseElement() : this(0, 0, 1, 1) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public EllipseElement(double x, double y, double width, double height) : base()
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
            this.OnElementChange(this);
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
                    this.keyPoint2.TractionPoint.Traction(point);
                    break;
            }
        }

        public override void Drawing(DrawingContext drawingContext)
        {
            Point loca = Coordinate.CoordinateTransport(new Point(X, Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
            loca.Y += RadiusY * Parent.Coordinate.Scale;
            loca.X += RadiusX * Parent.Coordinate.Scale;
            Brush br = new SolidColorBrush(this.Color);
            //Color borderColor = Colors.Black;
            //borderColor.A = 0x88;
            //drawingContext.DrawRectangle(null, new Pen(new SolidColorBrush(borderColor),0.5), new Rect(loca.X, loca.Y, Width * Parent.Coordinate.Scale, Height * Parent.Coordinate.Scale));
            drawingContext.DrawEllipse(null, new Pen(br, 1), loca, RadiusX * Parent.Coordinate.Scale, RadiusY * Parent.Coordinate.Scale);
            if (ShowTag && !string.IsNullOrEmpty(Tag))
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
}
