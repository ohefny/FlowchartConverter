// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace Crainiate.Diagramming.Forms.Rendering
{
    public class ControlRender: Render
    {
        private bool _drawDecorations;
        private ElementList _actionRenderList;
        private ElementList _highlightRenderList;
        private GraphicsPath _decorationPath;
        private string _feedback;
        private Point _feedbackLocation;
        private Rectangle _selectionRectangle;
        private RectangleF[] _vectors;

        //Constructors
        public ControlRender(): base()
        {
            _drawDecorations = true;
            _feedback = null;
            _feedbackLocation = new Point();
        }

        //Properties
        //Sets or gets the list of elements to be rendered
        public virtual ElementList Actions
        {
            get
            {
                return _actionRenderList;
            }
            set
            {
                _actionRenderList = value;
            }
        }


        //Sets or gets the list of elements to be rendered
        public virtual ElementList Highlights
        {
            get
            {
                return _highlightRenderList;
            }
            set
            {
                _highlightRenderList = value;
            }
        }

        //Determines whether the grid is displayed
        public virtual bool DrawDecorations
        {
            get
            {
                return _drawDecorations;
            }
            set
            {
                _drawDecorations = value;
            }
        }

        //Sets a decoration path such as the type used to render drag drop operations.
        public virtual GraphicsPath DecorationPath
        {
            get
            {
                return _decorationPath;
            }
            set
            {
                _decorationPath = value;
            }
        }

        public virtual string Feedback
        {
            get
            {
                return _feedback;
            }
            set
            {
                _feedback = value;
            }
        }

        public virtual Point FeedbackLocation
        {
            get
            {
                return _feedbackLocation;
            }
            set
            {
                _feedbackLocation = value;
            }
        }

        //Sets or retrieves the rectangle used to draw a selection decoration.
        public virtual Rectangle SelectionRectangle
        {
            get
            {
                return _selectionRectangle;
            }
            set
            {
                if (value.Right < value.Left)
                {
                    value = new Rectangle(value.X + value.Width, value.Y, value.Width * -1, value.Height);
                }
                if (value.Bottom < value.Top)
                {
                    value = new Rectangle(value.X, value.Y + value.Height, value.Width, value.Height * -1);
                }
                _selectionRectangle = value;
            }
        }

        //Sets or gets the parameters for a design vector line
        public virtual RectangleF[] Vectors
        {
            get
            {
                return _vectors;
            }
            set
            {
                _vectors = value;
            }
        }

        //Overrides
        public override void RenderLayers(Graphics graphics, Rectangle renderRectangle, Paging paging)
        {
            base.RenderLayers(graphics, renderRectangle, paging);

            //Render the decorations
            if (_drawDecorations) RenderDecorations(graphics, renderRectangle, paging);
        }

        public override void RenderLayer(Graphics graphics, Layer layer, ElementList elementRenderList, RectangleF renderRectangle)
        {
            base.RenderLayer(graphics, layer, elementRenderList, renderRectangle);

            //Render selections
            if (DrawSelections)
            {
                foreach (Element element in elementRenderList)
                {
                    if (element is ISelectable && element.Visible)
                    {
                        ISelectable selectable = (ISelectable)element;

                        if (element.Layer == layer && selectable.Selected && selectable.DrawSelected && element.Bounds.IntersectsWith(renderRectangle))
                        {
                            PointF transform;

                            //Calculate the transform
                            if (element is ITransformable)
                            {
                                ITransformable transformable = (ITransformable)element;
                                transform = new PointF(transformable.TransformRectangle.X, transformable.TransformRectangle.Y);
                            }
                            else
                            {
                                transform = new PointF(element.Bounds.X, element.Bounds.Y);
                            }

                            //Apply and render
                            graphics.TranslateTransform(transform.X, transform.Y);
                            graphics.SmoothingMode = element.SmoothingMode;

                            IFormsRenderer renderer = GetRenderer(element);
                            renderer.RenderSelection(element, graphics, this);
                            
                            graphics.TranslateTransform(-transform.X, -transform.Y);
                        }
                    }
                }
            }
        }

        protected virtual void RenderDecorations(Graphics graphics, Rectangle renderRectangle, Paging paging)
        {
            GraphicsState state = graphics.Save();
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //Undo any clipping
            if (paging.Enabled) graphics.ResetClip();

            //Draw any action paths
            if (Actions != null)
            {
                foreach (Element element in Actions)
                {
                    if (element.Visible)
                    {
                        GraphicsState graphicsState = graphics.Save();
                        Matrix matrix = graphics.Transform;

                        PointF translate = element.Bounds.Location;

                        //Translate to the required position
                        matrix.Translate(translate.X, translate.Y);

                        //Set up rotation
                        if (element is ITransformable)
                        {
                            ITransformable rotatable = (ITransformable)element;
                            PointF center = new PointF(element.Bounds.Width / 2, element.Bounds.Height / 2);

                            matrix.RotateAt(rotatable.Rotation, center);
                        }

                        //Apply transform
                        graphics.Transform = matrix;

                        IFormsRenderer renderer = GetRenderer(element);
                        renderer.RenderAction(element, graphics, this);

                        //Restore graphics state
                        graphics.Restore(graphicsState);
                    }
                }
            }

            //Draw any highlights
            if (Highlights != null)
            {
                foreach (Element element in Highlights)
                {
                    GraphicsState graphicsState = graphics.Save();
                    Matrix matrix = graphics.Transform;

                    PointF translate = element.Bounds.Location;

                    //Translate to the required position
                    matrix.Translate(translate.X, translate.Y);

                    //Set up rotation
                    if (element is ITransformable)
                    {
                        ITransformable rotatable = (ITransformable)element;
                        PointF center = new PointF(element.Bounds.Width / 2, element.Bounds.Height / 2);

                        matrix.RotateAt(rotatable.Rotation, center);
                    }

                    //Apply transform
                    graphics.Transform = matrix;

                    IFormsRenderer renderer = GetRenderer(element);
                    renderer.RenderHighlight(element, graphics, this);

                    //Restore graphics state
                    graphics.Restore(graphicsState);
                }
            }

            //Draw any decorations
            if (DecorationPath != null)
            {
                graphics.FillPath(Singleton.Instance.HighlightBrush, DecorationPath);
                graphics.DrawPath(Singleton.Instance.HighlightPen, DecorationPath);
            }

            //Reset transformation for non scaled transformations and translate for feedback and selection rectangle
            graphics.ResetTransform();
            graphics.TranslateTransform(-renderRectangle.X, -renderRectangle.Y);
            graphics.TranslateTransform(paging.WorkspaceOffset.X, paging.WorkspaceOffset.Y); //Offset for page border (if paged).

            //Draw the vector if required
            if (Vectors != null)
            {
                SmoothingMode mode = graphics.SmoothingMode;
                graphics.SmoothingMode  = SmoothingMode.None;

                foreach (RectangleF vector in Vectors)
                {
                    if (vector.Width != 0 || vector.Height != 0)
                    {
                        PointF start = new PointF(vector.Left, vector.Top);
                        PointF end = new PointF(vector.Right, vector.Bottom);

                        graphics.DrawLine(Singleton.Instance.VectorPen, start, end);
                    }
                }

                graphics.SmoothingMode = mode;
            }
            
            //Undo paged transform
            graphics.TranslateTransform(-paging.WorkspaceOffset.X, -paging.WorkspaceOffset.Y); //Offset for page border (if paged).

            //Draw any feedback if required
            if (Feedback != null && !FeedbackLocation.IsEmpty)
            {
                graphics.SmoothingMode = SmoothingMode.None;

                Pen pen = new Pen(Color.Black);
                SolidBrush brush = new SolidBrush(Color.FromArgb(224, SystemColors.Info));
                SolidBrush textBrush = new SolidBrush(SystemColors.InfoText);
                SizeF size = graphics.MeasureString(Feedback, Singleton.Instance.DefaultFont);
                SizeF padding = new SizeF(2 * ZoomFactor, 1 * ZoomFactor);
                RectangleF rectangle = new RectangleF(0, 0, size.Width, size.Height);
                rectangle.Inflate(padding);

                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

                graphics.TranslateTransform(FeedbackLocation.X, FeedbackLocation.Y);
                graphics.FillRectangle(brush, rectangle);
                graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                graphics.DrawString(Feedback, Singleton.Instance.DefaultFont, textBrush, 1 * ZoomFactor, 1 * ZoomFactor);
                graphics.TranslateTransform(-FeedbackLocation.X, -FeedbackLocation.Y);
            }

            //Draw the selection rectangle if required
            if (!SelectionRectangle.IsEmpty && !SelectionRectangle.IsEmpty)
            {
                graphics.SmoothingMode = SmoothingMode.None;

                graphics.FillRectangle(Singleton.Instance.SelectionFillBrush, SelectionRectangle);
                graphics.DrawRectangle(Singleton.Instance.SelectionPen, SelectionRectangle);
            }

            graphics.Restore(state);
        }
    }
}
