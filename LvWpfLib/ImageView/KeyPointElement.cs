using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ncer.UI
{
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
}
