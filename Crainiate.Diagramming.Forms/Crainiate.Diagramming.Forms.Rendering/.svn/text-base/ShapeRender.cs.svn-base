// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class ShapeRender: SolidRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            //Render this shape
            base.RenderElement(element, graphics, render);

            Shape shape = element as Shape;

            //Undo the rotate transform
            if (shape.Rotation != 0)
            {
                //Get the local center
                PointF center = new PointF(shape.Bounds.Width / 2, shape.Bounds.Height / 2);
                Matrix matrix = graphics.Transform;
                matrix.RotateAt(-shape.Rotation, center);

                graphics.Transform = matrix;
            }

            //Render the ports
            if (shape.Ports != null && !render.Summary)
            {
                foreach (Port port in shape.Ports.Values)
                {
                    if (port.Visible)
                    {
                        graphics.TranslateTransform(-shape.Bounds.X + port.Bounds.X, -shape.Bounds.Y + port.Bounds.Y);
                        graphics.RotateTransform(port.Rotation);
                        port.SuspendValidation();

                        IFormsRenderer renderer = render.GetRenderer(port);
                        renderer.RenderElement(port, graphics, render);

                        port.ResumeValidation();
                        graphics.RotateTransform(-port.Rotation);
                        graphics.TranslateTransform(shape.Bounds.X - port.Bounds.X, shape.Bounds.Y - port.Bounds.Y);
                    }
                }
            }

            //Redo the rotate transform
            if (shape.Rotation != 0)
            {
                //Get the local center
                PointF center = new PointF(shape.Bounds.Width / 2, shape.Bounds.Height / 2);
                Matrix matrix = graphics.Transform;
                matrix.RotateAt(shape.Rotation, center);

                graphics.Transform = matrix;
            }
        }

        //Implement a base rendering of an element selection
        public override void RenderSelection(IRenderable element, Graphics graphics, ControlRender render)
        {
            Shape shape = element as Shape;
            if (shape.Handles == null) return;

            RectangleF boundRect = shape.TransformRectangle;
            float boundsSize = 4 * render.ZoomFactor;
            float handleSize = 6 * render.ZoomFactor;
            boundRect.Inflate(boundsSize, boundsSize);

            graphics.SmoothingMode = SmoothingMode.None;

            SolidBrush brush = new SolidBrush(Color.White);
            SolidBrush fillBrush = (shape.Group == null) ? Singleton.Instance.SelectionBrush : Singleton.Instance.GroupBrush;
            Pen pen = Singleton.Instance.SelectionHatchPen;
            graphics.DrawRectangle(pen, -boundsSize, -boundsSize, boundRect.Width, boundRect.Height);

            foreach (Handle handle in shape.Handles)
            {
                if (handle.Type == HandleType.Rotate)
                {
                    graphics.FillPath(brush, handle.Path);
                    graphics.FillPath(Singleton.Instance.SelectionRotateBrush, handle.Path);
                    graphics.DrawPath(Singleton.Instance.SelectionRotatePen, handle.Path);
                }
                else
                {
                    graphics.FillPath(brush, handle.Path);
                    graphics.FillPath(fillBrush, handle.Path);
                    graphics.DrawPath(Singleton.Instance.SelectionPen, handle.Path);
                }
            }

            graphics.SmoothingMode = shape.SmoothingMode;
        }

        public override void RenderAction(IRenderable element, Graphics graphics, ControlRender render)
        {
            base.RenderAction(element, graphics, render);

            //Render the ports
            //if (Ports != null && renderDesign.ActionStyle == ActionStyle.Default)
            //{
            //    foreach (Port port in Ports.Values)
            //    {
            //        if (port.Visible)
            //        {
            //            graphics.TranslateTransform(-Rectangle.X + port.Rectangle.X,-Rectangle.Y + port.Rectangle.Y);
            //            port.SuspendValidation();
            //            port.Render(graphics,render);
            //            port.ResumeValidation();
            //            graphics.TranslateTransform(Rectangle.X - port.Rectangle.X,Rectangle.Y - port.Rectangle.Y);						
            //        }
            //    }
            //}
        }
    }
}
