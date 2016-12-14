// (c) Copyright Crainiate Software 2010




using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
    public class RotateCommand: TransformCommand 
    {
        //Property variables
        private float _degrees;
        private PointF _mousePosition;

        //Working variables
        private Stack<float> _undo;

        //Constructors
        public RotateCommand(Controller controller): base(controller)
        {

        }

        //Properties
        public float Degrees
        {
            get
            {
                return _degrees;
            }
            set
            {
                _degrees= value;
            }
        }

        public PointF MousePosition
        {
            get
            {
                return _mousePosition;
            }
            set
            {
                _mousePosition = value;
            }
        }

        //Methods
        public virtual void Rotate()
        {
            _undo = new Stack<float>();

            if (Controller.Model.Route != null) Controller.Model.Route.Reform();

            //Function will return -180 to 180
            if (_degrees < 0) _degrees += 360;

            //Snap to orientations
            if (_degrees > 355 || _degrees < 5) _degrees = 0;
            if (_degrees > 85 && _degrees < 95) _degrees = 90;
            if (_degrees > 175 && _degrees < 185) _degrees = 180;
            if (_degrees > 265 && _degrees < 275) _degrees = 270;

            foreach (Element element in Elements)
            {
                //Rotate transformable
                if (element is ITransformable && element is Shape)
                {
                    Shape shape = (Shape) element;
                    _undo.Push(shape.Rotation);
                    if (shape.AllowRotate && shape.Visible) shape.Rotation = _degrees;
                }
            }

            //Calculate the vector between the center of the rotated element and the mouse pointer
            if (MouseElements != null && MouseElements.MouseStartElement is Shape)
            {
                Shape shape = MouseElements.MouseStartElement as Shape;
                PointF center = new PointF(shape.Center.X, shape.Center.Y);

                PointF client = MousePosition;

                SetVectors(new RectangleF[] { new RectangleF(center, new SizeF(client.X - center.X, client.Y - center.Y)) });
            }
        }

        public override void Undo()
        {
            try
            {
                if (Controller.Model.Route != null) Controller.Model.Route.Reform();

                foreach (Element element in Elements)
                {
                    //Rotate transformable
                    if (element is ITransformable && element is Shape)
                    {
                        Shape shape = (Shape) element;
                        if (shape.AllowRotate && shape.Visible) shape.Rotation = _undo.Pop();
                    }
                }

                
                Execute();
            }
            finally
            {
                _undo = null;
            }
        }

        public override void Redo()
        {
            Rotate();
            Execute();
        }
    }
}
