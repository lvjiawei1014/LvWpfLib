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
    public class PointElement : KeyPointElement
    {
        private double size = 10;
        private PointElementStyle pointStyle = PointElementStyle.Circle | PointElementStyle.Cross;

        public double Size { get => size; set => size = value; }

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
        public PointElement()
        {
            this.ElementCursor = Cursors.Hand;
            KeyPointAmount = 1;
        }

        public PointElement(double x, double y,ImageViewElement parent)
        {
            this.Parent = parent;
            this.GlobalCoordinate = Parent.Coordinate;
            this.IsComplete = false;
            this.AddKeyPoint(new Point(x, y));
            this.ElementCursor = Cursors.Hand;
            KeyPointAmount = 1;
        }
        public override bool AddKeyPoint(Point p)
        {
            if (!this.IsComplete)
            {
                KeyPoint kp = new KeyPoint(p.X, p.Y, this);
                kp.TractionPoint.ElementCursor = Cursors.SizeAll;
                kp.KeyPointChangeEventHandler += KeyPointChangeEventHandler;
                this.keyPointList.Add(kp);

                IsComplete = true;
            }
            return IsComplete;
        }

        private void KeyPointChangeEventHandler(KeyPoint kp, double x, double y)
        {
            OnElementChange(this);
        }

        public override void AdjustNextKeyPoint(Point point)
        {
        }

        public override void BeSelect(bool b)
        {
            keyPointList[0].TractionPoint.Visible = b;
        }

        public override void Drawing(DrawingContext drawingContext)
        {
            Point loca = Coordinate.CoordinateTransport(new Point(X, Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
            Brush br = new SolidColorBrush(this.Color);
            Pen pen = new Pen(br, 1);

            if ((pointStyle & PointElementStyle.Dot) == PointElementStyle.Dot)
            {
                drawingContext.DrawEllipse(br, pen, loca, 1, 1);
            }
            if ((pointStyle & PointElementStyle.Cross) == PointElementStyle.Cross)
            {
                drawingContext.DrawLine(pen, new Point(loca.X, loca.Y - size*1.4), new Point(loca.X, loca.Y + size*1.4));
                drawingContext.DrawLine(pen, new Point(loca.X - size*1.4, loca.Y), new Point(loca.X + size*1.4, loca.Y));
            }
            if((pointStyle& PointElementStyle.Circle) == PointElementStyle.Circle)
            {
                drawingContext.DrawEllipse(null, pen, loca, size , size );

            }

            if (ShowTag && Tag != null && Tag != "")
            {
                drawingContext.DrawText(new FormattedText(this.Tag, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(new FontFamily("宋体"), FontStyles.Normal, FontWeights.Normal,
                FontStretches.Normal), 16, br), new Point(loca.X + 10, loca.Y + 10));
            }

            //if (this.Selected)
            //{
            //    for (int i = 0; i < keyPointList.Count; i++)
            //    {
            //        keyPointList[i].TractionPoint.Drawing(drawingContext);
            //    }
            //}

        }

        public override bool IsIn(double x, double y)
        {
            return this.Size >= Math.Sqrt(Math.Pow(this.X - x, 2) + Math.Pow(this.Y - y, 2));
        }
    }

    public enum PointElementStyle
    {
        Dot=0x01,
        Cross=0x10,
        Circle=0x0100,
    }
}
