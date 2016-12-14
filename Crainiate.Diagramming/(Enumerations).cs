// (c) Copyright Crainiate Software 2010




using System;

namespace Crainiate.Diagramming
{
	public enum InteractiveMode
	{
		Normal = 0,
		AddShape = 1,
		AddLine = 2,
		AddConnector = 3,
		AddComplexLine = 4,
		AddComplexShape = 5,
		AddCurve = 6
	}

	public enum Status
	{
		Default = 0,
		Updating = 1,
		DragEnter = 2,
		DragOver = 3,
		DragDrop = 4,
		DragScroll = 5,
		Animate = 6,
		UndoRedo = 7,
		Custom = 8
	}

	public enum Direction
	{
		Both = 0,
		In = 1,
		Out = 2,
		None = 3
	}

	[Flags]
	public enum UserInteraction
	{
		None = 0,
		BringToFront = 1,
		AlignToGrid = 2
	}

	public enum DiagramUnit
	{
		Pixel = 0,
		Display = 1,
		Document = 2,
		Inch =3,
		Millimeter = 4,
		Point = 5
	}

	public enum CurveType
	{
		Spline = 0,
		Bezier = 1
	}

	public enum HandleType
	{
		None = 0,
		Move = 1,
		Left = 2,
		TopLeft = 3,
		Top = 4,
		TopRight = 5,
		Right = 6,
		BottomRight = 7,
		Bottom = 8,
		BottomLeft = 9,
		Arrow = 10,
		Origin = 11,
		LeftRight = 12,
		UpDown = 13,
		Rotate = 14,
		Expand = 15
	}

	public enum MarkerStyle
	{
		Custom = 0,
		Rectangle = 1,
		Ellipse = 2,
		Diamond = 3
	}

	public enum PortStyle
	{
		Default = 0,
		Simple =1,
		Input=2,
		Output=3
	}

	public enum PortOrientation
	{
		Top = 0,
		Bottom = 1,
		Left = 2,
		Right = 3,
		None = 4
	}

	public enum PortAlignment
	{
		Center = 0,
		Inset = 1,
		Outset = 2
	}

	public enum GridStyle
	{
		Pixel = 0,
		Solid = 1,
		Dot = 2,
		DashDot = 3,
		DashDotDot = 4,
		Dash = 5,
		Complex = 6
	}

	public enum ActionStyle
	{
		Default = 0,
		Basic = 1
	}

	public enum ArrowStencilType
	{
		Left = 0,
		Right = 1,
		Up = 2,
		Down = 3,
		LeftRight = 4,
		UpDown = 5,
		LeftRightUpDown = 6,
		Striped = 7,
		Notched = 8,
		Pentagon = 9,
		Chevron = 10,
		RightCallout = 11,
		LeftCallout = 12,
		UpCallout = 13,
		DownCallout = 14
	}

	public enum BasicStencilType
	{
		Rectangle = 0,
		RoundedRectangle = 1,
		Ellipse = 2,
		Circle = 3,
		Diamond = 4,
		TopTriangle = 5,
		BottomTriangle = 6,
		LeftTriangle = 7,
		RightTriangle = 8,
		Octagon = 9
	}

	[Flags]
	public enum StencilItemOptions
	{
		None = 0,
		InnerRectangleHalf = 1,
		InnerRectangleFull = 2,
		SoftShadow = 4
	}

	public enum ExportFormat
	{
		Bmp = 1,
		Png = 2,
		Jpeg = 3,
		Gif = 4
	}

	public enum KeyCreateMode
	{
		Normal = 0,
		Guid = 1
	}

    public enum Themes
    {
        LightBlue = 0,
        Green = 1,
        Orange = 2
    }

    [Flags]
    public enum MouseCommandButtons
    {
        None = 0,
        Left = 1048576,
        Right = 2097152,
        Middle = 4194304,
        XButton1 = 8388608,
        XButton2 = 16777216,
    }
}

namespace Crainiate.Diagramming.Forms
{
	public enum RulerUnit
	{
		Pixel = 0,
		Display = 1,
		Document = 2,
		Inch =3,
		Millimeter = 4,
		Point = 5
	}

	public enum RulerOrientation
	{
		Left = 0,
		Top = 1
	}

	public enum RulerBorderStyle
	{
		None = 0,
		Edge = 1,
		Full = 2
	}

	public enum UndoAction
	{
		Add = 0,
		Edit = 1,
		Remove = 2
	}

	public enum UndoType
	{
		Shape = 0,
		Line = 1
	}

	public enum PaletteStyle
	{
		Single = 0,
		Multiple = 1
	}

	public enum ButtonStyle
	{
		None = 0,
		Up =1,
		Down = 2
	}

	public enum AutoSizeMode
	{
		None = 0,
		Vertical = 1,
		Horizontal = 2,
		Both = 3
	}
}

namespace Crainiate.Diagramming.Flowcharting
{
	public enum FlowchartOrientation
	{
		Vertical = 0,
		Horizontal = 1
	}

	public enum FlowchartStencilType
	{
		Default = 0,
		Process = 1,
		Process2 = 2,
		Decision = 3,
		Data = 4,
		PredefinedProcess = 5,
		InternalStorage = 6,
		Document = 7,
		MultiDocument = 8,
		Terminator = 9,
		Preparation = 10,
		ManualInput = 11,
		ManualOperation = 12,
		Connector = 13,
		OffPageConnector = 14,
		Card = 15,
		Tape = 16,
		Junction = 17,
		LogicalOr = 18,
		Collate = 19,
		Sort = 20,
		Extract = 21,
		Merge = 22,
		StoredData = 23,
		Delay = 24,
		Sequential = 25,
		Magnetic = 26,
		Direct = 27,
		Display = 28
	}

	public enum LineCreateMode
	{
		Line = 0,
		Connector = 1,
		Curve = 2
	}

	public enum ShapeCreateMode
	{
		Shape = 0,
		ComplexShape = 1,
		Table = 2
	}

	public enum PortCreateMode
	{
		None = 0,
		LeftRight = 1,
		TopBottom = 2,
		All = 3,
	}
}

namespace Crainiate.Diagramming.Printing
{
	public enum PageNumberStyle
	{
		None = 0,
		ColumnRow = 1,
		RowColumn = 2,
		Sequence = 3
	}

	public enum BorderStyle
	{
		None = 0,
		Solid = 1,
		Dash = 2,
		Dot = 3
	}
}

namespace Crainiate.Diagramming.Printing.PaperSizes
{
	public enum Iso
	{
		FourA = 0,
		TwoA = 1,
		A0 = 2,
		A1 = 3,
		A2 = 4,
		A3 = 5,
		A4 = 6,
		A5 = 7,
		A6 = 8,
		A7 = 9,
		A8 = 10,
		A9 = 11,
		A10 = 12,
		FourB = 13,
		TwoB = 14,
		B0 = 15,
		B1 = 16,
		B2 = 17,
		B3 = 18,
		B4 = 19,
		B5 = 20,
		B6 = 21,
		B7 = 22,
		B8 = 23,
		B9 = 24,
		B10 = 25,
		C0 = 26,
		C1 = 27,
		C2 = 28,
		C3 = 29,
		C4 = 30,
		C5 = 31,
		C6 = 32,
		C7 = 33,
		C8 = 34,
		C9 = 35,
		C10 = 36
	}

	public enum Jis
	{
		FourA = 0,
		TwoA = 1,
		A0 = 2,
		A1 = 3,
		A2 = 4,
		A3 = 5,
		A4 = 6,
		A5 = 7,
		A6 = 8,
		A7 = 9,
		A8 = 10,
		A9 = 11,
		A10 = 12,
		B0 = 13,
		B1 = 14,
		B2 = 15,
		B3 = 16,
		B4 = 17,
		B5 = 18,
		B6 = 19,
		B7 = 20,
		B8 = 21,
		B9 = 22,
		B10 = 23
	}
}