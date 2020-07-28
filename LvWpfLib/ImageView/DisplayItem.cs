using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace Ncer.UI
{
    public class DisplayItem
    {
        #region event
        public delegate void ElementDeleteEvent(ImageViewElement element);
        public event ElementDeleteEvent ElementDeleteEventHandler;
        #endregion



        #region member
        private Object item=null;
        private List<ImageViewElement> elements = new List<ImageViewElement>();
        private List<ImageViewElement> baseElements = new List<ImageViewElement>();
        private ImageElement imageElement;
        private ImageViewElement operatedElement;
        private KeyPointElement drawingElement;
        private ImageViewElement selectedElement;
        private ImageViewElement pointedElement;
        private System.Windows.Point operationStartControlPoint;
        private System.Windows.Point operationStartImagePoint;
        private Point operatedElementStartPoint;//被操作的element的初始位置

        #endregion
        #region params
        public ImageSource Image { get { return ImageElement.Image; } set { ImageElement.Image = value; } }
        /// <summary>
        /// DisplayItem的关联对象
        /// </summary>
        public object Item { get => item; set => item = value; }
        public string Name { get; set; }
        public List<ImageViewElement> Elements { get => elements; set => elements = value; }
        public List<ImageViewElement> BaseElements { get => baseElements; set => baseElements = value; }
        public ImageElement ImageElement { get => imageElement; set => imageElement = value; }
        public ImageViewElement OperatedElement { get => operatedElement; set => operatedElement = value; }
        public KeyPointElement DrawingElement { get => drawingElement; set => drawingElement = value; }
        public ImageViewElement SelectedElement { get => selectedElement; set => selectedElement = value; }
        public ImageViewElement PointedElement { get => pointedElement; set => pointedElement = value; }
        public Point OperationStartControlPoint { get => operationStartControlPoint; set => operationStartControlPoint = value; }
        public Point OperationStartImagePoint { get => operationStartImagePoint; set => operationStartImagePoint = value; }
        public Point OperatedElementStartPoint { get => operatedElementStartPoint; set => operatedElementStartPoint = value; }

        #endregion

        #region Constructor
        public DisplayItem(string name)
        {
            this.Name = name;
            this.ImageElement = new ImageElement();
        }

        public DisplayItem(string name,Object item,ImageSource image)
        {
            this.ImageElement = new ImageElement();
            this.Name = name;
            this.Item = item;
            this.Image = image;
        }

        #endregion

        #region elements

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

        public void AddElement(InteractionImageElement element)
        {
            this.Elements.Add(element);
            if (element.Parent == null)
            {
                element.Parent = ImageElement;
            }
        }

        public void AddPoint(PointElement point)
        {
            if (point.Parent == null) point.Parent = ImageElement;
            Elements.Add(point);
            BaseElements.Add(point);
            for (int i = 0; i < point.keyPointList.Count; i++)
            {
                BaseElements.Add(point.keyPointList[i].TractionPoint);
            }
            BaseElements.Sort();
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
        }
        public void DeleteElement(ImageViewElement element)
        {

            if (Elements.Contains(element))
            {
                Elements.Remove(element);   
            }


            for (int i = 0; i < BaseElements.Count; i++)
            {
                if (element == BaseElements[i].Parent)
                {
                    BaseElements.RemoveAt(i);
                }
            }
            element.Delete();
        }

        public void SelectElement(ImageViewElement element)
        {
            if (this.SelectedElement != null && !(element is TractionPoint))
            {
                this.SelectedElement.Selected = false;
                this.SelectedElement = element;
                this.SelectedElement.Selected = true;
            }
            else
            {
                this.SelectedElement = element;
                this.SelectedElement.Selected = true;
            }
        }

        public ImageViewElement GetElement(string name)
        {
            foreach (var item in this.Elements)
            {
                if (!string.IsNullOrWhiteSpace(item.Name) && item.Name == name)
                {
                    return item;
                }
            }

            return null;
        }

        public void DeleteAllElement()
        {
            //for (int i = 0; i < this.elements.Count; i++)
            //{
            //    this.DeleteElement(elements[i]);
            //}
            foreach(var item in this.Elements)
            {
                item.Delete();
            }
            Elements.Clear();
            BaseElements.Clear();
        }

        #endregion
    }

}
