// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Forms;
using Crainiate.Diagramming.Serialization;

//Required so that the resource can be found outside this namespace
internal class FlowchartingResources
{
}

namespace Crainiate.Diagramming.Flowcharting
{
	[ToolboxBitmap(typeof(FlowchartingResources), "Crainiate.ERM4.Resource.flowchart.bmp")]
	public class Flowchart: Diagram
	{
		//Property variables
		private SizeF _spacing;
		private FlowchartOrientation _orientation;
		private FlowchartStencil _flowchartStencil;
		private LineCreateMode _lineMode;
		private ShapeCreateMode _shapeMode;
		private PortCreateMode _portMode;

		//Working variables

		#region Interface

		//Constructors
		public Flowchart()
		{
			Suspend();
			
			Spacing = new SizeF(40, 40);
			Orientation = FlowchartOrientation.Vertical;
			_flowchartStencil = new FlowchartStencil();
			LineMode = LineCreateMode.Line;
			ShapeMode = ShapeCreateMode.Shape;

			Resume();
		}
		
		//Properties
		[Browsable(false), Category("Appearance"), Description("Sets or returns the horizontal and vertical spacing between shapes as a size.")]
		public virtual SizeF Spacing
		{
			get
			{
				return _spacing;
			}
			set
			{
				_spacing = value;
			}
		}

		[Category("Behavior"), DefaultValue(FlowchartOrientation.Vertical), Description("Sets or returns whether the flowchart is drawn vertically or horizontally.")]
		public virtual FlowchartOrientation Orientation
		{
			get
			{
				return _orientation;
			}
			set
			{
				_orientation = value;
			}
		}

		[Category("Behavior"), DefaultValue(LineCreateMode.Line), Description("Determines what type of lines are created when lines are automatically created.")]
		public virtual LineCreateMode LineMode
		{
			get
			{
				return _lineMode;
			}
			set
			{
				_lineMode = value;
			}
		}

		[Category("Behavior"), DefaultValue(ShapeCreateMode.Shape), Description("Determines what type of lines are created when lines are automatically created.")]
		public virtual ShapeCreateMode ShapeMode
		{
			get
			{
				return _shapeMode;
			}
			set
			{
				_shapeMode = value;
			}
		}

		[Category("Behavior"), DefaultValue(PortCreateMode.None), Description("Determines whether ports are automatically created for chapes which have been created according to the shape mode.")]
		public virtual PortCreateMode PortMode
		{
			get
			{
				return _portMode;
			}
			set
			{
				_portMode = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual FlowchartStencil Stencil
		{
			get
			{
				return _flowchartStencil;
			}
		}

		//Methods
		[Description("Adds a flowcharting shape to the diagram")]
		public virtual Shape AddFlowShape(PointF location, FlowchartStencilType type)
		{
			StencilItem stencil = Stencil[type];
			Shape shape = CreateElement(ShapeMode, location, type);
			Model.Shapes.Add(Model.Shapes.CreateKey(),shape);
			return shape;
		}

		[Description("Adds a flowcharting shape with the specified key to the diagram")]
		public virtual Shape AddFlowShape(string key, PointF location, FlowchartStencilType type)
		{
			StencilItem stencil = Stencil[type];
			Shape shape = CreateElement(ShapeMode, location, type);
			Model.Shapes.Add(key, shape);
			return shape;
		}

		[Description("Adds a line to the diagram")]
		public virtual Link AddFlowLine(PointF start, PointF end)
		{
			Link line = CreateElement(LineMode,start,end);
			Model.Lines.Add(Model.Lines.CreateKey(),line);
			line.End.Marker = new Arrow(false);
			return line;
		}

		[Description("Adds a line with the specified key to the diagram")]
		public virtual Link AddFlowLine(string key, PointF start, PointF end)
		{
			Link line = CreateElement(LineMode, start, end);
			Model.Lines.Add(key, line);
			
			line.End.Marker = new Arrow(false);
			return line;
		}

		[Description("Adds a line with the specified key to the diagram")]
		public virtual Link AddFlowLine(Shape start, Shape end)
		{
			Link line = CreateElement(LineMode);
			line.Start.Shape = start;
			line.End.Shape = end;
			Model.Lines.Add(Model.Lines.CreateKey(),line);
			
			line.End.Marker = new Arrow(false);
			return line;
		}

		[Description("Adds a line with the specified key to the diagram")]
		public virtual Link AddFlowLine(string key, Shape start, Shape end)
		{
			Link line = CreateElement(LineMode);
			line.Start.Shape = start;
			line.End.Shape = end;
			Model.Lines.Add(key,line);
			
			line.End.Marker = new Arrow(false);
			return line;
		}
		
		[Description("Adds a line with the specified key to the diagram")]
		public virtual Link AddFlowLine(Port start, Port end)
		{
			Link line = CreateElement(LineMode);
			line.Start.Port = start;
			line.End.Port = end;
			Model.Lines.Add(Model.Lines.CreateKey(),line);
			
			line.End.Marker = new Arrow(false);
			return line;
		}

		[Description("Adds a line with the specified key to the diagram")]
		public virtual Link AddFlowLine(string key, Port start, Port end)
		{
			Link line = CreateElement(LineMode);
			line.Start.Port = start;
			line.End.Port = end;
			Model.Lines.Add(key,line);
			
			line.End.Marker = new Arrow(false);
			return line;
		}

		[Description("Creates a new process for the specified parent shape.")]
		public virtual Process AddProcess(Shape parent)
		{
			if (! Model.Shapes.ContainsKey(parent.Key)) throw new ArgumentException("Parent shape not found in active container. Process could not be added.","parent");

			return AddProcessImplementation(Model.Shapes.CreateKey(), parent,null, FlowchartStencilType.Process2);
		}

		[Description("Creates a new process with the specified key and parent.")]
		public virtual Process AddProcess(string key, Shape parent)
		{
			if (! Model.Shapes.ContainsKey(parent.Key)) throw new ArgumentException("Parent shape not found in active container. Process could not be added.","parent");

			return AddProcessImplementation(key, parent,null,FlowchartStencilType.Process2);
		}

		[Description("Creates a new process of the specified stencil type for the parent shape.")]
		public virtual Process AddProcess(Shape parent,FlowchartStencilType type)
		{
			if (! Model.Shapes.ContainsKey(parent.Key)) throw new ArgumentException("Parent shape not found in active container. Process could not be added.","parent");

			return AddProcessImplementation(Model.Shapes.CreateKey(), parent, null, type);
		}

		[Description("Creates a new process of the specified stencil type with the specified key and parent.")]
		public virtual Process AddProcess(string key, Shape parent, FlowchartStencilType type)
		{
			if (! Model.Shapes.ContainsKey(parent.Key)) throw new ArgumentException("Parent shape not found in active container. Process could not be added.","parent");

			return AddProcessImplementation(key, parent, null, type);
		}

		[Description("Creates a new process with the specified key and parent.")]
		public virtual Process AddProcess(string key, Port port)
		{
			//Validate the parent
			if (! (port.Parent is Group))
			{
				Shape parent = (Shape) port.Parent;
				if (! Model.Shapes.ContainsKey(parent.Key)) throw new ArgumentException("Parent shape not found in active container. Process could not be added.","parent");
			}

			return AddProcessImplementation(key, null, port, FlowchartStencilType.Process2);
		}

		[Description("Creates a new process of the specified stencil type for the parent shape.")]
		public virtual Process AddProcess(Port port, FlowchartStencilType type)
		{
			//Validate the parent
			if (! (port.Parent is Group))
			{
				Shape parent = (Shape) port.Parent;
				if (! Model.Shapes.ContainsKey(parent.Key)) throw new ArgumentException("Parent shape not found in active container. Process could not be added.","parent");
			}

			return AddProcessImplementation(Model.Shapes.CreateKey(), null, port, type);
		}

		[Description("Creates a new process of the specified stencil type with the specified key and parent.")]
		public virtual Process AddProcess(string key, Port port, FlowchartStencilType type)
		{
			//Validate the parent
			if (! (port.Parent is Group))
			{
				Shape parent = (Shape) port.Parent;
				if (! Model.Shapes.ContainsKey(parent.Key)) throw new ArgumentException("Parent shape not found in active container. Process could not be added.","parent");
			}

			return AddProcessImplementation(key, null, port, type);
		}

		[Description("Creates a new loop line between two existing shapes.")]
		public virtual Connector AddLoop(Shape from, Shape to)
		{
			if (! Model.Shapes.ContainsKey(from.Key)) throw new ArgumentException("From Shape was not found in active container. Line could not be added.","from");
			if (! Model.Shapes.ContainsKey(to.Key)) throw new ArgumentException("To Shape was not found in active container. Line could not be added.","to");

			Connector line = Controller.Factory.CreateConnector();
			line.Start.Shape = from;
			line.End.Shape = to;
			line.Avoid = false;
			line.End.Marker = new Arrow(false);

			Model.Lines.Add(Model.Lines.CreateKey(),line);
			return line;
		}
		
		[Description("Creates a new loop line between two existing shapes.")]
		public virtual Connector AddLoop(string key, Shape from, Shape to)
		{
			if (! Model.Shapes.ContainsKey(from.Key)) throw new ArgumentException("From Shape was not found in active container. Line could not be added.","from");
			if (! Model.Shapes.ContainsKey(to.Key)) throw new ArgumentException("To Shape was not found in active container. Line could not be added.","to");

			Connector line = Controller.Factory.CreateConnector();
			line.Start.Shape = from;
			line.End.Shape = to;
			line.Avoid = false;
			line.End.Marker = new Arrow(false);

			Model.Lines.Add(key,line);
			return line;
		}

		[Description("Creates a new loop line between two existing shapes.")]
		public virtual Connector AddLoop(string key, Shape from, Shape to, SizeF padding)
		{
			if (! Model.Shapes.ContainsKey(from.Key)) throw new ArgumentException("From Shape was not found in active container. Line could not be added.","from");
			if (! Model.Shapes.ContainsKey(to.Key)) throw new ArgumentException("To Shape was not found in active container. Line could not be added.","to");

			Connector line = Controller.Factory.CreateConnector();
			line.Start.Shape = from;
			line.End.Shape = to;
			line.Avoid = false;
			line.Padding = padding;
			line.End.Marker = new Arrow(false);

			Model.Lines.Add(key,line);
			return line;
		}

		[Description("Creates a new loop line between two existing shapes.")]
		public virtual Connector AddLoop(Port from, Port to)
		{
			Connector line = Controller.Factory.CreateConnector();
			line.Start.Port = from;
			line.End.Port = to;
			line.Avoid = false;
			line.End.Marker = new Arrow(false);

			Model.Lines.Add(Model.Lines.CreateKey(),line);
			return line;
		}
		
		[Description("Creates a new loop line between two existing shapes.")]
		public virtual Connector AddLoop(string key, Port from, Port to)
		{
			Connector line = Controller.Factory.CreateConnector();
			line.Start.Port = from;
			line.End.Port = to;
			line.Avoid = false;
			line.End.Marker = new Arrow(false);

			Model.Lines.Add(key,line);
			return line;
		}

		[Description("Creates a new loop line between two existing shapes.")]
		public virtual Connector AddLoop(string key, Port from, Port to, SizeF padding)
		{
			Connector line = Controller.Factory.CreateConnector();
			line.Start.Port = from;
			line.End.Port = to;
			line.Avoid = false;
			line.Padding = padding;
			line.End.Marker = new Arrow(false);

			Model.Lines.Add(key,line);
			return line;
		}

		[Description("Creates a new group linked to an existing shape.")]
		public virtual SubChart AddSubChart(Shape parent)
		{
			return AddSubChartImplementation(Model.Shapes.CreateKey(),parent,null);
		}

		[Description("Creates a new group.")]
		public virtual SubChart AddSubChart(string key, Shape parent)
		{
			return AddSubChartImplementation(key,parent,null);
		}

		[Description("Creates a new group linked to an existing shape.")]
		public virtual SubChart AddSubChart(Port port)
		{
			return AddSubChartImplementation(Model.Shapes.CreateKey(),null,port);
		}

		[Description("Creates a new group.")]
		public virtual SubChart AddSubChart(string key, Port port)
		{
			return AddSubChartImplementation(key,null,port);
		}

		[Description("Adds a decision to the current flowchart.")]
		public virtual Decision AddDecision(Shape parent, string decisionKey,string trueKey, FlowchartStencilType trueType, string falseKey, FlowchartStencilType falseType)
		{
			if (! Model.Shapes.ContainsKey(parent.Key)) throw new ArgumentException("Parent Shape not found in active container. Decision could not be added.","parent");
			return AddDecisionImplementation(parent,null, null, decisionKey, trueKey, trueType, falseKey, falseType);
		}

		[Description("Adds decision true and false elements to the current flowchart.")]
		public virtual Decision AddDecision(Shape decision, string trueKey, FlowchartStencilType trueType, string falseKey, FlowchartStencilType falseType)
		{
			if (! Model.Shapes.ContainsKey(decision.Key)) throw new ArgumentException("Decision Shape not found in active container. Decision could not be added.","decision");
			return AddDecisionImplementation(null,null,decision,"", trueKey, trueType, falseKey, falseType);
		}

		[Description("Adds a decision to the current flowchart.")]
		public virtual Decision AddDecision(Port port, string decisionKey,string trueKey, FlowchartStencilType trueType, string falseKey, FlowchartStencilType falseType)
		{
			//Validate the parent
			if (! (port.Parent is Group))
			{
				Shape parent = (Shape) port.Parent;
				if (! Model.Shapes.ContainsKey(parent.Key))	throw new ArgumentException("Parent Shape not found in active container. Decision could not be added.","parent");
			}
			return AddDecisionImplementation(null, port, null, decisionKey, trueKey, trueType, falseKey, falseType);
		}

		#endregion

		#region Implementation

		//Adds a process shape and line and returns a process
		private Process AddProcessImplementation(string key, Shape parent, Port port, FlowchartStencilType type)
		{
			PointF location = new PointF();
			Shape flowShape = null;
			Solid start = (port == null) ? (Solid) parent : (Solid) port;

			//Set the location
			if (Orientation == FlowchartOrientation.Vertical)
			{
				location = new PointF(start.Location.X, start.Location.Y + Spacing.Height); 
				if (port == null) location.Y += parent.Bounds.Height;
			}
			else
			{
				location = new PointF(start.Location.X + Spacing.Width, start.Location.Y);
				if (port == null) location.X += parent.Bounds.Width;
			}

			flowShape = AddFlowShape(key, location, type);

			//Offset shape depending on size
			if (Orientation == FlowchartOrientation.Vertical)
			{
				flowShape.Location = new PointF(location.X + (start.Bounds.Width - flowShape.Bounds.Width) / 2,location.Y);
			}
			else
			{
				flowShape.Location = new PointF(location.X, location.Y + (start.Bounds.Height - flowShape.Bounds.Height) / 2);
			}

			//Add connecting flowline
			Link line = CreateElement(LineMode);
			if (port == null)
			{
				line.Start.Shape = parent;
			}
			else
			{
				line.Start.Port = port;
			}	
			line.End.Shape = flowShape;
			line.End.Marker = new Arrow(false);

			Model.Lines.Add(Model.Lines.CreateKey(),line);

			//Create the process object and return it 
			Process process = new Process();

			process.Line = line;
			process.Shape = flowShape;
			process.Port = port;

			return process;
		}

		//Adds a process shape and line and returns a process
		private SubChart AddSubChartImplementation(string key, Shape parent, Port port)
		{
			SubChart subChart = new SubChart();
			PointF location = new PointF();
			Group group = Controller.Factory.CreateGroup();
			Solid start = (port == null) ? (Solid) parent : (Solid) port;

			//Set the location
			if (Orientation == FlowchartOrientation.Vertical)
			{
				location = new PointF(start.Location.X, start.Location.Y + Spacing.Height);
				if (port == null) location.Y += parent.Bounds.Height;
			}
			else
			{
				location = new PointF(start.Location.X  + start.Bounds.Width + Spacing.Width, start.Location.Y);
				if (port == null) location.X += parent.Bounds.Width;
			}
			
			group.Location = location;
			Model.Shapes.Add(key,group);

			//Offset the group
			if (Orientation == FlowchartOrientation.Vertical)
			{
				group.Location = new PointF(location.X + (start.Bounds.Width - group.Bounds.Width) / 2,location.Y);
			}
			else
			{
				group.Location = new PointF(location.X,location.Y + (start.Bounds.Height - group.Bounds.Height) / 2);
			}

			//Next add two ports depending on the orientation
			Port newport = null;
			if (Orientation == FlowchartOrientation.Vertical)
			{
				newport = new Port(PortOrientation.Top); //Keep reference to add line
				group.Ports.Add("top",newport);
				
				group.Ports.Add("bottom",new Port(PortOrientation.Bottom));
			}
			else
			{
				newport = new Port(PortOrientation.Left); //Keep reference to add line
				group.Ports.Add("left",newport);

				group.Ports.Add("right",new Port(PortOrientation.Right));
			}
			
			//Finally link the group to the parent shape
			Link line = CreateElement(LineMode);
			
			if (port == null)
			{
				line.Start.Shape = parent;
			}
			else
			{
				line.Start.Port = port;
			}
			line.End.Port = newport;
			
			line.End.Marker = new Arrow(false);
			Model.Lines.Add(Model.Lines.CreateKey(),line);
			
			//Set up subchart
			subChart.Line = line;
			subChart.Group = group;
			subChart.Port = port;
			return subChart;
		}

		private Decision AddDecisionImplementation(Shape parentShape, Port port, Shape decisionShape, string decisionKey, string trueKey, FlowchartStencilType trueType, string falseKey, FlowchartStencilType falseType)
		{
			Decision decision = new Decision();
			Process process = null;

			//Set the autolayout flag to false
			Suspend();

			//Determine whether to create decision
			if (decisionShape == null)
			{
				//Create a new decision key if not provided
				if (decisionKey == "") decisionKey = Model.Shapes.CreateKey();

				//Add the decision shape and line
				if (port == null)
				{
					process = AddProcess(decisionKey, parentShape, FlowchartStencilType.Decision);
				}
				else
				{
					process = AddProcess(decisionKey, port, FlowchartStencilType.Decision);
				}
				decision.DecisionShape = process.Shape;
				decision.DecisionLine = process.Line;
			}
			else
			{
				decision.DecisionShape = decisionShape;
			}

			//Add the true branch
			process = AddProcess(trueKey, decision.DecisionShape, trueType);

			decision.TrueShape = process.Shape;
			decision.TrueLine = process.Line;

			//Add the true branch by swapping the orientation around and then swapping back
			Orientation = (Orientation == FlowchartOrientation.Vertical) ? Orientation = FlowchartOrientation.Horizontal : Orientation = FlowchartOrientation.Vertical;
			process = AddProcess(falseKey, decision.DecisionShape, falseType);
			Orientation = (Orientation == FlowchartOrientation.Vertical) ? Orientation = FlowchartOrientation.Horizontal : Orientation = FlowchartOrientation.Vertical;

			decision.FalseShape = process.Shape;
			decision.FalseLine = process.Line;
			decision.Port = port;

			Resume();
			if (! Suspended) Refresh();

			return decision;
		}

		protected virtual Link CreateElement(LineCreateMode mode)
		{
			if (mode == LineCreateMode.Line) return Controller.Factory.CreateLine();
			if (mode == LineCreateMode.Connector) return Controller.Factory.CreateConnector();
			return Controller.Factory.CreateCurve();
		}

		protected virtual Link CreateElement(LineCreateMode mode, PointF start, PointF end)
		{
			if (mode == LineCreateMode.Line) return Controller.Factory.CreateLine(start, end);
			if (mode == LineCreateMode.Connector) return Controller.Factory.CreateConnector(start, end);
			return Controller.Factory.CreateCurve(start, end);
		}

		protected virtual Shape CreateElement(ShapeCreateMode mode)
		{
			Shape shape = null;
			if (mode == ShapeCreateMode.Shape) shape = Controller.Factory.CreateShape();
			if (mode == ShapeCreateMode.Table) shape = Controller.Factory.CreateTable();
			if (mode == ShapeCreateMode.ComplexShape) shape = Controller.Factory.CreateComplexShape();
			
			//Add ports if required
			AddPorts(shape);

			return Controller.Factory.CreateComplexShape();
		}

		protected virtual Shape CreateElement(ShapeCreateMode mode, PointF location, FlowchartStencilType type )
		{
			Shape shape = null;
			if (mode == ShapeCreateMode.Shape) shape = Controller.Factory.CreateShape(location,new Size());
			if (mode == ShapeCreateMode.Table) shape = Controller.Factory.CreateTable(location, new Size());
			if (mode == ShapeCreateMode.ComplexShape) shape = Controller.Factory.CreateComplexShape(location, new Size());

			shape.Location = location;
			FlowchartStencil stencil = (FlowchartStencil) Singleton.Instance.GetStencil(typeof(FlowchartStencil));
			shape.StencilItem = stencil[type];

			//Add ports if required
			AddPorts(shape);

			return shape;
		}

		protected virtual void AddPorts(Shape shape)
		{
			if (PortMode == PortCreateMode.None) return;

			//Top and bottom or all
			if (PortMode == PortCreateMode.TopBottom || PortMode == PortCreateMode.All)
			{
				Port port = Controller.Factory.CreatePort(PortOrientation.Top);
				port.Style = PortStyle.Simple;
				port.Visible = false;
				shape.Ports.Add("top",port);

				port = Controller.Factory.CreatePort(PortOrientation.Bottom);
				port.Style = PortStyle.Simple;
				port.Visible = false;
				shape.Ports.Add("bottom",port);
			}

			//Left and right or all
			if (PortMode == PortCreateMode.LeftRight || PortMode == PortCreateMode.All)
			{
				Port port = Controller.Factory.CreatePort(PortOrientation.Left);
				port.Style = PortStyle.Simple;
				port.Visible = false;
				shape.Ports.Add("left",port);

				port = Controller.Factory.CreatePort(PortOrientation.Right);
				port.Style = PortStyle.Simple;
				port.Visible = false;
				shape.Ports.Add("right",port);
			}
		}

		#endregion
	}

}
