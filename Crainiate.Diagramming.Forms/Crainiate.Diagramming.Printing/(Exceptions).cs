namespace Crainiate.Diagramming.Printing
{
	public class PrintDocumentException : System.Exception 
	{
		public PrintDocumentException(string message) : base(message)
		{
		}
	}

	public class PrintRenderException : System.Exception 
	{
		public PrintRenderException(string message) : base(message)
		{
		}
	}
}
