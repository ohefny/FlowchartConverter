// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class TreeLineRender: ElementRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            TreeLine line = element as TreeLine;

            if (line.Points == null || line.Points.Count < 2) return;

            base.RenderElement(element, graphics, render);
        }

        //Implement a base rendering of an element
        public override void RenderShadow(IRenderable element, Graphics graphics, Render render)
        {
            TreeLine line = element as TreeLine;

            if (line.Points == null) return;
            if (line.Points.Count < 2) return;

            PointF startLocation = (PointF) line.Points[0];
            PointF startReference = (PointF) line.Points[1];
            PointF endLocation = (PointF) line.Points[line.Points.Count - 1];
            PointF endReference = (PointF) line.Points[line.Points.Count - 2];

            Layer layer = line.Layer;
            Pen shadowPen = new Pen(layer.ShadowColor);
            GraphicsPath shadowPath = line.GetPath();
            shadowPen.Color = render.AdjustColor(layer.ShadowColor, 0, line.Opacity);

            //Save the current region
            Region current = graphics.Clip;

            graphics.TranslateTransform(layer.ShadowOffset.X, layer.ShadowOffset.Y);

            //Draw line
            if (layer.SoftShadows)
            {
                shadowPen.Color = Color.FromArgb(20, shadowPen.Color);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
            }

            graphics.DrawPath(shadowPen, shadowPath);

            if (layer.SoftShadows)
            {
                graphics.CompositingQuality = render.CompositingQuality;
                graphics.SmoothingMode = line.SmoothingMode;
            }

            graphics.TranslateTransform(-layer.ShadowOffset.X, -layer.ShadowOffset.Y);
        }

        public override void RenderAction(IRenderable element, Graphics graphics, ControlRender render)
        {
            TreeLine line = element as TreeLine;

            if (line.Points == null || line.Points.Count < 2) return;

            PointF startLocation = (PointF) line.Points[0];
            PointF startReference = (PointF) line.Points[1];
            PointF endLocation = (PointF) line.Points[line.Points.Count - 1];
            PointF endReference = (PointF) line.Points[line.Points.Count - 2];

            //Render element action
            base.RenderAction(element, graphics, render);
        }

        //Implement a base rendering of an element selection
        public override void RenderSelection(IRenderable renderable, Graphics graphics, ControlRender render)
        {
            TreeLine line = renderable as TreeLine;
            if (line.Handles == null) return;

            SmoothingMode smoothing = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Handle previousHandle = null;
            SolidBrush brushWhite = new SolidBrush(Color.White);
            Pen pen = Singleton.Instance.SelectionStartPen;
            SolidBrush brush = Singleton.Instance.SelectionStartBrush;

            foreach (Handle handle in line.Handles)
            {
                if (previousHandle != null)
                {
                    graphics.FillPath(brushWhite, previousHandle.Path);
                    graphics.FillPath(brush, previousHandle.Path);
                    graphics.DrawPath(pen, previousHandle.Path);
                    pen = Singleton.Instance.SelectionPen; //Set to normal brush
                    brush = Singleton.Instance.SelectionBrush; //Set to normal pen
                }
                previousHandle = handle;
            }
            graphics.FillPath(brushWhite, previousHandle.Path);
            graphics.FillPath(Singleton.Instance.SelectionEndBrush, previousHandle.Path);
            graphics.DrawPath(Singleton.Instance.SelectionEndPen, previousHandle.Path);

            graphics.SmoothingMode = smoothing;
        }

        //Renders a graphics marker
        protected virtual void RenderMarker(MarkerBase marker, PointF markerPoint, PointF referencePoint, Graphics graphics, Render render)
        {
            if (marker == null) return;

            //Save the graphics state
            Matrix gstate = graphics.Transform;

            //Apply the marker transform and render the marker
            graphics.Transform = Link.GetMarkerTransform(marker, markerPoint, referencePoint, graphics.Transform);

            IFormsRenderer renderer = render.GetRenderer(marker);
            renderer.RenderElement(marker, graphics, render);

            //Restore the graphics state
            graphics.Transform = gstate;
        }

        //Renders a graphics marker
        protected virtual void RenderMarkerShadow(MarkerBase marker, PointF markerPoint, PointF referencePoint, Graphics graphics, Render render)
        {
            if (marker == null) return;

            //Save the graphics state
            Matrix gstate = graphics.Transform;

            //Apply the marker transform and render the marker
            graphics.Transform = Link.GetMarkerTransform(marker, markerPoint, referencePoint, graphics.Transform);
           
            IFormsRenderer renderer = render.GetRenderer(marker);
            renderer.RenderShadow(marker, graphics, render);

            //Restore the graphics state
            graphics.Transform = gstate;
        }

        //Renders a graphics marker
        protected virtual void RenderMarkerAction(MarkerBase marker, PointF markerPoint, PointF referencePoint, Graphics graphics, ControlRender render)
        {
            if (marker == null) return;

            //Save the graphics state
            Matrix gstate = graphics.Transform;

            //Apply the marker transform and render the marker
            graphics.Transform = Link.GetMarkerTransform(marker, markerPoint, referencePoint, graphics.Transform);

            IFormsRenderer renderer = render.GetRenderer(marker);
            renderer.RenderAction(marker, graphics, render);

            //Restore the graphics state
            graphics.Transform = gstate;
        }

    }
}
