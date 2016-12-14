// (c) Copyright Crainiate Software 2010




using System;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public class Segment
	{
		//Property variables
		private Origin _start;
		private Origin _end;
		private Label _label;
		private Image _image;

		private bool _suspendEvents;
		
		#region Interface

		public event EventHandler SegmentInvalid;

		//Constructors
		public Segment()
		{

		}

		public Segment(Origin start, Origin end)
		{
			SetStart(start);
			SetEnd(end);
		}

		//Properties
		//Returns the starting origin for this segment
		public virtual Origin Start
		{
			get
			{
				return _start;
			}
		}

		//Returns the ending origin for this segment
		public virtual Origin End
		{
			get
			{
				return _end;
			}
		}

		//Returns the annotation for this segment
		public virtual Label Label
		{
			get
			{
				return _label;
			}
			set
			{
				if (_label != null)
				{
					_label.LabelInvalid -= new EventHandler(Label_LabelInvalid);
				}

				_label = value;
				if (_label != null)
				{
					_label.LabelInvalid += new EventHandler(Label_LabelInvalid);
				}
				OnSegmentInvalid();
			}
		}

		//Returns the Image object which which displays an image for this shape.
		public Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (_image != null) 
				{
					_image.ImageInvalid -= new EventHandler(Image_ImageInvalid);
				}

				_image = value;
				if (_image != null) 
				{
					_image.ImageInvalid += new EventHandler(Image_ImageInvalid);
				}
				OnSegmentInvalid();
			}
		}

		//Determines whether events are prevented from being raised by this class.
		protected internal virtual bool SuspendEvents
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
		public void SetStart(Origin value)
		{
			_start = value;
		}

		public void SetEnd(Origin value)
		{
			_end = value;
		}

		//Raises the OriginInvalid event
		protected void OnSegmentInvalid()
		{
			if (!SuspendEvents && SegmentInvalid != null) SegmentInvalid(this,EventArgs.Empty);
		}

		#endregion

		#region Events

		//Handles annotation invalid events
		private void Label_LabelInvalid(object sender, EventArgs e)
		{
			OnSegmentInvalid();
		}

		//Handles image invalid events
		private void Image_ImageInvalid(object sender, EventArgs e)
		{
			OnSegmentInvalid();
		}

		#endregion
	}
}
