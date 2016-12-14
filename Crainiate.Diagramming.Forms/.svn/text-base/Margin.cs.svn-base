// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming.Forms
{
	public class Margin: ICloneable
	{
		//Property variables
		private float _top;
		private float _left;
		private float _bottom;
		private float _right;

		#region Interface

		//Constructors
		public Margin()
		{
		}

		public Margin(float left, float top,float right, float bottom)
		{
			_top = top;
			_left = left;
			_bottom = bottom;
			_right = right;
		}

		//Properties
		//Top
		public virtual float Top
		{
			get
			{
				return _top;
			}
			set
			{
				_top = value;
			}
		}

		//Left
		public virtual float Left
		{
			get
			{
				return _left;
			}
			set
			{
				_left = value;
			}
		}

		//Bottom
		public virtual float Bottom
		{
			get
			{
				return _bottom;
			}
			set
			{
				_bottom = value;
			}
		}

		//Right
		public virtual float Right
		{
			get
			{
				return _right;
			}
			set
			{
				_right = value;
			}
		}

		public virtual bool IsEmpty
		{
			get
			{
				return (_top == 0 && _left == 0 && _right ==0 && _bottom == 0);
			}
		}

		public override string ToString()
		{
            return string.Format("{Left={0},Top={1},Right={2},Bottom={3}}", new object[] { Left.ToString(), Top.ToString(), Right.ToString(), Bottom.ToString() });
		}

		//Methods
		public virtual bool Equals(Margin margin)
		{
			return (_top == margin.Top && _left == margin.Left && _right == margin.Right && _bottom == margin.Bottom);
		}

		public object Clone()
		{
			return new Margin(Left,Top,Right,Bottom);
		}

		#endregion

	}
}
