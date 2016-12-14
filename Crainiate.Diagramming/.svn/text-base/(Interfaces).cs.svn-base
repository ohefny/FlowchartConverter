// (c) Copyright Crainiate Software 2010

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Crainiate.Diagramming.Layouts;

namespace Crainiate.Diagramming
{
    public interface IView
    {
        void Suspend();
        void Resume();
        void Refresh();
        void Invalidate();
        void SetModel(Model model);
    }

	public interface IExpandable
	{
		bool Expanded {get; set;}
		GraphicsPath Expander {get;}
		bool DrawExpand {get; set;}
		SizeF ContractedSize {get; set;}
		SizeF ExpandedSize {get; set;}
	}

    //Capture windows forms clicks
	public interface IMouseEvents
	{
        bool ExecuteMouseCommand(MouseCommand command);
	}

	public interface ISelectable
	{
		event EventHandler SelectedChanged;
		bool DrawSelected {get; set;}
		bool Selected {get; set;}
	}

	public interface IUserInteractive
	{
		UserInteraction Interaction {get; set;}
	}

	public interface IPortContainer
	{
		Ports Ports {get; set;}

		void LocatePort(Port port);
		PortOrientation GetPortOrientation(Port port,PointF location);
		bool ValidatePortLocation(Port port,PointF location);
		float GetPortPercentage(Port port,PointF location);
        PointF Intercept(PointF location);
        PointF Forward(PointF location);
        bool Contains(PointF location);
	}

	public interface ILabelContainer
	{
		PointF GetLabelLocation();
		SizeF GetLabelSize();
	}

	public interface ITransformable
	{
		float Rotation {get; set;}
		GraphicsPath TransformPath {get;}
		RectangleF TransformRectangle {get;}
	}

    //Allows an element to maintain a link to a renderer
    public interface IRenderable
    {
        IRenderer Renderer { get; set; }
    }

    //Msrker interface to indiacte that the class is a content renderer
    public interface IRenderer
    {

    }

    public interface IReusable
    {
        void Reuse();
    }

    public interface IMomento<T>
    {
        T CreateItem();
        void WriteItem(T item);
    }

}

