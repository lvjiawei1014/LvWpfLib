using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Point = System.Windows.Point;
using System.Globalization;

namespace Ncer.UI
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

        #region 成员
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
        private bool editable = true;

        private Cursor elementCursor = Cursors.Arrow;
        private Color color = Colors.Lime;


        #endregion
        #region 属性
        //private Font font;
        public bool Focusable { get; set; } = true;
        public Coordinate Coordinate { get => coordinate; set => coordinate = value; }
        public string Name { get => name; set => name = value; }
        public string Tag { get => tag; set => tag = value; }
        public ImageViewElement Parent { get => parent; set => parent = value; }
        public List<ImageViewElement> Childrens { get => childrens; set => childrens = value; }
        public bool Visible { get => visible; set => visible = value; }

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
        public bool Editable { get => editable; set => editable = value; }

        public double LineWidth { get; set; } = 1;
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
        /// <summary>
        /// 对象的绘制
        /// </summary>
        /// <param name="drawingContext"></param>
        public abstract void Drawing(DrawingContext drawingContext);
        public virtual void OnElementChange(ImageViewElement element)
        {
            this.OnElementChangeEvent?.Invoke(this);
        }
        /// <summary>
        /// 被选中时
        /// </summary>
        /// <param name="b"></param>
        public abstract void BeSelect(bool b);
        /// <summary>
        /// 判断是否在对象内部
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public abstract bool IsIn(double x, double y);
        /// <summary>
        /// 比较对象的前后
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ImageViewElement other)
        {
            return this.Z == other.Z ? 0 : (this.Z > other.Z ? 1 : -1);
        }
        /// <summary>
        /// 对象被删除
        /// </summary>
        public void Delete()
        {
            this.OnElementDelete?.Invoke(this);
        }
        /// <summary>
        /// 对象修改完成
        /// </summary>
        public void ChangeDone()
        {
            this.OnElementChangeDoneEvent?.Invoke(this);
            Parent?.ChangeDone();
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
        Circle=6,
    }
}
