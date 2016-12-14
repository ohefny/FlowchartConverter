// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class TableRowRender: TableItemRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            TableRow row = element as TableRow;
            Table table = row.Table;
            byte opacity = 100;
            if (table != null) opacity = table.Opacity;

            //Draw indent
            SolidBrush brush = new SolidBrush(render.AdjustColor(row.Backcolor, 1, opacity));
            brush.Color = Color.FromArgb(brush.Color.A / 2, brush.Color);
            graphics.FillRectangle(brush, 0, row.Rectangle.Top, row.Indent, row.Rectangle.Height);

            //Draw image
            float imageWidth = 0;
            if (row.Image != null)
            {
                System.Drawing.Image bitmap = row.Image.Bitmap;

                //Work out position of image
                float imageTop = (row.Rectangle.Height - bitmap.Height) / 2;
                if (imageTop < 0) imageTop = 0;

                imageWidth = bitmap.Width;
                graphics.DrawImageUnscaled(bitmap, Convert.ToInt32(row.Indent), Convert.ToInt32(row.Rectangle.Top + imageTop));
            }

            //Draw text
            RectangleF textRectangle = new RectangleF(row.Indent + imageWidth + 4, row.Rectangle.Top, row.Rectangle.Width - row.Indent - 4, row.Rectangle.Height);
            brush = new SolidBrush(render.AdjustColor(row.Forecolor, 1, opacity));
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.FormatFlags = StringFormatFlags.NoWrap;
            graphics.DrawString(row.Text, Singleton.Instance.GetFont(row.FontName, row.FontSize, row.FontStyle), brush, textRectangle, format);
        }

        public override void RenderSelection(IRenderable element, Graphics graphics, ControlRender render)
        {
            TableRow row = element as TableRow;
            SmoothingMode mode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.None;

            Pen pen = Singleton.Instance.SelectionPen;
            SolidBrush brush = Singleton.Instance.SelectionBrush;
            RectangleF rect = new RectangleF(row.Rectangle.X + 2, row.Rectangle.Y, row.Rectangle.Width - 4, row.Rectangle.Height);
            graphics.FillRectangle(brush, rect);
            graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

            graphics.SmoothingMode = mode;
        }
    }
}
