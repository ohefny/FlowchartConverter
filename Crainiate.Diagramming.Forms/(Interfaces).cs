// (c) Copyright Crainiate Software 2010




using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Crainiate.Diagramming.Forms
{
    //Interface for a designer control for editing labels at runtime
	public interface ILabelEdit
	{
		StringFormat StringFormat {get; set;}
		string Text {get; set;}
		bool Visible {get; set;}
		bool Cancelled {get;}
		float Zoom {get; set;}
		AutoSizeMode AutoSizeMode {get; set;}

		void SendEnd();
		void SendHome();

		event EventHandler Complete;
		event EventHandler Cancel;
	}
}

