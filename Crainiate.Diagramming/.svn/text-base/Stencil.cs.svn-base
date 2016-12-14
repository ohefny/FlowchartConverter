// (c) Copyright Crainiate Software 2010




using System;
using System.Collections;
using System.Drawing;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	//Strongly typed key value pair collection for stencil items
	public abstract class Stencil : DictionaryBase
	{
		//Property variables
		private bool _suspendEvents;
		private bool _modifiable;

		#region Interface
	
		//Constructors
		public Stencil()
		{
		}

		//Properties
		public virtual StencilItem this[string key ]  
		{
			get  
			{
				return (StencilItem) Dictionary[key];
			}
			set  
			{
				Dictionary[key] = value;
			}
		}

		public virtual ICollection Keys  
		{
			get  
			{
				return Dictionary.Keys;
			}
		}

		public virtual ICollection Values  
		{
			get  
			{
				return Dictionary.Values;
			}
		}

		public object SyncRoot 
		{ 
			get 
			{ 
				return Dictionary.SyncRoot; 
			} 
		}

		//Determines whether the collection is modifiable
		public virtual bool Modifiable
		{
			get
			{
				return _modifiable;
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

		//Methods
		public virtual void Add(string key, StencilItem value )  
		{
			if (! Modifiable) throw new CollectionNotModifiableException("This collection is not modifiable.");
			if (value == null) throw new ArgumentNullException("value","StencilItem parameter cannot be null reference.");
			if (key == null) throw new ArgumentNullException("key","Key may not be null.");
			if (key == "") throw new ArgumentException("Key may not be null string.","key");

			value._key = key;
			Dictionary.Add(key, value );

			OnInsert(key,value);
		}

		public virtual bool Contains(string key)  
		{
			return Dictionary.Contains(key);
		}

		public virtual void Remove(string key)  
		{
			if (! _modifiable) throw new CollectionNotModifiableException("This collection is not modifiable.");
			Dictionary.Remove(key);
		}

		public virtual void Clear()
		{
			if (! _modifiable) throw new CollectionNotModifiableException("This collection is not modifiable.");
			Dictionary.Clear();
			OnClear();
		}

		//Sets wether this collection is modifiable
		protected internal virtual void SetModifiable(bool value)
		{
			_modifiable = value;
		}

		#endregion
	}
}