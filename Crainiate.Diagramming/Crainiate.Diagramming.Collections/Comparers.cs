// (c) Copyright Crainiate Software 2010




using System;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;

namespace Crainiate.Diagramming.Collections
{
	internal class RenderListComparer : IComparer<Element>
	{
        public int Compare(Element ex, Element ey)
        {
            if (ex is ISelectable && ey is ISelectable)
            {
                ISelectable sy = (ISelectable) ey;
                ISelectable sx = (ISelectable) ex;

                if (ey.ZOrder == ex.ZOrder)
                {
                    if (!sx.Selected && !sy.Selected) return 1;
                    if (sx.Selected && !sy.Selected) return 1;
                    if (sy.Selected && !sx.Selected) return -1;
                    if (sx.Selected && sx.Selected) return 1;
                }
            }
            return (ey.ZOrder - ex.ZOrder);
        }
    }

	internal class PointFComparer : IComparer<PointF>
	{
		private bool _horizonal;
		private bool _ascending;

		public PointFComparer()
		{
			_horizonal = true;
			_ascending = true;
		}

		public PointFComparer(bool horizontal, bool ascending)
		{
			_horizonal = horizontal;
			_ascending = ascending;
		}

		public bool Horizontal
		{
			get
			{
				return _horizonal;
			}
			set
			{
				_horizonal = value;
			}
		}

		public bool Ascending
		{
			get
			{
				return _ascending;
			}
			set
			{
				_ascending = value;
			}
		}

		public int Compare(PointF a, PointF b)
		{
			if (_ascending)
			{
				if (_horizonal)
				{
					return Convert.ToInt32(a.X - b.X);
				}
				else
				{
					return Convert.ToInt32(a.Y - b.Y);
				}
			}
			else
			{
				if (_horizonal)
				{
					return Convert.ToInt32(b.X - a.X);
				}
				else
				{
					return Convert.ToInt32(b.Y - a.Y);
				}
			}
		}
	}
}
