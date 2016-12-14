// (c) Copyright Crainiate Software 2010




using System;
using System.Collections;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public class Layer: ICloneable
	{
		//Property variables				
		private byte _opacity = 100;
		private bool _visible = true;
		private string _name;

		private bool _drawShadows = true;
		private bool _softShadows = true;
		private PointF _shadowOffset;
		private Color _shadowColor;
		private bool _default;

		//Working variables
		private bool _suspendEvents;

		#region Interface

		//Events
		public event EventHandler LayerInvalid;

		//Constructor
		public Layer()
		{
			_shadowOffset = Singleton.Instance.DefaultShadowOffset;
			_shadowColor = Singleton.Instance.DefaultShadowColor;
		}

		protected internal Layer(bool defaultLayer)
		{
			_default = defaultLayer;
			_shadowOffset = Singleton.Instance.DefaultShadowOffset;
			_shadowColor = Singleton.Instance.DefaultShadowColor;
		}

		public Layer(Layer prototype)
		{
			_opacity = prototype.Opacity;
			_visible = prototype.Visible;
			_name = prototype.Name;
			_drawShadows = prototype.DrawShadows;
			_shadowOffset = prototype.ShadowOffset;
			_shadowColor = prototype.ShadowColor;
			_softShadows = prototype.SoftShadows;
		}

		//Properties

		//Sets the opacity of this Layer
		public virtual byte Opacity
		{
			get
			{
				return _opacity;
			}
			set
			{
				if (value != _opacity)
				{
					_opacity = value;
					OnLayerInvalid();
				}
			}
		}

		//Determines if Layer is visible
		public virtual bool Visible
		{	
			get
			{
				return _visible;
			}
			set
			{
				if (value != _visible)
				{
					_visible = value;
					OnLayerInvalid();
				}
			}
		}

		//Sets or gets the name of this Layer
		public virtual string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		//Sets or retrieves a boolean value determining if the diagram renders shadows.
		public virtual bool DrawShadows
		{
			get
			{
				return _drawShadows;
			}
			set
			{
				if (_drawShadows != value)
				{
					_drawShadows = value;
					OnLayerInvalid();
				}
			}
		}

		//Sets or retrieves a point used to offset the shadows in the diagram
		public virtual PointF ShadowOffset
		{
			get
			{
				return _shadowOffset;
			}
			set
			{
				if (! _shadowOffset.Equals(value))
				{
					_shadowOffset = value;
					OnLayerInvalid();
				}
			}
		}

		//Sets or retrieves the color used to render the shadows.")]
		public virtual Color ShadowColor
		{
			get
			{
				return _shadowColor;
			}
			set
			{
				_shadowColor = value;
				OnLayerInvalid();
			}
		}

		//Sets or retrieves a boolean value determining if the diagram renders shadows with an additional penumbra.
		public virtual bool SoftShadows
		{
			get
			{
				return _softShadows;
			}
			set
			{
				if (_softShadows != value)
				{
					_softShadows = value;
					OnLayerInvalid();
				}
			}
		}

		//Returns whether this layer is the default layer.
		public bool Default
		{
			get
			{
				return _default;
			}
		}

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
		protected virtual void OnLayerInvalid()
		{
			if (LayerInvalid !=null && !_suspendEvents) LayerInvalid(this,EventArgs.Empty);
		}

		#endregion

		#region Implementation

		public virtual object Clone()
		{
			return new Layer(this);
		}

		#endregion
	}
}
