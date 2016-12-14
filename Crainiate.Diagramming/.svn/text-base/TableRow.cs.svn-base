// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public class TableRow: TableItem, ICloneable
	{
		//Property variables
		private Crainiate.Diagramming.Image _image;
				
		//Constructors
		public TableRow()
		{
			
		}

        public TableRow(string text)
        {
            Text = text;
        }

		public TableRow(TableRow prototype): base(prototype)
		{
			_image = prototype.Image;
		}

		//Properties
		public virtual Crainiate.Diagramming.Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				_image = value;
				OnTableItemInvalid();
			}
		}
	
		public virtual object Clone()
		{
			return new TableRow(this);
		}
	}
}
