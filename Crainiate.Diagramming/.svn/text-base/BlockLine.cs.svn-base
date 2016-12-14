// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming
{
    public class BlockLine: Link
    {
        public virtual GraphicsPath GetBlockPath()
        {
            PointF start = (PointF) Points[0];
            PointF end = (PointF) Points[1];

            GraphicsPath path = new GraphicsPath();
            RectangleF bounds = Geometry.CreateRectangle(start, end);

            //Draw the arrow using a 100x100 grid 
            path.AddLine(0, 30, 60, 30);
            path.AddLine(60, 30, 60, 0);
            path.AddLine(60, 0, 100, 50);
            path.AddLine(100, 50, 60, 100);
            path.AddLine(60, 100, 60, 70);
            path.AddLine(60, 70, 0, 70);

            //Close the figure
            path.CloseFigure();

            //Move the path 50 pixels up
            Matrix matrix = new Matrix();
            matrix.Translate(0, -50);
            path.Transform(matrix);

            //Stretch the path out along the x axis
            matrix = new Matrix();
            //Scale the path to the distance between p0 and p1
            float distance = Convert.ToSingle(Math.Sqrt(Math.Pow(bounds.Width, 2) + Math.Pow(bounds.Height, 2)));
            matrix.Scale(distance / 100, 1);
            path.Transform(matrix);

            //Move the path to the centre
            matrix = new Matrix();
            RectangleF newBounds = path.GetBounds();
            matrix.Translate((bounds.Width - newBounds.Width) / 2, bounds.Height / 2);
            path.Transform(matrix);

            //Rotate the path around the mid point
            matrix = new Matrix();
            float degrees = Convert.ToSingle(Geometry.DegreesFromRadians(Geometry.GetAngle(start, end)));
            PointF center = new PointF(bounds.Width / 2, bounds.Height / 2);
            matrix.RotateAt(degrees, center);

            path.Transform(matrix);

            return path;
        }

    }
}
