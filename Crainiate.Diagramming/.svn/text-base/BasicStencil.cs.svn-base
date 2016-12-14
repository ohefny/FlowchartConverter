// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public class BasicStencil: Stencil
	{
		#region Interface

		//Constructors
		public BasicStencil()
		{
			CreateStencilItems();
		}

		public virtual StencilItem this[BasicStencilType type]  
		{
			get  
			{
				string key = type.ToString();
				return (StencilItem) Dictionary[key];
			}
			set  
			{
				string key = type.ToString();
				Dictionary[key] = value;
			}
		}

		#endregion

		#region Events
		
		private void StencilItem_DrawShape(object sender, DrawShapeEventArgs e)
		{
			StencilItem stencil = (StencilItem) sender;
			DrawStencilItem(stencil,e);
		}

		#endregion

		#region Implementation

		private void CreateStencilItems()
		{
			SetModifiable(true);

			//Loop through each enumeration and add as a stencil item
			foreach (string type in Enum.GetNames(typeof(BasicStencilType)))
			{
				StencilItem item = new StencilItem();
				item.DrawShape +=new DrawShapeEventHandler(StencilItem_DrawShape);

				Add(type,item);
			}

			SetModifiable(false);
		}

		private void DrawStencilItem(StencilItem stencil, DrawShapeEventArgs e)
		{
			GraphicsPath path = e.Path;
			RectangleF rect = new Rectangle();
			BasicStencilType stencilType = (BasicStencilType) Enum.Parse(typeof(BasicStencilType),stencil.Key); 

			float width = e.Width;
			float height = e.Height;
			float percX = 0;
			float percY = 0;
			float midX = 0;
			float midY = 0;

			rect.Width = width;
			rect.Height = height;

			if (stencilType == BasicStencilType.BottomTriangle)
			{
				path.AddLine(0, 0, width, 0);
				path.AddLine(width, 0, width / 2, height);
				path.CloseFigure();
			}
			else if (stencilType == BasicStencilType.Circle)
			{
				percX = Convert.ToSingle(width * 0.5);
				percY = Convert.ToSingle(height * 0.5);

				path.AddEllipse(percX, percY, width, height);
			}
			else if (stencilType == BasicStencilType.Diamond)
			{

				percX = Convert.ToSingle(width * 0.5);
				percY = Convert.ToSingle(height * 0.5);

				path.AddLine(0, percY, percX, 0);
				path.AddLine(percX, 0, width, percY);
				path.AddLine(width, percY, percX, height);
				path.AddLine(percX, height, 0, percY);

			}
			else if (stencilType == BasicStencilType.Ellipse)
			{
				percX = Convert.ToSingle(width * 0.5);
				percY = Convert.ToSingle(height * 0.5);

				path.AddEllipse(percX, percY, width, height);
			}
			else if (stencilType == BasicStencilType.LeftTriangle)
			{
				percX = Convert.ToSingle(width * 0.5);
				percY = Convert.ToSingle(height * 0.5);

				path.AddLine(width, 0, 0, percY);
				path.AddLine(0, percY, width, height);
				path.CloseFigure();
			}
			else if (stencilType == BasicStencilType.Octagon)
			{
				float thirdx = width * 0.3F;
				float twothirdx = width * 0.7F;
				float thirdy = height * 0.3F;
				float twothirdy = height *0.7F;
				
				path.AddLine(thirdx, 0, twothirdx, 0);
				path.AddLine(twothirdx, 0, width, thirdy);
				path.AddLine(width, thirdy, width, twothirdy);
				path.AddLine(width,twothirdy, twothirdx, height);
				path.AddLine(twothirdx, height, thirdx, height);
				path.AddLine(thirdx, height, 0, twothirdy);
				path.AddLine(0, twothirdy, 0, thirdy);
				path.CloseFigure();
			}
			else if (stencilType == BasicStencilType.Rectangle)
			{
				path.AddRectangle(rect);
			}
			else if (stencilType == BasicStencilType.RightTriangle)
			{
				percY = height * 0.5F;

				path.AddLine(0, 0, width, percY);
				path.AddLine(width, percY, 0, height);
				path.CloseFigure();
			}
			else if (stencilType == BasicStencilType.RoundedRectangle)
			{
				percX = Convert.ToSingle(width * 0.2);
				percY = Convert.ToSingle(height * 0.2);

				if (percX < percY)
				{
					percY = percX;
				}
				else
				{
					percX = percY;
				}
				path.AddArc(0, 0, percX, percY, 180, 90);
				path.AddArc(width - percX, 0, percX, percY, 270, 90);
				path.AddArc(width - percX, height - percY, percX, percY, 0, 90);
				path.AddArc(0, height - percY, percX, percY, 90, 90);
				path.CloseFigure();

				stencil.Redraw = true;
			}
			else if (stencilType == BasicStencilType.TopTriangle)
			{
				percX = Convert.ToSingle(width * 0.5);

				path.AddLine(percX, 0, width, height);
				path.AddLine(width, height, 0, height);
				path.CloseFigure();
			}
		}

		#endregion

	}
}

