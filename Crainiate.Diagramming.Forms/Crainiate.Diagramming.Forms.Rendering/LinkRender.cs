// (c) Copyright Crainiate Software 2010



#define DEBUG

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class LinkRender: ElementRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            Link link = element as Link;

            if (link.Points == null || link.Points.Count < 2) return;

            PointF startLocation = (PointF) link.Points[0];
            PointF startReference = (PointF) link.Points[1];
            PointF endLocation = (PointF) link.Points[link.Points.Count - 1];
            PointF endReference = (PointF) link.Points[link.Points.Count - 2];

            //Change the points to local points
            startLocation = Geometry.OffsetPoint(startLocation, link.Bounds.Location);
            startReference = Geometry.OffsetPoint(startReference, link.Bounds.Location);
            endLocation = Geometry.OffsetPoint(endLocation, link.Bounds.Location);
            endReference = Geometry.OffsetPoint(endReference, link.Bounds.Location);

            //Save the current region
            Region current = graphics.Clip;

            //Mask out the start marker
            if (link.Start.Marker != null)
            {
                Region region = new Region(link.Start.Marker.GetPath());
                region.Transform(Link.GetMarkerTransform(link.Start.Marker, startLocation, startReference, new Matrix()));
                graphics.SetClip(region, CombineMode.Exclude);
            }

            //Mask out the end marker
            if (link.End.Marker != null)
            {
                Region region = new Region(link.End.Marker.GetPath());
                region.Transform(Link.GetMarkerTransform(link.End.Marker, endLocation, endReference, new Matrix()));
                graphics.SetClip(region, CombineMode.Exclude);
            }

            base.RenderElement(element, graphics, render);

            if (link.Start.Marker != null || link.End.Marker != null)
            {
                graphics.Clip = current;

                if (link.Start.Marker != null) RenderMarker(link.Start.Marker, startLocation, startReference, graphics, render);
                if (link.End.Marker != null) RenderMarker(link.End.Marker, endLocation, endReference, graphics, render);
            }

            //Render any ports
            if (link.Ports != null)
            {
                foreach (Port port in link.Ports.Values)
                {
                    if (port.Visible)
                    {
                        graphics.TranslateTransform(-link.Bounds.X + port.Bounds.X, -link.Bounds.Y + port.Bounds.Y);
                        port.SuspendValidation();

                        IFormsRenderer renderer = render.GetRenderer(port);
                        renderer.RenderElement(port, graphics, render);
                        
                        port.ResumeValidation();
                        graphics.TranslateTransform(link.Bounds.X - port.Bounds.X, link.Bounds.Y - port.Bounds.Y);
                    }
                }
            }

            //Render Image and Label
            if (link.Image != null || link.Label != null)
            {
                PointF center = link.GetLabelLocation();
                graphics.TranslateTransform(center.X, center.Y);

                if (link.Image != null)
                {
                    IFormsRenderer renderer = render.GetRenderer(link.Image);
                    renderer.RenderElement(link.Image, graphics, render);
                }
                if (link.Label != null)
                {
                    IFormsRenderer renderer = render.GetRenderer(link.Label);
                    renderer.RenderElement(link.Label, graphics, render);
                }

                graphics.TranslateTransform(-center.X, -center.Y);
            }
        }

        //Implement a base rendering of an element
        public override void RenderShadow(IRenderable element, Graphics graphics, Render render)
        {
            Link link = element as Link;
            if (link.Points == null) return;
            if (link.Points.Count < 2) return;

            PointF startLocation = (PointF) link.Points[0];
            PointF startReference = (PointF) link.Points[1];
            PointF endLocation = (PointF) link.Points[link.Points.Count - 1];
            PointF endReference = (PointF) link.Points[link.Points.Count - 2];

            startLocation = Geometry.OffsetPoint(startLocation, link.Bounds.Location);
            startReference = Geometry.OffsetPoint(startReference, link.Bounds.Location);
            endLocation = Geometry.OffsetPoint(endLocation, link.Bounds.Location);
            endReference = Geometry.OffsetPoint(endReference, link.Bounds.Location);

            Layer layer = link.Layer;
            Pen shadowPen = new Pen(layer.ShadowColor);
            GraphicsPath shadowPath = link.GetPath();
            shadowPen.Color = render.AdjustColor(layer.ShadowColor, 0, link.Opacity);

            //Save the current region
            Region current = graphics.Clip;

            //Mask out the start marker
            if (link.Start.Marker != null)
            {
                Region region = new Region(link.Start.Marker.GetPath());
                region.Transform(Link.GetMarkerTransform(link.Start.Marker, startLocation, startReference, new Matrix()));
                region.Translate(layer.ShadowOffset.X, layer.ShadowOffset.Y);
                graphics.SetClip(region, CombineMode.Exclude);
            }

            //Mask out the end marker
            if (link.End.Marker != null)
            {
                Region region = new Region(link.End.Marker.GetPath());
                region.Transform(Link.GetMarkerTransform(link.End.Marker, endLocation, endReference, new Matrix()));
                region.Translate(layer.ShadowOffset.X, layer.ShadowOffset.Y);
                graphics.SetClip(region, CombineMode.Exclude);
            }

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
                graphics.SmoothingMode = link.SmoothingMode;
            }

            //Restore graphics
            if (link.Start.Marker != null || link.End.Marker != null)
            {
                graphics.Clip = current;
                if (link.Start.Marker != null) RenderMarkerShadow(link.Start.Marker, startLocation, startReference, graphics, render);
                if (link.End.Marker != null) RenderMarkerShadow(link.End.Marker, endLocation, endReference, graphics, render);
            }

            graphics.TranslateTransform(-layer.ShadowOffset.X, -layer.ShadowOffset.Y);
        }

        public override void RenderAction(IRenderable element, Graphics graphics, ControlRender render)
        {
            Link link = element as Link;

            if (link.Points == null || link.Points.Count < 2) return;

            PointF startLocation = (PointF) link.Points[0];
            PointF startReference = (PointF) link.Points[1];
            PointF endLocation = (PointF) link.Points[link.Points.Count - 1];
            PointF endReference = (PointF) link.Points[link.Points.Count - 2];

            startLocation = Geometry.OffsetPoint(startLocation, link.Bounds.Location);
            startReference = Geometry.OffsetPoint(startReference, link.Bounds.Location);
            endLocation = Geometry.OffsetPoint(endLocation, link.Bounds.Location);
            endReference = Geometry.OffsetPoint(endReference, link.Bounds.Location);

            //Save the current region
            Region current = graphics.Clip;

            //Mask out the start marker
            if (link.Start.Marker != null)
            {
                Region region = new Region(link.Start.Marker.GetPath());
                region.Transform(Link.GetMarkerTransform(link.Start.Marker, startLocation, startReference, new Matrix()));
                graphics.SetClip(region, CombineMode.Exclude);
            }

            //Mask out the end marker
            if (link.End.Marker != null)
            {
                Region region = new Region(link.End.Marker.GetPath());
                region.Transform(Link.GetMarkerTransform(link.End.Marker, endLocation, endReference, new Matrix()));
                graphics.SetClip(region, CombineMode.Exclude);
            }

            //Render element action
            base.RenderAction(element, graphics, render);

            //Render markers
            if (link.Start.Marker != null || link.End.Marker != null)
            {
                graphics.Clip = current;

                if (link.Start.Marker != null) RenderMarkerAction(link.Start.Marker, startLocation, startReference, graphics, render);
                if (link.End.Marker != null) RenderMarkerAction(link.End.Marker, endLocation, endReference, graphics, render);
            }

            //Render any ports
            if (link.Ports != null)
            {
                foreach (Port port in link.Ports.Values)
                {
                    if (port.Visible)
                    {
                        graphics.TranslateTransform(-link.Bounds.X + port.Bounds.X, -link.Bounds.Y + port.Bounds.Y);
                        port.SuspendValidation();

                        IFormsRenderer renderer = render.GetRenderer(port);
                        renderer.RenderElement(port, graphics, render);
                        
                        port.ResumeValidation();
                        graphics.TranslateTransform(link.Bounds.X - port.Bounds.X, link.Bounds.Y - port.Bounds.Y);
                    }
                }
            }
        }

        //Implement a base rendering of an element selection
        public override void RenderSelection(IRenderable renderable, Graphics graphics, ControlRender render)
        {
            Link link = renderable as Link;
            if (link.Handles == null) return;

            SmoothingMode smoothing = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Handle previousHandle = null;
            SolidBrush brushWhite = new SolidBrush(Color.White);
            Pen pen = Singleton.Instance.SelectionStartPen;
            SolidBrush brush = Singleton.Instance.SelectionStartBrush;

            foreach (Handle handle in link.Handles)
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
