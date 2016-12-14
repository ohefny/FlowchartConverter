// (c) Copyright Crainiate Software 2010

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace Crainiate.Diagramming
{
    public class Handles : List<Handle>
    {
    }

	public class Handle
	{
		public Handle()
		{
			Type = HandleType.Move;
			CanDock = false;
		}

		public Handle(HandleType type)
		{
			Type = type;
			CanDock = false;
		}

		public Handle(GraphicsPath path, HandleType type)
		{
			Type = type;
			Path = path;
			CanDock = false;
		}

		public Handle(GraphicsPath path, HandleType type, bool canDock)
		{
			Type = type;
			Path = path;
			CanDock = canDock;
		}

		public virtual HandleType Type {get; set;}
		public virtual GraphicsPath Path {get; set;}
		public virtual bool CanDock {get; set;}
	}

	public class ConnectorHandle: Handle
	{
		public ConnectorHandle()
		{
			Type = HandleType.Move;
		}

		public ConnectorHandle(GraphicsPath path, HandleType type, int index): base(path, type)
		{
			Index = index;
		}

		public virtual int Index {get; set;}
	}

	public class ExpandHandle: Handle
	{
		public ExpandHandle(Segment segment)
		{
			Type = HandleType.Expand;
			Segment = segment;
		}

		public virtual Segment Segment {get; set;}
		public virtual int Index {get; set;}
	}
}
