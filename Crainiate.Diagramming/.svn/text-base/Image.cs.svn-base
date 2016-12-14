// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public class Image: ICloneable, IRenderable
	{
		//Property variables
		private Element _parent;
		private PointF _location;
        private InterpolationMode _interpolationMode;

		//Working variables
		private string _path;
		private string _resource;
		private string _assembly;
		private bool _suspendEvents;
		private System.Drawing.Image _image;
		
		#region Interface

		//Events
		public event EventHandler ImageInvalid;

		//Constructors
        protected Image()
        {

        }

		//Loads an image form a path
		public Image(string path)
		{
			Location = new PointF(0,0);
            _path = path;
            SetImage(Singleton.Instance.GetBitmap(path));
            InterpolationMode = InterpolationMode.HighQualityBicubic;
		}

		//Loads an image from an assembly
		public Image(string resourcename,string assembly)
		{
			Location = new PointF(0,0);
			_resource = resourcename;
			_assembly = assembly;
			
			SetImage(Singleton.Instance.GetResource(resourcename,assembly));
            InterpolationMode = InterpolationMode.HighQualityBicubic;
		}

		//Loads an image from an existing GDI+ image
		public Image(System.Drawing.Image image)
		{
			Location = new PointF(0,0);
			SetImage(image);
            InterpolationMode = InterpolationMode.HighQualityBicubic;
		}

		public Image(Image prototype)
		{
			_location = prototype.Location;
			_resource = prototype.Resource;
			_path = prototype.Path;
            _interpolationMode = prototype.InterpolationMode;
		}
		
		//Properties
		public virtual PointF Location
		{
			get
			{
				return _location;
			}
			set
			{
				if (!_location.Equals(value))
				{
					_location = value;
					OnImageInvalid();
				}
			}
		}

        //Contains the original path used to get the image resoure
		public virtual string Path
		{
			get
			{
				return _path;
			}
		}

        //Contains the resource name used to obtain the image resource
		public virtual string Resource
		{
			get
			{
				return _resource;
			}
		}
		
        //Contains the assembly name used to obtain the image resource.
		public virtual string Assembly
		{
			get
			{
				return _assembly;
			}
		}

		//Returns the internal image bitmap
		public virtual System.Drawing.Image Bitmap
		{
			get
			{
				return _image;
			}
		}

		public virtual Element Parent
		{
			get
			{
				return _parent;
			}
		}

		//Specifies how data is interpolated between endpoints.
		public virtual InterpolationMode InterpolationMode
		{
			get
			{
				return _interpolationMode;
			}
			set
			{
				_interpolationMode = value;
			}
		}

        //Contains a reference the current renderer being used to render the element
        public IRenderer Renderer { get; set; }

		//Methods
		public void SetParent(Element parent)
		{
			_parent = parent;
		}

		public void SetImage(System.Drawing.Image image)
		{
			_image = image;
        }


        public void SetPath(string path)
        {
            _path = path;
        }

        public void SetResource(string resource)
        {
            _resource = resource;
        }


        public void SetAssembly(string assembly)
        {
            _assembly = assembly;
        }

        //Raises the element invalid event.
		protected virtual void OnImageInvalid()
		{
			if (ImageInvalid != null) ImageInvalid(this,EventArgs.Empty);
		}

        public virtual object Clone()
        {
            return new Image(this);
        }

		#endregion
	}
}
