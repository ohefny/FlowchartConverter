// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class ConnectorRender: LinkRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            Connector connector = element as Connector;

            base.RenderElement(element, graphics, render);

            //Render Image
            if (connector.Image != null || connector.Label != null)
            {
                //Determine central point
                //If even number of points, then position half way on segment
                PointF center = connector.Center;

                graphics.TranslateTransform(center.X, center.Y);

                if (connector.Image != null)
                {
                    IFormsRenderer renderer = render.GetRenderer(connector.Image);
                    renderer.RenderElement(connector.Image, graphics, render);
                }

                if (connector.Label != null)
                {
                    IFormsRenderer renderer = render.GetRenderer(connector.Label);
                    renderer.RenderElement(connector.Label, graphics, render);
                }
                
                graphics.TranslateTransform(-center.X, -center.Y);
            }

            //Draw test squares for points
            //			if (Selected)
            //			{
            //				graphics.TranslateTransform(-Rectangle.X,-Rectangle.Y);
            //				Pen newPen = new Pen(Color.Red,1);
            //
            //				Font font = new Font("Arial",8);
            //				SolidBrush brush = new SolidBrush(Color.Blue);
            //				int offset = 0;
            //
            //				foreach (PointF point in Points)
            //				{
            //					offset+=20;
            //					graphics.DrawRectangle(newPen,point.X-2,point.Y-2,4,4);
            //					graphics.DrawString(point.ToString(),font,brush,10,offset);
            //					
            //				}	
            //				graphics.TranslateTransform(Rectangle.X,Rectangle.Y);
            //			}

        }

    }
}
