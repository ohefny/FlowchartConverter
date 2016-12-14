// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class TableGroupRender: TableItemRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            TableGroup group = element as TableGroup;
            Table table = group.Table;
            byte opacity = 100;
            if (table != null) opacity = table.Opacity;

            //Draw Background
            SolidBrush brush = new SolidBrush(render.AdjustColor(group.Backcolor, 1, opacity));
            brush.Color = Color.FromArgb(brush.Color.A / 2, brush.Color);

            graphics.FillRectangle(brush, group.Rectangle);

            //Draw plus or minus rectangle
            SmoothingMode smoothing = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            RectangleF expander = new RectangleF(4 + group.Indent, group.Rectangle.Top + 4, 11, 11);
            LinearGradientBrush gradientBrush = new LinearGradientBrush(expander, Color.FromArgb(255, Color.White), Color.FromArgb(255, 166, 176, 185), LinearGradientMode.Vertical);
            graphics.FillRectangle(gradientBrush, expander); // internal
            Pen pen = new Pen(Color.FromArgb(128, Color.Gray), 1);
            graphics.DrawRectangle(pen, expander.X, expander.Y, expander.Width, expander.Height); //border
            pen.Color = Color.FromArgb(128, Color.Black);
            pen.Width = 2;
            graphics.DrawLine(pen, expander.X + 2, expander.Y + 5.5F, expander.X + 9, expander.Y + 5.5F); //minus
            if (!group.Expanded) graphics.DrawLine(pen, expander.X + 5.5F, expander.Y + 2, expander.X + 5.5F, expander.Y + 9); //plus
            graphics.SmoothingMode = smoothing;

            //Draw text
            RectangleF textRectangle = new RectangleF(20 + group.Indent, group.Rectangle.Top, group.Rectangle.Width - 20 - group.Indent, group.Rectangle.Height);
            brush = new SolidBrush(render.AdjustColor(group.Forecolor, 1, opacity));
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.FormatFlags = StringFormatFlags.NoWrap;
            graphics.DrawString(group.Text, Singleton.Instance.GetFont(group.FontName, group.FontSize, group.FontStyle), brush, textRectangle, format);

            //Draw indent
            //brush = new SolidBrush(render.AdjustColor(Backcolor,1,opacity));
            //brush.Color = Color.FromArgb(brush.Color.A /2, brush.Color);
            //graphics.FillRectangle(brush,0,Rectangle.Top,Indent,Rectangle.Height);
        }

        public override void RenderSelection(IRenderable element, Graphics graphics, ControlRender render)
        {
            TableGroup group = element as TableGroup;
            SmoothingMode mode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.None;

            Pen pen = Singleton.Instance.SelectionPen;
            RectangleF rect = new RectangleF(group.Rectangle.X + 2, group.Rectangle.Y, group.Rectangle.Width - 4, group.Rectangle.Height);
            graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

            graphics.SmoothingMode = mode;
        }
    }
}
