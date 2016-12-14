// (c) Copyright Crainiate Software 2010




using System;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Collections;

namespace Crainiate.Diagramming
{
	public abstract class TableItems<T>: List<T>
        where T: TableItem
	{

		public event TableItemsEventHandler InsertItem;
		public event TableItemsEventHandler RemoveItem;
		public event EventHandler ClearList;

		public override void  Clear()
        {
 	        base.Clear();
			if (ClearList != null) ClearList(this,EventArgs.Empty);
		}

        protected internal override void OnInserted(T item)
        {
            if (InsertItem != null) InsertItem(this, new TableItemsEventArgs(item));
        }

        protected internal override void OnRemove(T item)
        {
            if (RemoveItem != null) RemoveItem(this, new TableItemsEventArgs(item));
        }
	}
}