// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class ImageRender: IFormsRenderer
    {
        public virtual void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            Image image = element as Image;
            if (image.Bitmap != null)
            {
                Size size = image.Bitmap.Size;
                Point location = Point.Round(image.Location);

                graphics.TranslateTransform(image.Location.X, image.Location.Y);
                graphics.InterpolationMode = image.InterpolationMode;
                graphics.DrawImage(image.Bitmap, location.X, location.Y, size.Width, size.Height);
                graphics.TranslateTransform(-image.Location.X, -image.Location.Y);
            }
        }

        //Implement a base rendering of an element
        public virtual void RenderShadow(IRenderable element, Graphics graphics, Render render)
        {

        }

        //Implement a base rendering of an element selection
        public virtual void RenderSelection(IRenderable element, Graphics graphics, ControlRender render)
        {
            
        }

        //Implement a base rendering of an element selection
        public virtual void RenderAction(IRenderable element, Graphics graphics, ControlRender render)
        {
        }

        //Implement a base rendering of an element selection
        public virtual void RenderHighlight(IRenderable element, Graphics graphics, ControlRender render)
        {

        }
       
    }
}
