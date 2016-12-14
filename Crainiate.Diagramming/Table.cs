// (c) Copyright Crainiate Software 2010

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public class Table: Shape, IExpandable, ICloneable, IMouseEvents
	{
		//Property variables
		private string _heading;
		private string _subHeading;
		private TableGroups _groups;
		private TableRows _rows;
		private float _headingHeight;
		private bool _expanded;
		private bool _drawExpand;
		private SizeF _expandedSize;
		private SizeF _contractedSize;
		private float _groupHeight;
		private float _rowHeight;
		private float _rowIndent;
		private Font _font;
		private Color _forecolor;
		private TableItem _selectedItem;
		private bool _drawSelectedItem;

		//Working variables
		private GraphicsPath _expandPath;
		
		//Events
		public event ExpandedChangedEventHandler ExpandedChanged;
		public event EventHandler SelectedItemChanged;
		
		//Constructors
		public Table()
		{			
			Groups = new TableGroups();
			Rows = new TableRows();
			HeadingHeight = 40;
			Heading = "Heading";
			SubHeading = "Sub Heading";
			GroupHeight = 20;
			RowHeight = 20;
			Indent = 16;
			ExpandedSize = Size;
			ContractedSize = new SizeF(Size.Width,CalculateItemRectangles());
			Expanded = true;
			DrawExpand = false;
			Forecolor = Color.Black;
			GradientColor = SystemColors.Control;
			DrawSelectedItem = true;
		}

		public Table(Table prototype): base(prototype)
		{
			_headingHeight = prototype.HeadingHeight;
			_heading = prototype.Heading;
			_subHeading = prototype.SubHeading;
			_groupHeight = prototype.GroupHeight;
			_rowHeight = prototype.RowHeight;
			_rowIndent = prototype.Indent;
			_expanded = prototype.Expanded;
			_drawExpand = prototype.DrawExpand;
			_forecolor = prototype.Forecolor;
			GradientColor = prototype.GradientColor;
			_drawSelectedItem = prototype.DrawSelectedItem;
			_font = prototype.Font;

			ContractedSize = prototype.ContractedSize;
			ExpandedSize = prototype.ExpandedSize;
			
			Groups = new TableGroups();
			Rows = new TableRows();

			Table.CopyGroups(prototype.Groups,Groups);
			Table.CopyRows(prototype.Rows,Rows);
		}

		//Properties
		public virtual TableGroups Groups
		{
			get
			{
				return _groups;
			}
			set
			{
				if (_groups != null)
				{
					_groups.InsertItem -=new TableItemsEventHandler(TableItems_InsertItem);
					_groups.RemoveItem -=new TableItemsEventHandler(TableItems_RemoveItem);
				}
				
				_groups = value;
				
				if (_groups != null)
				{
					_groups.InsertItem +=new TableItemsEventHandler(TableItems_InsertItem);
					_groups.RemoveItem +=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				OnElementInvalid();
			}
		}

		public virtual TableRows Rows
		{
			get
			{
				return _rows;
			}
			set
			{
				if (_rows != null)
				{
					_rows.InsertItem-=new TableItemsEventHandler(TableItems_InsertItem);
					_rows.RemoveItem-=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				_rows = value;

				if (_rows != null)
				{
					_rows.InsertItem+=new TableItemsEventHandler(TableItems_InsertItem);
					_rows.RemoveItem+=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				OnElementInvalid();
			}
		}

		public virtual Color Forecolor
		{
			get
			{
				return _forecolor;
			}
			set
			{
				_forecolor = value;
				OnElementInvalid();
			}
		}

		public virtual string Heading
		{
			get
			{
				return _heading;
			}
			set
			{
				if (_heading != value)
				{
					_heading = value;
					OnElementInvalid();
				}
			}
		}
		public virtual string SubHeading
		{
			get
			{
				return _subHeading;
			}
			set
			{
				if (_subHeading != value)
				{
					_subHeading = value;
					OnElementInvalid();
				}
			}
		}

		public virtual string FontName
		{
			get
			{
				if (_font == null) return Singleton.Instance.DefaultFont.FontFamily.Name;
				return _font.FontFamily.Name;
			}
			set
			{
				_font = Singleton.Instance.GetFont(value,FontSize,FontStyle);
				OnElementInvalid();
			}
		}

		public virtual float FontSize
		{
			get
			{
				if (_font == null) return Singleton.Instance.DefaultFont.Size;
				return _font.Size;
			}
			set
			{
				_font = Singleton.Instance.GetFont(FontName,value,FontStyle);
				OnElementInvalid();
			}
		}

		public virtual FontStyle FontStyle
		{
			get
			{
				if (_font == null) return Singleton.Instance.DefaultFont.Style;
				return _font.Style;
			}
			set
			{
				_font = Singleton.Instance.GetFont(FontName,FontSize,value);
				OnElementInvalid();
			}
		}

		public Font Font
		{
			get
			{
				if (_font == null) return Singleton.Instance.DefaultFont;
				return _font;
			}
		}

		public virtual float HeadingHeight
		{
			get
			{
				return _headingHeight;
			}
			set
			{
				if (_headingHeight != value)
				{
					_headingHeight = value;
				}
			}
		}

		public virtual float GroupHeight
		{
			get
			{
				return _groupHeight;
			}
			set
			{
				if (_groupHeight != value)
				{
					_groupHeight = value;
				}
			}
		}
		
		public virtual float RowHeight
		{
			get
			{
				return _rowHeight;
			}
			set
			{
				if (_rowHeight != value)
				{
					_rowHeight = value;
				}
			}
		}

		public virtual float Indent
		{
			get
			{
				return _rowIndent;
			}
			set
			{
				if (_rowIndent != value)
				{
					_rowIndent = value;
					OnElementInvalid();
				}
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

		public virtual TableItem SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				if (_selectedItem != value)
				{
					_selectedItem = value;
					OnSelectedItemChanged();
				}

				OnElementInvalid();
			}
		}

		public virtual bool DrawSelectedItem
		{
			get
			{
				return _drawSelectedItem;
			}
			set
			{
				_drawSelectedItem = value;
				OnElementInvalid();
			}
		}

		//Methods
		public virtual void SetHeight()
		{
            float height = CalculateItemRectangles();

            //Only set the height if the table is expanded
            if (Expanded)
            {
                Height = height;
            }
            else
            {
                ExpandedSize = new SizeF(ExpandedSize.Width, height);
            }
		}

        public virtual bool ExecuteMouseCommand(MouseCommand command)
        {
            //Only consider left button
            if (command.MouseButtons != MouseCommandButtons.Left) return false;
            
            //Only process mouse down
            if (command is MouseDownCommand)
            {
                PointF location = command.Location; //view.PointToDiagram(e.X, e.Y);
                PointF local = PointToElement(location);

                TableItem old = SelectedItem;
                SelectedItem = GetTableItemFromLocation(local);
                if (SelectedItem != old && Selected) return true;

                //Check to see if any groups can be expanded
                TableGroup group = GetTableGroupExpander(Groups, local);
                if (group != null)
                {
                    group.Expanded = !group.Expanded;
                    return true;
                }
            }

            return false;
        }

		//Returns the table item from a mouse point
		public virtual TableItem GetTableItem(IView view, PointF location)
		{
			PointF local = PointToElement(location);
			return GetTableItemFromLocation(local);
		}

		//Returns a table item from a local point
		public virtual TableItem GetTableItem(PointF location)
		{
			return GetTableItemFromLocation(location);
		}

		public virtual PointF GetItemPosition(TableItem item)
		{
			float height = HeadingHeight + 2;

			//Add top level groups
			foreach (TableGroup group in Groups)
			{
				if (group == item) return new PointF(0, height);
				if (CheckGroupHeight(item, group, ref height)) return new PointF(0, height);
			}
			
			//Add top level rows
			foreach (TableRow row in Rows)
			{
				if (row == item) return new PointF(0, height);
				height += RowHeight;
			}
			
			return Point.Empty;
		}

		//Methods
		//Sets the internal font directly
		protected internal virtual void SetFont(Font font)
		{
			_font = font;
		}

		protected virtual void OnExpandedChanged()
		{
			if (ExpandedChanged != null) ExpandedChanged(this,Expanded);
		}

		protected virtual void OnSelectedItemChanged()
		{
			if (SelectedItemChanged != null) SelectedItemChanged(this,EventArgs.Empty);
		}

		public override object Clone()
		{
			return new Table(this);
		}

		public override Handle Handle(PointF location)
		{
			//Check expander
			PointF local = PointToElement(location);
			if (Expander != null && Expander.IsVisible(local)) return new Handle(HandleType.Arrow);

			//Check groups
			if (GetTableGroupExpander(Groups, local) != null) return new Handle(HandleType.Arrow);

			return base.Handle(location);
		}

        public override void LocatePort(Port port)
        {
            if (port is TablePort)
            {
                TablePort tablePort = port as TablePort;
                if (tablePort.TableItem == null)
                {
                    base.LocatePort(port);
                    return;
                }

                //Calculate position of port based on position of table item
                TableItem visibleItem = GetVisibleItem(tablePort.TableItem);

                PointF start = TransformRectangle.Location;
                start.Y += visibleItem.Rectangle.Top + (visibleItem.Rectangle.Height / 2);

                //Determine which side of the shape the port should be placed
                if (tablePort.Orientation == PortOrientation.Right) start.X += visibleItem.Rectangle.Width;

                tablePort.Validate = false;
                tablePort.Location = start;
                tablePort.Validate = true;
            }
            else
            {
                base.LocatePort(port);
            }
        }

        public override float Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                CalculateItemRectangles();
            }
        }

        public override float Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
                CalculateItemRectangles();
            }
        }

        public override SizeF Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
                CalculateItemRectangles();
            }
        }

		//Fired when a group or top level row is added to the table
		private void TableItems_InsertItem(object sender, TableItemsEventArgs e)
		{
			TableItem item = e.Value;

			//Set common values and event handlers for row or group
			item.SetTable(this);
			item.Backcolor = GradientColor;

			//If is a row then set the indent
			if (item is TableRow) item.SetIndent(Indent);

			if (item is TableGroup)
			{
				TableGroup group = (TableGroup) item;
				group.HeightChanged +=new EventHandler(TableGroup_HeightChanged);
				group.ExpandedChanged += new ExpandedChangedEventHandler(TableGroup_ExpandedChanged);

				//Set all the table references correctly
				group.SetTable();
			}

			item.TableItemInvalid +=new EventHandler(TableItems_TableItemInvalid);

			SetHeight();

			//Make sure diagram is redrawn
			OnElementInvalid();
		}
		
		private void TableItems_RemoveItem(object sender, TableItemsEventArgs e)
		{
			TableItem item = e.Value;

			//Remove handlers
			if (item is TableGroup)
			{
				TableGroup group = (TableGroup) item;
				group.HeightChanged -=new EventHandler(TableGroup_HeightChanged);
				group.ExpandedChanged -= new ExpandedChangedEventHandler(TableGroup_ExpandedChanged);
			}
			item.TableItemInvalid -=new EventHandler(TableItems_TableItemInvalid);

			SetHeight();
			OnElementInvalid();
		}

		//Fired when a group or toplevel row becomes invalid
		private void TableItems_TableItemInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		private void TableGroup_HeightChanged(object sender, EventArgs e)
		{
			SetHeight();
		}

		private void TableGroup_ExpandedChanged(object sender, bool Expanded)
		{
			SetHeight();
		}
	
		//Copy all rows from source to target via clone
		public static void CopyRows(TableRows source, TableRows target)
		{
			foreach (TableRow row in source)
			{
				target.Add((TableRow) row.Clone());
			}
		}

		//Copy all rows from source to target via clone
		public static void CopyGroups(TableGroups source, TableGroups target)
		{
			foreach (TableGroup group in source)
			{
				target.Add((TableGroup) group.Clone());
			}
		}
		
		private TableGroup GetTableGroupExpander(TableGroups groups, PointF local)
		{
			foreach (TableGroup group in groups)
			{
				RectangleF expander = new RectangleF(group.Indent + 4, group.Rectangle.Top + 4, 11, 11);
				if (expander.Contains(local)) return group;

				//Sub groups
				TableGroup result = GetTableGroupExpander(group.Groups, local);
				if (result != null) return result;
			}

			return null;
		}

		protected virtual float CalculateItemRectangles()
		{
			float height = HeadingHeight;
            float indent = 0;

            SetItemRectangles(Groups, Rows, ref height, indent);
			
			//Add padding of 10 pixels
			height += 10;
			
			return height;
		}

		private void SetItemRectangles(TableGroups groups, TableRows rows, ref float height, float indent)
		{
			//Add sub groups
			foreach (TableGroup group in groups)
			{
				group.SetItemRectangle(new RectangleF(0, height, Width, GroupHeight));
                group.SetIndent(indent);
                height += GroupHeight;
                
                if (group.Expanded) SetItemRectangles(group.Groups, group.Rows, ref height, indent + Indent);
			}

			//Add sub rows
            indent += Indent;
			foreach (TableRow row in rows)
			{
                row.SetItemRectangle(new RectangleF(0, height, Width, RowHeight));
                row.SetIndent(indent);
                height += RowHeight;
			}
		}

		protected virtual TableItem GetTableItemFromLocation(PointF local)
		{
			float currentHeight = HeadingHeight;

			//Check the top level rows (if any)
			if (Rows.Count > 0)
			{
				foreach (TableRow tableRow in Rows)
				{
					if (local.Y >= currentHeight && local.Y <= currentHeight + RowHeight) return tableRow;
					currentHeight += RowHeight;
				}
			}

			//Check the groups and each rows collection inside groups
			return GetGroupTableItem(Groups, local, ref currentHeight);
		}

		private TableItem GetGroupTableItem(TableGroups groups, PointF local, ref float currentHeight)
		{
			//Check the groups and each rows collection inside groups
			foreach (TableGroup tableGroup in groups)
			{
				if (local.Y >= currentHeight && local.Y <= currentHeight + GroupHeight) return tableGroup;
				currentHeight += GroupHeight;

				if (tableGroup.Expanded)
				{
					TableItem item = GetGroupTableItem(tableGroup.Groups, local, ref currentHeight);
					if (item != null) return item;

					foreach (TableRow tableRow in tableGroup.Rows)
					{
						if (local.Y >= currentHeight && local.Y <= currentHeight + GroupHeight) return tableRow;
						currentHeight += RowHeight;
					}
				}
			}
			return null;
		}

		private bool CheckGroupHeight(TableItem check, TableGroup parent, ref float height)
		{
			height += GroupHeight;
			
			if (parent.Expanded) 
			{
				//Add sub groups
				foreach (TableGroup subgroup in parent.Groups)
				{
					if (subgroup == check) return true;
					if (CheckGroupHeight(check, subgroup, ref height)) return true;
				}

				//Add sub rows
				if (GetRowsHeight(check, parent, ref height)) return true;
			}

			return false;
		}

		private bool GetRowsHeight(TableItem check, TableGroup parent, ref float height)
		{
			//Add top level rows
			foreach (TableRow row in parent.Rows)
			{
				if (row == check) return true;
				height += RowHeight;
			}

			return false;
		}

        //Returns the first visible group or row from the item provided.
        private TableItem GetVisibleItem(TableItem item)
        {
            List<TableItem> list = new List<TableItem>();
            list.Add(item);

            TableItem child = item;

            //Create a list parent items
            while (child.Parent != null)
            {
                list.Insert(0, child.Parent);
                child = child.Parent;
            }

            //Loop through list and find bottom most viisble item
            foreach (TableItem loop in list)
            {
                if (loop is TableGroup)
                {
                    TableGroup group = loop as TableGroup;
                    if (!group.Expanded) return group;
                }
            }

            return item;
        }
	}
}
