// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class ComplexShapeRender: ShapeRender
    {
        public override void RenderElement(IRenderable element, Graphics graphics, Render render)
        {
            ComplexShape complex = element as ComplexShape;

            //Render this shape
            base.RenderElement(element, graphics, render);

            Region current = null;

            //Set up clipping if required
            if (complex.Clip)
            {
                Region region = new Region(complex.GetPath());
                current = graphics.Clip;
                graphics.SetClip(region, CombineMode.Intersect);
            }

            //Render the children
            if (complex.Children != null)
            {
                foreach (Solid solid in complex.RenderList)
                {
                    graphics.TranslateTransform(solid.Bounds.X, solid.Bounds.Y);

                    IFormsRenderer renderer = render.GetRenderer(solid);
                    renderer.RenderElement(solid, graphics, render);

                    graphics.TranslateTransform(-solid.Bounds.X, -solid.Bounds.Y);
                }
            }

            if (complex.Clip) graphics.Clip = current;
        }

        public override void RenderShadow(IRenderable element, Graphics graphics, Render render)
        {
            ComplexShape complex = element as ComplexShape;
            base.RenderShadow(element, graphics, render);

            Region current = null;

            //Set up clipping if required
            if (complex.Clip)
            {
                Region region = new Region(complex.GetPath());
                current = graphics.Clip;
                graphics.SetClip(region, CombineMode.Intersect);
            }

            //Render the children
            if (complex.Children != null)
            {
                foreach (Solid solid in complex.RenderList)
                {
                    graphics.TranslateTransform(solid.Bounds.X, solid.Bounds.Y);

                    IFormsRenderer renderer = render.GetRenderer(solid);
                    renderer.RenderShadow(solid, graphics, render);

                    graphics.TranslateTransform(-solid.Bounds.X, -solid.Bounds.Y);
                }
            }

            if (complex.Clip) graphics.Clip = current;
        }

        public override void RenderAction(IRenderable element, Graphics graphics, ControlRender render)
        {
            ComplexShape complex = element as ComplexShape;
            base.RenderAction(element, graphics, render);

            Region current = null;

            //Set up clipping if required
            if (complex.Clip)
            {
                Region region = new Region(complex.GetPath());
                current = graphics.Clip;
                graphics.SetClip(region, CombineMode.Intersect);
            }

            //Render the children
            if (complex.Children != null)
            {
                foreach (Solid solid in complex.RenderList)
                {
                    graphics.TranslateTransform(solid.Bounds.X, solid.Bounds.Y);

                    IFormsRenderer renderer = render.GetRenderer(solid);
                    renderer.RenderAction(solid, graphics, render);
                    graphics.TranslateTransform(-solid.Bounds.X, -solid.Bounds.Y);
                }
            }

            if (complex.Clip) graphics.Clip = current;
        }

    }
}
