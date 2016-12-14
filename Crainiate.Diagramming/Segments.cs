// (c) Copyright Crainiate Software 2010




using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public class Segments: CollectionBase
	{
		//Working Variables
		private bool _suspendEvents;

		#region Interface

		public event SegmentsEventHandler InsertItem;
		public event SegmentsEventHandler RemoveItem;
		public event EventHandler Clear;

		public Segments()
		{
		}

		//Collection indexers
		public virtual Segment this[int index]  
		{
			get  
			{
				return (Segment) List[index];
			}
			set  
			{
				List[index] = value;
			}
		}

		//Determines whether events are suspended inside the control
		protected virtual bool SuspendEvents
		{
			get
			{
				return _suspendEvents;
			}
			set
			{
				_suspendEvents = value;
			}
		}

		//Adds an Segment to the list 
		protected internal virtual int Add(Segment value)  
		{
			if (value == null) throw new ArgumentNullException("Segment parameter cannot be null reference.","value");
			return List.Add(value);
		}

		//Inserts an elelemnt into the list
		protected internal virtual void Insert(int index, Segment value)  
		{
			if (value == null) throw new ArgumentNullException("Segment parameter cannot be null reference.","value");
			List.Insert(index, value);
		}

		//Removes an Segment from the list
		protected internal virtual void Remove(Segment value )  
		{
			List.Remove(value);
		}

		//Returns true if list contains Segment
		public virtual bool Contains(Segment value)  
		{
			return List.Contains(value);
		}

		//Returns the index of an Segment
		public virtual int IndexOf(Segment value)  
		{
			return List.IndexOf(value);
		}

		//Raises the InsertItem event
		//Original OnInsert method does not raise any events
		protected override void OnInsert(int index,object value)
		{
			base.OnInsert(index,value);
			if (! _suspendEvents && InsertItem!=null) InsertItem(value,new SegmentsEventArgs((Segment) value));
		}

		//Raises the RemoveItem event
		//Original OnRemove method does not raise any events
		protected override void OnRemove(int index,object value)
		{
			base.OnRemove(index,value);
			if (! _suspendEvents && RemoveItem!=null) RemoveItem(value,new SegmentsEventArgs((Segment) value));
		}

		//Raises the Clear event.
		protected virtual void OnClear()
		{
			base.OnClear();
			if (! _suspendEvents && Clear!=null) Clear(this,EventArgs.Empty);
		}

		#endregion
		
		#region Implementation

		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			Object[] values = new Object[base.Count];

			base.InnerList.CopyTo(values,0);
			info.AddValue("Values",values);
		}

		#endregion
	}
}