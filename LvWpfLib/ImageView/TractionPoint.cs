using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Ncer.UI
{
    /// <summary>
    /// 拖拽点对象
    /// </summary>
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
            this.Size = 8;
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="parent"></param>
        public TractionPoint(double x, double y, ImageViewElement parent) : base()
        {
            this.Size = 8;
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
}
