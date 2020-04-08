using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Ncer.UI
{
    public class ImageElement : ImageViewElement
    {
        private ImageSource image;
        private double imageDisplayScale = 1.0;
        public ImageSource Image
        {
            get { return image; }

            set { this.image = value; this.Width = (Image == null) ? 0 : image.Width; this.Height = (Image == null) ? 0 : image.Height; }
        }

        public double ImageDisplayScale { get => imageDisplayScale; set => imageDisplayScale = value; }

        public ImageElement() : base()
        {

        }
        public void FitToWindow(double w, double h)
        {
            this.Scale = Math.Min(h / image.Height, w / image.Width);
            this.X = Math.Max(0, (w - image.Width * this.Scale) / 2);
            this.Y = Math.Max(0, (h - image.Height * this.Scale) / 2);
        }

        public void ScaleImage(System.Windows.Point anchor, double scale)
        {
            Point imageAnchor = Coordinate.CoordinateTransport(anchor, Coordinate.BaseCoornidate, this.Coordinate);
            this.X = anchor.X - scale * imageAnchor.X;
            this.Y = anchor.Y - scale * imageAnchor.Y;
            this.Scale = scale;
        }

        public override void Drawing(DrawingContext drawingContext)
        {
            //base.Drawing(drawingContext);
            if (this.image != null)
            {
                drawingContext.DrawImage(this.image, new Rect(this.X, this.Y, image.Width * this.Scale * ImageDisplayScale, image.Height * this.Scale * ImageDisplayScale));
            }

        }

        public override void OnElementChange(ImageViewElement element)
        {
            throw new NotImplementedException();
        }

        public override void BeSelect(bool b)
        {
            throw new NotImplementedException();
        }

        public override bool IsIn(double x, double y)
        {
            throw new NotImplementedException();
        }
    }
}
