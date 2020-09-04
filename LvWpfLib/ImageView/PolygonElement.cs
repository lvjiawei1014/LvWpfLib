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
    /// 多边形
    /// </summary>
    public class PolygonElement : KeyPointElement
    {

        private KeyPoint tmpPoint;//绘图时的临时关键点

        public int PointNum { get; set; } = 0;

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
            int i, j, m, n;
            int x = keyPointList.Count;
            for (i = keyPointList.Count - 1, j = 0; j < keyPointList.Count; i = j++)
            {
                for (m = keyPointList.Count - 1, n = 0; n < keyPointList.Count; m = n++)
                {
                    if (m == i || i == n || j == m) continue;
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
            if(PointNum>2 && this.keyPointList.Count == PointNum)
            {
                return this.Complete();
            }
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
                Point p1 = Coordinate.CoordinateTransport(new Point(keyPointList[i].X, keyPointList[i].Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
                Point p2 = Coordinate.CoordinateTransport(new Point(keyPointList[i + 1].X, keyPointList[i + 1].Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
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
            return GeometryCal.PointInPolygon(x, y, this.GetPoints());
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

}
