// (c) Copyright Crainiate Software 2010




using System.Drawing;
using System.Drawing.Drawing2D;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class PortRender: SolidRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            Port port = element as Port;
            if (port.GetPath() == null) return;

            //Translate by offset
            graphics.TranslateTransform(port.Offset.X, port.Offset.Y);

            //Fill and draw the port
            RenderPort(port, graphics, render);

            //Render image
            if (port.Image != null)
            {
                IFormsRenderer renderer = render.GetRenderer(port.Image);
                renderer.RenderElement(port.Image, graphics, render);
            }

            //Render label
            if (port.Label != null) 
            {
                IFormsRenderer renderer = render.GetRenderer(port.Label);
                renderer.RenderElement(port.Label, graphics, render);
            }
            graphics.TranslateTransform(-port.Offset.X, -port.Offset.Y);
        }

        public override void RenderAction(IRenderable element, Graphics graphics, ControlRender render)
        {
            base.RenderAction(element, graphics, render);
        }

        public override void RenderHighlight(IRenderable element, Graphics graphics, ControlRender render)
        {
            Port port = element as Port;

            graphics.TranslateTransform(port.Offset.X, port.Offset.Y);
            base.RenderHighlight(element, graphics, render);
            graphics.TranslateTransform(-port.Offset.X, -port.Offset.Y);
        }

        private void RenderPort(Port port, Graphics graphics, Render render)
        {
            GraphicsPath path = port.GetPath();

            //Create a brush if no custom brush defined
            if (port.DrawBackground)
            {
                if (port.CustomBrush == null)
                {
                    //Use a linear gradient brush if gradient requested
                    if (port.DrawGradient)
                    {
                        LinearGradientBrush brush;
                        brush = new LinearGradientBrush(new RectangleF(0, 0, port.Bounds.Width, port.Bounds.Height), render.AdjustColor(port.BackColor, 0, port.Opacity), render.AdjustColor(port.GradientColor, 0, port.Opacity), port.GradientMode);
                        brush.GammaCorrection = true;
                        graphics.FillPath(brush, path);
                    }
                    //Draw normal solid brush
                    else
                    {
                        SolidBrush brush;
                        brush = new SolidBrush(render.AdjustColor(port.BackColor, 0, port.Opacity));
                        graphics.FillPath(brush, path);
                    }
                }
                else
                {
                    graphics.FillPath(port.CustomBrush, path);
                }
            }

            Pen pen = null;

            if (port.CustomPen == null)
            {
                pen = new Pen(port.BorderColor, port.BorderWidth);
                pen.DashStyle = port.BorderStyle;

                //Check if winforms renderer and adjust color as required
                pen.Color = render.AdjustColor(port.BorderColor, port.BorderWidth, port.Opacity);
            }
            else
            {
                pen = port.CustomPen;
            }

            graphics.SmoothingMode = port.SmoothingMode;
            graphics.DrawPath(pen, path);

            //Render internal rectangle
            //Pen tempPen = new Pen(Color.Red,2);
            //graphics.DrawRectangle(tempPen,_internalRectangle.X,_internalRectangle.Y,_internalRectangle.Width,_internalRectangle.Height);
        }

    }
}
