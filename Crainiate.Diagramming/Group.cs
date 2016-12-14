// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Collections;
using Crainiate.Diagramming.Serialization;
using Crainiate.Diagramming.Layouts;

namespace Crainiate.Diagramming
{
	public class Group: Shape, IExpandable 
	{
		//Property variables
        private ElementList _elements;
		private bool _checkBounds;
		private bool _expanded;
		private bool _drawExpand;
		private SizeF _expandedSize;
		private SizeF _contractedSize;

		//Working variables
		private GraphicsPath _expandPath;

		public event ExpandedChangedEventHandler ExpandedChanged;
		
		//Constructors
		public Group()
		{
            _elements = new ElementList();
            Elements.SetModifiable(false);

			DrawBackground = false;
			DrawShadow = false;
			BorderStyle = DashStyle.Dash;
			ExpandedSize = MaximumSize;
			ContractedSize = Size;
			DrawExpand = true;
			Expanded = true;
			CheckBounds = true;
		}

		public Group(Group prototype): base(prototype)
		{
            _elements = new ElementList();

			CheckBounds = prototype.CheckBounds;
			DrawExpand = prototype.DrawExpand;
			Expanded = prototype.Expanded;
			ContractedSize = prototype.ContractedSize;
			ExpandedSize = prototype.ExpandedSize;
		}

        public virtual ElementList Elements
        {
            get
            {
                return _elements;
            }
        }

		public virtual bool CheckBounds
		{
			get
			{
				return _checkBounds;
			}
			set
			{
				_checkBounds = value;
			}
		}

		public virtual bool Expanded
		{
			get
			{
				return _expanded;
			}
			set
			{
				if (_expanded != value)
				{
					_expanded = value;
					
					//Adjust size of group
					if (_expanded)
					{
						ContractedSize = Size;
						Size = ExpandedSize;
					}
					else
					{
						ExpandedSize = Size;
						Size = ContractedSize;
					}
					
					OnExpandedChanged();
				}
			}
		}

		public virtual SizeF ExpandedSize
		{
			get
			{
				return _expandedSize;
			}
			set
			{
				if (!_expandedSize.Equals(value))
				{
					_expandedSize = value;
					if (MaximumSize.Width < _expandedSize.Width) MaximumSize = new SizeF(_expandedSize.Width,MaximumSize.Height);
					if (MaximumSize.Height < _expandedSize.Height) MaximumSize = new SizeF(MaximumSize.Width, _expandedSize.Height);
				}
			}
		}

		public virtual SizeF ContractedSize
		{
			get
			{
				return _contractedSize;
			}
			set
			{
				if (!_contractedSize.Equals(value))
				{
					_contractedSize = value;
					if (MinimumSize.Width > _contractedSize.Width) MinimumSize = new SizeF(_contractedSize.Width,MinimumSize.Height);
					if (MinimumSize.Height > _contractedSize.Height) MinimumSize = new SizeF(MinimumSize.Width, _contractedSize.Height);
				}
			}
		}

		public virtual bool DrawExpand
		{
			get
			{
				return _drawExpand;
			}
			set
			{
				if (_drawExpand != value)
				{
					_drawExpand = value;
					OnElementInvalid();
				}
			}
		}

		//Returns the expander path
		public virtual GraphicsPath Expander
		{
			get
			{
				return _expandPath;
			}
		}


        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
                Elements.Select(value);
            }
        }

		//Methods

        public void AddElement(Element element)
        {
            Elements.SetModifiable(true);
            Elements.Add(element);
            element.SetGroup(this);
            Elements.SetModifiable(false);
        }

        public void AddElements(ElementList elements)
        {
            Elements.SetModifiable(true);

            foreach (Element element in elements)
            {
                Elements.Add(element);
                element.SetGroup(this);
            }

            Elements.SetModifiable(false);
        }

        public void RemoveElement(Element element)
        {
            Elements.SetModifiable(true);
            Elements.Remove(element);
            element.SetGroup(null);
            Elements.SetModifiable(false);
        }

        public void RemoveElements(ElementList elements)
        {
            Elements.SetModifiable(true);

            foreach (Element element in elements)
            {
                Elements.Remove(element);
                element.SetGroup(null);
            }

            Elements.SetModifiable(false);
        }
        
        //Additional contains to the standard override
		protected virtual void OnExpandedChanged()
		{
			if (ExpandedChanged != null) ExpandedChanged(this,Expanded);
		}

		public override bool Contains(PointF location)
		{
			return GroupContains(location,true);
		}
	
		private bool GroupContains(PointF location,bool transparent)
		{
			//Inflate rectangle to include selection handles
			RectangleF bounds = Bounds;
			bounds.Inflate(6,6);

			//If inside inflate boundary
			if (bounds.Contains(location))
			{
				//Return true if clicked in selection rectangle but not path rectangle
				if (Selected && !Bounds.Contains(location)) return true;

				//Check the outline offset to the path (0,0)
				location.X -= Bounds.X;
				location.Y -= Bounds.Y;

				//If background is drawn or transparency checking is not enabled
				if (DrawBackground || !transparent)
				{
					if (GetPath().IsVisible(location)) return true;
				}
				else
				{
                    //The expand path may be null if it has not been rendered
					if (DrawExpand && _expandPath != null && _expandPath.IsVisible(location)) return true;
					
					//Check if contains an element when draw transparently
					if (Expanded)
					{
						//Check local renderlist
						foreach (Element element in Elements)
						{
							if (element.Contains(location)) return true;
						}
					}

					//Check outline of path
					if (GetPath().IsOutlineVisible(location,new Pen(Color.Black, BorderWidth + 2))) return true;
				}
			}
			
			return false;
		}
	}
}
