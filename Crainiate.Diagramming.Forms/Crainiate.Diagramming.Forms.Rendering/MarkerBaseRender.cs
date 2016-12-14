// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public abstract class MarkerBaseRender: ElementRender
    {
        //fill the marker background
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            MarkerBase markerBase = element as MarkerBase;

            graphics.SmoothingMode = markerBase.SmoothingMode;
            if (markerBase.DrawBackground)
            {
                SolidBrush brush;
                brush = new SolidBrush(render.AdjustColor(markerBase.BackColor, 0, markerBase.Opacity));
                graphics.FillPath(brush, markerBase.GetPath());
            }

            base.RenderElement(element, graphics, render);

            //Draw any images and annotations
            if (markerBase.Image != null)
            {
                IFormsRenderer renderer = render.GetRenderer(markerBase.Image);
                renderer.RenderElement(markerBase.Image, graphics, render);
            }
            if (markerBase.Label != null)
            {
                IFormsRenderer renderer = render.GetRenderer(markerBase.Image);
                renderer.RenderElement(markerBase.Label, graphics, render);
            }
        }

        //fill the marker background
        public override void RenderShadow(IRenderable element, Graphics graphics, Render render)
        {
            MarkerBase markerBase = element as MarkerBase;
            Layer layer = render.CurrentLayer;
            Color shadowColor = render.AdjustColor(layer.ShadowColor, markerBase.BorderWidth, markerBase.Opacity);

            if (markerBase.DrawBackground)
            {
                SolidBrush brush = new SolidBrush(shadowColor);

                //Draw soft shadows
                if (layer.SoftShadows)
                {
                    shadowColor = Color.FromArgb(10, shadowColor);
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                }

                graphics.FillPath(brush, markerBase.GetPath());

                if (layer.SoftShadows)
                {
                    graphics.CompositingQuality = render.CompositingQuality;
                    graphics.SmoothingMode = markerBase.SmoothingMode;
                }
            }

            base.RenderShadow(element, graphics, render);
        }
    }
}
