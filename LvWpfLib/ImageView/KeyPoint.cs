using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ncer.UI
{
    /// <summary>
    /// 关键点对象
    /// </summary>
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
}
