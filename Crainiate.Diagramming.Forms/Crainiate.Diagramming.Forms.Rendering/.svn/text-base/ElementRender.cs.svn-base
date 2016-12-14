// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class ElementRender: IFormsRenderer
    {
        public virtual void RenderElement(IRenderable renderable, Graphics graphics, Render render)
        {
            Element element = renderable as Element;
            GraphicsPath path = element.GetPath();
            if (path == null) return;
            Pen pen = null;

            if (element.CustomPen == null)
            {
                pen = new Pen(element.BorderColor, element.BorderWidth);
                pen.DashStyle = element.BorderStyle;

                //Check if winforms renderer and adjust color as required
                pen.Color = render.AdjustColor(element.BorderColor, element.BorderWidth, element.Opacity);
            }
            else
            {
                pen = element.CustomPen;
            }

            //Can throw an out of memory exception in System.Drawing
            try
            {
                graphics.SmoothingMode = element.SmoothingMode;
                graphics.DrawPath(pen, path);
            }
            catch
            {

            }
        }

        //Implement a base rendering of an element
        public virtual void RenderShadow(IRenderable renderable, Graphics graphics, Render render)
        {
            Element element = renderable as Element;
            GraphicsPath path = element.GetPath();
            Layer layer = element.Layer;
            if (path == null) return;
            if (element.Layer == null) return;

            Pen shadowPen = new Pen(render.AdjustColor(layer.ShadowColor, element.BorderWidth, element.Opacity));

            graphics.TranslateTransform(layer.ShadowOffset.X, layer.ShadowOffset.Y);

            if (layer.SoftShadows)
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawPath(shadowPen, path);
                graphics.CompositingQuality = render.CompositingQuality;
                graphics.SmoothingMode = element.SmoothingMode;
            }
            else
            {
                graphics.DrawPath(shadowPen, path);
            }

            //Restore graphics
            graphics.TranslateTransform(-layer.ShadowOffset.X, -layer.ShadowOffset.Y);
        }

        //Implement a base rendering of an element selection
        public virtual void RenderSelection(IRenderable element, Graphics graphics, ControlRender render)
        {
            
        }

        //Implement a base rendering of an element selection
        public virtual void RenderAction(IRenderable renderable, Graphics graphics, ControlRender render)
        {
            Element element = renderable as Element;
            if (render.ActionStyle == ActionStyle.Default)
            {
                RenderElement(renderable, graphics, render);
            }
            else
            {
                GraphicsPath path = element.GetPath();
                if (path == null) return;
                graphics.DrawPath(Singleton.Instance.ActionPen, path);
            }
        }

        //Implement a base rendering of an element selection
        public virtual void RenderHighlight(IRenderable renderable, Graphics graphics, ControlRender render)
        {
            Element element = renderable as Element;
            GraphicsPath path = element.GetPath();
            if (path == null) return;

            //graphics.FillPath(Component.Instance.HighlightBrush,GetPath());
            graphics.DrawPath(Singleton.Instance.HighlightPen, path);
        }
    }
}
