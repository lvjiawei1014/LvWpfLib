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
    public class LineElement : KeyPointElement
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

        public LineElement()
        {
            this.ElementCursor = Cursors.Hand;
            tmpPoint = new KeyPoint(0, 0, this);
            KeyPointAmount = 2;

        }

        public override bool AddKeyPoint(Point p)
        {
            if (!this.IsComplete)
            {
                switch (this.keyPointList.Count)
                {
                    case 0:
                        this.tmpPoint.X = p.X;
                        this.tmpPoint.Y = p.Y;
                        break;
                    default:
                        break;
                }
                KeyPoint kp = new KeyPoint(p.X, p.Y, this);
                kp.TractionPoint.ElementCursor = Cursors.SizeAll;
                kp.KeyPointChangeEventHandler += Kp_KeyPointChangeEventHandler;
                this.keyPointList.Add(kp);
                if (this.keyPointList.Count == KeyPointAmount)
                {
                    IsComplete = true;
                }
            }
            return IsComplete;
        }

        private void Kp_KeyPointChangeEventHandler(KeyPoint kp, double x, double y)
        {
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

            if (IsComplete)
            {
                Point p1 = Coordinate.CoordinateTransport(new Point(keyPointList[0].X, keyPointList[0].Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                Point p2 = Coordinate.CoordinateTransport(new Point(keyPointList[1].X, keyPointList[1].Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                drawingContext.DrawLine(pen, p1, p2);
            }
            else if (tmpPoint != null && keyPointList.Count > 0)
            {
                Point p1 = Coordinate.CoordinateTransport(new Point(keyPointList[0].X, keyPointList[0].Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                Point p2 = Coordinate.CoordinateTransport(new Point(tmpPoint.X, tmpPoint.Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
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
            return GeometryCal.PointInLine(x, y, this.GetPoints());
        }
    }
}
