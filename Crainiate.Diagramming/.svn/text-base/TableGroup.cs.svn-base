// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public class TableGroup: TableItem, ICloneable
	{
		//Property variables
		private TableGroups _groups;
		private TableRows _rows;
		private bool _expanded;

		//Working variables
		
		//Events
		public event ExpandedChangedEventHandler ExpandedChanged;
		public event EventHandler HeightChanged;
						
		//Constructors
		public TableGroup()
		{
			Expanded = true;
			Rows = new TableRows();
			Groups = new TableGroups();
		}

        public TableGroup(string text):this()
        {
            Text = text;
        }

		public TableGroup(TableGroup prototype): base(prototype)
		{
			_expanded = prototype.Expanded;
			Rows = new TableRows();
			Groups = new TableGroups();
			Table.CopyRows(prototype.Rows,Rows);
			Table.CopyGroups(prototype.Groups, Groups);
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
				if (value == null) throw new ArgumentNullException("Groups may not be set to null.");
			
				if (_groups != null)
				{
					_groups.InsertItem-=new TableItemsEventHandler(TableItems_InsertItem);
					_groups.RemoveItem-=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				_groups = value;
				_groups.InsertItem+=new TableItemsEventHandler(TableItems_InsertItem);
				_groups.RemoveItem+=new TableItemsEventHandler(TableItems_RemoveItem);
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
				if (value == null) throw new ArgumentNullException();
				
				if (_rows != null)
				{
					_rows.InsertItem-=new TableItemsEventHandler(TableItems_InsertItem);
					_rows.RemoveItem-=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				_rows = value;
				_rows.InsertItem+=new TableItemsEventHandler(TableItems_InsertItem);
				_rows.RemoveItem+=new TableItemsEventHandler(TableItems_RemoveItem);
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
					OnExpandedChanged(this, value);
				}
			}
		}

		protected internal virtual void SetTable()
		{
			foreach (TableGroup group in Groups)
			{
				group.SetTable(); //Calls this method recursively
			}
			foreach (TableRow row in Rows)
			{
				row.SetTable(Table);
			}	
		}

		//Methods
		protected virtual void OnExpandedChanged(object sender, bool expanded)
		{
			if (ExpandedChanged != null) ExpandedChanged(sender, expanded);
		}

		protected virtual void OnHeightChanged(object sender, EventArgs e)
		{
			if (HeightChanged != null) HeightChanged(sender, e);
		}

		//Handles events for rows and groups
		private void TableItems_InsertItem(object sender, TableItemsEventArgs e)
		{
			//Check isnt same group
			if (e.Value == this) throw new TableItemsException("Cannot add a TableGroup to itself.");

            if (Table != null)
            {
                e.Value.SetTable(Table);
                e.Value.SetIndent(Indent + Table.Indent);
            }
			e.Value.SetParent(this);
			e.Value.TableItemInvalid +=new EventHandler(TableItem_TableItemInvalid);

			if (e.Value is TableGroup)
			{
				TableGroup group = (TableGroup) e.Value;

				group.ExpandedChanged +=new ExpandedChangedEventHandler(TableGroup_ExpandedChanged);
				group.HeightChanged +=new EventHandler(TableGroup_HeightChanged);
			}
			
			OnHeightChanged(this, EventArgs.Empty);
			OnTableItemInvalid();
		}
		
		private void TableItems_RemoveItem(object sender, TableItemsEventArgs e)
		{
			//Remove handlers
			if (e.Value != null)
			{
				e.Value.TableItemInvalid -=new EventHandler(TableItem_TableItemInvalid);

				if (e.Value is TableGroup)
				{
					TableGroup group = (TableGroup) e.Value;

					group.ExpandedChanged -=new ExpandedChangedEventHandler(TableGroup_ExpandedChanged);
					group.HeightChanged -=new EventHandler(TableGroup_HeightChanged);
				}
			}

			OnHeightChanged(this, EventArgs.Empty);
			OnTableItemInvalid();
		}

		private void TableItem_TableItemInvalid(object sender, EventArgs e)
		{
			OnTableItemInvalid();
		}

		private void TableGroup_ExpandedChanged(object sender, bool expanded)
		{
			OnExpandedChanged(sender, expanded);
		}

		private void TableGroup_HeightChanged(object sender, EventArgs e)
		{
			OnHeightChanged(sender, e);
		}
	
		//Implement cloning for this class
		public virtual object Clone()
		{
			return new TableGroup(this);
		}

	}
}
