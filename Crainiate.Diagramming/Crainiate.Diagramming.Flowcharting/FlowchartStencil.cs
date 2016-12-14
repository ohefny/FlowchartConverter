// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Flowcharting;

namespace Crainiate.Diagramming.Flowcharting
{
	public class FlowchartStencil: Stencil
	{
		#region Interface

		//Constructors
		public FlowchartStencil()
		{
			CreateStencilItems();
		}

		public virtual StencilItem this[FlowchartStencilType type]  
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
			foreach (string type in Enum.GetNames(typeof(FlowchartStencilType)))
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
			FlowchartStencilType stencilType = (FlowchartStencilType) Enum.Parse(typeof(FlowchartStencilType), stencil.Key); 

			float width = e.Width;
			float height = e.Height;
			float percX = 0;
			float percY = 0;
			float perc = 0;
			float midX = 0;
			float midY = 0;

			rect.Width = width;
			rect.Height = height;
			midX = width / 2;
			midY = height / 2;

			if (stencilType == FlowchartStencilType.Default)
			{
				percX = 20;
				percY = 20;

				path.AddArc(0, 0, percX, percY, 180, 90);
				path.AddArc(width - percX, 0, percX, percY, 270, 90);
				path.AddArc(width - percX, height - percY, percX, percY, 0, 90);
				path.AddArc(0, height - percY, percX, percY, 90, 90);
				path.CloseFigure();

				stencil.Redraw = true;
			}
			else if (stencilType == FlowchartStencilType.Card)
			{
				percX = width * 0.2F;
				percY = height * 0.2F;

				path.AddLine(percX, 0, width, 0);
				path.AddLine(width, 0, width, height);
				path.AddLine(width, height, 0, height);
				path.AddLine(0, height, 0, percY);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Collate)
			{
				path.AddLine(0, 0, width, 0);
				path.AddLine(width, 0, 0, height);
				path.AddLine(0, height, width, height);
				path.AddLine(width, height, 0, 0);
			}
			else if (stencilType == FlowchartStencilType.Connector)
			{
				percX = width * 0.5F;
				percY = height * 0.5F;

				path.AddEllipse(percX, percY, width, height);
			}
			else if (stencilType == FlowchartStencilType.Data)
			{
				path.AddLine(midX / 2, 0, width, 0);
				path.AddLine(width, 0, (midX / 2) + midX, height);
				path.AddLine((midX / 2) + midX, height, 0, height);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Decision)
			{
				path.AddLine(midX, 0, width, midY);
				path.AddLine(width, midY, midX, height);
				path.AddLine(midX, height, 0, midY);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Delay)
			{
				percX = width * 0.2F;

				path.AddArc(percX, 0 , width * 0.8F, height, 270, 180);
				path.AddLine(0, height, 0, 0);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Display)
			{
				percX = width * 0.2F;

				path.AddArc(percX, 0 , width * 0.8F, height, 270, 180);
				
				path.AddLine(percX, height, 0, height / 2);
				path.AddLine(0, height / 2,percX, 0);
				
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Direct)
			{
				percX = width * 0.3F;

				path.AddLine(width * 0.7F, height, percX, height);
				path.AddArc(0, 0, percX, height, 90, 180);
				path.AddLine(width * 0.7F, 0, percX, 0);
				path.AddArc(width * 0.7F, 0, percX, height, 270, -180);
				path.CloseFigure();

				path.StartFigure();
				path.AddEllipse(width * 0.7F, 0, percX, height);
			}
			else if (stencilType == FlowchartStencilType.Document)
			{
				PointF[] points = new PointF[4];

				points[0].X = 0;
				points[0].Y = height * 0.95F;
				points[1].X = width * 0.25F;
				points[1].Y = height;
				points[2].X = width * 0.75F;
				points[2].Y = height * 0.85F;
				points[3].X = width;
				points[3].Y = height * 0.8F;

				path.AddLine(new Point(0, 0), points[0]);
				path.AddCurve(points);
				path.AddLine(points[3], new PointF(width, 0));
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Extract)
			{
				percX = width * 0.25F;
				percY = height * 0.75F;

				path.AddLine(percX, 0, percX * 2, percY);
				path.AddLine(percX * 2, percY, 0, percY);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.InternalStorage)
			{
				perc = width * 0.15F;

				path.AddRectangle(rect);
				path.AddLine(perc, 0, perc, height);
				path.CloseFigure();

				path.AddLine(0, perc, width, perc);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.ManualInput)
			{
				percY = height * 0.25F;

				path.AddLine(0, percY, width, 0);
				path.AddLine(width, 0, width, height);
				path.AddLine(width, height, 0, height);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.ManualOperation)
			{
				percX = width * 0.2F;

				path.AddLine(0, 0, width, 0);
				path.AddLine(width, 0, width - percX, height);
				path.AddLine(width - percX, height, percX, height);

				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.MultiDocument)
			{

				PointF[] points = new PointF[4];

				width = width * 0.8F;

				points[0].X = 0;
				points[0].Y = height * 0.95F;
				points[1].X = width * 0.25F;
				points[1].Y = height;
				points[2].X = width * 0.75F;
				points[2].Y = height * 0.85F;
				points[3].X = width;
				points[3].Y = height * 0.8F;

				path.AddLine(new PointF(0, height * 0.2F), points[0]);
				path.AddCurve(points);
				path.AddLine(points[3], new PointF(width, height * 0.2F));
				path.CloseFigure();

				width = rect.Width;

				path.AddLine(width * 0.2F, height * 0.1F, width * 0.2F, 0);
				path.AddLine(width * 0.2F, 0, width, 0);
				path.AddLine(width, 0, width, height * 0.6F);
				path.AddLine(width, height * 0.6F, width * 0.9F, height * 0.6F);
				path.AddLine(width * 0.9F, height * 0.6F, width * 0.9F, height * 0.1F);
				path.CloseFigure();

				path.AddLine(width * 0.1F, height * 0.2F, width * 0.1F, height * 0.1F);
				path.AddLine(width * 0.1F, height * 0.1F, width * 0.9F, height * 0.1F);
				path.AddLine(width * 0.9F, height * 0.1F, width * 0.9F, height * 0.7F);
				path.AddLine(width * 0.9F, height * 0.7F, width * 0.8F, height * 0.7F);
				path.AddLine(width * 0.8F, height * 0.7F, width * 0.8F, height * 0.2F);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.OffPageConnector)
			{
				percX = width * 0.5F;
				percY = height * 0.75F;

				path.AddLine(0, 0, width, 0);
				path.AddLine(width, 0, width , percY);
				path.AddLine(width, percY, percX, height);
				path.AddLine(percX, height, 0, percY);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.PredefinedProcess)
			{
				perc = width * 0.1F;

				path.AddRectangle(rect);
				path.CloseFigure();

				path.AddLine(perc, 0, perc, height);
				path.CloseFigure();

				path.AddLine(width - perc, 0, width - perc, height);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Preparation)
			{
				percX = width * 0.2F;

				path.AddLine(0, midY, percX, 0);
				path.AddLine(percX, 0, width - percX, 0);
				path.AddLine(width - percX, 0, width, midY);
				path.AddLine(width, midY, width - percX, height);
				path.AddLine(width - percX, height, percX, height);

				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Process)
			{
				path.AddRectangle(new RectangleF(0, 0, width, height));
			}
			else if (stencilType == FlowchartStencilType.Process2)
			{

				percX = width * 0.2F;
				percY = height * 0.2F;

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
			}
			else if (stencilType == FlowchartStencilType.Terminator)
			{
				percX = width * 0.5F;
				percY = height * 0.20F;

				path.AddArc(0, percY, percX, percY * 2, 90, 180);
				path.AddArc(width - percX, percY, percX, percY * 2, 270, 180);
				path.CloseFigure();
				
				stencil.KeepAspect = true;
			}
			else if (stencilType == FlowchartStencilType.Tape)
			{
				PointF[] points = new PointF[5];
				PointF[] pointsBottom = new PointF[5];

				points[0].X = 0;
				points[0].Y = height * 0.05F;
				points[1].X = width * 0.25F;
				points[1].Y = height * 0.1F;
				points[2].X = width * 0.5F;
				points[2].Y = height * 0.05F;
				points[3].X = width * 0.75F;
				points[3].Y = 0;
				points[4].X = width;
				points[4].Y = height * 0.05F;

				pointsBottom[4].X = 0;
				pointsBottom[4].Y = height * 0.95F;
				pointsBottom[3].X = width * 0.25F;
				pointsBottom[3].Y = height;
				pointsBottom[2].X = width * 0.5F;
				pointsBottom[2].Y = height * 0.95F;
				pointsBottom[1].X = width * 0.75F;
				pointsBottom[1].Y = height * 0.9F;
				pointsBottom[0].X = width;
				pointsBottom[0].Y = height * 0.95F;

				path.AddCurve(points);
				path.AddLine(points[4], pointsBottom[0]);

				path.AddCurve(pointsBottom);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Junction)
			{
				percX = width * 0.25F;
				percY = height * 0.25F;

				if (percX > percY)
				{
					perc = percX;
				}
				else
				{
					perc = percY;
				}

				path.AddEllipse(perc, perc, perc * 2, perc * 2);
				path.CloseFigure();

				path.StartFigure();
				path.AddLine(perc * 2, perc, perc * 2, perc * 3);
				
				path.StartFigure();
				path.AddLine(perc, perc * 2, perc * 3, perc * 2);
				

				Matrix matrix = new Matrix(1, 0, 0, 1, 0, 0);

				//Rotate the matrix through 45 degress
				perc = perc * 2;
				matrix.RotateAt(45, new PointF(perc, perc));

				//Transform the graphicspath object
				path.Transform(matrix);
			}
			else if (stencilType == FlowchartStencilType.LogicalOr)
			{
				percX = width * 0.5F;
				percY = height * 0.5F;

				if (percX > percY)
				{
					perc = percX;
				}
				else
				{
					perc = percY;
				}

				path.AddEllipse(perc, perc, perc * 2, perc * 2);
				path.AddLine(perc * 2, perc, perc * 2, perc * 3);
				path.CloseFigure();
				path.AddLine(perc, perc * 2, perc * 3, perc * 2);
			}
			else if (stencilType == FlowchartStencilType.Sort)
			{
				percX = width * 0.5F;
				percY = height * 0.5F;

				path.AddLine(0, percY, percX, 0);
				path.AddLine(percX, 0, width, percY);
				
				path.AddLine(width, percY, percX, height);
				path.AddLine(percX, height, 0, percY);
				
				path.CloseFigure();
				path.StartFigure();
				
				path.AddLine(0,percY, width, percY);


			}
			else if (stencilType == FlowchartStencilType.Merge)
			{
				path.AddLine(0, 0, width, 0);
				path.AddLine(width, 0, width * 0.5F, height);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.StoredData)
			{
				percX = width * 0.3F;

				path.AddArc(0, 0, percX, height, 90, 180);
				path.AddArc(width * 0.85F, 0, percX, height, 270, -180);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Sequential)
			{
				if (width > height)
				{
					perc = height;
				}
				else
				{
					perc = width;
				}

				path.AddArc(0, 0, perc, perc, 45, -315);
				path.AddLine(perc * 0.5F, perc, perc, perc);
				path.AddLine(perc, perc, perc, perc * 0.85F);
				path.CloseFigure();
			}
			else if (stencilType == FlowchartStencilType.Magnetic)
			{
				percY = height * 0.4F;

				path.AddArc(0, - height * 0.2F, width, percY, 180, -180);
				path.AddLine(width, 0, width, height * 0.8F);
				path.AddArc(0, height * 0.6F, width, percY, 0, 180);
				path.AddLine(0, height * 0.8F, 0, 0);
				//path.CloseFigure();

				//Define two shapes so that not see through
				path.StartFigure();
				path.AddLine(0, 0, width, 0);
				path.AddArc(0, - height * 0.2F, width, percY, 0, 180);
			}
		}
		#endregion
	}
}

