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

namespace LvWpfLib.LvImageView
{
    /// <summary>
    /// ImageView.xaml 的交互逻辑
    /// </summary>
    public partial class ImageView : UserControl
    {
        #region 事件
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
        private System.Windows.Media.Color color=System.Windows.Media.Colors.Lime;
        private ImageViewState defaultState = ImageViewState.Normal;
        #endregion
        #region 属性
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

        public ImageElement imageElement { get { return curItem.imageElement; } set { curItem.imageElement = value; } }
        public ImageViewElement operatedElement { get { return curItem.operatedElement; } set { curItem.operatedElement = value; } }
        /// <summary>
        /// 图形绘制缓存对象 
        /// </summary>
        public KeyPointElement drawingElement { get { return curItem.drawingElement; } set { curItem.drawingElement = value; } }
        public ImageViewElement selectedElement { get { return curItem.selectedElement; } set { curItem.selectedElement = value; } }

        public ImageViewElement pointedElement { get { return curItem.pointedElement; } set { curItem.pointedElement = value; } }
        public Point operationStartControlPoint { get { return curItem.operationStartControlPoint; } set { curItem.operationStartControlPoint = value; } }
        /// <summary>
        /// 鼠标操作起始点在图像坐标系的坐标
        /// </summary>
        public Point operationStartImagePoint { get { return curItem.operationStartImagePoint; } set { curItem.operationStartImagePoint = value; } }
        public Point operatedElementStartPoint { get { return curItem.operatedElementStartPoint; } set { curItem.operatedElementStartPoint = value; } }//被操作的element的初始位置

        public List<ImageViewElement> elements { get { return curItem.elements; } set { curItem.elements = value; } }
        public List<ImageViewElement> baseElements { get { return curItem.baseElements; } set { curItem.baseElements = value; } }

        public DisplayItem CurItem { get => curItem; set => curItem = value; }
        public bool ContinuousDraw { get => continuousDraw; set => continuousDraw = value; }
        public System.Windows.Media.Color Color { get => color; set => color = value; }
        public ImageViewState DefaultState { get => defaultState; set => defaultState = value; }

        #endregion
        #region 事件
        public delegate void ElementCreateEvent(ImageViewElement element);
        public event ElementCreateEvent ElementCreateEventHandler;

        public delegate void ElementDeleteEvent(ImageViewElement element);
        public event ElementDeleteEvent ElementDeleteEventHandler;
        #endregion
        #region 初始化
        public ImageView()
        {
            InitializeComponent();
            this.ClipToBounds = true;
            curItem = new DisplayItem("default");
            //Items.Add(curItem);
            this.AddDisplayItem(curItem);
        }
        #endregion

        protected override void OnRender(DrawingContext drawingContext)  
        {
            
            base.OnRender(drawingContext);
            
            DrawingBrush tileBack = (DrawingBrush)this.Resources["TileBack"];
            drawingContext.DrawRectangle(tileBack, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            if (this.imageElement != null)
            {
                imageElement.Drawing(drawingContext);

                foreach (ImageViewElement element in elements)
                {
                    if (element.Visible)
                    {
                        element.Drawing(drawingContext);
                    }
                }
                if (drawingElement != null && drawingElement.Visible && this.ImageViewState == ImageViewState.Draw)
                {
                    drawingElement.Drawing(drawingContext);
                }
            }
        }
        #region 核心逻辑
        //public void AddDisplayItem(string name)
        //{
        //    this.Items.Add(new DisplayItem(name));
        //}
        public void AddDisplayItem(DisplayItem item)
        {
            item.ElementDeleteEventHandler += Item_ElementDeleteEventHandler;
            this.Items.Add(item);
        }

        private void Item_ElementDeleteEventHandler(ImageViewElement element)
        {
            this.ElementDeleteEventHandler?.Invoke(element);
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
            this.imageElement.FitToWindow(this.ActualWidth, this.ActualHeight);
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
        private void OnImageSet(ImageSource image)
        {

                if (image != null)
                {
                    if (AutoFit)
                    {
                        imageElement.FitToWindow(this.ActualWidth, this.ActualHeight);
                    }
                    else
                    {
                        this.imageElement.Scale = Math.Min(imageElement.Scale * imageElement.Height / image.Height, imageElement.Scale * imageElement.Width / image.Width);
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
            if (imageElement != null)
            {
                imageElement.ScaleImage(mouseLocation, scale);
            }

            this.InvalidateVisual();
        }
        
        public void Zoom(float relative)
        {
            this.OnScale(new Point(this.Width / 2, this.Height / 2), this.ImageScale * relative);
        }
        public void AddElement(ImageViewElement element)
        {
            if (element is RectElement)
            {
                AddRectangle(element as RectElement);
                return;
            }
            if (element is LineElement)
            {
                AddLine(element as LineElement);
                return;

            }
            if (element is PolygonElement)
            {
                AddPolypon(element as PolygonElement);
            }
            //if (element is EllipseElement)
            //{
            //    AddEllipse(element as EllipseElement);
            //}
        }
        public void AddLine(LineElement line)
        {
            line.GlobalCoordinate = imageElement.Coordinate;
            line.Parent = imageElement;
            elements.Add(line);
            baseElements.Add(line);
            for (int i = 0; i < line.keyPointList.Count; i++)
            {
                baseElements.Add(line.keyPointList[i].TractionPoint);
            }
            baseElements.Sort();
            this.InvalidateVisual();
        }
        public void AddPolypon(PolygonElement polygon)
        {
            polygon.GlobalCoordinate = imageElement.Coordinate;
            polygon.Parent = imageElement;
            elements.Add(polygon);
            baseElements.Add(polygon);
            for (int i = 0; i < polygon.keyPointList.Count; i++)
            {
                baseElements.Add(polygon.keyPointList[i].TractionPoint);
            }
            baseElements.Sort();
            this.InvalidateVisual();
        }
        public void AddRectangle(RectElement rect)
        {
            rect.GlobalCoordinate = imageElement.Coordinate;
            rect.Parent = imageElement;
            elements.Add(rect);
            baseElements.Add(rect);
            baseElements.Add(rect.leftTopPoint);
            baseElements.Add(rect.leftBottomPoint);
            baseElements.Add(rect.rightTopPoint);
            baseElements.Add(rect.rightBottomPoint);
            baseElements.Sort();
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

            for (int i = 0; i < baseElements.Count; i++)
            {
                if (baseElements[i].IsIn(x, y))
                {
                    return baseElements[i];
                }
            }
            return imageElement;
        }
        

        public void AbsoluteMove(double x,double y)
        {
            this.imageElement.X = x ;
            this.imageElement.Y = y;
        }


        public void DeleteElement(ImageViewElement element)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (element == elements[i])
                {
                    elements.RemoveAt(i);
                }
            }

            for (int i = 0; i < baseElements.Count; i++)
            {
                if (element == baseElements[i].Parent)
                {
                    baseElements.RemoveAt(i);
                }
            }


            this.InvalidateVisual();
        }

        #endregion

        #region 创建图形元素
        public void CreateElement(ElementType type)
        {
            CreateElement(type, "");
        }
        /// <summary>
        /// 在绘制前先创建好对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void CreateElement(ElementType type, string name)
        {
            switch (type)
            {
                case ElementType.Image:
                    break;
                case ElementType.Point:
                    break;
                case ElementType.Line:
                    this.drawingElement = new LineElement();
                    break;
                case ElementType.Rectangle:
                    this.drawingElement = new RectElement();
                    break;
                case ElementType.Polygon:
                    this.drawingElement = new PolygonElement();
                    break;
                //case ElementType.Ellipse:
                //    this.drawingElement = new EllipseElement();
                //    break;
                default:
                    break;
            }
            this.drawingElement.Name = name;
            this.drawingElement.Parent = this.imageElement;
            this.drawingElement.GlobalCoordinate = this.imageElement.Coordinate;
            this.MouseState = MouseState.Idle;
            this.ImageViewState = ImageViewState.Draw;
            this.DrawingElementType = type;
            this.drawingElement.Visible = false;
            this.drawingElement.IsComplete = false;
            this.drawingElement.Color = this.Color;
        }



        #endregion

        public void SelectElement(ImageViewElement element)
        {
            if (this.selectedElement != null && !(element is TractionPoint))
            {
                this.selectedElement.Selected = false;
                this.selectedElement = element;
                this.selectedElement.Selected = true;
                this.InvalidateVisual();
            }
            else
            {
                this.selectedElement = element;
                this.selectedElement.Selected = true;
                this.InvalidateVisual();
            }
            
        }

        private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Console.WriteLine(e.Delta + "  " + e.GetPosition(this));
            this.OnScale(e.GetPosition(this), this.imageElement.Scale * (e.Delta > 0 ? 0.8f : 1.25f));
        }


        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = Coordinate.CoordinateTransport(e.GetPosition(this), Coordinate.BaseCoornidate, imageElement.Coordinate);
            this.ImageMouseMove?.Invoke(this, new ImageMouseMoveEventArgs() { Location = p });
            ImageViewElement targetElement = this.GetTargetElement(p.X, p.Y);
            pointedElement = targetElement;
            if (ImageViewState == ImageViewState.Normal)
            {
                if (MouseState == MouseState.Operating)
                {
                    if (operatedElement != imageElement)
                    {
                        //operatedElement.Move(p);
                    }
                    else
                    {
                        AbsoluteMove(e.GetPosition(this).X - operationStartImagePoint.X * ImageScale, e.GetPosition(this).Y - operationStartImagePoint.Y * ImageScale);

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
                    if(drawingElement is KeyPointElement)
                    {
                        (drawingElement as KeyPointElement).AdjustNextKeyPoint(p);
                    }
                    
                    this.InvalidateVisual();
                }
            }
            if (ImageViewState == ImageViewState.Edit)
            {
                if (MouseState == MouseState.Operating)
                {
                    if (operatedElement != imageElement)
                    {
                        operatedElement.Move(this.operatedElementStartPoint.X + p.X - operationStartImagePoint.X, this.operatedElementStartPoint.Y + p.Y - operationStartImagePoint.Y);
                    }
                    else
                    {
                        AbsoluteMove(e.GetPosition(this).X - operationStartImagePoint.X * ImageScale, e.GetPosition(this).Y - operationStartImagePoint.Y * ImageScale);

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
            Point p = Coordinate.CoordinateTransport(e.GetPosition(this), Coordinate.BaseCoornidate, imageElement.Coordinate);
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
                this.operatedElement = targetElement;
                this.MouseState = MouseState.Operating;
                this.operationStartControlPoint = e.GetPosition(this);
                this.operationStartImagePoint = p;
            }
            if (ImageViewState == ImageViewState.Draw)
            {

                this.SelectElement(this.drawingElement);
                if (MouseState == MouseState.Idle)
                {
                    MouseState = MouseState.Operating;
                    drawingElement.Visible = true;
                }
                //如果添加了最后一个点
                if (drawingElement.AddKeyPoint(p))
                {
                    MouseState = MouseState.Idle;
                    AddElement(drawingElement);
                    this.ElementCreateEventHandler?.Invoke(drawingElement);//触发事件
                    if (this.ContinuousDraw)
                    {
                        CreateElement(this.DrawingElementType);
                    }
                    else
                    {
                        drawingElement = null;
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
                this.operatedElement = targetElement;
                this.MouseState = MouseState.Operating;
                this.operationStartControlPoint = e.GetPosition(this);
                this.operationStartImagePoint = p;
                this.operatedElementStartPoint = new Point(targetElement.X,targetElement.Y);
                System.Console.WriteLine("operationStartImagePoint:" + operationStartImagePoint.X + "  " + operationStartImagePoint.Y);
                this.SelectElement(operatedElement);
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
                if (operatedElement != null)
                {
                    operatedElement.ChangeDone();
                }
                MouseState = MouseState.Idle;
                operatedElement = null;
            }  
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)sender).Name)
            {
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
                    this.CurItem.DeleteElement(this.selectedElement);
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
                if (this.drawingElement is PolygonElement)
                {
                    e.Handled = true;
                    MouseState = MouseState.Idle;
                    if ((drawingElement as PolygonElement).Complete())
                    {
                        AddElement(drawingElement);
                        this.ElementCreateEventHandler?.Invoke(drawingElement);//触发事件
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
                        drawingElement = null;
                        this.ImageViewState = this.DefaultState;
                    }
                }
            }
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
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
