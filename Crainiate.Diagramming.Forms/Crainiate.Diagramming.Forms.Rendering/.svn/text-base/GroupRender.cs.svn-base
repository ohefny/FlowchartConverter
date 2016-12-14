// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class GroupRender: ShapeRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            base.RenderElement(element, graphics, render);

            //Draw the expander
            RenderExpand(element as Group, graphics, render);
        }

        public override void RenderAction(IRenderable element, Graphics graphics, ControlRender render)
        {
            base.RenderAction(element, graphics, render);
        }

        private void RenderExpand(Group group, Graphics graphics, Render render)
        {
            //Obtain a reference to IExpandable interface
            IExpandable expand = (IExpandable) group;

            if (!expand.DrawExpand) return;

            //Draw the expander
            Pen pen = new Pen(Color.FromArgb(128, Color.Gray), 1);
            SolidBrush brush = new SolidBrush(Color.White);

            //Set up the expand path
            GraphicsPath expandPath = new GraphicsPath();
            expandPath.AddEllipse(group.Width - 20, 5, 14, 14);

            SmoothingMode smoothing = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.FillPath(brush, expandPath); //Fill Circle
            graphics.DrawPath(pen, expandPath); //Outline

            pen.Color = Color.FromArgb(128, Color.Navy);
            pen.Width = 2;
            PointF[] points;

            if (expand.Expanded)
            {
                points = new PointF[] { new PointF(group.Width - 16, 11), new PointF(group.Width - 13, 8), new PointF(group.Width - 10, 11) };
            }
            else
            {
                points = new PointF[] { new PointF(group.Width - 16, 8), new PointF(group.Width - 13, 11), new PointF(group.Width - 10, 8) };
            }
            graphics.DrawLines(pen, points);
            points[0].Y += 5;
            points[1].Y += 5;
            points[2].Y += 5;
            graphics.DrawLines(pen, points);
            graphics.SmoothingMode = smoothing;
        }
    }
}
