using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ncer.UI
{
    public class InteractionImageElement : ImageViewElement
    {
        private ImageSource image;

        public InteractionImageElement()
        {
            this.Z = 1.01;
            this.ElementCursor = Cursors.Hand;
            this.Focusable = false;
        }

        public ImageSource Image { get => image; set => image = value; }

        public override void BeSelect(bool b)
        {
            
        }

        public override void Drawing(DrawingContext drawingContext)
        {
            if (image == null) return;
            Point loca = Coordinate.CoordinateTransport(new Point(X, Y), this.Parent.Coordinate, Coordinate.BaseCoornidate);
            Point loca2 = Coordinate.CoordinateTransport(new Point(X+Width, Y+Height), this.Parent.Coordinate, Coordinate.BaseCoornidate);
            drawingContext.DrawImage(image, new System.Windows.Rect(loca, loca2));
        }

        public override bool IsIn(double x, double y)
        {
            return x > this.X && x < this.X + Width && y > this.Y && y < this.Y + Height;
        }
    }
}
