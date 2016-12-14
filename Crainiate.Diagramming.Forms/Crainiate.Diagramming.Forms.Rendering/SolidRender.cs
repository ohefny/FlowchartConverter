// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class SolidRender: ElementRender
    {
        public override void RenderElement(IRenderable renderable, Graphics graphics, Render render)
        {   
            Solid solid = renderable as Solid;
            GraphicsPath path = solid.GetPath();
            if (path == null) return;

            //Fill the solid
            RenderSolid(solid, graphics, render);

            //Set clipping for this shape
            Region current = null;

            //Add local clipping
            if (solid.Clip)
            {
                Region region = new Region(path);
                current = graphics.Clip;
                graphics.SetClip(region, CombineMode.Intersect);
            }

            if (!render.Summary)
            {
                //Render image
                if (solid.Image != null)
                {
                    IFormsRenderer renderer = render.GetRenderer(solid.Image);
                    renderer.RenderElement(solid.Image, graphics, render);
                }

                //Render label
                if (solid.Label != null)
                {
                    LabelRender renderer = render.GetRenderer(solid.Label) as LabelRender;
                    if (solid.Label.Wrap)
                    {
                        renderer.RenderElement(solid.Label, graphics, render, solid.InternalRectangle);
                    }
                    else
                    {
                        renderer.RenderElement(solid.Label, graphics, render);
                    }
                }
            }

            //Restore clipping
            if (solid.Clip) graphics.Clip = current;

            //Call the base implementation of render to draw border
            if (solid.DrawBorder) base.RenderElement(renderable, graphics, render);
        }

        public override void RenderShadow(IRenderable renderable, Graphics graphics, Render render)
        {
            Solid solid = renderable as Solid;
            Layer layer = solid.Layer;
            if (layer == null) return;

            if (solid.DrawBackground)
            {
                //Use transformed path as shadows are not rotated
                GraphicsPath shadowPath = Geometry.ScalePath(solid.TransformPath, 1F, 1F);

                graphics.TranslateTransform(layer.ShadowOffset.X, layer.ShadowOffset.Y);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                //Draw soft shadows
                if (layer.SoftShadows && solid.StencilItem != null && ((solid.StencilItem.Options & StencilItemOptions.SoftShadow) == StencilItemOptions.SoftShadow))
                {
                    PathGradientBrush brush = new PathGradientBrush(shadowPath);

                    //Calculate position factor based on 0.3 for 100 pixels
                    //0.6 for 50 pixels, 0.15 for 200 pixels
                    float factor = Convert.ToSingle(0.3 * (100 / solid.Bounds.Width));

                    //Set up the brush blend
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0F, factor, 1F };
                    blend.Factors = new float[] { 0F, 0.8F, 1F };
                    brush.Blend = blend;

                    brush.CenterColor = render.AdjustColor(layer.ShadowColor, 1, solid.Opacity);
                    //brush.CenterColor = Color.FromArgb(brush.CenterColor.A * 30 / 100,brush.CenterColor);
                    brush.SurroundColors = new Color[] { Color.FromArgb(0, layer.ShadowColor) };

                    graphics.FillPath(brush, shadowPath);

                    brush.Dispose();
                }
                else
                {
                    SolidBrush shadowBrush = new SolidBrush(render.AdjustColor(Color.FromArgb(10, layer.ShadowColor), 1, solid.Opacity));
                    graphics.FillPath(shadowBrush, shadowPath);
                }

                //Restore graphics
                graphics.TranslateTransform(-layer.ShadowOffset.X, -layer.ShadowOffset.Y);
                graphics.SmoothingMode = solid.SmoothingMode;
            }
            else
            {
                if (solid.DrawBorder)
                {
                    Pen shadowPen = new Pen(render.AdjustColor(layer.ShadowColor, solid.BorderWidth, solid.Opacity));
                    GraphicsPath shadowPath = solid.TransformPath;

                    graphics.TranslateTransform(layer.ShadowOffset.X, layer.ShadowOffset.Y);

                    if (layer.SoftShadows)
                    {
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.DrawPath(shadowPen, shadowPath);
                        graphics.CompositingQuality = render.CompositingQuality;
                        graphics.SmoothingMode = solid.SmoothingMode;
                    }
                    else
                    {
                        graphics.DrawPath(shadowPen, shadowPath);
                    }

                    //Restore graphics
                    graphics.TranslateTransform(-layer.ShadowOffset.X, -layer.ShadowOffset.Y);
                }
            }
        }

        public override void RenderAction(IRenderable renderable, Graphics graphics, ControlRender render)
        {
            Solid solid = renderable as Solid;
            GraphicsPath path = solid.GetPath();

            if (solid.DrawBackground)
            {
                if (render.ActionStyle == ActionStyle.Default)
                {
                    RenderSolid(solid, graphics, render);

                    //Add local clipping
                    Region current = null;
                    if (solid.Clip)
                    {
                        Region region = new Region(path);
                        current = graphics.Clip;
                        graphics.SetClip(region, CombineMode.Intersect);
                    }
                    //Render annotation and image
                    if (solid.Label != null)
                    {
                        LabelRender renderer = render.GetRenderer(solid.Label) as LabelRender;
                        renderer.RenderAction(solid.Label, graphics, render, solid.InternalRectangle);
                    }

                    //Restore clipping
                    if (solid.Clip) graphics.Clip = current;
                }
                else
                {
                    if (path == null) return;

                    graphics.FillPath(Singleton.Instance.ActionBrush, path);
                }
            }
            if (solid.DrawBorder) base.RenderAction(renderable, graphics, render);
        }

        private void RenderSolid(Solid solid, Graphics graphics, Render render)
        {
            GraphicsPath path = solid.GetPath();

            //Create a brush if no custom brush defined
            if (solid.DrawBackground)
            {
                if (solid.CustomBrush == null)
                {
                    //Use a linear gradient brush if gradient requested
                    if (solid.DrawGradient)
                    {
                        LinearGradientBrush brush;
                        brush = new LinearGradientBrush(new RectangleF(0, 0, solid.Bounds.Width, solid.Bounds.Height), render.AdjustColor(solid.BackColor, 0, solid.Opacity), render.AdjustColor(solid.GradientColor, 0, solid.Opacity), solid.GradientMode);
                        brush.GammaCorrection = true;
                        graphics.FillPath(brush, path);
                    }
                    //Draw normal solid brush
                    else
                    {
                        SolidBrush brush;
                        brush = new SolidBrush(render.AdjustColor(solid.BackColor, 0, solid.Opacity));
                        graphics.FillPath(brush, path);
                    }
                }
                else
                {
                    graphics.FillPath(solid.CustomBrush, path);
                }
            }

            //			//Render internal rectangle
            //			Pen tempPen = new Pen(Color.Red,1);
            //			graphics.DrawRectangle(tempPen,_internalRectangle.X,_internalRectangle.Y,_internalRectangle.Width,_internalRectangle.Height);
            //
            //			tempPen = new Pen(Color.Green,1);
            //			graphics.DrawRectangle(tempPen,_transforminternalRectangle.X,_transforminternalRectangle.Y,_transforminternalRectangle.Width,_transforminternalRectangle.Height);

        }
    }
}
