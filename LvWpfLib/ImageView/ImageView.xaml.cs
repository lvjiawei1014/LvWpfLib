using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace Ncer.UI
{
    /// <summary>
    /// ImageView.xaml 的交互逻辑
    /// </summary>
    public partial class ImageView : UserControl
    {
        #region 事件

        //public delegate void ImageCli

        public delegate void ElementCreateEvent(ImageViewElement element);
        public event ElementCreateEvent ElementCreateEventHandler;

        public delegate void ElementDeleteEvent(ImageViewElement element);
        public event ElementDeleteEvent ElementDeleteEventHandler;

        public delegate void ImageMouseMoveHandler(object sender, ImageMouseMoveEventArgs imageMouseMoveEventArgs);
        public event ImageMouseMoveHandler ImageMouseMove;
        #endregion
        #region 静态成员
        private static float MaxScale = 10f;
        private static float MinScale = 0.1f;
        #endregion
        #region 成员变量
        public List<DisplayItem> Items = new List<DisplayItem>();
        private DisplayItem curItem;
        private bool continuousDraw = false;
        private System.Windows.Media.Color color = System.Windows.Media.Colors.Lime;
        private ImageViewState defaultState = ImageViewState.Normal;
        private Visibility toolBoxVisibility = Visibility.Visible;
        #endregion
        #region 属性
        /// <summary>
        /// 图像
        /// </summary>
        public ImageSource Image
        {
            get { return curItem.Image; }
            set { curItem.Image = value; this.OnImageSet(curItem.Image); }
        }
        public ImageViewState ImageViewState { get; set; }
        public ElementType DrawingElementType { get; set; }
        public MouseState MouseState { get; set; }
        public double ImageScale { get { return curItem.imageElement.Scale; } set { this.curItem.imageElement.Scale = value; } }
        public bool AutoFit { get; set; }

        public bool FixImage { get; set; } = false;

        public ImageElement ImageElement { get { return curItem.imageElement; } set { curItem.imageElement = value; } }
        public ImageViewElement OperatedElement { get { return curItem.operatedElement; } set { curItem.operatedElement = value; } }
        /// <summary>
        /// 图形绘制缓存对象 
        /// </summary>
        public KeyPointElement DrawingElement { get { return curItem.drawingElement; } set { curItem.drawingElement = value; } }
        public ImageViewElement SelectedElement { get { return curItem.selectedElement; } set { curItem.selectedElement = value; } }

        public ImageViewElement PointedElement { get { return curItem.pointedElement; } set { curItem.pointedElement = value; } }
        public Point operationStartControlPoint { get { return curItem.operationStartControlPoint; } set { curItem.operationStartControlPoint = value; } }
        /// <summary>
        /// 鼠标操作起始点在图像坐标系的坐标
        /// </summary>
        public Point OperationStartImagePoint { get { return curItem.operationStartImagePoint; } set { curItem.operationStartImagePoint = value; } }
        public Point OperatedElementStartPoint { get { return curItem.operatedElementStartPoint; } set { curItem.operatedElementStartPoint = value; } }//被操作的element的初始位置

        public List<ImageViewElement> Elements { get { return curItem.elements; } set { curItem.elements = value; } }
        public List<ImageViewElement> BaseElements { get { return curItem.baseElements; } set { curItem.baseElements = value; } }

        public DisplayItem CurItem { get => curItem; set => curItem = value; }
        public bool ContinuousDraw { get => continuousDraw; set => continuousDraw = value; }
        public System.Windows.Media.Color Color { get => color; set => color = value; }
        public ImageViewState DefaultState { get => defaultState; set => defaultState = value; }
        public Visibility ToolBoxVisibility { get => toolBoxVisibility; set => toolBoxVisibility = value; }

        #endregion
        #region 初始化
        public ImageView()
        {
            InitializeComponent();
            this.DataContext = this;
            this.ClipToBounds = true;
            curItem = new DisplayItem("default");
            //Items.Add(curItem);
            this.AddDisplayItem(curItem);
        }
        #endregion
        #region 核心逻辑
        protected override void OnRender(DrawingContext drawingContext)
        {

            base.OnRender(drawingContext);

            DrawingBrush tileBack = (DrawingBrush)this.Resources["TileBack"];
            drawingContext.DrawRectangle(tileBack, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            if (this.ImageElement != null)
            {
                ImageElement.Drawing(drawingContext);

                foreach (ImageViewElement element in Elements)
                {
                    if (element.Visible)
                    {
                        element.Drawing(drawingContext);
                    }
                }
                if (DrawingElement != null && DrawingElement.Visible && this.ImageViewState == ImageViewState.Draw)
                {
                    DrawingElement.Drawing(drawingContext);
                }
            }
        }

        private void Item_ElementDeleteEventHandler(ImageViewElement element)
        {
            this.ElementDeleteEventHandler?.Invoke(element);
        }
        private void OnImageSet(ImageSource image)
        {

                if (image != null)
                {
                    if (AutoFit)
                    {
                        ImageElement.FitToWindow(this.ActualWidth, this.ActualHeight);
                    }
                    else
                    {
                        this.ImageElement.Scale = Math.Min(ImageElement.Scale * ImageElement.Height / image.Height, ImageElement.Scale * ImageElement.Width / image.Width);
                    }

                    this.InvalidateVisual();
                }
            
            
        }
        public void ChangeMode(ImageViewState mode)
        {
            if (mode != this.ImageViewState)
            {
                this.MouseState = MouseState.Idle;
            }
            this.ImageViewState = mode;
            switch (mode)
            {
                case ImageViewState.Normal:
                    this.Cursor = Cursors.Hand;
                    break;
                case ImageViewState.Edit:
                    break;
                case ImageViewState.Draw:
                    break;
                default:
                    break;
            }


        }

        private void OnScale(Point mouseLocation, double scale)
        {
            if (scale > MaxScale || scale < MinScale)
            {
                return;
            }
            if (ImageElement != null)
            {
                ImageElement.ScaleImage(mouseLocation, scale);
            }

            this.InvalidateVisual();
        }
        
        public void Zoom(float relative)
        {
            this.OnScale(new Point(this.Width / 2, this.Height / 2), this.ImageScale * relative);
        }
        public void AbsoluteMove(double x,double y)
        {
            this.ImageElement.X = x ;
            this.ImageElement.Y = y;
        }
        #endregion
        #region display item
        public void AddDisplayItem(DisplayItem item)
        {
            item.ElementDeleteEventHandler += Item_ElementDeleteEventHandler;
            this.Items.Add(item);
        }
        public DisplayItem GetDisplayItem(string name)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == name)
                {
                    return Items[i];
                }
            }
            return null;
        }

        public void SwitchDisplayItem(string name)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == name)
                {
                    this.curItem = Items[i];
                    break;
                }
            }
            this.MouseState = MouseState.Idle;
            this.ImageViewState = ImageViewState.Normal;
            this.ImageElement.FitToWindow(this.ActualWidth, this.ActualHeight);
            this.InvalidateVisual();
        }
        public void SwitchDisplayItem(int index)
        {
            if (index < Items.Count && index >= 0)
            {
                this.curItem = Items[index];
                this.MouseState = MouseState.Idle;
                this.ImageViewState = ImageViewState.Normal;
                this.InvalidateVisual();
            }

        }
        /// <summary>
        /// 根据displayitem中附带的object切换页面
        /// </summary>
        /// <param name="obj"></param>
        public void SwitchDispalyItemByObject(object obj)
        {
            if (obj == null) return;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Item == obj)
                {
                    SwitchDisplayItem(i);
                    return;
                }
            }
        }

        public DisplayItem FindDisplayItemByObject(object obj)
        {
            if (obj == null) return null;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Item == obj)
                {
                    return Items[i];
                }
            }
            return null;
        }

        public void DeleteDisplayItem(string name)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == name)
                {
                    Items.Remove(Items[i]);
                    break;
                }
            }
        }

        public void DeleteAllDisplay()
        {
            while (Items.Count > 1)
            {
                Items.RemoveAt(1);
            }
        }

        #endregion
        #region Elements
        /// <summary>
        /// 在绘制前先创建好对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void CreateElement(ElementType type, string name = "")
        {
            switch (type)
            {
                case ElementType.Image:
                    break;
                case ElementType.Point:
                    this.DrawingElement = new PointElement();
                    break;
                case ElementType.Line:
                    this.DrawingElement = new LineElement();
                    break;
                case ElementType.Rectangle:
                    this.DrawingElement = new RectElement();
                    break;
                case ElementType.Polygon:
                    this.DrawingElement = new PolygonElement();
                    break;
                //case ElementType.Ellipse:
                //    this.drawingElement = new EllipseElement();
                //    break;
                default:
                    break;
            }
            this.DrawingElement.Name = name;
            this.DrawingElement.Parent = this.ImageElement;
            this.DrawingElement.GlobalCoordinate = this.ImageElement.Coordinate;
            this.MouseState = MouseState.Idle;
            this.ImageViewState = ImageViewState.Draw;
            this.DrawingElementType = type;
            this.DrawingElement.Visible = false;
            this.DrawingElement.IsComplete = false;
            this.DrawingElement.Color = this.Color;
        }

        public void AddElement(ImageViewElement element)
        {
            switch (element)
            {
                case PointElement point when element is PointElement:
                    AddPoint(point);
                    break;
                case RectElement rect when element is RectElement:
                    AddRectangle(rect);
                    break;
                case LineElement line when element is LineElement:
                    AddLine(line);
                    break;
                case PolygonElement polygon when element is PolygonElement:
                    AddPolygon(polygon);
                    break;
                default:
                    break;
            }
        }

        public void AddPoint(PointElement point)
        {
            //point.GlobalCoordinate = imageElement.Coordinate;
            //point.Parent = imageElement;
            Elements.Add(point);
            BaseElements.Add(point);
            for (int i = 0; i < point.keyPointList.Count; i++)
            {
                BaseElements.Add(point.keyPointList[i].TractionPoint);
            }
            BaseElements.Sort();
            this.InvalidateVisual();
        }

        public void AddLine(LineElement line)
        {
            line.GlobalCoordinate = ImageElement.Coordinate;
            line.Parent = ImageElement;
            Elements.Add(line);
            BaseElements.Add(line);
            for (int i = 0; i < line.keyPointList.Count; i++)
            {
                BaseElements.Add(line.keyPointList[i].TractionPoint);
            }
            BaseElements.Sort();
            this.InvalidateVisual();
        }

        public void AddPolygon(PolygonElement polygon)
        {
            polygon.GlobalCoordinate = ImageElement.Coordinate;
            polygon.Parent = ImageElement;
            Elements.Add(polygon);
            BaseElements.Add(polygon);
            for (int i = 0; i < polygon.keyPointList.Count; i++)
            {
                BaseElements.Add(polygon.keyPointList[i].TractionPoint);
            }
            BaseElements.Sort();
            this.InvalidateVisual();
        }

        public void AddRectangle(RectElement rect)
        {
            rect.GlobalCoordinate = ImageElement.Coordinate;
            rect.Parent = ImageElement;
            Elements.Add(rect);
            BaseElements.Add(rect);
            BaseElements.Add(rect.leftTopPoint);
            BaseElements.Add(rect.leftBottomPoint);
            BaseElements.Add(rect.rightTopPoint);
            BaseElements.Add(rect.rightBottomPoint);
            BaseElements.Sort();
            this.InvalidateVisual();
        }

        //public void AddEllipse(EllipseElement ellipse)
        //{
        //    ellipse.ParentCoordinate = imageElement.coordinate;
        //    ellipse.ParentElement = imageElement;
        //    elements.Add(ellipse);
        //    baseElements.Add(ellipse);
        //    baseElements.Add(ellipse.leftTopPoint);
        //    baseElements.Add(ellipse.leftBottomPoint);
        //    baseElements.Add(ellipse.rightTopPoint);
        //    baseElements.Add(ellipse.rightBottomPoint);
        //    baseElements.Sort();
        //}
        public ImageViewElement GetTargetElement(double x, double y)
        {

            for (int i = 0; i < BaseElements.Count; i++)
            {
                if (BaseElements[i].Focusable&& BaseElements[i].IsIn(x, y))
                {
                    return BaseElements[i];
                }
            }
            return ImageElement;
        }

        public void DeleteElement(ImageViewElement element)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                if (element == Elements[i])
                {
                    Elements.RemoveAt(i);
                }
            }

            for (int i = 0; i < BaseElements.Count; i++)
            {
                if (element == BaseElements[i].Parent)
                {
                    BaseElements.RemoveAt(i);
                }
            }


            this.InvalidateVisual();
        }

        public void SelectElement(ImageViewElement element)
        {
            if (this.SelectedElement != null && !(element is TractionPoint))
            {
                this.SelectedElement.Selected = false;
                this.SelectedElement = element;
                this.SelectedElement.Selected = true;
                this.InvalidateVisual();
            }
            else
            {
                this.SelectedElement = element;
                this.SelectedElement.Selected = true;
                this.InvalidateVisual();
            }
            
        }


        public ImageViewElement GetElement(string name)
        {
            foreach (var item in this.Elements)
            {
                if(!string.IsNullOrWhiteSpace(item.Name) && item.Name == name)
                {
                    return item;
                }
            }

            return null;
        }



        /// <summary>
        /// 移除所有元素
        /// </summary>
        public void ClearElement()
        {
            this.BaseElements.Clear();
            this.Elements.Clear();
        }

        #endregion
        #region ui 交互事件
        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Console.WriteLine(e.Delta + "  " + e.GetPosition(this));
            this.OnScale(e.GetPosition(this), this.ImageElement.Scale * (e.Delta < 0 ? 0.8f : 1.25f));
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = Coordinate.CoordinateTransport(e.GetPosition(this), Coordinate.BaseCoornidate, ImageElement.Coordinate);
            this.ImageMouseMove?.Invoke(this, new ImageMouseMoveEventArgs() { Location = p });
            ImageViewElement targetElement = this.GetTargetElement(p.X, p.Y);
            PointedElement = targetElement;

            if (ImageViewState == ImageViewState.Normal)
            {
                if (MouseState == MouseState.Operating)
                {
                    if (OperatedElement != ImageElement)
                    {
                        //operatedElement.Move(p);
                    }
                    else
                    {
                        if(!FixImage) AbsoluteMove(e.GetPosition(this).X - OperationStartImagePoint.X * ImageScale, e.GetPosition(this).Y - OperationStartImagePoint.Y * ImageScale);

                    }
                    this.InvalidateVisual();
                }
                else
                {
                    //this.Cursor = targetElement.ElememtCursor;
                }
                return;
            }
            if (ImageViewState == ImageViewState.Draw)
            {
                if (MouseState == MouseState.Operating)
                {
                    if(DrawingElement is KeyPointElement)
                    {
                        (DrawingElement as KeyPointElement).AdjustNextKeyPoint(p);
                    }
                    
                    this.InvalidateVisual();
                }
            }
            if (ImageViewState == ImageViewState.Edit)
            {
                if (MouseState == MouseState.Operating)
                {
                    if (OperatedElement != ImageElement)
                    {
                        OperatedElement.Move(this.OperatedElementStartPoint.X + p.X - OperationStartImagePoint.X, this.OperatedElementStartPoint.Y + p.Y - OperationStartImagePoint.Y);
                    }
                    else
                    {
                        AbsoluteMove(e.GetPosition(this).X - OperationStartImagePoint.X * ImageScale, e.GetPosition(this).Y - OperationStartImagePoint.Y * ImageScale);

                    }
                    this.InvalidateVisual();
                }
                else
                {
                    this.Cursor = targetElement.ElementCursor;
                }
                return;
            }


        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Coordinate.CoordinateTransport(e.GetPosition(this), Coordinate.BaseCoornidate, ImageElement.Coordinate);
            ImageViewElement targetElement = this.GetTargetElement(p.X, p.Y);
            if (ImageViewState == ImageViewState.Normal)
            {
                if (MouseState == MouseState.Operating)
                {

                }
                else
                {

                }
                //图片拖动准备
                this.OperatedElement = targetElement;
                this.MouseState = MouseState.Operating;
                this.operationStartControlPoint = e.GetPosition(this);
                this.OperationStartImagePoint = p;
            }
            if (ImageViewState == ImageViewState.Draw)
            {

                this.SelectElement(this.DrawingElement);
                if (MouseState == MouseState.Idle)
                {
                    MouseState = MouseState.Operating;
                    DrawingElement.Visible = true;
                }
                //如果添加了最后一个点
                if (DrawingElement.AddKeyPoint(p))
                {
                    MouseState = MouseState.Idle;
                    AddElement(DrawingElement);
                    this.ElementCreateEventHandler?.Invoke(DrawingElement);//触发事件
                    if (this.ContinuousDraw)
                    {
                        CreateElement(this.DrawingElementType);
                    }
                    else
                    {
                        DrawingElement = null;
                        this.ImageViewState = this.DefaultState;
                    }
                }
                //else if (e.Button == MouseButtons.Right)
                //{
                //    if (drawingElement is PolygonElement)
                //    {
                //        PolygonElement polygon = drawingElement as PolygonElement;
                //        if (polygon.Complete())
                //        {
                //            MouseState = MouseState.Idle;
                //            this.ElementCreateEventHandler(drawingElement);
                //            if (this.ContinuousDraw)
                //            {
                //                CreateElement(this.DrawingElementType);
                //            }
                //            else
                //            {
                //                drawingElement = null;
                //                this.ImageViewState = ImageViewState.Normal;
                //            }
                //        }
                //        else
                //        {
                //            MouseState = MouseState.Idle;
                //            this.drawingElement = null;
                //        }
                //    }
                //}
            }
            if (ImageViewState == ImageViewState.Edit)//编辑模式 确定编辑对象
            {
                //
                this.OperatedElement = targetElement;
                this.MouseState = MouseState.Operating;
                this.operationStartControlPoint = e.GetPosition(this);
                this.OperationStartImagePoint = p;
                this.OperatedElementStartPoint = new Point(targetElement.X,targetElement.Y);
                System.Console.WriteLine("operationStartImagePoint:" + OperationStartImagePoint.X + "  " + OperationStartImagePoint.Y);
                this.SelectElement(OperatedElement);
            }

        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ImageViewState == ImageViewState.Normal)
            {
                if (MouseState == MouseState.Operating)
                {
                    MouseState = MouseState.Idle;
                }
                else
                {

                }

            }
            if (ImageViewState == ImageViewState.Draw)
            {

            }
            if (ImageViewState == ImageViewState.Edit)
            {
                if (OperatedElement != null)
                {
                    OperatedElement.ChangeDone();
                }
                MouseState = MouseState.Idle;
                OperatedElement = null;
            }  
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)sender).Name)
            {
                case "DrawPoint":
                    this.CreateElement(ElementType.Point);
                    break;

                case "DrawRect":
                    this.CreateElement(ElementType.Rectangle);
                    break;

                case "DrawLine":
                    this.CreateElement(ElementType.Line);
                    break;
                case "EditMode":
                    this.ImageViewState = ImageViewState.Edit;
                    break;
                case "DisableAutoFit":
                    this.AutoFit = false;
                    break;
                case "AutoFit":
                    this.AutoFit = true;
                    break;
                case "DeleteAllElement":
                    this.CurItem.DeleteAllElement();
                    break;
                case "DeleteSelectedElement":
                    this.CurItem.DeleteElement(this.SelectedElement);
                    break;
                default:
                    break;
                    
            }
            this.InvalidateVisual();
        }

        private void RbColor_Checked(object sender, RoutedEventArgs e)
        {
            string name = (((RadioButton)sender).Name);
            switch (name)
            {
                case "rbColorGreen":
                    this.Color = Colors.Lime;
                    break;
                case "rbColorRed":
                    this.Color = Colors.Red;
                    break;
                case "rbColorYellow":
                    this.Color = Colors.Yellow;
                    break;
                case "rbColorBlue":
                    this.Color = Colors.Blue;
                    break;
                case "rbColorWhite":
                    this.Color = Colors.White;
                    break;
                case "rbColorPurple":
                    this.Color = Colors.Purple;
                    break;
                default:
                    break;
            }
        }

        private void IbElement_Click(object sender, RoutedEventArgs e)
        {
            string name = (((Button)sender).Name);
            switch (name)
            {
                case "ibRect":
                    this.CreateElement(ElementType.Rectangle);
                    break;
                case "ibPolygon":
                    this.CreateElement(ElementType.Polygon);
                    break;
                case "ibEllipse":
                    this.CreateElement(ElementType.Polygon);
                    break;
                default:break;
            }
        }

        private void UserControl_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (ImageViewState == ImageViewState.Draw)
            {
                if (this.DrawingElement is PolygonElement)
                {
                    e.Handled = true;
                    MouseState = MouseState.Idle;
                    if ((DrawingElement as PolygonElement).Complete())
                    {
                        AddElement(DrawingElement);
                        this.ElementCreateEventHandler?.Invoke(DrawingElement);//触发事件
                    }
                    else{
                        this.InvalidateVisual();
                    }
                    
                    
                    
                    if (this.ContinuousDraw)
                    {
                        CreateElement(this.DrawingElementType);
                    }
                    else
                    {
                        DrawingElement = null;
                        this.ImageViewState = this.DefaultState;
                    }
                }
            }
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        #endregion
    }


    public class DisplayItem
    {

        public delegate void ElementDeleteEvent(ImageViewElement element);
        public event ElementDeleteEvent ElementDeleteEventHandler;

        public string Name { get; set; }

        private Object item=null;
        public List<ImageViewElement> elements = new List<ImageViewElement>();
        public List<ImageViewElement> baseElements = new List<ImageViewElement>();
        public ImageElement imageElement;
        public ImageViewElement operatedElement;
        public KeyPointElement drawingElement;
        public ImageViewElement selectedElement;
        public ImageViewElement pointedElement;
        public System.Windows.Point operationStartControlPoint;
        public System.Windows.Point operationStartImagePoint;
        public Point operatedElementStartPoint;//被操作的element的初始位置
        public ImageSource Image { get { return imageElement.Image; } set { imageElement.Image = value; } }
        /// <summary>
        /// DisplayItem的关联对象
        /// </summary>
        public object Item { get => item; set => item = value; }

        public DisplayItem(string name)
        {
            this.Name = name;
            this.imageElement = new ImageElement();
        }

        public DisplayItem(string name,Object item,ImageSource image)
        {
            this.imageElement = new ImageElement();
            this.Name = name;
            this.Item = item;
            this.Image = image;
        }
        public void AddElement(ImageViewElement element)
        {
            //if (element is RectElement)
            //{
            //    AddRectangle(element as RectElement);
            //    return;
            //}
            //if (element is Line)
            //{
            //    AddLine(element as Line);
            //    return;

            //}
            //if (element is PolygonElement)
            //{
            //    AddPolypon(element as PolygonElement);
            //}
            //if (element is EllipseElement)
            //{
            //    AddEllipse(element as EllipseElement);
            //}
        }
        public void AddLine(Line line)
        {
            //line.ParentCoordinate = imageElement.coordinate;
            //line.ParentElement = imageElement;
            //elements.Add(line);
            //baseElements.Add(line);
            //baseElements.Add(line.TractionPoints[0]);
            //baseElements.Add(line.TractionPoints[1]);
            //baseElements.Sort();
        }
        //public void AddPolypon(PolygonElement polygon)
        //{
        //    polygon.ParentCoordinate = imageElement.coordinate;
        //    polygon.ParentElement = imageElement;
        //    elements.Add(polygon);
        //    baseElements.Add(polygon);
        //    for (int i = 0; i < polygon.keyPointList.Count; i++)
        //    {
        //        baseElements.Add(polygon.keyPointList[i].tractionPoint);
        //    }
        //    baseElements.Sort();
        //}
        //public void AddRectangle(RectElement rect)
        //{
        //    rect.ParentCoordinate = imageElement.coordinate;
        //    rect.ParentElement = imageElement;
        //    elements.Add(rect);
        //    baseElements.Add(rect);
        //    baseElements.Add(rect.leftTopPoint);
        //    baseElements.Add(rect.leftBottomPoint);
        //    baseElements.Add(rect.rightTopPoint);
        //    baseElements.Add(rect.rightBottomPoint);
        ////    baseElements.Sort();
        ////}
        //public void AddEllipse(EllipseElement ellipse)
        //{
        //    ellipse.ParentCoordinate = imageElement.coordinate;
        //    ellipse.ParentElement = imageElement;
        //    elements.Add(ellipse);
        //    baseElements.Add(ellipse);
        //    baseElements.Add(ellipse.leftTopPoint);
        //    baseElements.Add(ellipse.leftBottomPoint);
        //    baseElements.Add(ellipse.rightTopPoint);
        //    baseElements.Add(ellipse.rightBottomPoint);
        //    baseElements.Sort();
        //}
        public void DeleteElement(ImageViewElement element)
        {

            if (elements.Contains(element))
            {
                elements.Remove(element);   
            }


            for (int i = 0; i < baseElements.Count; i++)
            {
                if (element == baseElements[i].Parent)
                {
                    baseElements.RemoveAt(i);
                }
            }
            element.Delete();
        }

        public void DeleteAllElement()
        {
            //for (int i = 0; i < this.elements.Count; i++)
            //{
            //    this.DeleteElement(elements[i]);
            //}
            foreach(var item in this.elements)
            {
                item.Delete();
            }
            elements.Clear();
            baseElements.Clear();
        }


    }

}
