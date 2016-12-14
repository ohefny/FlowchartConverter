// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public class ArrowStencil: Stencil
	{
		#region Interface

		//Constructors
		public ArrowStencil()
		{
			CreateStencilItems();
		}

		public virtual StencilItem this[ArrowStencilType type]  
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
			foreach (string type in Enum.GetNames(typeof(ArrowStencilType)))
			{
				StencilItem item = new StencilItem();
				item.DrawShape +=new DrawShapeEventHandler(StencilItem_DrawShape);

				Add(type,item);
			}

			SetModifiable(false);
		}

		private void DrawStencilItem(StencilItem stencil, DrawShapeEventArgs e)
		{
			ArrowStencilType stencilType = (ArrowStencilType) Enum.Parse(typeof(ArrowStencilType),stencil.Key); 
			Matrix translateMatrix = new Matrix();

			if (stencilType == ArrowStencilType.Left || stencilType == ArrowStencilType.Right || stencilType == ArrowStencilType.Up || stencilType == ArrowStencilType.Down)
			{
				//Draw the arrow using a 100x100 grid 
				e.Path.AddLine(0,30,60,30);
				e.Path.AddLine(60,30,60,0);
				e.Path.AddLine(60,0,100,50);
				e.Path.AddLine(100,50,60,100);
				e.Path.AddLine(60,100,60,70);
				e.Path.AddLine(60,70,0,70);

				//Close the figure
				e.Path.CloseFigure(); 

				//Rotate the path depending on the type of arrow
				if (stencilType == ArrowStencilType.Right) translateMatrix.RotateAt(180,new PointF(50,50));
				if (stencilType == ArrowStencilType.Up) translateMatrix.RotateAt(-90,new PointF(50,50));
				if (stencilType == ArrowStencilType.Down) translateMatrix.RotateAt(90,new PointF(50,50));

				//Scale the matrix and apply it back to the path
				translateMatrix.Scale(e.Width / 100, e.Height / 100);
				e.Path.Transform(translateMatrix);
			}
			else if (stencilType == ArrowStencilType.LeftRight || stencilType == ArrowStencilType.UpDown)
			{
				//Draw the arrow using a 120x100 grid 
				e.Path.AddLine(40,30,80,30);
				e.Path.AddLine(80,30,80,0);
				e.Path.AddLine(80,0,120,50);
				e.Path.AddLine(120,50,80,100);
				e.Path.AddLine(80,100,80,70);
				e.Path.AddLine(80,70,40,70);

				e.Path.AddLine(40,70,40,100);
				e.Path.AddLine(40,100,0,50);
				e.Path.AddLine(0,50,40,0);
				e.Path.AddLine(40,0,40,30);

				if (stencilType == ArrowStencilType.UpDown) translateMatrix.RotateAt(90,new PointF(50,50));

				//Scale the matrix and apply it back to the path
				translateMatrix.Scale(e.Width / 120, e.Height / 100);
				e.Path.Transform(translateMatrix);

				stencil.Options = StencilItemOptions.InnerRectangleFull;
			}
			else if (stencilType == ArrowStencilType.LeftRightUpDown)
			{
				//Draw the arrow using a 140x140 grid 
				e.Path.AddLine(80,60,110,60);
				e.Path.AddLine(110,60,110,50);
				e.Path.AddLine(110,50,140,70);
				e.Path.AddLine(140,70,110,90);
				e.Path.AddLine(110,90,110,80);
				e.Path.AddLine(110,80,80,80);
				
				e.Path.AddLine(80,80,80,110);
				e.Path.AddLine(80,110,90,110);
				e.Path.AddLine(90,110,70,140);
				e.Path.AddLine(70,140,50,110);
				e.Path.AddLine(50,110,60,110);
				e.Path.AddLine(60,110,60,80);
				
				e.Path.AddLine(60,80,30,80);
				e.Path.AddLine(30,80,30,90);
				e.Path.AddLine(30,90,0,70);
				e.Path.AddLine(0,70,30,50);
				e.Path.AddLine(30,50,30,60);

				e.Path.AddLine(30,60,60,60);
				e.Path.AddLine(60,60,60,30);
				e.Path.AddLine(60,30,50,30);
				e.Path.AddLine(50,30,70,0);
				e.Path.AddLine(70,0,90,30);
				e.Path.AddLine(90,30,80,30);
				e.Path.AddLine(80,30,80,60);

				//Scale the matrix and apply it back to the path
				translateMatrix.Scale(e.Width / 140, e.Height / 140);
				e.Path.Transform(translateMatrix);
			}
			if (stencilType == ArrowStencilType.Striped)
			{
			
				//Draw the arrow using a 100x100 grid 
				e.Path.AddRectangle(new Rectangle(0,30,4,40));
				e.Path.AddRectangle(new Rectangle(8,30,8,40));

				e.Path.AddLine(20,30,60,30);
				e.Path.AddLine(60,30,60,0);
				e.Path.AddLine(60,0,100,50);
				e.Path.AddLine(100,50,60,100);
				e.Path.AddLine(60,100,60,70);
				e.Path.AddLine(60,70,20,70);

				//Close the figure
				e.Path.CloseFigure(); 

				//Scale the matrix and apply it back to the path
				translateMatrix.Scale(e.Width / 100, e.Height / 100);
				e.Path.Transform(translateMatrix);
				
				stencil.Options = StencilItemOptions.InnerRectangleFull;
			}
			if (stencilType == ArrowStencilType.Notched)
			{
				//Draw the arrow using a 100x100 grid 
				e.Path.AddLine(0,30,60,30);
				e.Path.AddLine(60,30,60,0);
				e.Path.AddLine(60,0,100,50);
				e.Path.AddLine(100,50,60,100);
				e.Path.AddLine(60,100,60,70);
				e.Path.AddLine(60,70,0,70);
				e.Path.AddLine(0,70,20,50);

				//Close the figure
				e.Path.CloseFigure(); 

				//Scale the matrix and apply it back to the path
				translateMatrix.Scale(e.Width / 100, e.Height / 100);
				e.Path.Transform(translateMatrix);
			}
			if (stencilType == ArrowStencilType.Pentagon)
			{
				//Draw the arrow using a 120x100 grid 
				e.Path.AddLine(0,0,80,0);
				e.Path.AddLine(80,0,120,50);
				e.Path.AddLine(120,50,80,100);
				e.Path.AddLine(80,100,0,100);

				//Close the figure
				e.Path.CloseFigure(); 

				//Scale the matrix and apply it back to the path
				translateMatrix.Scale(e.Width / 100, e.Height / 100);
				e.Path.Transform(translateMatrix);
			}
			else if (stencilType == ArrowStencilType.Chevron)
			{
				//Draw the arrow using a 120x100 grid 
				e.Path.AddLine(0,0,80,0);
				e.Path.AddLine(80,0,120,50);
				e.Path.AddLine(120,50,80,100);
				e.Path.AddLine(80,100,0,100);
				e.Path.AddLine(0,100,20,50);

				//Close the figure
				e.Path.CloseFigure(); 

				//Scale the matrix and apply it back to the path
				translateMatrix.Scale(e.Width / 100, e.Height / 100);
				e.Path.Transform(translateMatrix);
			}
			else if (stencilType == ArrowStencilType.RightCallout || stencilType == ArrowStencilType.LeftCallout || stencilType == ArrowStencilType.UpCallout || stencilType == ArrowStencilType.DownCallout)
			{
				//Draw the arrow using a 120x100 grid 
				e.Path.AddLine(0,0,80,0);
				e.Path.AddLine(80,0,80,40);
				e.Path.AddLine(80,40,90,40);
				e.Path.AddLine(90,40,90,30);
				e.Path.AddLine(90,30,120,50);
				e.Path.AddLine(120,50,90,70);
				e.Path.AddLine(90,70,90,60);
				e.Path.AddLine(90,60,80,60);
				e.Path.AddLine(80,60,80,100);
				e.Path.AddLine(80,100,0,100);

				//Close the figure
				e.Path.CloseFigure(); 

				//Rotate the matrix depending on type
				if (stencilType == ArrowStencilType.LeftCallout) translateMatrix.RotateAt(180,new PointF(60,50));
				if (stencilType == ArrowStencilType.UpCallout) translateMatrix.RotateAt(-90,new PointF(60,50));
				if (stencilType == ArrowStencilType.DownCallout) translateMatrix.RotateAt(90,new PointF(60,50));

				//Scale the matrix and apply it back to the path
				translateMatrix.Scale(e.Width / 100, e.Height / 100);
				e.Path.Transform(translateMatrix);
			}
		
		}
		#endregion

	}
}

