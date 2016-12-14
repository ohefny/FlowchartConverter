// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Crainiate.Diagramming.Printing.PaperSizes;

namespace Crainiate.Diagramming
{
	public static class Geometry
	{
        //Returns the pixel scaling factors on both axis
        public static PointF CalculateUnitFactors(Graphics g)
        {
            switch (g.PageUnit)
            {
                case GraphicsUnit.Pixel:
                    return new PointF(1f, 1f);
                case GraphicsUnit.Display:
                    return new PointF(g.DpiX / 75f, g.DpiY / 75f);
                case GraphicsUnit.Document:
                    return new PointF(g.DpiX / 300f, g.DpiY / 300f);
                case GraphicsUnit.Inch:
                    return new PointF(g.DpiX, g.DpiY);
                case GraphicsUnit.Millimeter:
                    return new PointF(g.DpiX / 25.4f, g.DpiY / 25.4f);
                case GraphicsUnit.Point:
                    return new PointF(g.DpiX / 72f, g.DpiY / 72f);
                default:
                    return new PointF(1f, 1f);
            }
        }

        //Converts diagram units to graphics units
        public static GraphicsUnit ConvertUnit(DiagramUnit diagramUnit)
        {
            return (GraphicsUnit) Enum.Parse(typeof(GraphicsUnit), diagramUnit.ToString());
        }

        //Convert a point in source units to target units
        public static PointF ConvertPoint(PointF point, DiagramUnit source, DiagramUnit target)
        {
            return ConvertPoint(point, source, target, null);
        }

        //Convert a point in source units to target units
        public static PointF ConvertPoint(PointF point, DiagramUnit source, DiagramUnit target, Graphics g)
        {
            if (g == null) g = Singleton.Instance.CreateGraphics();

            g.PageUnit = ConvertUnit(source);
            PointF sourceFactors = CalculateUnitFactors(g);

            g.PageUnit = ConvertUnit(target);
            PointF targetFactors = CalculateUnitFactors(g);

            //Convert point by dividing source factors and multiplying target factors
            return new PointF(point.X * sourceFactors.X / targetFactors.X, point.Y * sourceFactors.Y / targetFactors.Y);
        }

        //Convert a point in source units to target units
        public static PointF ConvertPoint(PointF point, DiagramUnit target)
        {
            return ConvertPoint(point, target, null);
        }

        //Convert a point in source units to target units
        public static PointF ConvertPoint(PointF point, DiagramUnit target, Graphics g)
        {
            if (g == null) g = Singleton.Instance.CreateGraphics();

            g.PageUnit = ConvertUnit(target);
            PointF targetFactors = CalculateUnitFactors(g);

            //Convert point by dividing source factors and multiplying target factors
            return new PointF(point.X / targetFactors.X, point.Y / targetFactors.Y);
        }

        //Convert a point in source units to target units
        public static RectangleF ConvertRectangle(RectangleF rectangle, DiagramUnit source, DiagramUnit target)
        {
            //Convert rectangle by dividing source factors and multiplying target factors
            return ConvertRectangle(rectangle, source, target, null);
        }

        //Convert a point in source units to target units
        public static RectangleF ConvertRectangle(RectangleF rectangle, DiagramUnit source, DiagramUnit target, Graphics g)
        {
            if (g == null) g = Singleton.Instance.CreateGraphics();

            g.PageUnit = ConvertUnit(source);
            PointF sourceFactors = CalculateUnitFactors(g);

            g.PageUnit = ConvertUnit(target);
            PointF targetFactors = CalculateUnitFactors(g);

            //Convert rectangle by dividing source factors and multiplying target factors
            return new RectangleF(rectangle.X * sourceFactors.X / targetFactors.X, rectangle.Y * sourceFactors.Y / targetFactors.Y, rectangle.Width * sourceFactors.X / targetFactors.X, rectangle.Height * sourceFactors.Y / targetFactors.Y);
        }

        //Convert a point in pixels to target units
        public static RectangleF ConvertRectangle(RectangleF rectangle, DiagramUnit target)
        {
            return ConvertRectangle(rectangle, target, null);
        }

        //Convert a point in pixels to target units
        public static RectangleF ConvertRectangle(RectangleF rectangle, DiagramUnit target, Graphics g)
        {
            if (g == null) g = Singleton.Instance.CreateGraphics();

            g.PageUnit = ConvertUnit(target);
            PointF targetFactors = CalculateUnitFactors(g);

            //Convert rectangle by dividing source factors and multiplying target factors
            return new RectangleF(rectangle.X / targetFactors.X, rectangle.Y / targetFactors.Y, rectangle.Width / targetFactors.X, rectangle.Height / targetFactors.Y);
        }

        //Convert a point in source units to target units
        public static SizeF ConvertSize(SizeF size, DiagramUnit source, DiagramUnit target)
        {
            //Return if the same
            if (source == target) return size;

            return ConvertSize(size, source, target, null);
        }

        public static SizeF ConvertSize(SizeF size, DiagramUnit source, DiagramUnit target, Graphics g)
        {
            if (g == null) g = Singleton.Instance.CreateGraphics();

            //Return if the same
            if (source == target) return size;

            g.PageUnit = ConvertUnit(source);
            PointF sourceFactors = CalculateUnitFactors(g);

            g.PageUnit = ConvertUnit(target);
            PointF targetFactors = CalculateUnitFactors(g);

            //Convert point by dividing source factors and multiplying target factors
            return new SizeF(size.Width * sourceFactors.X / targetFactors.X, size.Height * sourceFactors.Y / targetFactors.Y);
        }

        //Convert a point in pixels to target units
        public static SizeF ConvertSize(SizeF size, DiagramUnit target)
        {
            return ConvertSize(size, target, null);
        }

        //Convert a point in pixels to target units
        public static SizeF ConvertSize(SizeF size, DiagramUnit target, Graphics g)
        {
            if (g == null) g = Singleton.Instance.CreateGraphics();

            g.PageUnit = ConvertUnit(target);
            PointF targetFactors = CalculateUnitFactors(g);

            //Convert point by dividing source factors and multiplying target factors
            return new SizeF(size.Width / targetFactors.X, size.Height / targetFactors.Y);
        }

        public static PointF RoundPoint(PointF point, int decimals)
        {
            return new PointF(Convert.ToSingle(Math.Round(point.X, decimals)), Convert.ToSingle(Math.Round(point.Y, decimals)));
        }

        public static SizeF RoundSize(SizeF size, int decimals)
        {
            return new SizeF(Convert.ToSingle(Math.Round(size.Width, decimals)), Convert.ToSingle(Math.Round(size.Height, decimals)));
        }

        public static string Abbreviate(DiagramUnit unit)
        {
            switch (unit)
            {
                case DiagramUnit.Pixel:
                    return "px";
                    break;
                case DiagramUnit.Display:
                    return "ds";
                    break;
                case DiagramUnit.Document:
                    return "dc";
                    break;
                case DiagramUnit.Inch:
                    return "in";
                    break;
                case DiagramUnit.Millimeter:
                    return "mm";
                    break;
                case DiagramUnit.Point:
                    return "pt";
                    break;
                default:
                    return "";
            }
        }
        public static SizeF GetPaperSize(DiagramUnit unit, Iso name)
        {
            return GetPaperSize(unit, name, null);
        }

        public static SizeF GetPaperSize(DiagramUnit unit, Iso name, Graphics g)
        {
            Size mm = new Size();

            if (name == Iso.FourA) mm = new Size(1682, 2378);
            if (name == Iso.TwoA) mm = new Size(1189, 1682);
            if (name == Iso.A0) mm = new Size(841, 1189);
            if (name == Iso.A1) mm = new Size(594, 841);
            if (name == Iso.A2) mm = new Size(420, 594);
            if (name == Iso.A3) mm = new Size(297, 420);
            if (name == Iso.A4) mm = new Size(210, 297);
            if (name == Iso.A5) mm = new Size(148, 210);
            if (name == Iso.A6) mm = new Size(105, 148);
            if (name == Iso.A7) mm = new Size(74, 105);
            if (name == Iso.A8) mm = new Size(52, 74);
            if (name == Iso.A9) mm = new Size(37, 52);
            if (name == Iso.A10) mm = new Size(26, 37);

            if (name == Iso.FourB) mm = new Size(2000, 2828);
            if (name == Iso.TwoB) mm = new Size(1414, 2000);
            if (name == Iso.B0) mm = new Size(1000, 1414);
            if (name == Iso.B1) mm = new Size(707, 1000);
            if (name == Iso.B2) mm = new Size(500, 707);
            if (name == Iso.B3) mm = new Size(353, 500);
            if (name == Iso.B4) mm = new Size(250, 353);
            if (name == Iso.B5) mm = new Size(176, 250);
            if (name == Iso.B6) mm = new Size(125, 176);
            if (name == Iso.B7) mm = new Size(88, 125);
            if (name == Iso.B8) mm = new Size(62, 88);
            if (name == Iso.B9) mm = new Size(44, 62);
            if (name == Iso.B10) mm = new Size(31, 44);

            if (name == Iso.C0) mm = new Size(917, 1297);
            if (name == Iso.C1) mm = new Size(648, 917);
            if (name == Iso.C2) mm = new Size(458, 648);
            if (name == Iso.C3) mm = new Size(324, 458);
            if (name == Iso.C4) mm = new Size(229, 324);
            if (name == Iso.C5) mm = new Size(162, 229);
            if (name == Iso.C6) mm = new Size(114, 162);
            if (name == Iso.C7) mm = new Size(81, 114);
            if (name == Iso.C8) mm = new Size(57, 81);
            if (name == Iso.C9) mm = new Size(40, 57);
            if (name == Iso.C10) mm = new Size(28, 40);

            //Convert to diagram unit supplied
            return ConvertSize(mm, DiagramUnit.Millimeter, unit, g);
        }

        public static SizeF GetPaperSize(DiagramUnit unit, Jis name)
        {
            Size mm = new Size();

            if (name == Jis.FourA) mm = new Size(1682, 2378);
            if (name == Jis.TwoA) mm = new Size(1189, 1682);
            if (name == Jis.A0) mm = new Size(841, 1189);
            if (name == Jis.A1) mm = new Size(594, 841);
            if (name == Jis.A2) mm = new Size(420, 594);
            if (name == Jis.A3) mm = new Size(297, 420);
            if (name == Jis.A4) mm = new Size(210, 297);
            if (name == Jis.A5) mm = new Size(148, 210);
            if (name == Jis.A6) mm = new Size(105, 148);
            if (name == Jis.A7) mm = new Size(74, 105);
            if (name == Jis.A8) mm = new Size(52, 74);
            if (name == Jis.A9) mm = new Size(37, 52);
            if (name == Jis.A10) mm = new Size(26, 37);

            if (name == Jis.B0) mm = new Size(1030, 1456);
            if (name == Jis.B1) mm = new Size(728, 1030);
            if (name == Jis.B2) mm = new Size(515, 728);
            if (name == Jis.B3) mm = new Size(364, 515);
            if (name == Jis.B4) mm = new Size(257, 364);
            if (name == Jis.B5) mm = new Size(182, 257);
            if (name == Jis.B6) mm = new Size(128, 182);
            if (name == Jis.B7) mm = new Size(91, 128);
            if (name == Jis.B8) mm = new Size(64, 91);
            if (name == Jis.B9) mm = new Size(45, 64);
            if (name == Jis.B10) mm = new Size(32, 45);

            //Convert to diagram unit supplied
            return ConvertSize(mm, DiagramUnit.Millimeter, unit);
        }

		//Returns the first point of intersection with a rectangle
		public static PointF RectangleIntersection(PointF p1, PointF p2, RectangleF rect)
		{
			PointF intersection = new PointF();

			if (LineIntersection(p1, p2, new PointF(rect.X, rect.Y), new PointF(rect.Right, rect.Y), ref intersection)) return intersection;
			if (LineIntersection(p1, p2, new PointF(rect.Right, rect.Y), new PointF(rect.Right, rect.Bottom), ref intersection)) return intersection;
			if (LineIntersection(p1, p2, new PointF(rect.Right, rect.Bottom), new PointF(rect.X, rect.Bottom), ref intersection)) return intersection;
			if (LineIntersection(p1, p2, new PointF(rect.X, rect.Bottom), new PointF(rect.X, rect.Y), ref intersection)) return intersection;

			return new PointF();
		}

		//Returns the first point of intersection with a rectangle
		public static bool RectangleIntersects(PointF p1, PointF p2, RectangleF rect)
		{
			PointF intersection = new PointF();

			if (LineIntersection(p1, p2, new PointF(rect.X, rect.Y), new PointF(rect.Right, rect.Y), ref intersection)) return true;
			if (LineIntersection(p1, p2, new PointF(rect.Right, rect.Y), new PointF(rect.Right, rect.Bottom), ref intersection)) return true;
			if (LineIntersection(p1, p2, new PointF(rect.Right, rect.Bottom), new PointF(rect.X, rect.Bottom), ref intersection)) return true;
			if (LineIntersection(p1, p2, new PointF(rect.X, rect.Bottom), new PointF(rect.X, rect.Y), ref intersection)) return true;

			return false;
		}

		// Based on the 2d line intersection method from "comp.graphics.algorithms Frequently Asked Questions"
		//Returns a boolean if they intersect and a reference parameter with the location
		public static bool LineIntersection(PointF line1Point1, PointF line1Point2, PointF line2Point1, PointF line2Point2)
		{
			//	   (Ay-Cy)(Dx-Cx)-(Ax-Cx)(Dy-Cy)
			//   r = -----------------------------  (eqn 1)
			//	   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

			double q = (line1Point1.Y - line2Point1.Y) * (line2Point2.X - line2Point1.X) - (line1Point1.X - line2Point1.X) * (line2Point2.Y - line2Point1.Y);
			double d = (line1Point2.X - line1Point1.X) * (line2Point2.Y - line2Point1.Y) - (line1Point2.Y - line1Point1.Y) * (line2Point2.X - line2Point1.X);

			//parallel lines so no intersection anywhere in Space(in curved space, maybe, but not here in Euclidian space.)
			if (d == 0)
			{
				return false;
			}

			double r = q / d;

			//	   (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
			//   s = -----------------------------  (eqn 2)
			//	   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

			q = (line1Point1.Y - line2Point1.Y) * (line1Point2.X - line1Point1.X) - (line1Point1.X - line2Point1.X) * (line1Point2.Y - line1Point1.Y);

			double s = q / d;

			//If r>1, P is located on extension of AB
			//If r<0, P is located on extension of BA
			//If s>1, P is located on extension of CD
			//If s<0, P is located on extension of DC

			//The above basically checks if the intersection is located at an extrapolated()
			//point outside of the line segments. To ensure the intersection is only(within)
			//the line segments then the above must all be false, ie r between 0 and 1 and s between 0 and 1.

			if (r < 0 || r > 1 || s < 0 || s > 1)
			{
				return false;
			}

			//Px = Ax + r(Bx - Ax)
			//Py = Ay + r(By - Ay)
			return true;
		}

		// Based on the 2d line intersection method from "comp.graphics.algorithms Frequently Asked Questions"
		//Returns a boolean if they intersect and a reference parameter with the location
		public static bool LineIntersection(PointF line1Point1, PointF line1Point2, PointF line2Point1, PointF line2Point2, ref PointF intersection)
		{
			//	   (Ay-Cy)(Dx-Cx)-(Ax-Cx)(Dy-Cy)
			//   r = -----------------------------  (eqn 1)
			//	   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

			double q = (line1Point1.Y - line2Point1.Y) * (line2Point2.X - line2Point1.X) - (line1Point1.X - line2Point1.X) * (line2Point2.Y - line2Point1.Y);
			double d = (line1Point2.X - line1Point1.X) * (line2Point2.Y - line2Point1.Y) - (line1Point2.Y - line1Point1.Y) * (line2Point2.X - line2Point1.X);

			//parallel lines so no intersection anywhere in Space(in curved space, maybe, but not here in Euclidian space.)
			if (d == 0)
			{
				return false;
			}

			double r = q / d;

			//	   (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
			//   s = -----------------------------  (eqn 2)
			//	   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

			q = (line1Point1.Y - line2Point1.Y) * (line1Point2.X - line1Point1.X) - (line1Point1.X - line2Point1.X) * (line1Point2.Y - line1Point1.Y);

			double s = q / d;

			//If r>1, P is located on extension of AB
			//If r<0, P is located on extension of BA
			//If s>1, P is located on extension of CD
			//If s<0, P is located on extension of DC

			//The above basically checks if the intersection is located at an extrapolated()
			//point outside of the line segments. To ensure the intersection is only(within)
			//the line segments then the above must all be false, ie r between 0 and 1 and s between 0 and 1.

			if (r < 0 || r > 1 || s < 0 || s > 1)
			{
				return false;
			}

			//Px = Ax + r(Bx - Ax)
			//Py = Ay + r(By - Ay)
			intersection.X = Convert.ToSingle(line1Point1.X + r * (line1Point2.X - line1Point1.X));
			intersection.Y = Convert.ToSingle(line1Point1.Y + r * (line1Point2.Y - line1Point1.Y));
			return true;
		}

		
		public static double GetAngle(float x1, float y1, float x2, float y2)
		{
			return Math.Atan2(y2 - y1,x2 - x1);
		}

        public static double GetAngle(PointF a, PointF b)
        {
            return Math.Atan2(b.Y - a.Y, b.X - a.X);
        }

        public static PointF SetAngle(PointF a, double radians, float length)
        {
            float x = Convert.ToSingle(Math.Cos(radians) * length);
            float y = Convert.ToSingle(Math.Sin(radians) * length);

            return new PointF(a.X + x, a.Y + y);
        }

		public static double DegreesFromRadians(double Radians)
		{
			return 180 / Math.PI * Radians;
		}

		public static double RadiansFromDegrees(double Degrees)
		{
			return Degrees * Math.PI / 180;
		}

		public static PointF OffsetPoint(PointF location, PointF offset)
		{
			return new PointF(location.X - offset.X, location.Y - offset.Y);
		}

        public static PointF CombinePoint(PointF location1, PointF location2)
        {
            return new PointF(location1.X + location2.X, location1.Y + location2.Y);
        }

		public static PointF GetMiddlePoint(PointF p1, PointF p2)
		{
			PointF mid = new PointF(Math.Abs(p2.X - p1.X) / 2, Math.Abs(p2.Y - p1.Y) / 2);
			
			mid.X += (p1.X < p2.X) ? p1.X : p2.X;
			mid.Y += (p1.Y < p2.Y) ? p1.Y : p2.Y;

			return mid;
		}

		public static double DistancefromOrigin(PointF point)
		{
			return Math.Sqrt(Math.Pow(point.X,2) + Math.Pow(point.Y,2));
		}

		//Creates a rectangle from 2 points
		public static RectangleF CreateRectangle(PointF A,PointF B)
		{
			RectangleF rect = new RectangleF();

			if (A.X < B.X)
			{
				rect.X = A.X;
				rect.Width = B.X - A.X;
			}
			else
			{
				rect.X = B.X;
				rect.Width = A.X - B.X;
			}
			if (A.Y < B.Y)
			{
				rect.Y = A.Y;
				rect.Height = B.Y - A.Y;
			}
			else
			{
				rect.Y = B.Y;
				rect.Height = A.Y - B.Y;
			}			
			return rect;
		}

		public static Rectangle CreateRectangle(Point A, Point B)
		{
			Rectangle rect = new Rectangle();

			if (A.X < B.X)
			{
				rect.X = A.X;
				rect.Width = B.X - A.X;
			}
			else
			{
				rect.X = B.X;
				rect.Width = A.X - B.X;
			}
			if (A.Y < B.Y)
			{
				rect.Y = A.Y;
				rect.Height = B.Y - A.Y;
			}
			else
			{
				rect.Y = B.Y;
				rect.Height = A.Y - B.Y;
			}			
			return rect;
		}

		public static RectangleF ComplementRectangle(RectangleF a,RectangleF b)
		{
			RectangleF c = new RectangleF();
			c.X = (a.Left < b.Left) ? a.Left : b.Left;
			c.Y = (a.Top < b.Top) ? a.Top : b.Top;
			c.Width = (a.Right > b.Right) ? a.Right - c.X : b.Right - c.X;
			c.Height = (a.Bottom > b.Bottom) ? a.Bottom - c.Y : b.Bottom - c.Y;
			return c;
		}

        public static RectangleF ComplementRectangle(RectangleF a, PointF b)
        {
            RectangleF c = new RectangleF();
            c.X = (a.Left < b.X) ? a.Left : b.X;
            c.Y = (a.Top < b.Y) ? a.Top : b.Y;
            c.Width = (a.Right > b.X) ? a.Right - c.X : b.X - c.X;
            c.Height = (a.Bottom > b.Y) ? a.Bottom - c.Y : b.Y - c.Y;
            return c;
        }

		public static bool AreAdjacent(RectangleF a, RectangleF b)
		{
			//X are adjacent
			if ((b.X > a.X && a.Right == b.X) || (a.X > b.X && b.Right == a.X))
			{
				//Check y overlap
				if ((a.Y > b.Y && a.Y < b.Bottom) || (b.Y > a.Y && b.Y < a.Bottom)) return true;
			}
			
			//Y are adjacent
			if ((b.Y > a.Y && a.Bottom == b.Y) || (a.Y > b.Y && b.Bottom == a.Y))
			{
				//Check x overlap
				if ((a.X > b.X && a.X < b.Right) || (b.X > a.X && b.X < a.Right)) return true;
			}
			
			return false;
		}

		public static RectangleF ScaleRectangle(RectangleF rect,float sx, float sy)
		{
			return new RectangleF(rect.X * sx,rect.Y * sy, rect.Width * sx,rect.Height * sy);
		}

		public static RectangleF RoundRectangleF(RectangleF rect,int decimals)
		{
			return new RectangleF(Convert.ToSingle(Math.Round(rect.X,decimals)),Convert.ToSingle(Math.Round(rect.Y,decimals)),Convert.ToSingle(Math.Round(rect.Width,decimals)),Convert.ToSingle(Math.Round(rect.Height,decimals)));
		}

		public static PointF RectangleFarPoint(RectangleF rectangle)
		{
			return new PointF(rectangle.Right,rectangle.Bottom);
		}

		public static RectangleF GetInternalRectangle(GraphicsPath path)
		{
			RectangleF boundingRect = path.GetBounds();
			RectangleF internalRectangle = new RectangleF();
			
			//Set origin to centre of shape
			PointF origin = new PointF(boundingRect.X + (boundingRect.Width / 2),boundingRect.Y + (boundingRect.Height / 2));

			//Set up pen used to test borders
			Pen pen = Singleton.Instance.DefaultPen;

			//Define the four intercept points
			PointF topLeft,topRight,bottomLeft,bottomRight;
			PointF top,right,left,bottom;

			//Get the four diagonal intercepts from the origin
			topLeft = GetPathIntercept(boundingRect.Location,origin,path,pen);
			topRight = GetPathIntercept(new PointF(boundingRect.Right, boundingRect.Y), origin, path, pen);
			bottomLeft = GetPathIntercept(new PointF(boundingRect.X, boundingRect.Bottom), origin, path, pen);
			bottomRight = GetPathIntercept(new PointF(boundingRect.Right, boundingRect.Bottom), origin, path, pen);

			//Get the common rectangle from the four points
			internalRectangle.X = ((topLeft.X > bottomLeft.X) ? topLeft.X : bottomLeft.X);
			internalRectangle.Y = ((topLeft.Y > topRight.Y) ? topLeft.Y : topRight.Y);
			internalRectangle.Width = ((topRight.X < bottomRight.X) ? topRight.X - internalRectangle.X: bottomRight.X- internalRectangle.X);
			internalRectangle.Height = ((bottomLeft.Y < bottomRight.Y) ? bottomLeft.Y - internalRectangle.Y : bottomRight.Y- internalRectangle.Y);					

			//Get the four orthogonal intercepts
			top = GetPathIntercept(new PointF(origin.X,boundingRect.Y), origin, path, pen);
			right = GetPathIntercept(new PointF(boundingRect.Right,origin.Y), origin, path, pen);
			left = GetPathIntercept(new PointF(boundingRect.X,origin.Y), origin, path, pen);
			bottom = GetPathIntercept(new PointF(origin.X,boundingRect.Bottom), origin, path, pen);

			//Apply to internal rectangle
			if (top.Y  > internalRectangle.Y)
			{
				internalRectangle.Y = top.Y;
				internalRectangle.Height -= internalRectangle.Y - top.Y;
			}
			if (right.X < internalRectangle.Right) internalRectangle.Width -= internalRectangle.Right - right.X;
			if (left.X > internalRectangle.X)
			{
				internalRectangle.X = left.X;
				internalRectangle.Width -= internalRectangle.X - left.X;
			}
			if (bottom.Y < internalRectangle.Bottom) internalRectangle.Height -= internalRectangle.Height - bottom.Y;
			
			return internalRectangle;
		}
		
		//Gets the intercept on the boundary of a graphics path for a line drawn between two points
		public static PointF GetPathIntercept(PointF startPoint,PointF endPoint,GraphicsPath path,Pen outlinePen)
		{
			float x = 0;
			float y = 0;
			float xStep;
			float yStep;

			//work out interval in steps
			xStep = ((startPoint.X <= endPoint.X) ? 1.0F : -1.0F);
			yStep = ((startPoint.Y <= endPoint.Y) ? 1.0F : -1.0F);

			float gradient = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
			float reverseGradient = 1 / gradient;

			//Loop making smaller and smaller step adjustments, longer processing time but more accuracy
			while (xStep != 0)
			{
				//Check for vertical line
				if (startPoint.X != endPoint.X)
				{
					//Step through each value of x, determining y and checking if outline visible
					for (x = startPoint.X; ((startPoint.X < endPoint.X) ? x <= endPoint.X : x >= endPoint.X); x += xStep)
					{
						//calculate Y
						//y = Convert.ToSingle((gradient * (x - startPoint.X)) + startPoint.Y);
						y = (gradient * (x - startPoint.X)) + startPoint.Y;

						//Check if we have hit the outline 
						if (path.IsOutlineVisible(x, y, outlinePen)) return new PointF(x, y);
					}
				}
				//Try stepping through each value of y, this is for a line with a high gradient
				//where a small change in x produces a large change in y
				//therefore try small changes in y and work out x

				//Step through each value of y, determining x and checking if outline visible
				if (startPoint.Y != endPoint.Y)
				{
					for (y = startPoint.Y; ((startPoint.Y < endPoint.Y) ? y <= endPoint.Y : y >= endPoint.Y); y += yStep)
					{
						//calculate X
						//x = Convert.ToSingle((reverseGradient * (y - startPoint.Y) + startPoint.X));
						x = (reverseGradient * (y - startPoint.Y) + startPoint.X);

						//check if we have hit the outline 
						if (path.IsOutlineVisible(x, y, outlinePen)) return new PointF(x, y);
					}
				}

				//Make smaller steps if havent found intercept
				xStep += ((startPoint.X <= endPoint.X)? -0.25F : 0.25F);
				yStep += ((startPoint.Y <= endPoint.Y)? -0.25F : 0.25F);
			}

			return startPoint;
		}

		//Creates a copy of a graphics path scaled by x and y
		public static GraphicsPath ScalePath(GraphicsPath path,float sx, float sy)
		{
			if (sx == 1 && sy == 1) return path;

			GraphicsPath newPath = (GraphicsPath) path.Clone();
			Matrix translateMatrix = new Matrix();

			translateMatrix.Scale(sx, sy);
			newPath.Transform(translateMatrix);
			translateMatrix.Dispose();

			return newPath;
		}

		public static void MovePathToOrigin(GraphicsPath path)
		{
			PointF location = path.GetBounds().Location;

			if (location.X != 0 || location.Y != 0)
			{
				Matrix matrix = new Matrix();
				matrix.Translate(-location.X,-location.Y);
				path.Transform(matrix);
				matrix.Dispose();
			}
		}

		public static GraphicsPath RotatePath(GraphicsPath path, float degrees)
		{
			GraphicsPath result = (GraphicsPath) path.Clone(); 
			RectangleF bounds = result.GetBounds();
			PointF center = new PointF(bounds.Width / 2, bounds.Height / 2);

			Matrix matrix = new Matrix();
			matrix.RotateAt(degrees, center);
			result.Transform(matrix);
			return result;
		}

		public static GraphicsPath RotatePath(GraphicsPath path, PointF location, float degrees)
		{
			GraphicsPath result = (GraphicsPath) path.Clone(); 
			RectangleF bounds = result.GetBounds();
			PointF center = new PointF(bounds.Width / 2, bounds.Height / 2);

			Matrix matrix = new Matrix();
			matrix.Translate(location.X, location.Y);
			matrix.RotateAt(degrees, center);
			result.Transform(matrix);
			return result;
		}

		//Return the orientation of a point compared to a center point
		public static PortOrientation GetOrientation(PointF location,PointF center,RectangleF bounds)
		{
			//Calculate the angle between the port and center of the shape
			double angle = DegreesFromRadians(GetAngle(center.X,center.Y,location.X,location.Y));
			double absolute = Math.Abs(angle);

			//Get the limits from the bounds
			double topLeft = Math.Abs(DegreesFromRadians(GetAngle(center.X,center.Y,bounds.X,bounds.Y)));
			double topRight = Math.Abs(DegreesFromRadians(GetAngle(center.X,center.Y,bounds.Right,bounds.Y)));
			double bottomLeft = Math.Abs(DegreesFromRadians(GetAngle(center.X,center.Y,bounds.X,bounds.Bottom)));
			double bottomRight = Math.Abs(DegreesFromRadians(GetAngle(center.X,center.Y,bounds.Right,bounds.Bottom)));
			
			if (absolute >=0 && absolute <=topRight) return PortOrientation.Right;
			if (absolute >= bottomLeft) return PortOrientation.Left;
			if ((absolute >= bottomRight && absolute <= bottomLeft) && angle > 0) return PortOrientation.Bottom;
			return PortOrientation.Top;
		}

		public static PortOrientation GetOrientationOrthogonal(PointF source, PointF previous)
		{
			//Work out old orientation
			if (source.X == previous.X)
			{
				if (source.Y > previous.Y) 
				{
					return PortOrientation.Top;
				}
				else
				{
					return PortOrientation.Bottom;
				}
			}
			else
			{

				if (source.X > previous.X)
				{
					return PortOrientation.Left;
				}
				else
				{
					return PortOrientation.Right;
				}
			}
		}
	}
}

