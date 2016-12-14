// (c) Copyright Crainiate Software 2010




using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Collections;
using System.Text;
using System.Xml;

namespace Crainiate.Diagramming.Serialization
{
	public static class Serialize
	{
		//Returns a string representation of a path
		public static string AddPath(GraphicsPath path)
		{
			PathData data = path.PathData;
			StringBuilder builder = new StringBuilder();
			
			//Write out the path as type point triples
			for (int i=0;i <= data.Points.GetUpperBound(0);i++)
			{
				if (i>0) builder.Append(",");
				builder.Append(XmlConvert.ToString(data.Types[i]));
				builder.Append(",");
				builder.Append(XmlConvert.ToString(data.Points[i].X));
				builder.Append(",");
				builder.Append(XmlConvert.ToString(data.Points[i].Y));
			}
			return builder.ToString();
		}

		//Creates a graphicspath from a string 
		public static GraphicsPath GetPath(string path)
		{
            //If empty path was serialized, return a blank path
            if (path.Length == 0) return new GraphicsPath();

			//Read the path triples into 2 arrays and recreate path
			string[] arr = path.Split(",".ToCharArray());
			int size = (arr.GetUpperBound(0) / 3) + 1;
			PointF[] points = new PointF[size];
			byte[] types = new byte[size];

			for (int i = 0; i <= arr.GetUpperBound(0); i+=3)
			{
				types[i / 3] = XmlConvert.ToByte(arr[i]);
				points[i / 3] = new PointF(XmlConvert.ToSingle(arr[i+1]),XmlConvert.ToSingle(arr[i+2]));
			}	
	
			return new GraphicsPath(points,types);
		}

		//Returns a string representing a SizeF
		public static string AddSizeF(SizeF size)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(XmlConvert.ToString(size.Width));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(size.Height));
			return builder.ToString();
		}

		//Creates a SizeF from a string
		public static SizeF GetSizeF(string size)
		{
			string[] arr = size.Split(",".ToCharArray());
			return new SizeF(XmlConvert.ToSingle(arr[0]),XmlConvert.ToSingle(arr[1]));
		}

		//Returns a string representing a Rectangle
		public static string AddRectangle(Rectangle rectangle)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(XmlConvert.ToString(rectangle.Top));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(rectangle.Left));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(rectangle.Width));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(rectangle.Height));

			return builder.ToString();
		}

		//Creates a Rectangle from a string
		public static Rectangle GetRectangle(string rectangle)
		{
			string[] arr = rectangle.Split(",".ToCharArray());
			return new Rectangle(XmlConvert.ToInt32(arr[0]),XmlConvert.ToInt32(arr[1]),XmlConvert.ToInt32(arr[2]),XmlConvert.ToInt32(arr[3]));
		}

		//Returns a string representing a RectangleF
		public static string AddRectangleF(RectangleF rectangle)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(XmlConvert.ToString(rectangle.Left));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(rectangle.Top));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(rectangle.Width));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(rectangle.Height));

			return builder.ToString();
		}

		//Creates a RectangleF from a string
		public static RectangleF GetRectangleF(string rectangle)
		{
			string[] arr = rectangle.Split(",".ToCharArray());
			return new RectangleF(XmlConvert.ToSingle(arr[0]),XmlConvert.ToSingle(arr[1]),XmlConvert.ToSingle(arr[2]),XmlConvert.ToSingle(arr[3]));
		}

		//Returns a string representing a Size
		public static string AddSize(Size size)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(XmlConvert.ToString(size.Width));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(size.Height));
			return builder.ToString();
		}

		//Creates a SizeF from a string
		public static Size GetSize(string size)
		{
			string[] arr = size.Split(",".ToCharArray());
			return new Size(XmlConvert.ToInt32(arr[0]),XmlConvert.ToInt32(arr[1]));
		}

		//Returns a string representing a PointF
		public static string AddPointF(PointF point)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(XmlConvert.ToString(point.X));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(point.Y));
			return builder.ToString();
		}

		//Creates a PointF from a string
		public static PointF GetPointF(string point)
		{
			string[] arr = point.Split(",".ToCharArray());
			return new PointF(XmlConvert.ToSingle(arr[0]),XmlConvert.ToSingle(arr[1]));
		}

		//Returns a string representing a Point
		public static string AddPoint(Point point)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(XmlConvert.ToString(point.X));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(point.Y));
			return builder.ToString();
		}

		//Creates a PointF from a string
		public static Point GetPoint(string point)
		{
			string[] arr = point.Split(",".ToCharArray());
			return new Point(XmlConvert.ToInt32(arr[0]),XmlConvert.ToInt32(arr[1]));
		}

		public static string AddPointFArray(PointF[] points)
		{
			StringBuilder builder = new StringBuilder();
			bool flag = false;

			foreach (PointF point in points)
			{
				if (flag) builder.Append(",");
				builder.Append(XmlConvert.ToString(point.X));
				builder.Append(",");
				builder.Append(XmlConvert.ToString(point.Y));
				flag = true;
			}
			return builder.ToString();
		}

		public static PointF[] GetPointFArray(string points)
		{
			string[] arr = points.Split(",".ToCharArray());
			PointF[] output = new PointF[arr.GetLength(0) / 2];
			int count = 0;

			for(int i=0; i < arr.GetUpperBound(0); i+=2)
			{
				output[count] = new PointF(XmlConvert.ToSingle(arr[i]),XmlConvert.ToSingle(arr[i+1]));
				count++;
			}
			return output;
		}

		public static string AddPointArray(Point[] points)
		{
			StringBuilder builder = new StringBuilder();
			bool flag = false;

			foreach (Point point in points)
			{
				if (flag) builder.Append(",");
				builder.Append(XmlConvert.ToString(point.X));
				builder.Append(",");
				builder.Append(XmlConvert.ToString(point.Y));
				flag=true;
			}
			return builder.ToString();
		}

		public static Point[] GetPointArray(string points)
		{
			string[] arr = points.Split(",".ToCharArray());
			Point[] output = new Point[arr.GetLength(0) / 2];
			int count = 0;

			for(int i=0; i < arr.GetUpperBound(0); i+=2)
			{
				output[count] = new Point(XmlConvert.ToInt32(arr[i]),XmlConvert.ToInt32(arr[i+1]));
				count++;
			}
			return output;
		}

		public static string AddPointFArrayList(ArrayList points)
		{
			StringBuilder builder = new StringBuilder();
			bool flag = false;

			foreach (PointF point in points)
			{
				if (flag) builder.Append(",");
				builder.Append(XmlConvert.ToString(point.X));
				builder.Append(",");
				builder.Append(XmlConvert.ToString(point.Y));
				flag=true;
			}
			return builder.ToString();
		}

		public static ArrayList GetPointFArrayList(string points)
		{
			string[] arr = points.Split(",".ToCharArray());
			ArrayList output = new ArrayList(arr.GetLength(0) / 2);

			for(int i=0; i < arr.GetUpperBound(0); i+=2)
			{
				output.Add(new PointF(XmlConvert.ToSingle(arr[i]),XmlConvert.ToSingle(arr[i+1])));
			}
			return output;
		}

		public static string AddStringFormat(StringFormat stringFormat)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(Convert.ToInt32(stringFormat.Alignment).ToString());
			builder.Append(",");
			builder.Append(Convert.ToInt32(stringFormat.FormatFlags).ToString());
			builder.Append(",");
			builder.Append(Convert.ToInt32(stringFormat.LineAlignment).ToString());
			builder.Append(",");
			builder.Append(Convert.ToInt32(stringFormat.Trimming).ToString());
 
			return builder.ToString();
		}

		public static StringFormat GetStringFormat(string delimitedString)
		{
			StringFormat format = new StringFormat();
			string[] arr = delimitedString.Split(",".ToCharArray());

			format.Alignment = (StringAlignment) Enum.Parse(typeof(StringAlignment), arr[0]);
			format.FormatFlags = (StringFormatFlags) Enum.Parse(typeof(StringFormatFlags) ,arr[1]);
			format.LineAlignment = (StringAlignment) Enum.Parse(typeof(StringAlignment),arr[2]);
			format.Trimming = (StringTrimming) Enum.Parse(typeof(StringTrimming),arr[3]);

			return format;
		}

		public static string AddFont(Font font)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(font.Name);
			builder.Append(",");
			builder.Append(XmlConvert.ToString(font.Size));
			builder.Append(",");
			builder.Append(Convert.ToInt32(font.Style).ToString());

			return builder.ToString();
		}

		public static Font GetFont(string delimitedString)
		{
			string[] arr = delimitedString.Split(",".ToCharArray());
			string familyName = "";
			float size;
			FontStyle style;
						
			familyName = arr[0];
			size = XmlConvert.ToSingle(arr[1]);
			style = (FontStyle) Enum.Parse(typeof(FontStyle),arr[2]);

			return Singleton.Instance.GetFont(familyName,size,style);
		}

		public static void SerializeTag(SerializationInfo info, object tag)
		{
			//Add tag 
			if (tag != null)
			{
				Type typ = tag.GetType();
				if (typ.IsValueType || typ == typeof(string)) info.AddValue("Tag",tag);
			}
		}

		//Checks for a string instance of the name in the info provided
		public static bool ContainsString(SerializationInfo info, string name)
		{
            return Contains(info, name, typeof(String));
		}

		public static bool Contains(SerializationInfo info,string name, System.Type type)
		{
			try
			{
				object obj = info.GetValue(name, type);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static Type ResolveType(string name)
		{
			Type type = null;

			//Try load from assembly path or resource			
			//Make an array for the list of assemblies.
			System.Reflection.Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();

			foreach (System.Reflection.Assembly assembly in assemblyArray)
			{
				if (!assembly.FullName.StartsWith("mscorlib") && !assembly.FullName.StartsWith("System"))
				{
					type = assembly.GetType(name, false);
					if (type != null) return type;
				}
			}
			return null;
		}
	}
}
