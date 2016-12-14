// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class TableRender: ShapeRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            Table table = element as Table;
            RenderTable(table, graphics, render);
            RenderExpand(table, graphics, render);

            //Render the ports
            if (table.Ports != null)
            {
                foreach (Port port in table.Ports.Values)
                {
                    graphics.TranslateTransform(-table.Bounds.X + port.Bounds.X, -table.Bounds.Y + port.Bounds.Y);
                    port.SuspendValidation();

                    IFormsRenderer renderer = render.GetRenderer(port);
                    renderer.RenderElement(port, graphics, render);
                    
                    port.ResumeValidation();
                    graphics.TranslateTransform(table.Bounds.X - port.Bounds.X, table.Bounds.Y - port.Bounds.Y);
                }
            }
        }

        //Implement a base rendering of an element selection
        public override void RenderAction(IRenderable element, Graphics graphics, ControlRender render)
        {
            Table table = element as Table;
            if (render.ActionStyle == ActionStyle.Default)
            {
                RenderTable(table, graphics, render);

                //Render the ports
                if (table.Ports != null)
                {
                    foreach (Port port in table.Ports.Values)
                    {
                        if (port.Visible)
                        {
                            graphics.TranslateTransform(-port.Bounds.X + port.Bounds.X, -port.Bounds.Y + port.Bounds.Y);
                            port.SuspendValidation();

                            IFormsRenderer renderer = render.GetRenderer(port);
                            renderer.RenderElement(port, graphics, render);
                            
                            port.ResumeValidation();
                            graphics.TranslateTransform(port.Bounds.X - port.Bounds.X, port.Bounds.Y - port.Bounds.Y);
                        }
                    }
                }
            }
            else
            {
                base.RenderAction(element, graphics, render);
            }
        }

        private void RenderExpand(Table table, Graphics graphics, Render render)
        {
            //Obtain a reference to IExpandable interface
            IExpandable expand = (IExpandable) table;

            if (!expand.DrawExpand) return;

            //Draw the expander
            Pen pen = new Pen(Color.FromArgb(128, Color.Gray), 1);
            SolidBrush brush = new SolidBrush(Color.White);

            //Set up the expand path
            GraphicsPath expandPath = new GraphicsPath();
            expandPath.AddEllipse(table.Width - 20, 5, 14, 14);

            SmoothingMode smoothing = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.FillPath(brush, expandPath); //Fill Circle
            graphics.DrawPath(pen, expandPath); //Outline

            pen.Color = Color.FromArgb(128, Color.Navy);
            pen.Width = 2;
            PointF[] points;

            if (expand.Expanded)
            {
                points = new PointF[] { new PointF(table.Width - 16, 11), new PointF(table.Width - 13, 8), new PointF(table.Width - 10, 11) };
            }
            else
            {
                points = new PointF[] { new PointF(table.Width - 16, 8), new PointF(table.Width - 13, 11), new PointF(table.Width - 10, 8) };
            }
            graphics.DrawLines(pen, points);
            points[0].Y += 5;
            points[1].Y += 5;
            points[2].Y += 5;
            graphics.DrawLines(pen, points);
            graphics.SmoothingMode = smoothing;
        }

        private void RenderTable(Table table, Graphics graphics, Render render)
        {
            GraphicsPath path = table.GetPath();
            if (path == null) return;

            //Draw background
            Color backColor = render.AdjustColor(table.BackColor, 0, table.Opacity);
            Color gradientColor = render.AdjustColor(table.GradientColor, 0, table.Opacity);
            SolidBrush brush = new SolidBrush(backColor);
            graphics.FillPath(brush, path);

            Region current = graphics.Clip;
            Region region = new Region(path);
            graphics.SetClip(region, CombineMode.Intersect);

            //Draw Heading
            RectangleF headingRect = new RectangleF(0, 0, table.Width, table.HeadingHeight);
            LinearGradientBrush gradient = new LinearGradientBrush(headingRect, gradientColor, backColor, LinearGradientMode.Horizontal);
            graphics.FillRectangle(gradient, headingRect);

            //Draw Heading text
            brush.Color = render.AdjustColor(table.Forecolor, 1, table.Opacity);
            graphics.DrawString(table.Heading, Singleton.Instance.GetFont(table.FontName, table.FontSize, FontStyle.Bold), brush, 8, 5);
            graphics.DrawString(table.SubHeading, Singleton.Instance.GetFont(table.FontName, table.FontSize - 1, FontStyle.Regular), brush, 8, 20);

            if (table.Expanded)
            {
                float iHeight = table.HeadingHeight;

                //Draw the top level rows (if any)
                if (table.Rows.Count > 0)
                {
                    brush.Color = table.GradientColor;
                    graphics.FillRectangle(brush, 0, iHeight, table.Indent, 1);
                    iHeight += 1;

                    RenderTableRows(table, graphics, render, table.Rows, ref iHeight);
                }

                if (table.Groups.Count > 0)
                {
                    foreach (TableGroup tableGroup in table.Groups)
                    {
                        iHeight += 1;

                        IFormsRenderer renderer = render.GetRenderer(tableGroup);
                        renderer.RenderElement(tableGroup, graphics, render);
                        iHeight += table.RowHeight;

                        if (tableGroup.Groups.Count > 0 && tableGroup.Expanded) RenderTableGroups(table, graphics, render, tableGroup.Groups, ref iHeight);
                        if (tableGroup.Rows.Count > 0 && tableGroup.Expanded) RenderTableRows(table, graphics, render, tableGroup.Rows, ref iHeight);
                    }
                }

                //Render highlight (if any)
                if (table.DrawSelectedItem && table.SelectedItem != null && render is ControlRender)
                {
                    IFormsRenderer renderer = render.GetRenderer(table.SelectedItem);
                    renderer.RenderSelection(table.SelectedItem, graphics, render as ControlRender);
                }
            }

            graphics.Clip = current;

            //Draw outline
            Pen pen;
            if (table.CustomPen == null)
            {
                pen = new Pen(table.BorderColor, table.BorderWidth);
                pen.DashStyle = table.BorderStyle;

                //Check if winforms renderer and adjust color as required
                pen.Color = render.AdjustColor(table.BorderColor, table.BorderWidth, table.Opacity);
            }
            else
            {
                pen = table.CustomPen;
            }
            graphics.DrawPath(pen, path);
        }

        private void RenderTableGroups(Table table, Graphics graphics, Render render, TableGroups groups, ref float iHeight)
        {
            foreach (TableGroup tableGroup in groups)
            {
                IFormsRenderer renderer = render.GetRenderer(tableGroup);
                renderer.RenderElement(tableGroup, graphics, render);
                
                iHeight += table.GroupHeight;

                //Render groups and rows recursively
                if (tableGroup.Groups.Count > 0 && tableGroup.Expanded) RenderTableGroups(table, graphics, render, tableGroup.Groups, ref iHeight);
                if (tableGroup.Rows.Count > 0 && tableGroup.Expanded) RenderTableRows(table, graphics, render, tableGroup.Rows, ref iHeight);
            }
        }

        private void RenderTableRows(Table table, Graphics graphics, Render render, TableRows rows, ref float iHeight)
        {
            foreach (TableRow tableRow in rows)
            {
                IFormsRenderer renderer = render.GetRenderer(tableRow);
                renderer.RenderElement(tableRow, graphics, render);

                iHeight += table.RowHeight;
            }
        }

    }
}
