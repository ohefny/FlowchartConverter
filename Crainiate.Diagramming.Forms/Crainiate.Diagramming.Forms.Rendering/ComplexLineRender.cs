// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class ComplexLineRender: LinkRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
		{
            ComplexLine complex = element as ComplexLine;
			if (complex.Points == null) return;

			PointF location;
			PointF reference;
			Segment segment = null;

			//Save the current region
			Region current = graphics.Clip;

			//Mask out each marker
            for (int i = 0; i < complex.Points.Count - 1; i++)
			{
                location = (PointF) complex.Points[i];
                reference = (PointF) complex.Points[i + 1];

                segment = complex.Segments[i];
				
				//Mask out the start marker
				if (segment.Start.Marker != null && !segment.Start.Marker.DrawBackground)
				{
					Region region = new Region(segment.Start.Marker.GetPath());
					region.Transform(Link.GetMarkerTransform(segment.Start.Marker,location,reference,new Matrix()));
					graphics.SetClip(region, CombineMode.Exclude);
				}
			}

			//Mask out final marker
			if (segment.End.Marker != null && !segment.End.Marker.DrawBackground)
			{
                location = (PointF)complex.Points[complex.Points.Count - 1];
                reference = (PointF)complex.Points[complex.Points.Count - 2];

				Region region = new Region(segment.End.Marker.GetPath());
				region.Transform(Link.GetMarkerTransform(segment.End.Marker,location,reference,new Matrix()));
				graphics.SetClip(region,CombineMode.Exclude);
			}
			
			//Draw the path
			Pen pen = null;

            if (complex.CustomPen == null)
			{
                pen = new Pen(complex.BorderColor, complex.BorderWidth);
                pen.DashStyle = complex.BorderStyle;
	
				//Check if winforms renderer and ajdust color as required
                pen.Color = render.AdjustColor(complex.BorderColor, complex.BorderWidth, complex.Opacity);
			}
			else	
			{
                pen = complex.CustomPen;
			}
            graphics.DrawPath(pen, complex.GetPath());
			
			//Reset the clip
			graphics.Clip = current;

			//Render the segment items
            for (int i = 0; i < complex.Points.Count - 1; i++)
			{
                segment = complex.Segments[i];
                location = (PointF)complex.Points[i];
                reference = (PointF)complex.Points[i + 1];

				if (segment.Start.Marker != null) RenderMarker(segment.Start.Marker,location,reference,graphics,render);

				//Render the segment image and annotation
				RenderSegment(complex, segment, location, reference, graphics, render);
			}

			//Render final marker
			if (segment.End.Marker != null)
			{
                location = (PointF)complex.Points[complex.Points.Count - 1];
                reference = (PointF)complex.Points[complex.Points.Count - 2];
				RenderMarker(segment.End.Marker,location,reference,graphics,render);				
			}
		}

		public override void RenderAction(IRenderable element, Graphics graphics, ControlRender render)
		{
            ComplexLine complex = element as ComplexLine;
			if (complex.Points == null) return;

			PointF location;
			PointF reference;
			Segment segment = null;

			//Save the current region
			Region current = graphics.Clip;

			//Mask out each marker
            for (int i = 0; i < complex.Points.Count - 1; i++)
			{
                location = (PointF)complex.Points[i];
                reference = (PointF)complex.Points[i + 1];

                segment = complex.Segments[i];
				
				//Mask out the start marker
				if (segment.Start.Marker != null)
				{
					Region region = new Region(segment.Start.Marker.GetPath());
					region.Transform(Link.GetMarkerTransform(segment.Start.Marker,location,reference,new Matrix()));
					graphics.SetClip(region,CombineMode.Exclude);
				}
			}

			//Mask out final marker
			if (segment.End.Marker != null)
			{
                location = (PointF)complex.Points[complex.Points.Count - 1];
                reference = (PointF)complex.Points[complex.Points.Count - 2];

				Region region = new Region(segment.End.Marker.GetPath());
				region.Transform(Link.GetMarkerTransform(segment.End.Marker,location,reference,new Matrix()));
				graphics.SetClip(region,CombineMode.Exclude);
			}
			
			//Draw the path
            GraphicsPath path = complex.GetPath();
			if (path == null) return;

			if (render.ActionStyle == ActionStyle.Default)
			{
                Pen pen = new Pen(render.AdjustColor(complex.BorderColor, complex.BorderWidth, complex.Opacity));
                pen.Width = complex.BorderWidth;
				graphics.DrawPath(pen,path);
			}
			else
			{
				graphics.DrawPath(Singleton.Instance.ActionPen,path);
			}

			//Reset the clip
			graphics.Clip = current;

			//Render the markers
            for (int i = 0; i < complex.Points.Count - 1; i++)
			{
                segment = complex.Segments[i];
                location = (PointF)complex.Points[i];
                reference = (PointF)complex.Points[i + 1];

				if (segment.Start.Marker != null) RenderMarkerAction(segment.Start.Marker,location,reference,graphics,render);
			}

			//Render final marker
			if (segment.End.Marker != null)
			{
                location = (PointF)complex.Points[complex.Points.Count - 1];
                reference = (PointF)complex.Points[complex.Points.Count - 2];
				RenderMarkerAction(segment.End.Marker,location,reference,graphics,render);				
			}
		}

		public override void RenderShadow(IRenderable element, Graphics graphics, Render render)
		{
            ComplexLine complex = element as ComplexLine;
            if (complex.Points == null) return;

			PointF location;
			PointF reference;
			Segment segment = null;

			Layer layer = complex.Layer;
			Pen shadowPen = new Pen(layer.ShadowColor);
            GraphicsPath shadowPath = complex.GetPath();
            shadowPen.Color = render.AdjustColor(layer.ShadowColor, 0, complex.Opacity);

			//Save the current region
			Region current = graphics.Clip;

			//Mask out each marker
            for (int i = 0; i < complex.Points.Count - 1; i++)
			{
                location = (PointF)complex.Points[i];
                reference = (PointF)complex.Points[i + 1];

                segment = complex.Segments[i];
				
				//Mask out the start marker
				if (segment.Start.Marker != null)
				{
					Region region = new Region(segment.Start.Marker.GetPath());
					region.Transform(Link.GetMarkerTransform(segment.Start.Marker,location,reference,new Matrix()));
					region.Translate(layer.ShadowOffset.X, layer.ShadowOffset.Y);
					graphics.SetClip(region, CombineMode.Exclude);
				}
			}

			//Mask out final marker
			if (segment.End.Marker != null)
			{
                location = (PointF)complex.Points[complex.Points.Count - 1];
                reference = (PointF)complex.Points[complex.Points.Count - 2];

				Region region = new Region(segment.End.Marker.GetPath());
				region.Transform(Link.GetMarkerTransform(segment.End.Marker,location,reference,new Matrix()));
				region.Translate(layer.ShadowOffset.X, layer.ShadowOffset.Y);
				graphics.SetClip(region, CombineMode.Exclude);
			}
			
			//Draw the path
			graphics.TranslateTransform(layer.ShadowOffset.X ,layer.ShadowOffset.Y);
			graphics.DrawPath(shadowPen,shadowPath);

			//Reset the clip
			graphics.Clip = current;

			//Render the markers
            for (int i = 0; i < complex.Points.Count - 1; i++)
			{
                segment = complex.Segments[i];
                location = (PointF)complex.Points[i];
                reference = (PointF)complex.Points[i + 1];

				if (segment.Start.Marker != null) RenderMarkerShadow(segment.Start.Marker,location,reference,graphics,render);
			}

			//Render final marker
			if (segment.End.Marker != null)
			{
                location = (PointF) complex.Points[complex.Points.Count - 1];
                reference = (PointF) complex.Points[complex.Points.Count - 2];
				RenderMarkerShadow(segment.End.Marker,location,reference,graphics,render);				
			}

			graphics.TranslateTransform(-layer.ShadowOffset.X ,-layer.ShadowOffset.Y);
		}

		public override void RenderSelection(IRenderable element, Graphics graphics,ControlRender render)
		{
            ComplexLine complex = element as ComplexLine;
			SmoothingMode smoothing = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			Handle previousHandle = null;
			SolidBrush brushWhite = new SolidBrush(Color.White);
			Pen pen = Singleton.Instance.SelectionStartPen;
			SolidBrush brush = Singleton.Instance.SelectionStartBrush;

            foreach (Handle handle in complex.Handles)
			{
				if (previousHandle != null)	
				{
					graphics.FillPath(brushWhite,previousHandle.Path);
					graphics.FillPath(brush,previousHandle.Path);
					graphics.DrawPath(pen,previousHandle.Path);
					
					if (handle.Type == HandleType.Expand)
					{
						pen = Singleton.Instance.ExpandPen; //Set to normal brush
						brush = Singleton.Instance.ExpandBrush; //Set to normal pen
					}
					else
					{
						pen = Singleton.Instance.SelectionPen; //Set to normal brush
						brush = Singleton.Instance.SelectionBrush; //Set to normal pen
					}
				}
				previousHandle = handle;
			}
			graphics.FillPath(brushWhite,previousHandle.Path);
			graphics.FillPath(Singleton.Instance.SelectionEndBrush,previousHandle.Path);
			graphics.DrawPath(Singleton.Instance.SelectionEndPen,previousHandle.Path);

			graphics.SmoothingMode = smoothing;
		}

		

        //Renders a graphics marker
        protected virtual void RenderSegment(ComplexLine line, Segment segment, PointF targetPoint, PointF referencePoint, Graphics graphics, Render render)
        {
            if (segment.Label == null && segment.Image == null) return;

            //Get midpoint of segment
            PointF midPoint = new PointF(targetPoint.X + ((referencePoint.X - targetPoint.X) / 2), targetPoint.Y + ((referencePoint.Y - targetPoint.Y) / 2));

            //Save the graphics state
            Matrix gstate = graphics.Transform;

            //Apply the rotation transform around the centre
            graphics.Transform = line.GetSegmentTransform(midPoint, referencePoint, graphics.Transform);

            //Offset and draw image 
            if (segment.Image != null)
            {
                graphics.TranslateTransform(0, -segment.Image.Bitmap.Height / 2);

                IFormsRenderer renderer = render.GetRenderer(segment.Image);
                renderer.RenderElement(segment.Image, graphics, render);
                
                graphics.TranslateTransform(0, segment.Image.Bitmap.Height / 2);
            }

            //Draw annotation
            if (segment.Label != null)
            {
                IFormsRenderer renderer = render.GetRenderer(segment.Label);
                renderer.RenderElement(segment.Label, graphics, render);
            }

            //Restore the graphics state
            graphics.Transform = gstate;
        }
    }
}
