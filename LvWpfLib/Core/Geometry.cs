using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Ncer.UI
{
    public class GeometryCal
    {

        public static bool SegmentIntersect(Point point1,Point point2,Point point3,Point point4)
        {
            double a1, b1, c1, a2, b2, c2,x,y;
            if (point1.X == point2.X)
            {
                b1 = 0;
                a1 = 1;
                c1 = -point1.X;
            }
            else
            {
                b1 = 1;
                a1 = (point1.Y - point2.Y) / (point2.X - point1.X);
                c1 = -(a1 * point2.X + point2.Y);
            }

            if (point3.X == point4.X)
            {
                b2 = 0;
                a2 = 1;
                c2 = -point3.X;
            }
            else
            {
                b2 = 1;
                a2 = (point3.Y - point4.Y) / (point4.X - point3.X);
                c2 = -(a2 * point4.X + point4.Y);
            }
            y = (a1 * c2 - a2 * c1) / (a2 * b1 - a1 * b2);
            //x = -(b2 * y + c2) / a2;

            return (y > Math.Min(point1.Y, point2.Y)) && (y < Math.Max(point1.Y, point2.Y)) &&
                (y > Math.Min(point3.Y, point4.Y)) && (y < Math.Max(point3.Y, point4.Y));
        }

        /// <summary>
        /// 判断点是否在矩形内部
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool PointInPolygon(double x,double y,List<Point> points)
        {
            double maxx, maxy, minx, miny;
            maxx = points[0].X;
            maxy = points[0].Y;
            minx = points[0].X;
            miny = points[0].Y;
            for (int k = 0; k < points.Count; k++)
            {
                maxx = Math.Max(maxx, points[k].X);
                maxy = Math.Max(maxy, points[k].Y);
                minx = Math.Min(minx, points[k].X);
                miny = Math.Min(miny, points[k].Y);
            }
            if(x<minx|| x > maxx || y < miny || y > maxy)
            {
                return false;
            }
            bool c = false;
            int n = points.Count;
            int i, j;
            for (i = 0,j=n-1; i < n; j=i++)
            { 
                if(((points[i].Y>y)!=(points[j].Y>y)) &&
                    (x < (points[j].X - points[i].X) * (y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
                {
                    c = !c;
                }
            }
            return c;

        }

        public static bool PointInLine(double x,double y,List<Point> line)
        {
            return false;
            if(line==null || line.Count != 2)
            {
                return false;
            }

        }
    }


}
