// (c) Copyright Crainiate Software 2010




using System;
using System.Windows.Forms;
using System.Drawing;

namespace Crainiate.Diagramming
{
    //Factory follows the factory design pattern
	public class Factory
	{
		//Events
		public event EventHandler CreateElement;

		//Constructors
		public Factory()
		{
		}

		//Methods
		public virtual Link CreateLine()
		{
			Link line = new Link();

			OnCreateElement(line);
			return line;
		}

		public virtual Link CreateLine(PointF start,PointF end)
		{
			Link line = new Link(start,end);

			OnCreateElement(line);
			return line;
		}

		public virtual Connector CreateConnector()
		{
			Connector line = new Connector();

			OnCreateElement(line);
			return line;
		}

		public virtual Connector CreateConnector(PointF start,PointF end)
		{
			Connector line = new Connector(start,end);

			OnCreateElement(line);
			return line;
		}

		public virtual ComplexLine CreateComplexLine()
		{
			ComplexLine line = new ComplexLine();

			OnCreateElement(line);
			return line;
		}


		public virtual ComplexLine CreateComplexLine(PointF start,PointF end)
		{
			ComplexLine line = new ComplexLine(start,end);

			OnCreateElement(line);
			return line;
		}

		public virtual Curve CreateCurve()
		{
			Curve curve = new Curve();

			OnCreateElement(curve);
			return curve;
		}

		public virtual Curve CreateCurve(PointF start,PointF end)
		{
			Curve curve = new Curve(start,end);

			OnCreateElement(curve);
			return curve;
		}

		public virtual Shape CreateShape()
		{
			Shape shape = new Shape();

			OnCreateElement(shape);
			return shape;
		}

		public virtual Shape CreateShape(PointF start,SizeF size)
		{
			Shape shape = new Shape();
			shape.Location = start;
			if (! size.IsEmpty) shape.Size = size;

			OnCreateElement(shape);
			return shape;
		}

		public virtual ComplexShape CreateComplexShape()
		{
			ComplexShape shape = new ComplexShape();

			OnCreateElement(shape);
			return shape;
		}

		public virtual ComplexShape CreateComplexShape(PointF start,SizeF size)
		{
			ComplexShape shape = new ComplexShape();
			shape.Location = start;
			if (! size.IsEmpty) shape.Size = size;

			OnCreateElement(shape);
			return shape;
		}

		public virtual Shape CreateTable()
		{
			Table table = new Table();

			OnCreateElement(table);
			return table;
		}

		public virtual Shape CreateTable(PointF start,SizeF size)
		{
			Table table = new Table();
			table.Location = start;
			if (!size.IsEmpty) table.Size = size;

			OnCreateElement(table);
			return table;
		}

		public virtual Group CreateGroup()
		{
			Group group = new Group();

			OnCreateElement(group);
			return group;
		}

		public virtual Port CreatePort(PortOrientation orientation)
		{
			Port port = new Port(orientation);

			OnCreateElement(port);
			return port;
		}

		//Raises the create element event
		protected virtual void OnCreateElement(object sender)
		{
			if (CreateElement != null) CreateElement(sender,EventArgs.Empty);
		}
	}
}
