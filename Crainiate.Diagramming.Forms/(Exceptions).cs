// (c) Copyright Crainiate Software 2010




namespace Crainiate.Diagramming.Forms
{
    public class RenderException : System.Exception
    {
        public RenderException(string message): base(message)
		{}
    }
	public class UndoPointException : System.Exception 
	{
		public UndoPointException(string message) : base(message)
		{}
	}

	public class TabsException : System.Exception 
	{
		public TabsException(string message) : base(message)
		{}
	}
}
